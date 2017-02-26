using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GramsConversion.Calculator
{
    /// <summary>
    /// Molar converter. Converts Molucules to grams
    /// </summary>
    public class MolarCalculator : ICalculator
    {
        public double GetGramsValue(string elementName, double mass)
        {
            return PeriodicTableWrapper.MolToGrams(elementName, mass);
        }
    }
}
