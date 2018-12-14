using FluentAssertions;
using Xunit;

namespace Mayhap.Tests
{
    public class EmptyTests
    {
        [Fact]
        public void WhenSuccessMethodCalled_ThenShouldReturnSuccessfulMaybe()
        {
            // when
            var maybe = Empty.Success();

            // then
            maybe.IsSuccess.Should().Be(true);
        }
    }
}