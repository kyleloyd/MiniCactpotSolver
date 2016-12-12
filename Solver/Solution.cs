using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    public class Solution
    {
        public Selection MaximumOutput { get; set; }
        public Selection MaximumAverage { get; set; }

        private List<int> _grid;

        //Selections
        private List<int> _rowOne;
        private List<int> _rowTwo;
        private List<int> _rowThree;

        private List<int> _columnOne;
        private List<int> _columnTwo;
        private List<int> _columnThree;

        public Solution(List<int> values)
        {
            _grid = values;
        }
    }
}
