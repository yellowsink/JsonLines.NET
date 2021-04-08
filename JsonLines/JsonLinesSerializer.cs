using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utf8Json;

namespace JsonLines
{
	public static class JsonLinesSerializer
	{
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
				curlyBraceLevel = c switch // faster than if else
				{
					'{' => curlyBraceLevel + 1,
					'}' => curlyBraceLevel - 1,
					_   => curlyBraceLevel
				};

				if (objects.Count > objectIndex + 1)
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
				objects[i] = JsonSerializer.Deserialize<T>(split[i]);

			return objects.ToArray();
		}

		/// <summary>
		/// Deserializes JSON Lines data to an array of objects
		/// </summary>
		/// <param name="jsonLines">JSON Lines data</param>
		/// <returns>An array of deserialized JSON</returns>
		public static object[] Deserialize(string jsonLines) => Deserialize<object>(jsonLines);

		/// <summary>
		/// Serializes a collection of a provided type into JSON Lines format
		/// </summary>
		/// <param name="objs">A collection of objects</param>
		/// <typeparam name="T">The type of the objects</typeparam>
		/// <returns>JSON Lines data</returns>
		public static string Serialize<T>(IEnumerable<T> objs)
		{
			var objArray = objs as T[] ?? objs.ToArray();

			var jsonSnippets = new string[objArray.Length];

			for (var i = 0; i < objArray.Length; i++)
				jsonSnippets[i] = JsonSerializer.Serialize(objArray[i]).ToString();

			return string.Join('\n', jsonSnippets);
		}

		/// <summary>
		/// Serializes a collection of objects into JSON Lines format
		/// </summary>
		/// <param name="objs">A collection of objects</param>
		/// <returns>JSON Lines data</returns>
		public static string Serialize(IEnumerable<object> objs) => Serialize<object>(objs);
	}
}