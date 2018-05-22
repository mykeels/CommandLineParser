## Command Line Parser

A simple .NET CLI arguments parser library using C# attributes.

Tired of reading the `string[] args` passed to your main method yourself? This simple library lets you parse the arguments into any standard .NET object.

### Installation

```
dotnet add package CommandLine.Parser
```

### Examples

Here are a few examples:

#### Default Option-Name Mode

Take this simple program for example:

```bat
copy.exe --source "./my-stuff.txt" --destination "./new-folder"
```

We want the `--source` and `--destination` values.

```csharp
public class FileCopyModel {
    public string source { get; set; }
    public string destination { get; set; }
}

public class Program {
    public static void Main(string[] args) {
        CommandLineParser parser = new CommandLineParser(args);
        FileCopyModel copyInfo = parser.Parse<FileCopyModel>();
        /* copyinfo contains:
            {
                "source": "./my-stuff.txt",
                "destination": "./new-folder",
            }
        */
        Console.Read();
    }
}
```

#### Custom Option-Name Mode

If for the same program above, we wanted to use short-hand option-name representation instead. E.g.

```bat
copy.exe -s "./my-stuff.txt" -d "./new-folder"
```

Our `FileCopyModel` class would have to change a bit

```csharp
using CommandLineParser.Attributes;
public class FileCopyModel {
    [Flag("s")]
    public string source { get; set; }
    [Flag("d")]
    public string destination { get; set; }
}
```

Our `Program` class would be exactly the same. The `FlagAttribute` class can be found [here](CommandLineParser/Attributes/FlagAttribute.cs)

#### Required Flags Mode

It might be neccessary for some flags/options to be made compulsory so the program would not proceed unless their values are provided. Set the `required` boolean parameter in the `Flag` attribute constructor to `true` on the property you want to be compulsory

```csharp
[Flag("s", required: true)]
public string source { get; set; }
```

The full definition for `FlagsAttribute` is:

```csharp
[Flag(shortName: "s", name: "source", required: true)]
```

#### Help Text Mode

In command-line interfaces, help information is neccessary for navigating through command APIs. Even here, `CommandLineParser` has got your back.

```csharp
[Help("Handles file-copy actions")]
public class FileCopyModel {
    [Flag("s", required: true)]
    public string source { get; set; }
    [Flag("d")]
    public string destination { get; set; }
}

public class Program {
    public static void Main(string[] args) {
        CommandLineParser parser = new CommandLineParser(args);
        string helpText = parser.GetHelpInfo<FileCopyModel>();
        if (!string.IsNullOrEmpty(helpText)) {
            Console.WriteLine(helpText);
        }
        Console.Read();
    }
}
```

Now run in command-line:

```bat
copy.exe --help
```

Results:
```bat
========== Help Information ==========
Handles file-copy actions

copy.exe -s [-d]
========== End Help Information ==========
```

#### Help Text for Option Mode

Your users can query help information for a particular option. E.g.

```bat
copy.exe --source --help
```

Result:

```bat
--source (The original uri location of the file)
```

```csharp
[Help("Handles file-copy actions")]
public class FileCopyModel {
    [Flag("s", required: true)]
    [Help("The original uri location of the file")]
    public string source { get; set; }
    [Flag("d")]
    [Help("The uri location the file is to be replicated in")]
    public string destination { get; set; }
}
```

### Supported Types

- String
- Integer
- Long
- Boolean
- DateTime
- Enum
- .NET Complex Objects

### Support for Complex .NET Objects using Transforms

When one of the `flags` or `options` has a string value or values that you intend to convert into a complex type like say [AddressModel](CommandLineParser.Console/Models/AddressModel.cs) for instance, you can use the `[TransformAttribute]` attribute on the corresponding property.

The `TransformAttribute` takes a `Type` and `string` parameter. The `string` parameter should be the name of an existing method that takes a string parameter, converts it to and returns an object of the desired complex type.

The `Type` parameter should be the `Type` of the parent class that contains the method discussed above.

E.g.

```bat
contact.exe --add "{"name":"Mykeels","phone":8012345678,"email":"contact@example.com"}"
```

```csharp
public class ContactSetupModel {
    
    [Flag(name: "add")]
    [Transform(typeof(ContactSetupModel), nameof(ConvertToContact))]
    public ContactInsertModel Contact { get; set; }

    public ContactInsertModel ConvertToContact(string contactInfo) {
        return Newtonsoft.Json.JsonConvert.DeserializeObject<ContactInsertModel>(contactInfo);
    }

    public class ContactInsertModel {
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
    }
}
```