# JSON Lines.NET

[![buddy pipeline](https://app.buddy.works/cainy-a/jsonlines-net/pipelines/pipeline/320493/badge.svg?token=537b401f436f16f2fb7cc5edefd041f3aa593895b89e70cf2ca999b54ea0fdb8 "buddy pipeline")](https://app.buddy.works/cainy-a/jsonlines-net/pipelines/pipeline/320493)

A library to work with the [JSON Lines](https://jsonlines.org/) format in .NET

Licensed under `LGPL-3.0-or-later`.

JSONLines.NET is super fast.

How? JSONLines.NET is powered by [Utf8Json](https://github.com/neuecc/Utf8Json/).
According to [this article](https://michaelscodingspot.com/the-battle-of-c-to-json-serializers-in-net-core-3/)
and [this benchmark too](https://aloiskraus.wordpress.com/2019/09/29/net-serialization-benchmark-2019-roundup/),
it's the fastest serializer and an incredibly close 2nd for deserializing, after Jil.

TODO for future: rewrite with SimdJsonSharp because speed go brrr.

## Provided functions

### `JsonLines.JsonLinesSerializer`

| Name              | Params                              | Returns    | Description                                                  |
| ----------------- | ----------------------------------- | ---------- | ------------------------------------------------------------ |
| `Split()`         | - `string jsonLines`                | `string[]` | Splits JSON Lines data into individual JSON objects. Even though the JSON Lines spec requires one object per line, this supports pretty-printed files |
| `Deserialize<>()` | - `Type T`<br/>- `string jsonLines` | `T[]`      | Deserializes JSON Lines data to an array of the given type `T` |
| `Deserialize()`   | - `string jsonLines`                | `object[]` | Deserializes JSON Lines data to an array of objects          |
| `Serialize()`     | - `IEnumerable<object>`             | `string`   | Serializes a collection of objects into JSON Lines format    |



## Examples

### Deserialization

Input data:

```json
{"Name":"Alice","Age":22,"DarkHair":true}
{"Name":"Bob","Age":36,"DarkHair":false}
{"Name":"Jim","Age":41,"DarkHair":false}
```

C# class to represent our data type:

```cs
public class Person
{
    public string Name;
    public int    Age;
    public bool   DarkHair;
}
```

C# code to deserialize JSON Lines:

```cs
using JsonLines;
var rawData = ""; // JSON data goes in here, or could be loaded any other way
var deserialized = JsonLinesSerializer.Deserialize<Person>(rawData);
```

Resulting data structure:

`Person[]`

- `[0]`
  - `Name == "Alice"`
  - `Age == 22`
  - `DarkHair == true`
- `[1]`
  - `Name == "Bob"`
  - `Age == 36`
  - `DarkHair == false`
- `[2]`
  - `Name == "Jim"`
  - `Age == 41`
  - `DarkHair == false`

### Serialization

Input data structure:

`Person[]`

- `[0]`
  - `Name = "Thomas"`
  - `Age = 35`
  - `DarkHair = true`
- `[1]`
  - `Name = "Jane"`
  - `Age = 24`
  - `DarkHair = true`
- `[2]`
  - `Name = "Kate"`
  - `Age = 33`
  - `DarkHair = false`

C# class to represent our data type:

```cs
public class Person
{
    public string Name;
    public int    Age;
    public bool   DarkHair;
}
```

C# code to serialize JSON Lines:

```cs
using JsonLines;
var rawData = new Person[] { /* create data structure here */ };
var serialized = JsonLinesSerializer.Serialize(rawData);
```

Output data:

```json
{"name":"Thomas","age": 35,"DarkHair":true}
{"name":"Jane","age":24,"DarkHair":true}
{"name":"Kate","age":33,"DarkHair":false}
```
