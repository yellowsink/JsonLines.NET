using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utf8Json;

namespace JsonLines
{
	public static class JsonLinesSerializer
	{
		/// <summary>
		/// Splits JSON Lines data into individual JSON objects, asynchronously
		/// </summary>
		/// <param name="s">IAsyncEnumerable of chars</param>
		/// <returns>An async enumerable json objects</returns>
		public static async IAsyncEnumerable<string> Split(IAsyncEnumerable<char> s)
		{
			var curlyBraceLevel = 0;
			var objectIndex     = 0;
			
			var working = "";
			await foreach (var c in s)
			{
				if (c == '{') curlyBraceLevel++;
				if (c == '}') curlyBraceLevel--;

				working += c;

				if (curlyBraceLevel != 0 || c != '}') continue;
				
				var w = working;
				working = "";
				yield return w.Trim('\n');
			}
		}

		/// <summary>
		/// Splits JSON Lines data into individual JSON objects
		/// </summary>
		/// <param name="jsonLines">JSON Lines data</param>
		/// <returns>An array of JSON objects</returns>
		public static string[] Split(string jsonLines)
		{
			var objects         = new List<StringBuilder>();
			var curlyBraceLevel = 0;
			var objectIndex     = 0;
			foreach (var c in jsonLines.Where(c => c != '\n'))
			{
				curlyBraceLevel = c switch // faster than if else (ok, but is it though, past me? - 2022-11-25)
				{
					'{' => curlyBraceLevel + 1,
					'}' => curlyBraceLevel - 1,
					_   => curlyBraceLevel
				};

				if (objects.Count > objectIndex)
					objects[objectIndex].Append(c);
				else
					objects.Add(new StringBuilder(c.ToString()));
				if (curlyBraceLevel == 0 && c == '}')
					objectIndex++;
			}

			return objects.Select(o => o.ToString()).ToArray();
		}

		/// <summary>
		/// Deserializes JSON Lines data to an array of the given type T
		/// </summary>
		/// <param name="jsonLines">JSON Lines data</param>
		/// <typeparam name="T">The type to deserialize to</typeparam>
		/// <returns>An array of deserialized JSON</returns>
		public static T[] Deserialize<T>(string jsonLines)
		{
			var split   = Split(jsonLines);
			var objects = new T[split.Length];

			for (var i = 0; i < split.Length; i++)
				objects[i] = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetBytes(split[i]));

			return objects.ToArray();
		}

		/// <summary>
		/// Deserializes JSON Lines data to an array of objects
		/// </summary>
		/// <param name="jsonLines">JSON Lines data</param>
		/// <returns>An array of deserialized JSON</returns>
		public static object[] Deserialize(string jsonLines) => Deserialize<object>(jsonLines);

		/// <summary>
		/// Serializes a collection of a objects into JSON Lines format
		/// </summary>
		/// <param name="objs">A collection of objects</param>
		/// <returns>JSON Lines data</returns>
		public static string Serialize(IEnumerable<object> objs)
		{
			var objArray = objs as object[] ?? objs.ToArray();

			var jsonSnippets = new string[objArray.Length];

			for (var i = 0; i < objArray.Length; i++)
				jsonSnippets[i] = Encoding.UTF8.GetString(JsonSerializer.Serialize(objArray[i]));

			return string.Join('\n', jsonSnippets);
		}

		public static async IAsyncEnumerable<T> DeserializeAsync<T>(IAsyncEnumerable<char> c)
		{
			await foreach (var obj in Split(c))
				yield return JsonSerializer.Deserialize<T>(Encoding.UTF8.GetBytes(obj));
		}

		public static async IAsyncEnumerable<char> SerializeAsync<T>(IAsyncEnumerable<T> objs)
		{
			await foreach (var obj in objs)
			{
				foreach (var c in JsonSerializer.Serialize(obj))
					yield return (char) c;

				yield return '\n';
			}
		}
	}
}