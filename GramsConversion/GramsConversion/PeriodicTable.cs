using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GramsConversion
{
    /// <summary>
    /// Representing one element from Periodic table
    /// </summary>
    public class Element
    {
        [JsonProperty(PropertyName="group", Required = Required.AllowNull)]
        public string Group { get; set; }

        [JsonProperty(PropertyName ="position", Required = Required.AllowNull)]
        public int Position { get; set; }

        [JsonProperty(PropertyName ="name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "number", Required = Required.Always)]
        public int Number { get; set; }

        [JsonProperty(PropertyName = "small", Required = Required.Always)]
        public string Symbol{ get; set; }

        [JsonProperty(PropertyName = "molar", Required = Required.Always)]
        public double MolarValue{ get; set; }

        [JsonProperty(PropertyName = "electrons", Required = Required.AllowNull)]
        public int[] Electrons { get; set; }
    }

    public class PeriodicTable
    {
        /// <summary>
        /// List of elements from periodic table
        /// </summary>
        [JsonProperty(PropertyName = "elements")]
        public Element[] Elements { get; set; }
    }

    public class PeriodicTableWrapper
    {
        /// <summary>
        /// List of periodic table components
        /// </summary>
        public static PeriodicTable PeriodicTable { get; set; }

        /// <summary>
        /// Parse and load Periodic table data from JSON file
        /// </summary>
        /// <param name="settings"></param>
        public static void LoadPeriodicTableFromJSON(JsonSerializerSettings settings)
        {
            //read the periodic table element values from JSON file
            //REFER: JSON file from https://github.com/diniska/chemistry/blob/master/PeriodicalTable/periodicTable.json
            var periodicTableText = System.IO.File.ReadAllText("periodicTable.json");
            if(periodicTableText.Length == 0)
            {
                ConsoleHelper.PrintError($"USer input file \"periodicTable.json\" is an empty file.");
                Environment.Exit(0);
            }
            var periodicTable = JsonConvert.DeserializeObject<PeriodicTable>(periodicTableText, settings);

            //Set the periodic table as a static fieild to be able to access from anywhere
            PeriodicTableWrapper.PeriodicTable = periodicTable;
        }

        /// <summary>
        /// Converts Molecules to grams
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mass"></param>
        /// <returns></returns>
        public static double MolToGrams(string name, double mass)
        {
            var element = PeriodicTable.Elements.Where(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase) == true).FirstOrDefault();
            if (element != null)
            {
                //ConsoleHelper.PrintInfo($"Molar mass of {name} & {mass} with molar of {element.MolarValue} is {element.MolarValue * mass}");
                return element.MolarValue * mass;
            }

            return 0;
        }
    }
}
