using Microsoft.VisualStudio.TestTools.UnitTesting;
using SingleResponsibilityPrinciple;
using SingleResponsibilityPrinciple.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace SingleResponsibilityPrinciple.Tests
{
    [TestClass]
    public class SimpleTradeParserTests
    {
        // Mock positive validator: always returns true
        public class dummyPositiveTradeValidator : ITradeValidator
        {
            public bool Validate(string[] tradeData)
            {
                return true;
            }
        }

        // Mock negative validator: always returns false
        public class dummyNegativeTradeValidator : ITradeValidator
        {
            public bool Validate(string[] tradeData)
            {
                return false;
            }
        }

        // Mock mapper: returns a sample TradeRecord with dummy values
        public class dummyTradeMapper : ITradeMapper
        {
            public TradeRecord Map(string[] fields)
            {
                TradeRecord sampleRec = new TradeRecord
                {
                    DestinationCurrency = "XXX",
                    SourceCurrency = "YYY",
                    Price = 1.11M,
                    Lots = 2.22F
                };
                return sampleRec;
            }
        }

        [TestMethod]
        public void TestNumberOfPosLines()
        {
            // Arrange
            var validator = new dummyPositiveTradeValidator();
            var mapper = new dummyTradeMapper();
            var parser = new SimpleTradeParser(validator, mapper);
            List<string> sampleInput = new List<string> { "XXXYYY,1111,9.99", "XXXYYY,2222,9.99", "XXXYYY,3333,9.99" };

            // Act
            IEnumerable<TradeRecord> result = parser.Parse(sampleInput);

            // Assert
            Assert.AreEqual(sampleInput.Count, result.Count(), "The count of parsed trades should match the input count.");
        }

        [TestMethod]
        public void TestNumberOfNegLines()
        {
            // Arrange
            var validator = new dummyNegativeTradeValidator();
            var mapper = new dummyTradeMapper();
            var parser = new SimpleTradeParser(validator, mapper);
            List<string> sampleInput = new List<string> { "XXXYYY,1111,9.99", "XXXYYY,2222,9.99", "XXXYYY,3333,9.99" };

            // Act
            IEnumerable<TradeRecord> result = parser.Parse(sampleInput);

            // Assert
            Assert.AreEqual(0, result.Count(), "The count of parsed trades should be zero when validation fails.");
        }
    }
}
