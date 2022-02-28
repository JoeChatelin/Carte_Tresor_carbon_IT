using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;


namespace TreasureHunt {
    public static class Parser {
        public static IList<String> ReadFile(string filePath) {
            List<String> fileElements = new List<String>();
            
            if (File.Exists(filePath))
            {
                using (StreamReader streamReader = File.OpenText(filePath))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        fileElements.Add(line);
                    }
                }
            }
            return fileElements;
        }

        public static Position GetMapBorders(string mapInfo) {
            try {
                mapInfo = mapInfo.Trim();
                if(!mapInfo.StartsWith("C") && mapInfo.Where(e => (e == '-')).Count() != 2) {
                    throw new FormatException("Wrong map informations format");
                }
                string[] splitInfo = mapInfo.Replace(" ", "").Remove(0, 2).Split("-");
                int borderX, borderY;
                Position borders;
                if(Int32.TryParse(splitInfo[0], out borderX) && Int32.TryParse(splitInfo[1], out borderY)) {
                    borders = new Position(borderX, borderY);
                } else {
                    throw new FormatException("Wrong map informations format");
                }
                return borders;
            } catch(Exception ex) {
                throw ex;
            } 
        }
        
        public static IList<Position> GetMountains(IList<String> mountainsInfos) {
            try {
                IList<Position>   mountainCoords = new List<Position>();
                foreach(var mountain in mountainsInfos) {
                    string[] mountainInfo = mountain.Replace(" ", "").Remove(0, 2).Split("-");
                    int coordX, coordY;
                    Position coords;
                    if(Int32.TryParse(mountainInfo[0], out coordX) && Int32.TryParse(mountainInfo[1], out coordY)) {
                        coords = new Position(coordX, coordY);
                        mountainCoords.Add(coords);
                    } else {
                        throw new FormatException("Wrong mountains informations format");
                    }
                }
                return mountainCoords;
            } catch(Exception ex) {
                throw ex;
            } 
        }

        public static IDictionary<Position, int> GetTreasures(IList<String> treasuresInfos) {
            try {
                IDictionary<Position, int> treasures = new Dictionary<Position, int>();
                foreach(var info in treasuresInfos) {
                    string[] treasureInfo = info.Replace(" ", "").Remove(0, 2).Split("-");
                    int coordX, coordY, numberOfTreasure;
                    if(Int32.TryParse(treasureInfo[0], out coordX) && Int32.TryParse(treasureInfo[1], out coordY) && Int32.TryParse(treasureInfo[2], out numberOfTreasure)) {
                        Position coords = new Position(coordX, coordY);
                        treasures[coords] = numberOfTreasure;
                    } else {
                        throw new FormatException("Wrong treasures informations format");
                    }
                }
                return treasures;
            } catch(Exception ex) {
                throw ex;
            } 
        }

        public static IList<Adventurer> GetAdventurers(IList<String> adventurersInfos) {
            try {
                IList<Adventurer> adventurers = new List<Adventurer>();
                foreach(var info in adventurersInfos) {
                    string[] adventurerInfo = info.Replace(" ", "").Remove(0, 2).Split("-");
                    int coordX, coordY;
                    char orientation;
                    Adventurer newAdventurer = new Adventurer();
                    newAdventurer.Name = adventurerInfo[0];
                    newAdventurer.Instructions = new Queue<char>(adventurerInfo[4]);
                    newAdventurer.CollectedTreasures = 0;
                    if(Int32.TryParse(adventurerInfo[1], out coordX) && Int32.TryParse(adventurerInfo[2], out coordY) && Char.TryParse(adventurerInfo[3], out orientation)) {
                        Position coords = new Position(coordX, coordY);
                        newAdventurer.Coords = coords;
                        newAdventurer.Orientation = Adventurer.ConvertOrientationFormat(orientation);
                    } else {
                        throw new FormatException("Wrong Adventurers informations format");
                    }
                    adventurers.Add(newAdventurer);
                }
                return adventurers;
            } catch(Exception ex) {
                throw ex;
            } 
        }
    }
}