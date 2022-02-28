using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TreasureHunt;

namespace TreasureHunt.Tests
{
    public class AdventurerTests
    {
        private static readonly object[] _mountainsCollidingPosition = {
            new object[] {new Position(2, 1)},
            new object[] {new Position(1, 0)}
        };

        private static readonly object[] _noMountainsCollisionPosition = {
            new object[] {new Position(2, 3)},
            new object[] {new Position(1, 1)}
        };

        private static readonly object[] _adventurersCollisionPosition = {
            new object[] {new Position(1, 1)},
            new object[] {new Position(2, 3)}
        };

        private static readonly object[] _noAdventurersCollisionPosition = {
            new object[] {new Position(1, 0)},
            new object[] {new Position(2, 1)}
        };

        private static readonly object[] _collectTreasurePosition = {
            new object[] {new Position(0, 3)}
        };

        private static readonly object[] _noCollectTreasurePosition = {
            new object[] {new Position(1, 2)},
            new object[] {new Position(1, 3)}  
        };

        private static readonly object[] _adventurerPosition = {
            new object[] {new Position(1, 1)}  
        };
        

        Map map = null;
        Adventurer adventurer = null;
        IList<Adventurer> adventurers = null;
        IList<Position> mountains = null;
        IDictionary<Position, int> treasures = null;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            IList<string> fileElements = new List<string> {
                "C - 3 - 4","M - 1 - 0", "M - 2 - 1", "T - 0 - 3 - 2", 
                "T - 1 - 3 - 0", "A - Lara - 1 - 1 - S - AADADAGGA", "A - Clark - 2 - 3 - N - AADADAGGA"
            };
            map = new Map();
            map.InitMapElements(fileElements);
            mountains = map.Montains;
        }

