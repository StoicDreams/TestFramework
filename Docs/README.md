# Stoic Dreams Test Framework
###### Nuget: [www.nuget.org/packages/StoicDreams.TestFramework](https://www.nuget.org/packages/StoicDreams.TestFramework/)
###### GitHub: [github.com/StoicDreams/TestFramework](https://github.com/StoicDreams/TestFramework)

This library includes an `abstract` class called `TestFramework` that developers can inherit from their test classes to access helper methods for unit and integration tests.

Functionality includes extending and simplifying functionality from the `Moq` mocking framework.

## Project Goals

The goal of this library is to provide a framework to use in unit tests and integration tests that grealy simplify organizing tests using the Arrange / Act / Assert testing pattern.

### Framework Features

- Inclusion of Moq mocking framework to use for mocking components.
- Inclusion of FluentAssertions framework to use for human readable assertions.
- Automatic default mocking (using Moq's Mock framework) of a class's constructor dependencies when running unit tests.
- Test framework agnostic: While we use XUnit, we have no restrictions against using other frameworks such as NUnit, MSTest, etc.

### Noted Restrictions

This framework assumes use of IServiceCollection and IServiceProvider to handle dependency injection. And so, also uses ServiceCollection to manage components and build IServiceProvider which is used to handle dependency injection.

Because of this usage:

- Classes being tested with `TestFramework.ArrangeUnitTest` should have a single public constructor. If a class has multiple public constructors it will use the first one.

## Setting up your Test Project

Add the [StoicDreams.TestFramework](https://www.nuget.org/packages/StoicDreams.TestFramework/) Nuget package to your test project.

```xml
<ItemGroup>
	<PackageReference Include="StoicDreams.TestFramework" Version="1.4.20" />
</ItemGroup>
```

Add assembly settings that will allow testing to access internal classes for the projects you are testing.

This update needs to be added to any project that utilizes `internal` classes.

`Usings.cs`
```csharp
// Your existing global using statements
global using System;
...

// Add these 2 lines to allow testing to access internal classes
using System.Runtime.CompilerServices;
// Needed by the testing framework to access internals during reflection for automated mocking
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
// Reference your test project so it can see your main projects internals
[assembly: InternalsVisibleTo("MyCompany.MyApp.Tests")]
```

Inherit the StoicDreams.TestFramework class in all test files.

`ExampleTests.cs` (See a full [example on GitHub](https://github.com/StoicDreams/TestFramework/blob/master/StoicDreams.TestFramework.Tests/SampleParentTests.cs))
```csharp
namespace MyCompany.MyApp;

public class SampleChildATests : StoicDreams.TestFramework
{
	[Theory]
	[InlineData("Test One")]
	[InlineData("Test Two")]
	public void Verify_DoSomething_ReturnsExpectedData(string input)
	{
		IActions<SampleParent> actions = ArrangeUnitTest<SampleParent>(options =>
		{
			options.GetMock<ISampleChildA>().Setup(m => m.DoSomething(input)).Returns($"Mock A: {input}");
			options.GetMock<ISampleChildB>().Setup(m => m.DoSomething(input)).Returns($"Mock B: {input}");
		});

		actions.Act(arrangment => arrangment.Service.DoSomething(input));

		actions.Assert(arrangement =>
		{
			string? result = arrangement.GetResult<string>();
			result.Should().NotBeNullOrWhiteSpace();
			result.Should().BeEquivalentTo($"Parent: Mock A: {input} - Mock B: {input}");
		});
	}
}
```

## Author

**[Erik Gassler](https://www.erikgassler.com/home) - [Stoic Dreams](https://www.stoicdreams.com/home)** - Just a simpleton who likes making stuff with bits and bytes.

**Support** - Visit [Stoic Dreams' GitHub Sponsor page](https://github.com/sponsors/StoicDreams) if you would like to provide support.

**Software Development Standards** - Check out my [Simple-Holistic-Agile Software Engineering Standards](https://www.softwarestandards.dev/home) website to see my standards for developing software.
