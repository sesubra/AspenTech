using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GramsConversion.Calculator
{
    /// <summary>
    /// Kilograms converter. Converts Kilo grams to grams
    /// </summary>
    public class KilogramsCalculator : ICalculator
    {
        public double GetGramsValue(string elementName, double mass)
        {
            //ConsoleHelper.PrintInfo($"Element name {elementName} with mass {mass} has weight of {mass * 1000}");
            return mass * 1000;
        }
    }
}