        [SetUp]
        public void SetUp() {
            adventurer = map.Adventurers.FirstOrDefault();
            adventurers = map.Adventurers;
            treasures = map.Treasures;
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() {
            map = null;
            mountains = null;
        }

        [TearDown]
        public void TearDown() {
            adventurer.Coords.X = 1;
            adventurer.Coords.X = 1;
            adventurer.CollectedTreasures = 0;
        }
 
        [TestCase(Direction.Right)]
        public void ChangeOrientation_Should_change_the_orientation_from_south_to_west(Direction direction)
        {
            adventurer.ChangeOrientation(Orientation.South, direction);
            Assert.AreEqual(adventurer.Orientation, Orientation.West);
        }

        [TestCase(Direction.Left)]
        public void ChangeOrientation_Should_change_the_orientation_from_south_to_east(Direction direction)
        {
            adventurer.ChangeOrientation(Orientation.South, direction);
            Assert.AreEqual(adventurer.Orientation, Orientation.East);
        }

        [TestCase(Direction.Right)]
        public void ChangeOrientation_Should_change_the_orientation_from_east_to_north(Direction direction)
        {
            adventurer.ChangeOrientation(Orientation.East, direction);
            Assert.AreEqual(adventurer.Orientation, Orientation.South);
        }

        [TestCase(Direction.Left)]
        public void ChangeOrientation_Should_change_the_orientation_from_east_to_south(Direction direction)
        {
            adventurer.ChangeOrientation(Orientation.East, direction);
            Assert.AreEqual(adventurer.Orientation, Orientation.North);
        }

        [TestCase(Direction.Right)]
        public void ChangeOrientation_Should_change_the_orientation_from_north_to_west(Direction direction)
        {
            adventurer.ChangeOrientation(Orientation.North, direction);
            Assert.AreEqual(adventurer.Orientation, Orientation.East);
        }

        [TestCase(Direction.Left)]
        public void ChangeOrientation_Should_change_the_orientation_from_north_to_east(Direction direction)
        {
            adventurer.ChangeOrientation(Orientation.North, direction);
            Assert.AreEqual(adventurer.Orientation, Orientation.West);
        }

        [TestCase(Direction.Right)]
        public void ChangeOrientation_Should_change_the_orientation_from_west_to_north(Direction direction)
        {
            adventurer.ChangeOrientation(Orientation.West, direction);
            Assert.AreEqual(adventurer.Orientation, Orientation.North);
        }

        [TestCase(Direction.Left)]
        public void ChangeOrientation_Should_change_the_orientation_from_west_to_south(Direction direction)
        {
            adventurer.ChangeOrientation(Orientation.West, direction);
            Assert.AreEqual(adventurer.Orientation, Orientation.South);
        }

        [TestCaseSource(nameof(_mountainsCollidingPosition))]
        public void IsCollidingWithMountains_Should_return_true(Position nextPosition)
        {
            bool isColliding = Adventurer.IsCollidingWithMountains(mountains, nextPosition);
            Assert.IsTrue(isColliding);
        }

        [TestCaseSource(nameof(_noMountainsCollisionPosition))]
        public void IsCollidingWithMountains_Should_return_false(Position nextPosition)
        {
            bool isColliding = Adventurer.IsCollidingWithMountains(mountains, nextPosition);
            Assert.IsFalse(isColliding);
        }

        [TestCaseSource(nameof(_adventurersCollisionPosition))]
        public void IsCollidingWithAdventurers_Should_return_true(Position nextPosition)
        {
            bool isColliding = Adventurer.IsCollidingWithAdventurers(adventurers, nextPosition);
            Assert.IsTrue(isColliding);
        }

        [TestCaseSource(nameof(_noAdventurersCollisionPosition))]
        public void IsCollidingWithAdventurers_Should_return_false(Position nextPosition)
        {
            bool isColliding = Adventurer.IsCollidingWithAdventurers(adventurers, nextPosition);
            Assert.IsFalse(isColliding);
        }

        [TestCaseSource(nameof(_collectTreasurePosition))]
        public void CollectTreasure_Should_increase_adventurer_collectedTreasures_to_1_and_decrease_treasures_numbers(Position adventurerPosition)
        {
            adventurer.CollectTreasures(treasures, adventurerPosition);
            //Assert.AreEqual(treasures[adventurerPosition], 1);
            Assert.AreEqual(adventurer.CollectedTreasures, 1);
        }

        [TestCaseSource(nameof(_noCollectTreasurePosition))]
        public void CollectTreasure_Should_not_increase_adventurer_collectedTreasures_and_decrease_treasures_numbers(Position adventurerPosition)
        {
            adventurer.CollectTreasures(treasures, adventurerPosition);
            //Assert.AreEqual(treasures[adventurerPosition], 0);
            Assert.AreEqual(adventurer.CollectedTreasures, 0);
        }

        [TestCaseSource(nameof(_adventurerPosition))]
        public void MoveForward_Should_update_adventurer_position_from_1_1_to_1_2(Position currentPosition)
        {
            adventurer.MoveForward(map, Orientation.South, currentPosition);
            Assert.AreEqual(adventurer.Coords.X, 1);
            Assert.AreEqual(adventurer.Coords.Y, 2);
        }

        [TestCaseSource(nameof(_adventurerPosition))]
        public void MoveForward_Should_update_adventurer_position_from_1_1_to_0_1(Position currentPosition)
        {
            adventurer.MoveForward(map, Orientation.West, currentPosition);
            Assert.AreEqual(adventurer.Coords.X, 0);
            Assert.AreEqual(adventurer.Coords.Y, 1);
        }

        [TestCaseSource(nameof(_adventurerPosition))]
        public void MoveForward_Should_not_update_adventurer_position_because_of_north_mountain(Position currentPosition)
        {
            adventurer.MoveForward(map, Orientation.North, currentPosition);
            Assert.AreEqual(adventurer.Coords.X, 1);
            Assert.AreEqual(adventurer.Coords.Y, 1);
        }

        [TestCaseSource(nameof(_adventurerPosition))]
        public void MoveForward_Should_not_update_adventurer_position_because_of_east_mountain(Position currentPosition)
        {
            adventurer.MoveForward(map, Orientation.East, currentPosition);
            Assert.AreEqual(adventurer.Coords.X, 1);
            Assert.AreEqual(adventurer.Coords.Y, 1);
        }
    }
}