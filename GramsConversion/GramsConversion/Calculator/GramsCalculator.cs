using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GramsConversion.Calculator
{
    public class GramsCalculator : ICalculator
    {
        /// <summary>
        /// Converts from Grams to Grams
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="mass"></param>
        /// <returns></returns>
        public double GetGramsValue(string elementName, double mass)
        {
            //ConsoleHelper.PrintInfo($"Element name {elementName} with mass {mass} has weight of {mass * 1}");
            return mass * 1;
        }
    }
}
