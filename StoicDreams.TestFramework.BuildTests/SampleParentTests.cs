namespace StoicDreams.Tests;

public class SampleParentTests : TestFramework
{
    [Theory]
    [InlineData("Test One")]
    [InlineData("Test Two")]
    public void Verify_DoSomething_ReturnsExpectedData(string input)
    {
        ArrangeUnitTest<SampleParent>(options =>
        {
            options.GetService<ISampleChildA>().DoSomething(input).Returns($"Mock A: {input}");
            options.GetService<ISampleChildB>().DoSomething(input).Returns($"Mock B: {input}");
        })
        .Act(arrangment => arrangment.Service.DoSomething(input))
        .Assert(arrangement =>
        {
            string? result = arrangement.GetNullableResult<string>();
            result.Should().NotBeNullOrWhiteSpace();
            result.Should().BeEquivalentTo($"Parent: Mock A: {input} - Mock B: {input}");
        });
    }

    [Theory]
    [InlineData("Test One")]
    [InlineData("Test Two")]
    public void Verify_DoSomethingWithIsAny_ReturnsExpectedData(string input)
    {
        ArrangeUnitTest<SampleParent>(options =>
        {
            options.GetService<ISampleChildA>().DoSomething(IsAny<string>()).Returns($"Mock A: {input}");
            options.GetService<ISampleChildB>().DoSomething(Is(input)).Returns($"Mock B: {input}");
        })
        .Act(act => act.Service.DoSomething(input))
        .Assert(assert =>
        {
            string? result = assert.GetNullableResult<string>();
            result.Should().NotBeNullOrWhiteSpace();
            Assert.Equal($"Parent: Mock A: {input} - Mock B: {input}", result);
        });
    }

    [Theory]
    [InlineData("Test One")]
    [InlineData("Test Two")]
    public void Verify_DoSomethingWithIsPredicate_ReturnsExpectedData(string input)
    {
        ArrangeUnitTest<SampleParent>(options =>
        {
            options.GetService<ISampleChildA>().DoSomething(IsAny<string>()).Returns($"Mock A: {input}");
            options.GetService<ISampleChildB>().DoSomething(Is<string>(x => x == input)).Returns($"Mock B: {input}");
        })
        .Act(act => act.Service.DoSomething(input))
        .Assert(assert =>
        {
            string? result = assert.GetNullableResult<string>();
            result.Should().NotBeNullOrWhiteSpace();
            Assert.Equal($"Parent: Mock A: {input} - Mock B: {input}", result);
        });
    }

    [Theory]
    [InlineData("Test One")]
    [InlineData("Test Two")]
    public void Verify_DoSomething_ReturnsNull(string input)
    {
        ArrangeUnitTest<SampleParent>(options =>
        {
            options.GetService<ISampleChildA>().DoSomething(input).Returns($"Mock A: {input}");
            options.GetService<ISampleChildB>().DoSomething(input).Returns($"Mock B: {input}");
        })
        .Act(arrangment => (string?)null)
        .Assert(arrangement =>
        {
            string? result = arrangement.GetNullableResult<string>();
            result.Should().BeNull();
        });
    }

    [Theory]
    [InlineData("Test One")]
    [InlineData("Test Two")]
    public void Verify_DoSomethingElse_ReturnsExpectedData(string input)
    {
        ArrangeUnitTest<SampleParent>(options =>
        {
            options.GetService<ISampleChildA>(mock =>
            {
                mock.Value.Returns($"Mock A: {input}");
            });
            options.GetService<ISampleChildB>(mock =>
            {
                mock.Value.Returns($"Mock B: {input}");
            });
        })
        .Act(arrangment => arrangment.Service.DoSomethingElse(input))
        .Assert(arrangement =>
        {
            string result = arrangement.Service.Value;
            result.Should().NotBeNullOrWhiteSpace();
            result.Should().BeEquivalentTo($"Parent: Mock A: {input} - Mock B: {input}");
            arrangement.GetService<ISampleChildA>().Received().DoSomethingElse(input);
            arrangement.GetService<ISampleChildB>().Received().DoSomethingElse(input);
        });
    }

    [Theory]
    [InlineData("Test One")]
    [InlineData("Test Two")]
    public void Verify_Async_DoSomethingElse_ReturnsExpectedData(string input)
    {
        ArrangeUnitTest<SampleParent>(options =>
        {
            options.GetService<ISampleChildA>(mock =>
            {
                mock.Value.Returns($"Mock A: {input}");
            });
            options.GetService<ISampleChildB>(mock =>
            {
                mock.Value.Returns($"Mock B: {input}");
            });
        })
        .Act(async arrangment => await Task.Run(() => arrangment.Service.DoSomethingElse(input)))
        .Assert(arrangement =>
        {
            string result = arrangement.Service.Value;
            result.Should().NotBeNullOrWhiteSpace();
            result.Should().BeEquivalentTo($"Parent: Mock A: {input} - Mock B: {input}");
            arrangement.GetService<ISampleChildA>().Received().DoSomethingElse(input);
            arrangement.GetService<ISampleChildB>().Received().DoSomethingElse(input);
        });
    }

