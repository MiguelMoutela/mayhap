using System.Collections.Generic;
using FluentAssertions;
using Mayhap.Error;
using Mayhap.Maybe;
using Mayhap.Option;
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
            maybe.IsSuccessful.Should().BeTrue();
            maybe.Value.Unwrap().Should().Be(value);
        }

        [Fact]
        public void GivenErrorCode_WhenFailExtensionMethodCalled_ThenShouldReturnFailedMaybeWithTheErrorCode()
        {
            // given
            string fault = "fault.code.500";

            //when
            var maybe = fault.Fail<object>();

            // then
            maybe.IsSuccessful.Should().BeFalse();
            maybe.Error.Should().BeOfType<Some<IProblem>>();
            var problem = (Problem) maybe.Error.Unwrap();
            problem.Type.Should().Be(fault);
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
            actual.Should().Be(maybe.Value.Unwrap());
        }
    }
}