using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace TreasureHunt.Tests
{
    public class ParserTests
    {
        private static readonly object[] _mountainsData = {
            new object[] { new List<string>{"M - 1 - 0", "M - 20 - 15"}}
        };

        private static readonly object[] _treasuresData = {
            new object[] { new List<string>{"T - 0 - 3 - 2", "T - 1 - 3 - 3"}}
        };

        private static readonly object[] _adventurersData = {
            new object[] { new List<string>{"A - Lara - 1 - 1 - S - AADADAGGA"}}
        };

        private static readonly object[] _mapData = {
            new object[] {"C - 35 - 40"}
        };

        [SetUp]
        public void Setup()
        {  
        }
 
        [TestCaseSource(nameof(_mapData))]
        public void GetBorders_Should_Return_a_Tuple_with_value_35_and_40(string value)
        {
            var result = Parser.GetMapBorders(value);
            Assert.IsInstanceOf<Position>(result);
            Assert.AreEqual(result.X, 35);
            Assert.AreEqual(result.Y, 40);
        }

        [TestCaseSource(nameof(_mountainsData))]
        public void GetMountains_Should_Return_a_List_of_Tuple_containing_2_elements(List<string> value)
        {
            var result = Parser.GetMountains(value);
            Assert.IsInstanceOf<IList<Position>>(result);
            Assert.AreEqual(result.Count, 2);

            Assert.AreEqual(result.First().X, 1);
            Assert.AreEqual(result.First().Y, 0);
        }

        [TestCaseSource(nameof(_treasuresData))]
        public void GetTreasures_Should_Return_a_dictionary_of_keys_tuple_and_values_int_containing_2_elements(List<string> value)
        {
            var result = Parser.GetTreasures(value);
            Assert.IsInstanceOf<IDictionary<Position, int>>(result);
            Assert.AreEqual(result.Count, 2);

            Assert.AreEqual(result.First().Key.X, 0);
            Assert.AreEqual(result.First().Key.Y, 3);
            Assert.AreEqual(result.First().Value, 2);
        }

        [TestCaseSource(nameof(_adventurersData))]
        public void GetAdventurer_Should_Return_a_List_of_Adventurers_containing_1_element(List<string> value)
        {
            var result = Parser.GetAdventurers(value);
            Assert.IsInstanceOf<IList<Adventurer>>(result);
            Assert.AreEqual(result.Count, 1);

            Assert.AreEqual(result.First().Name, "Lara");
            Assert.AreEqual(result.First().Coords.X, 1);
            Assert.AreEqual(result.First().Coords.Y, 1);
            Assert.AreEqual(result.First().Orientation, Orientation.South);
        }
    }
}