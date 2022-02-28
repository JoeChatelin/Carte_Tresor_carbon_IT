using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreasureHunt {
    public class Adventurer {
        public string Name { get; set;}
        public Position Coords { get; set;}
        public Orientation Orientation { get; set; }
        public Queue<char> Instructions { get; set; }
        public int CollectedTreasures { get; set; }

        public static Orientation ConvertOrientationFormat(char orientation) {
            switch(orientation) {
                case 'N':
                    return Orientation.North;
                case 'S':
                    return Orientation.South;
                case 'E':
                    return Orientation.East;
                case 'O':
                    return Orientation.West;
                default: throw new NotSupportedException("The orientation is not supported");
            }
        }

        public static char ConvertOrientationToFileFormat(Orientation orientation) {
            switch(orientation) {
                case Orientation.North:
                    return 'N';
                case Orientation.South:
                    return 'S';
                case Orientation.East:
                    return 'E';
                case Orientation.West:
                    return 'O';
                default: throw new NotSupportedException("The orientation is not supported");
            }
        }

        public static Direction ConvertInstructionFormat(char direction) {
            switch(direction) {
                case 'D':
                    return Direction.Right;
                case 'G':
                    return Direction.Left;
                case 'A':
                    return Direction.Forward;
                default: throw new NotSupportedException("The direction is not supported");
            }
        }

        public void ChangeOrientation(Orientation orientation, Direction direction) {
            switch(orientation) {
                case Orientation.North : 
                    if (direction == Direction.Right) {
                        this.Orientation = Orientation.East;
                    } else if (direction == Direction.Left) {
                        this.Orientation = Orientation.West;
                    } else {
                        throw new NotSupportedException("The direction is not supported");
                    }
                    break;
                case Orientation.South : 
                    if (direction == Direction.Right) {
                        this.Orientation = Orientation.West;
                    } else if (direction == Direction.Left) {
                        this.Orientation = Orientation.East;
                    } else {
                        throw new NotSupportedException("The direction is not supported");
                    }
                    break;
                case Orientation.West : 
                    if (direction == Direction.Right) {
                        this.Orientation = Orientation.North;
                    } else if (direction == Direction.Left) {
                        this.Orientation =  Orientation.South;
                    } else {
                        throw new NotSupportedException("The direction is not supported");
                    }
                    break;
                case Orientation.East : 
                    if (direction == Direction.Right) {
                        this.Orientation = Orientation.South;
                    } else if (direction == Direction.Left) {
                        this.Orientation = Orientation.North;
                    } else {
                        throw new NotSupportedException("The direction is not supported");
                    }
                    break;
                default: throw new NotSupportedException("The orientation is not supported");
            }
        }

        public void MoveForward(Map map, Orientation orientation, Position currentPosition) {
            Position borders = map.Borders;
            IDictionary<Position, int> treasures = map.Treasures;
            IList<Position> mountains = map.Montains;
            IList<Adventurer> adventurers = map.Adventurers;
            Position nextPosition = null;
            int nextCoordX, nextCoordY;

            switch(orientation) {
                case Orientation.North :
                    nextCoordY = this.Coords.Y - 1;
                    nextPosition = new Position(this.Coords.X, nextCoordY);
                    if (nextCoordY >= 0 && !IsCollidingWithMountains(mountains, nextPosition) && !IsCollidingWithAdventurers(adventurers, nextPosition)) {
                        this.UpdateCoords(treasures, nextPosition);
                    }
                    break;
                case Orientation.South : 
                    nextCoordY = this.Coords.Y + 1;
                    nextPosition = new Position(this.Coords.X, nextCoordY);
                    if (nextCoordY <= borders.Y && !IsCollidingWithMountains(mountains, nextPosition) && !IsCollidingWithAdventurers(adventurers, nextPosition)) {
                        this.UpdateCoords(treasures, nextPosition);
                    }
                    break;
                case Orientation.West : 
                    nextCoordX = this.Coords.X - 1;
                    nextPosition = new Position(nextCoordX, this.Coords.Y);
                    if (nextCoordX >= 0 && !IsCollidingWithMountains(mountains, nextPosition) && !IsCollidingWithAdventurers(adventurers, nextPosition)) {
                        this.UpdateCoords(treasures, nextPosition);
                    }
                    break;
                case Orientation.East : 
                    nextCoordX = this.Coords.X + 1;
                    nextPosition = new Position(nextCoordX, this.Coords.Y);
                    if (nextCoordX <= borders.X && !IsCollidingWithMountains(mountains, nextPosition) && !IsCollidingWithAdventurers(adventurers, nextPosition)) {
                        this.UpdateCoords(treasures, nextPosition);
                    }
                    break;
                default: throw new NotSupportedException("The orientation is not supported");
            }
        }

        public static bool IsCollidingWithAdventurers(IList<Adventurer> adventurers, Position nextCoords) {
            var result = adventurers.Where(adventurer => (adventurer.Coords.X == nextCoords.X && adventurer.Coords.Y == nextCoords.Y)).ToList();
            return result.Count > 0;
        }

        public static bool IsCollidingWithMountains(IList<Position> mountains, Position nextCoords) {
            var result = mountains.Where(mountain => (mountain.X == nextCoords.X && mountain.Y == nextCoords.Y)).ToList();
            return result.Count > 0;
        }

        public void UpdateCoords(IDictionary<Position, int> treasures, Position nextPosition) {
            this.Coords = nextPosition;
            this.CollectTreasures(treasures, this.Coords);
        }

        public void CollectTreasures(IDictionary<Position, int> treasures, Position currentPosition) {
            try {
                IList<Position> treasuresPosition = treasures.Keys.ToList();
                Position adventurerPosition = treasuresPosition.Where(position => (position.X == currentPosition.X && position.Y == currentPosition.Y)).FirstOrDefault();
                if (adventurerPosition != null && treasures[adventurerPosition] > 0) {
                    treasures[adventurerPosition] -= 1;
                    this.CollectedTreasures += 1;
                }
            } catch(Exception ex) {
                throw ex;
            }
        }

        public string stringFormat() {
            string info = string.Format("A - {0} - {1} - {2} - {3} - {4}", 
            this.Name, this.Coords.X, this.Coords.Y, ConvertOrientationToFileFormat(this.Orientation), this.CollectedTreasures);
            return info;
        }
    }
}