    [Theory]
    [InlineData("Test One", "Else One")]
    [InlineData("Test Two", "Else Two")]
    public void Verify_Raw_Integration_Test(string inputDoSomething, string inputDoSomethingElse)
    {
        ArrangeIntegrationTest<ISampleParent>(startupHandlers: SampleStartup.ConfigureServices)
        .Act(arrangement =>
        {
            arrangement.Service.DoSomethingElse(inputDoSomethingElse);
            return arrangement.Service.DoSomething(inputDoSomething);
        })
        .Assert(arrangement =>
        {
            string somethingResult = arrangement.GetResult<string>();
            string elseResult = arrangement.Service.Value;
            somethingResult.Should().NotBeNullOrWhiteSpace();
            elseResult.Should().NotBeNullOrWhiteSpace();
            somethingResult.Should().NotBeEquivalentTo(elseResult);
            Assert.Equal($"Parent: Something A: {inputDoSomething} - Something B: Something A: {inputDoSomething}", somethingResult);
            Assert.Equal($"Parent: Something Else A: {inputDoSomethingElse} - Something Else B: Something Else A: {inputDoSomethingElse}", elseResult);
        });
    }

    [Theory]
    [InlineData("Test One", "Else One")]
    [InlineData("Test Two", "Else Two")]
    public void Verify_Itegration_Testing_Overwriting_Service_With_Mock(string inputDoSomething, string inputDoSomethingElse)
    {
        ArrangeIntegrationTest<ISampleParent>(options =>
        {
            options.ReplaceServiceWithSub<ISampleChildB>(mock =>
            {
                mock.DoSomething(inputDoSomething).Returns($"Mocked {inputDoSomething}");
                mock.Value.Returns($"Mocked {inputDoSomethingElse}");
            });
        }, SampleStartup.ConfigureServices)
        .Act(arrangement =>
        {
            arrangement.Service.DoSomethingElse(inputDoSomethingElse);
            return arrangement.Service.DoSomething(inputDoSomething);
        })
        .Assert(arrangement =>
        {
            string somethingResult = arrangement.GetResult<string>();
            string elseResult = arrangement.Service.Value;
            somethingResult.Should().NotBeNullOrWhiteSpace();
            elseResult.Should().NotBeNullOrWhiteSpace();
            somethingResult.Should().NotBeEquivalentTo(elseResult);
            arrangement.GetService<ISampleChildB>().Received().DoSomethingElse(inputDoSomethingElse);
            Assert.Equal($"Parent: Something A: {inputDoSomething} - Mocked {inputDoSomething}", somethingResult);
            Assert.Equal($"Parent: Something Else A: {inputDoSomethingElse} - Mocked {inputDoSomethingElse}", elseResult);
        });
    }

    [Theory]
    [InlineData("Test One", "Else One")]
    [InlineData("Test Two", "Else Two")]
    public void Verify_Itegration_Using_Passed_In_IServiceCollection(string inputDoSomething, string inputDoSomethingElse)
    {
        IServiceCollection customServices = new ServiceCollection();
        ArrangeIntegrationTest<ISampleParent>(options =>
        {
            options.ReplaceServiceWithSub<ISampleChildB>(mock =>
            {
                mock.DoSomething(inputDoSomething).Returns($"Mocked {inputDoSomething}");
                mock.Value.Returns($"Mocked {inputDoSomethingElse}");
            });
        }, SampleStartup.ConfigureServices)
        .Act(arrangement =>
        {
            arrangement.Service.DoSomethingElse(inputDoSomethingElse);
            return arrangement.Service.DoSomething(inputDoSomething);
        })
        .Assert(arrangement =>
        {
            string somethingResult = arrangement.GetResult<string>();
            string elseResult = arrangement.Service.Value;
            somethingResult.Should().NotBeNullOrWhiteSpace();
            elseResult.Should().NotBeNullOrWhiteSpace();
            somethingResult.Should().NotBeEquivalentTo(elseResult);
            arrangement.GetService<ISampleChildB>().Received().DoSomethingElse(inputDoSomethingElse);
            Assert.Equal($"Parent: Something A: {inputDoSomething} - Mocked {inputDoSomething}", somethingResult);
            Assert.Equal($"Parent: Something Else A: {inputDoSomethingElse} - Mocked {inputDoSomethingElse}", elseResult);
        });
    }

