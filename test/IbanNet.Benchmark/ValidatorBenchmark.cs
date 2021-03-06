﻿using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
#if VALIDATOR_COMPARISONS
using SinKien.IBAN4Net;
#endif

namespace IbanNet.Benchmark
{
    [SimpleJob(RuntimeMoniker.HostProcess)]
    [MarkdownExporterAttribute.GitHub]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Alphabetical)]
    [MemoryDiagnoser]
    public class ValidatorBenchmark
    {
        private IIbanValidator _strictValidator, _looseValidator;
#if VALIDATOR_COMPARISONS
        private IbanValidation.IbanValidator _nugetIbanValidator;
#endif
        private IList<string> _testData;

        [Params(1, 10)]
        public int Count { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            // IbanNet setup
            _strictValidator = new IbanValidator();
            _looseValidator = new IbanValidator(new IbanValidatorOptions { Method = ValidationMethod.Loose });

#if VALIDATOR_COMPARISONS
            _nugetIbanValidator = new IbanValidation.IbanValidator();
#endif

            _testData = TestSamples.GetIbanSamples(Count);
        }

        [Benchmark(Baseline = true)]
        public void IbanNet_Strict()
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (int i = 0; i < _testData.Count; i++)
            {
                _strictValidator.Validate(_testData[i]);
            }
        }

        [Benchmark]
        public void IbanNet_Strict_CacheReuse()
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (int i = 0; i < _testData.Count; i++)
            {
                // Validate same IBAN to hit structure validator cache.
                _strictValidator.Validate(_testData[0]);
            }
        }

        [Benchmark]
        public void IbanNet_Loose()
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (int i = 0; i < _testData.Count; i++)
            {
                _looseValidator.Validate(_testData[i]);
            }
        }

#if VALIDATOR_COMPARISONS
        [Benchmark]
        public void NuGet_IbanValidator()
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (int i = 0; i < _testData.Count; i++)
            {
                _nugetIbanValidator.Validate(_testData[i]);
            }
        }

        [Benchmark]
        public void NuGet_IBAN4NET()
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (int i = 0; i < _testData.Count; i++)
            {
                IbanUtils.IsValid(_testData[i], out IbanFormatViolation _);
            }
        }
#endif
    }
}
