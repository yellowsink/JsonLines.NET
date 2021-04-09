using NUnit.Framework;

namespace JsonLines.Tests
{
	public class SerializerTests
	{
		[SetUp]
		public void Setup() { }

		[Test]
		public void SerializeSingleTest()
		{
			var raw = new []
			{
				new Person
				{
					Name = "Thomas",
					Age = 35,
					DarkHair = true
				}
			};
			
			var actual = JsonLinesSerializer.Serialize(raw);

			const string expected = "{\"Name\":\"Thomas\",\"Age\":35,\"DarkHair\":true}";
			
			Assert.AreEqual(expected, actual);
		}
		
		[Test]
		public void SerializeMultiTest()
		{
			var raw = new []
			{
				new Person
				{
					Name     = "Thomas",
					Age      = 35,
					DarkHair = true
				},
				new Person
				{
					Name     = "Jane",
					Age      = 24,
					DarkHair = true
				},
				new Person
				{
					Name     = "Kate",
					Age      = 33,
					DarkHair = false
				}
			};
			
			var actual = JsonLinesSerializer.Serialize(raw);

			const string expected = "{\"Name\":\"Thomas\",\"Age\":35,\"DarkHair\":true}\n{\"Name\":\"Jane\",\"Age\":24,\"DarkHair\":true}\n{\"Name\":\"Kate\",\"Age\":33,\"DarkHair\":false}";
			
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void DeserializeSingleTest()
		{
			const string raw = "{\"Name\":\"Alice\",\"Age\":22,\"DarkHair\":true}";
			
			var expected = new []
			{
				new Person
				{
					Name     = "Alice",
					Age      = 22,
					DarkHair = true
				}
			};
			
			var actual = JsonLinesSerializer.Deserialize<Person>(raw);

			Assert.AreEqual(expected, actual);
		}
		
		[Test]
		public void DeserializeMultiTest()
		{
			const string raw = "{\"Name\":\"Alice\",\"Age\":22,\"DarkHair\":true}\n{\"Name\":\"Bob\",\"Age\":36,\"DarkHair\":false}\n{\"Name\":\"Jim\",\"Age\":41,\"DarkHair\":false}";
			
			var expected = new []
			{
				new Person
				{
					Name     = "Alice",
					Age      = 22,
					DarkHair = true
				},
				new Person
				{
					Name     = "Bob",
					Age      = 36,
					DarkHair = false
				},
				new Person
				{
					Name     = "Jim",
					Age      = 41,
					DarkHair = false
				}
			};
			
			var actual = JsonLinesSerializer.Deserialize<Person>(raw);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void SplitTest()
		{
			const string raw = "{\"Name\":\"Alice\",\"Age\":22,\"DarkHair\":true}\n{\"Name\":\"Bob\",\"Age\":36,\"DarkHair\":false}\n{\"Name\":\"Jim\",\"Age\":41,\"DarkHair\":false}";
			var expected = new[]
			{
				"{\"Name\":\"Alice\",\"Age\":22,\"DarkHair\":true}",
				"{\"Name\":\"Bob\",\"Age\":36,\"DarkHair\":false}",
				"{\"Name\":\"Jim\",\"Age\":41,\"DarkHair\":false}"
			};

			var actual = JsonLinesSerializer.Split(raw);
			
			Assert.AreEqual(expected, actual);
		}
	}
}