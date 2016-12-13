using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    public class Selection
    {
        public List<int> Values { get; set; }
        public int NumberOfEmpty { get; set; }

        public List<int> PossibleSums { get; set; }

        public int MinimumValue { get; set; }
        public int MaximumValue { get; set; }
        public int Average { get; set; }

        //To Be Used Later
        public string RiskLevel { get; set; }

        public Selection(List<int> values)
        {
            NumberOfEmpty = 0;
            foreach(var digit in values)
            {
                if (digit == 0)
                {
                    NumberOfEmpty++;
                }
            }

            Values = values;
            PossibleSums = new List<int>();
            MinimumValue = 0;
            MaximumValue = 0;
            Average = 0;
        }
    }
}