    [Theory]
    [InlineData("Test One")]
    [InlineData("Test Two")]
    public void Verify_ArrangeTest_Explicitly_Adds_Service(string input)
    {
        ArrangeTest<ISampleParent>(options =>
        {
            // For this type of test we need to explicitly add the service we're going to test
            options.AddService<ISampleParent, SampleParent>();
            // And explicitly add any dependent services, using mocks if desired.
            options.AddSub<ISampleChildA>();
            options.AddSub<ISampleChildB>();
            // Demonstrating that adding the service without the expected interface will not properly override previous entries.
            options.AddService<SampleChildA>(() => { return new SampleChildA(); });
            options.AddService<SampleChildB>();
        })
        .Act(arrangement => arrangement.Service.DoSomething(input))
        .Assert(arrangement =>
        {
            // Final result is from mocked services, not the classes that were improperly added without their expected interfaces.
            Assert.Equal($"Parent:  - ", arrangement.GetResult<string>());
        });
    }

    [Theory]
    [InlineData("Test One")]
    [InlineData("Test Two")]
    public void Verify_ArrangeTest_Returning_Service_To_Test(string input)
    {
        ArrangeTest(options =>
        {
            ISampleChildA childA = new SampleChildA();
            return childA;
        })
        .Act(arrangement => arrangement.Service.DoSomething(input))
        .Assert(arrangement =>
        {
            Assert.Equal($"Something A: {input}", arrangement.GetResult<string>());
        });
    }

    [Theory]
    [InlineData("Test One")]
    [InlineData("Test Two")]
    public void Verify_Async_ArrangeTest_Returning_Service_To_Test(string input)
    {
        ArrangeTest(options =>
        {
            ISampleChildA childA = new SampleChildA();
            return childA;
        })
        .Act(async arrangement => await Task.Run(() => arrangement.Service.DoSomething(input)))
        .Assert(arrangement =>
        {
            Assert.Equal($"Something A: {input}", arrangement.GetResult<string>());
        });
    }

    [Theory]
    [InlineData("Test One")]
    [InlineData("Test Two")]
    public void Verify_Act_Throws_Exception(string input)
    {
        ArrangeTest(options =>
        {
            ISampleChildA childA = Sub<ISampleChildA>(mock =>
            {
                mock.DoSomething(input).Returns(input => throw new Exception("Mock exception"));
            });
            return childA;
        })
        .ActThrowsException(arrangement => arrangement.Service.DoSomething(input))
        .Assert(arrangement =>
        {
            Assert.Equal("Mock exception", arrangement.GetResult<Exception>().IsNotNull().Message);
        });
    }

    public class MockException : Exception
    {
        public MockException(string message) : base(message) { }
    }

    public class UnusedException : Exception
    {
        public UnusedException(string message) : base(message) { }
    }

    [Theory]
    [InlineData("Test One")]
    [InlineData("Test Two")]
    public void Verify_Act_Throws_Specific_Exception(string input)
    {
        ArrangeTest(options =>
        {
            ISampleChildA childA = Sub<ISampleChildA>(mock =>
            {
                mock.DoSomething(input).Throws(new MockException("Mock exception"));
            });
            return childA;
        })
        .ActThrowsException<MockException>(arrangement => arrangement.Service.DoSomething(input))
        .Assert(arrangement =>
        {
            Assert.Equal("Mock exception", arrangement.GetResult<Exception>().IsNotNull().Message);
        });
    }

    [Theory]
    [InlineData("Test One")]
    [InlineData("Test Two")]
    public void Verify_Async_Act_Throws_Exception(string input)
    {
        ArrangeTest(options =>
        {
            ISampleChildA childA = Sub<ISampleChildA>(mock =>
            {
                mock.DoSomething(input).Throws(new Exception("Mock exception"));
            });
            return childA;
        })
        .ActThrowsException(async arrangement => await Task.Run(() => arrangement.Service.DoSomething(input)))
        .Assert(arrangement =>
        {
            Assert.Equal("Mock exception", arrangement.GetResult<Exception>().IsNotNull().Message);
        });
    }

    [Theory]
    [InlineData("Test One")]
    [InlineData("Test Two")]
    public void Verify_Async_Act_Throws_Specific_Exception(string input)
    {
        ArrangeTest(options =>
        {
            ISampleChildA childA = Sub<ISampleChildA>(mock =>
            {
                mock.DoSomething(input).Throws(new MockException("Mock exception"));
            });
            return childA;
        })
        .ActThrowsException<MockException>(async arrangement => await Task.Run(() => arrangement.Service.DoSomething(input)))
        .Assert(arrangement =>
        {
            Assert.Equal("Mock exception", arrangement.GetResult<Exception>().IsNotNull().Message);
        });
    }
}
