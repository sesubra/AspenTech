using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GramsConversion.Calculator
{
    /// <summary>
    /// Factory method to get the corrent Calculator object
    /// </summary>
    public class CalculatorFactory
    {
        public static ICalculator GetCalculator(string unit)
        {
            switch (unit)
            {
                case "kilograms":
                    return new KilogramsCalculator();
                case "grams":
                    return new GramsCalculator();
                case "mol":
                    return new MolarCalculator();
                default:
                    throw new Exception($"Unit calculator \"{unit}\" is not implemented yet");
            }
        }
    }
}
