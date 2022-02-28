using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TreasureHunt {
    public class Map {
        public Position Borders { get; set; } 
        public IList<Position> Montains { get; set; }
        public IDictionary<Position, int> Treasures { get; set; }
        public IList<Adventurer> Adventurers { get; set; }
        private StringBuilder builder;

        public void InitMapElements(IList<String> fileElements) {
            builder = new StringBuilder();
            string bordersInfos = fileElements.FirstOrDefault(element => element.StartsWith('C'));
            IList<String> mountainsInfos = fileElements.Where(element => element.StartsWith('M')).ToList();
            IList<String> treasuresInfos = fileElements.Where(element => element.StartsWith('T')).ToList();
            IList<String> adventurersInfos = fileElements.Where(element => element.StartsWith('A')).ToList();
            
            this.Borders = Parser.GetMapBorders(bordersInfos);
            this.Montains = Parser.GetMountains(mountainsInfos);
            this.Treasures = Parser.GetTreasures(treasuresInfos);
            this.Adventurers = Parser.GetAdventurers(adventurersInfos);

            builder.AppendLine(bordersInfos);
            builder.AppendLine(string.Join('\n', mountainsInfos));
        }

        public void run() {
            try {
                IList<Queue<char>> adventurersInstructions = Adventurers.Select(adventurer => adventurer.Instructions).ToList();
                while(adventurersInstructions.Any(instruction => instruction.Count > 0)) {
                    foreach(var adventurer in Adventurers) {
                        if(adventurer.Instructions.Count > 0) {
                            char instruction = adventurer.Instructions.Dequeue();
                            switch(instruction) {
                                case 'A':
                                    adventurer.MoveForward(this, adventurer.Orientation, adventurer.Coords);
                                    break;
                                case 'D':
                                    adventurer.ChangeOrientation(adventurer.Orientation, Adventurer.ConvertInstructionFormat(instruction));
                                    break;
                                case 'G':
                                    adventurer.ChangeOrientation(adventurer.Orientation, Adventurer.ConvertInstructionFormat(instruction));
                                    break;
                                default: throw new NotSupportedException("The instruction is not supported.");
                            }
                        }
                    }
                }
                this.GetRemainingTreasures();
                builder.AppendLine("# {A comme Aventurier} - {Nom de l’aventurier} - {Axe horizontal} - {Axevertical} - {Orientation} - {Nb. trésors ramassés}");
                foreach(var adventurer in Adventurers) {
                    builder.AppendLine(adventurer.stringFormat());
                }
            } catch(Exception ex) {
                throw ex;
            }
        }

        public void GetRemainingTreasures() {
            var remainingTreasures = this.Treasures.Where(treasure => treasure.Value > 0);
            if (remainingTreasures != null && remainingTreasures.Count() > 0) {
                builder.AppendLine("# {T comme Trésor} - {Axe horizontal} - {Axe vertical} - {Nb. de trésorsrestants}");
                foreach(var treasure in remainingTreasures) {
                    string treasureInfo = string.Format("T - {0} - {1} - {2}", treasure.Key.X, treasure.Key.Y, treasure.Value);
                    builder.AppendLine(treasureInfo);
                }
            }
        } 

        public void WriteOutputFile(string filePath) {
            using (StreamWriter outputFile = new StreamWriter(filePath))
            {
                outputFile.WriteLine(builder.ToString());
            }
        }
    }
}