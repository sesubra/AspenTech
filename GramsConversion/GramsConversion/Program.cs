using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GramsConversion
{
    class Program
    {
        static void Main(string[] args)
        {
            //Make sure the input file exists
            if(System.IO.File.Exists("periodicTable.json") == false || System.IO.File.Exists("userInput.json") == false)
            {
                ConsoleHelper.PrintError($"Either one or both files \"periiodicTable.json\" and \"userInput.json\" is not found.");
                Environment.Exit(0);
            }

            if(new System.IO.FileInfo("periodicTable.json").Length == 0 || new System.IO.FileInfo("userInput.json").Length == 0)
            {
                ConsoleHelper.PrintError($"Either one or both files \"periiodicTable.json\" and \"userInput.json\" an empty file.");
                Environment.Exit(0);
            }

            //Serializer settings to handle any malformed json file input
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.Error = (object sender, ErrorEventArgs errorArgs) =>
            {
                if (errorArgs.ErrorContext.Member != null) ConsoleHelper.PrintError($"Error occured when trying to parse JSON for member: {errorArgs.ErrorContext.Member}");
                ConsoleHelper.PrintError(errorArgs.ErrorContext.Error.Message);
                errorArgs.ErrorContext.Handled = true;
                Environment.Exit(0);
            };

            //Load the period table values from JSOn file
            PeriodicTableWrapper.LoadPeriodicTableFromJSON(serializerSettings);

            //read the user's input from JSON file
            var userInputfile = System.IO.File.ReadAllText("userInput.json");
            if (userInputfile.Length == 0)
            {
                ConsoleHelper.PrintError($"User input file \"userInput.json\" is an empty file.");
                Environment.Exit(0);
            }
            var userInput = JsonConvert.DeserializeObject<UserInput>(userInputfile, serializerSettings);

            //Validate the user input's every component matching against the periodic table values
            var invalidComponents = userInput.ValidateComponents();

            //found some invalid components in user's input
            if (invalidComponents.Count() > 0)
            {
                ConsoleHelper.PrintError($"There are some invalid components found in the unser input. Invalid compoenents: [{String.Join(", ", invalidComponents.Select(c => c.Name).ToArray())}]");
                Environment.Exit(0);
            }

            //print the user input's values on to the console
            userInput.PrintUserInputDetails();
            try
            {
                //Calculate the full weight in grams and pounds and display it in the console
                userInput.CalculateTotalWeight();
            }
            catch(Exception ex)
            {
                //Any error catch and display the error in console
                ConsoleHelper.PrintError(ex.Message);
                Environment.Exit(0);
            }

            //wait before exit
            Console.Read();
        }
    }
}
