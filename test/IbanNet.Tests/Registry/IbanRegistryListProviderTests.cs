﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace IbanNet.Registry
{
    public class IbanRegistryListProviderTests
    {
        [Fact]
        public void Given_list_of_countries_when_creating_it_should_wrap_as_provider()
        {
            var ibanCountries = new List<IbanCountry>
            {
                new IbanCountry("AA"),
                new IbanCountry("BB")
            };
            var sut = new IbanRegistryListProvider(ibanCountries);

            sut.Count.Should().Be(ibanCountries.Count);
            sut.Should()
                .NotBeEmpty()
                .And.Subject.Select(c => c.TwoLetterISORegionName)
                .Should()
                .BeEquivalentTo("AA", "BB");
        }
    }
}
