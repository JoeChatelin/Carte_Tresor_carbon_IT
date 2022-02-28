using System;
using System.Collections.Generic;
using System.Linq;

namespace TreasureHunt
{
    class Program
    {
        static void Main(string[] args)
        {
            try {
                string inputFile = "./files/input.txt";
                string outputFile = "./files/output.txt";
                Map map = new Map();
                map.InitMapElements(Parser.ReadFile(inputFile));
                map.run();
                map.WriteOutputFile(outputFile);
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }       
        }
    }
}
