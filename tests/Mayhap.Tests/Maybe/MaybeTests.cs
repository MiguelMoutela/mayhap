using System.Collections.Generic;
using FluentAssertions;
using Mayhap.Error;
using Mayhap.Maybe;
using Xunit;

namespace Mayhap.Tests.Maybe
{
    public class MaybeTests
    {
        [Fact]
        public void GivenValue_WhenCalledSuccessExtensionMethod_ThenShouldReturnSuccessfulMaybeWithTheValue()
        {
            // given
            int value = 150;

            // when
            var maybe = value.Success();

            // then
            maybe.IsSuccess.Should().BeTrue();
            maybe.Value.Should().Be(value);
        }

        [Fact]
        public void GivenErrorCode_WhenFailExtensionMethodCalled_ThenShouldReturnFailedMaybeWithTheErrorCode()
        {
            // given
            string fault = "fault.code.500";

            //when
            var maybe = fault.Fail<object>();

            // then
            maybe.IsSuccess.Should().BeFalse();
            maybe.Error.Should().BeOfType<Problem>();
            ((Problem) maybe.Error).Type.Should().Be(fault);
        }

        public static IEnumerable<object[]> ImplicitBoolTheory => new[]
        {
            new object[] { new object().Success(), true }, 
            new object[] { "fault".Fail<object>(), false }, 
        };

        [Theory]
        [MemberData(nameof(ImplicitBoolTheory))]
        public void GivenMaybe_WhenImplicitlyConvertedToBoolean_ThenShouldReturn(Maybe<object> maybe, bool expected)
        {
            // when
            bool actual = maybe;

            // then
            actual.Should().Be(expected);
        }

        [Fact]
        public void GivenMaybe_WhenAssignedToAVariableOfWrappedType_ThenShouldAssignItsValue()
        {
            // given
            var maybe = 100.Success();

            // when
            int actual = maybe;

            //then
            actual.Should().Be(maybe.Value);
        }

        public static IEnumerable<object[]> ConvertToTheory => new[]
        {
            new object[] { 100.Success(), "new val", "new val", true }, 
            new object[] { "fault".Fail<int>(), "new val", null, false },
        };

        [Theory]
        [MemberData(nameof(ConvertToTheory))]
        public void GivenMaybe_WhenCalledToMethod_ThenShouldCreateNewMaybeWithProperties(
            Maybe<int> maybe, string newValue, string expectedValue, bool expectedSuccess)
        {
            // when
            var newMaybe = maybe.To(newValue);

            //then
            newMaybe.Value.Should().Be(expectedValue);
            newMaybe.IsSuccess.Should().Be(expectedSuccess);
        }
    }
}