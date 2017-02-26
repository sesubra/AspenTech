using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GramsConversion.Calculator
{
    /// <summary>
    /// Calculator funtion for all the Unit of Measure
    /// </summary>
    public interface ICalculator
    {
        /// <summary>
        /// Converts the element mass to grams
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="mass"></param>
        /// <returns></returns>
        double GetGramsValue(string elementName, double mass);
    }
}
