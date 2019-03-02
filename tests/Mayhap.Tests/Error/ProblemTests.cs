using System;
using System.Collections.Generic;
using FluentAssertions;
using Mayhap.Error;
using Xunit;

namespace Mayhap.Tests.Error
{
    public class ProblemTests
    {
        [Theory]
        [InlineData(TestProblemType.NoAttributes, "NoAttributes")]
        [InlineData(TestProblemType.WithTypeAttribute, "Type attr val")]
        [InlineData(TestProblemType.WithAllAttributes, "Type attr val")]
        public void GivenProblemType_WhenProblemOfTypeCreated_ThenShouldContainType(TestProblemType type, string expectedType)
        {
            // act
            var actual = Problem.OfType(type).Create();

            // assert
            actual.Type.Should().Be(expectedType);
        }

        [Theory]
        [InlineData(TestProblemType.NoAttributes, null)]
        [InlineData(TestProblemType.WithTitleAttribute, "Title attr val")]
        [InlineData(TestProblemType.WithAllAttributes, "Title attr val")]
        public void GivenProblemType_WhenProblemOfTypeCreated_ThenShouldContainTitle(TestProblemType type, string expectedTitle)
        {
            // act
            var actual = Problem.OfType(type).Create();

            // assert
            actual.Title.Should().Be(expectedTitle);
        }

        [Theory]
        [InlineData(TestProblemType.NoAttributes, null)]
        [InlineData(TestProblemType.WithDetailAttribute, "Detail attr val")]
        [InlineData(TestProblemType.WithAllAttributes, "Detail attr val")]
        public void GivenProblemType_WhenProblemOfTypeCreated_ThenShouldContainDetail(TestProblemType type, string expectedDetail)
        {
            // act
            var actual = Problem.OfType(type).Create();

            // assert
            actual.Detail.Should().Be(expectedDetail);
        }

        [Theory]
        [InlineData(TestProblemType.NoAttributes, null)]
        [InlineData(TestProblemType.WithInstanceAttribute, "Instance attr val")]
        [InlineData(TestProblemType.WithAllAttributes, "Instance attr val")]
        public void GivenProblemType_WhenProblemOfTypeCreated_ThenShouldContainInstance(TestProblemType type, string expectedInstance)
        {
            // act
            var actual = Problem.OfType(type).Create();

            // assert
            actual.Instance.Should().Be(expectedInstance);
        }

        [Theory]
        [InlineData(TestProblemType.NoAttributes, null)]
        [InlineData(TestProblemType.WithStatusAttribute, 100)]
        [InlineData(TestProblemType.WithAllAttributes, 100)]
        public void GivenProblemType_WhenProblemOfTypeCreated_ThenShouldContainStatus(TestProblemType type, int? expectedStatus)
        {
            // act
            var actual = Problem.OfType(type).Create();

            // assert
            actual.Status.Should().Be(expectedStatus);
        }

        [Theory]
        [MemberData(nameof(ProblemPropertyTestCases))]
        public void GivenProblemType_WhenProblemOfTypeCreated_ThenShouldContainProperties(
            TestProblemType type,
            IDictionary<string, object> expectedProperties)
        {
            // act
            var actual = Problem.OfType(type).Create();

            // assert
            actual.Properties.Should().BeEquivalentTo(expectedProperties);
        }

        public static readonly IEnumerable<object[]> ProblemPropertyTestCases =
            new[]
            {
                new object[]
                {
                    TestProblemType.NoAttributes,
                    null
                },
                new object[]
                {
                    TestProblemType.WithSinglePropertyAttribute,
                    new Dictionary<string, object>
                    {
                        ["prop1"] = "Property attr val 1"
                    }
                },
                new object[]
                {
                    TestProblemType.WithTwoPropertyAttributes,
                    new Dictionary<string, object>
                    {
                        ["prop1"] = "Property attr val 1",
                        ["prop2"] = "Property attr val 2"
                    }
                },
                new object[]
                {
                    TestProblemType.WithAllAttributes,
                    new Dictionary<string, object>
                    {
                        ["prop1"] = "Property attr val 1",
                        ["prop2"] = "Property attr val 2",
                    }
                }
            };

        [Theory]
        [MemberData(nameof(ProblemCustomizationTestCases))]
        public void GivenParameterlessProblemCreation_WhenProblemCreated_ThenShouldContainData(
            Func<ProblemBuilder, Problem> factory,
            Action<Problem> assert)
        {
            // arrange
            var builder = Problem.New();

            // act
            var actual = factory.Invoke(builder);

            // assert
            assert.Invoke(actual);
        }

        public static readonly IEnumerable<object[]> ProblemCustomizationTestCases =
            new[]
            {
                new object[]
                {
                    (Func<ProblemBuilder, Problem>)(b => b.WithTitle("title val").Create()),
                    (Action<Problem>)(p => p.Title.Should().Be("title val"))
                },
                new object[]
                {
                    (Func<ProblemBuilder, Problem>)(b => b.WithType("type val").Create()),
                    (Action<Problem>)(p => p.Type.Should().Be("type val"))
                },
                new object[]
                {
                    (Func<ProblemBuilder, Problem>)(b => b.WithDetail("detail val").Create()),
                    (Action<Problem>)(p => p.Detail.Should().Be("detail val"))
                },
                new object[]
                {
                    (Func<ProblemBuilder, Problem>)(b => b.WithInstance("instance val").Create()),
                    (Action<Problem>)(p => p.Instance.Should().Be("instance val"))
                },
                new object[]
                {
                    (Func<ProblemBuilder, Problem>)(b => b.WithStatus(100).Create()),
                    (Action<Problem>)(p => p.Status.Should().Be(100))
                },
                new object[]
                {
                    (Func<ProblemBuilder, Problem>)(b => b.WithProperties(new Dictionary<string, object>
                    {
                        ["prop1"] = "Property attr val 1",
                        ["prop2"] = "Property attr val 2"
                    }).Create()),
                    (Action<Problem>)(p => p.Properties.Should().BeEquivalentTo(new Dictionary<string, object>
                    {
                        ["prop1"] = "Property attr val 1",
                        ["prop2"] = "Property attr val 2"
                    }))
                },
            };

        public enum TestProblemType
        {
            NoAttributes,

            [ProblemType("Type attr val")]
            WithTypeAttribute,

            [ProblemTitle("Title attr val")]
            WithTitleAttribute,

            [ProblemDetail("Detail attr val")]
            WithDetailAttribute,

            [ProblemInstance("Instance attr val")]
            WithInstanceAttribute,

            [ProblemStatus(100)]
            WithStatusAttribute,

            [ProblemProperty("prop1", "Property attr val 1")]
            WithSinglePropertyAttribute,

            [ProblemProperty("prop1", "Property attr val 1")]
            [ProblemProperty("prop2", "Property attr val 2")]
            WithTwoPropertyAttributes,

            [ProblemDetail("Detail attr val")]
            [ProblemType("Type attr val")]
            [ProblemTitle("Title attr val")]
            [ProblemInstance("Instance attr val")]
            [ProblemStatus(100)]
            [ProblemProperty("prop1", "Property attr val 1")]
            [ProblemProperty("prop2", "Property attr val 2")]
            WithAllAttributes,
        }
    }
}