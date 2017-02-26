using GramsConversion.Calculator;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GramsConversion
{
    public class Component
    {
        public Component()
        {

        }

        [JsonProperty(PropertyName ="name", Required =Required.Always)]
        public string Name { get; set; }

        [JsonProperty(PropertyName ="mass", Required = Required.Always)]
        public double Mass { get; set; }

        [JsonProperty(PropertyName ="units", Required =Required.Always)]
        public string Unit { get; set; }

        [JsonIgnore]
        public double WeightInGrams { get; set; }

        [JsonIgnore]
        public bool IsValid { get; set; }

        public ICalculator GetCalculator()
        {
            return CalculatorFactory.GetCalculator(this.Unit);
        }

        public void CalculateWeight()
        {
            this.WeightInGrams = this.GetCalculator().GetGramsValue(this.Name, this.Mass);
        }
    }

    public class UserInput
    {
        [JsonProperty(PropertyName = "components", Required =Required.Always)]
        public Component[] Components { get; set; }

        double GramsToPounds(double grams)
        {
            return grams * 0.00220462;  //0.00220462 = 1 grams
        }

        public void PrintUserInputDetails()
        {
            this.Components.ToList().ForEach(c =>
            {
                ConsoleHelper.PrintInfo($"Component Name: {c.Name}, Mass: {c.Mass}, Units: {c.Unit}");
            });
        }

        public IEnumerable<Component> ValidateComponents()
        {
            this.Components.ToList().ForEach(c =>
            {
                var matchedEle = PeriodicTableWrapper.PeriodicTable.Elements.Where(e => e.Name.Equals(c.Name, StringComparison.OrdinalIgnoreCase) == true).FirstOrDefault();
                if(matchedEle != null)
                {
                    c.IsValid = true;
                }
            });

            return this.Components.Where(c => c.IsValid == false).ToList();
        }

        public void CalculateTotalWeight()
        {
            this.Components.ToList().ForEach(c => c.CalculateWeight());
            var totalWeight = this.Components.Sum(c => c.WeightInGrams);

            ConsoleHelper.PrintSuccess($"Total weight of all the components is {String.Format("{0:0}", totalWeight)} grams OR {String.Format("{0:0.##}", GramsToPounds(totalWeight))}lbs.");
        }
    }
}
