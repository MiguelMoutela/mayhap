using FluentAssertions;
using Mayhap.Maybe;
using Xunit;

namespace Mayhap.Tests.Maybe
{
    public class EmptyTests
    {
        [Fact]
        public void WhenSuccessMethodCalled_ThenShouldReturnSuccessfulMaybe()
        {
            // when
            var maybe = Empty.Success();

            // then
            maybe.IsSuccessful.Should().Be(true);
        }
    }
}