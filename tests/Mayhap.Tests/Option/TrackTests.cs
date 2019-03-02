using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Mayhap.Option;
using Xunit;

namespace Mayhap.Tests.Option
{
    public class TrackTests
    {
        public static readonly IEnumerable<object[]> MappingTheory =
            new[]
            {
                new object[] { 100.Some(), Optional.None<string>(), Optional.None<string>() },
                new object[] { Optional.None<int>(), "abc".Some(), Optional.None<string>() },
                new object[] { 100.Some(), "abc".Some(), "abc".Some() },
            };

        [Theory]
        [MemberData(nameof(MappingTheory))]
        public void GivenOption_WhenMapped_ThenShouldReturnAsExpected(
            IOption<int> prevOption,
            IOption<string> mappingResult,
            IOption<string> expected)
        {
            IOption<string> MapFunctor(int _) => mappingResult;

            var actual = prevOption.Map(MapFunctor);

            actual.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(MappingTheory))]
        public async Task GivenOption_WhenMappedAsync_ThenShouldReturnAsExpected(
            IOption<int> prevOption,
            IOption<string> mappingResult,
            IOption<string> expected)
        {
            Task<IOption<string>> MapFunctor(int _) => Task.FromResult(mappingResult);

            var actual = await prevOption.Map(MapFunctor);

            actual.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(MappingTheory))]
        public void GivenOption_WhenMapped_ThenShouldOutAsExpected(
            IOption<int> prevOption,
            IOption<string> mappingResult,
            IOption<string> expected)
        {
            IOption<string> MapFunctor(int _) => mappingResult;

            prevOption.Map(MapFunctor, out var actual);

            actual.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(MappingTheory))]
        public async Task GivenOption_WhenMappedAsync_ThenShouldOutAsExpected(
            IOption<int> prevOption,
            IOption<string> mappingResult,
            IOption<string> expected)
        {
            Task<IOption<string>> MapFunctor(int _) => Task.FromResult(mappingResult);

            await prevOption.Map(MapFunctor, out var nextTask);
            var actual = await nextTask;

            actual.Should().Be(expected);
        }
    }
}