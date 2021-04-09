using System;

namespace JsonLines.Tests
{
	public class Person
	{
		public string Name;
		public int    Age;
		public bool   DarkHair;

		public override bool Equals(object? obj) => obj is Person person && Equals(person);

		protected bool Equals(Person other) => Name == other.Name && Age == other.Age && DarkHair == other.DarkHair;

		public override int GetHashCode() => HashCode.Combine(Name, Age, DarkHair);
	}
}