using FluentAssertions;
using Mayhap.Option;
using Xunit;

namespace Mayhap.Tests.Option
{
    public class OptionTests
    {
        [Fact]
        public void GivenValue_WhenSomeInvoked_ThenShouldReturnSomeOfValueType()
        {
            var x = 1;
            var some = x.Some();
            some.Value.Should().Be(x);
        }

        [Fact]
        public void GivenSomeValue_WhenImplicitlyCasted_ThenShouldReturnStoredValue()
        {
            var some = 1.Some();
            int actual = some;
            actual.Should().Be(1);
        }

        [Fact]
        public void GivenSomeValue_WhenUnwrapCalled_ThenShouldReturnStoredValue()
        {
            var some = 1.Some();
            var actual = some.Unwrap();
            actual.Should().Be(1);
        }

        [Fact]
        public void GivenSomeValue_WhenToStringCalled_ThenShouldContainSomeWordAndValueRepresentation()
        {
            var x = 1;
            var some = x.Some();

            some.ToString().Should().Contain("Some")
                .And.Contain(x.GetType().Name)
                .And.Contain(x.ToString());
        }

        [Fact]
        public void GivenNoneOfObject_WhenUnwrapCalled_ThenShouldReturnNull()
        {
            var none = Optional.None<object>();
            var unwrap = none.Unwrap();
            unwrap.Should().BeNull();
        }

        [Fact]
        public void GivenNoneOfInt_WhenImplicitlyCasted_ThenShouldReturnDefaultOfInt()
        {
            var none = Optional.None<int>();
            int unwrap = none;
            unwrap.Should().Be(default(int));
        }
    }
}