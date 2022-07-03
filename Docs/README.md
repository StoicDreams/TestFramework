# Stoic Dreams Test Framework
###### Nuget: StoicDreams.TestFramework

This library includes an `abstract` class to access helper methods for unit and integration tests.

Functionality includes extending and simplifying functionality from the `Moq` mocking framework.

## Project Goals

The goal of this library is to provide a framework to use in unit tests and integration tests that grealy simplify organizing tests using the Arrange / Act / Assert testing pattern.

### Framework Features

- Inclusion of Moq mocking framework to use for mocking components.
- Inclusion of FLuentAssertions framework to use for human readable assertions.
- Automatic default mocking (using Moq's Mock framework) of a class's constructor dependencies when running unit tests.
- Test framework agnostic: While we use XUnit, we have not restrictions against using other frameworks such as NUnit, MSTest, etc.

### Noted Restrictions

This framework assumes use of IServiceCollection and IServiceProvider to handle dependency injection. And so, also uses ServiceCollection to manage components and build IServiceProvider which is used to handle dependency injection. Because of this usage:

- Classes being tested with `TestFramework.ArrangeUnitTest` should have a single public constructor.

## Setting up your Test Project

Add the `StoicDreams.TestFramework` Nuget package to your test project.

```xml
<ItemGroup>
	<PackageReference Include="StoicDreams.TestFramework" Version="0.1.9" />
</ItemGroup>
```


## Author

* **[Erik Gassler](https://www.erikgassler.com/home) - [Stoic Dreams](https://www.stoicdreams.com/home)** - Just a simpleton who likes making stuff with bits and bytes. Visit [my Patreon page](https://www.patreon.com/stoicdreams) if you would like to provide support.
