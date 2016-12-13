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

        private List<int> _remainingNumbers;

        #region Rewards Dictionary
        private Dictionary<int, int> _rewards = new Dictionary<int, int>()
        {
            {6, 10000},
            {7, 36},
            {8, 720},
            {9, 360},
            {10, 80},
            {11, 252},
            {12, 108},
            {13, 72},
            {14, 54},
            {15, 180},
            {16, 72},
            {17, 180},
            {18, 119},
            {19, 36},
            {20, 306},
            {21, 1080},
            {22, 144},
            {23, 1800},
            {24, 3600},
        };
        #endregion

        #region Possible Selections
        //Selections
        private Selection _rowOne;
        private Selection _rowTwo;
        private Selection _rowThree;

        private Selection _columnOne;
        private Selection _columnTwo;
        private Selection _columnThree;

        private Selection _topLeftToBottomRight;
        private Selection _topRightToBottomLeft;
        #endregion

        public Solution(List<int> values)
        {
            _remainingNumbers = new List<int>();
            for (var counter = 1; counter <= 9; counter++)
            {
                if (!values.Contains(counter))
                {
                    _remainingNumbers.Add(counter);
                }
            }

            var selections = new List<Selection>();

            selections.Add(_rowOne = new Selection(new List<int>() { values[0], values[1], values[2] }));
            selections.Add(_rowTwo = new Selection(new List<int>() { values[3], values[4], values[5] }));
            selections.Add(_rowThree = new Selection(new List<int>() { values[6], values[7], values[8] }));

            selections.Add(_columnOne = new Selection(new List<int>() { values[0], values[3], values[6] }));
            selections.Add(_columnTwo = new Selection(new List<int>() { values[1], values[4], values[7] }));
            selections.Add(_columnThree = new Selection(new List<int>() { values[2], values[5], values[8] }));

            selections.Add(_topLeftToBottomRight = new Selection(new List<int>() { values[0], values[4], values[8] }));
            selections.Add(_topRightToBottomLeft = new Selection(new List<int>() { values[2], values[4], values[6] }));

            foreach (var selection in selections)
            {
                switch (selection.NumberOfEmpty)
                {
                    case 1:
                        GetPossibleRewardsForOneMissing(selection);
                        break;
                    case 2:
                        GetPossibleRewardsForTwoMissing(selection);
                        break;
                    case 3:
                        selection.MinimumValue = 36;
                        selection.MaximumValue = 10000;
                        selection.Average = 1010;
                        break;
                    default:
                        break;
                }
            }

            MaximumOutput = selections.Where(x => x.MaximumValue == selections.Max(y => y.MaximumValue)).First();
            MaximumAverage = selections.Where(x => x.Average == selections.Max(y => y.Average)).First();
            var minValues = new List<int>(selections.Select(x => x.MinimumValue));
        }

        private void GetPossibleRewardsForOneMissing(Selection current)
        {
            var currentSum = current.Values.Sum();
            for (var counter = 1; counter <= 9; counter++)
            {
                if (_remainingNumbers.Contains(counter))
                {
                    current.PossibleSums.Add(counter + currentSum);
                }
            }

            CalculateAdditionalSelectionValues(current);
        }

        private void GetPossibleRewardsForTwoMissing(Selection current)
        {
            var currentSum = current.Values.Sum();
            for (var counter = 1; counter <= 9; counter++)
            {
                if (_remainingNumbers.Contains(counter))
                {
                    for (var subcounter = 1; subcounter <= 9; subcounter++)
                    {
                        if (_remainingNumbers.Contains(subcounter) && subcounter != counter)
                        {
                            current.PossibleSums.Add(counter + subcounter + currentSum);
                        }
                    }
                }
            }

            CalculateAdditionalSelectionValues(current);            
        }

        private void CalculateAdditionalSelectionValues(Selection current)
        {
            foreach (var sum in current.PossibleSums)
            {
                if (current.MinimumValue == 0)
                {
                    current.MinimumValue = _rewards[sum];
                    current.MaximumValue = _rewards[sum];
                    continue;
                }

                if (current.MinimumValue > _rewards[sum])
                {
                    current.MinimumValue = _rewards[sum];
                }

                if (current.MaximumValue < _rewards[sum])
                {
                    current.MaximumValue = _rewards[sum];
                }

                current.Average += _rewards[sum];
            }

            current.Average = current.Average / current.PossibleSums.Count();
        }  
    }
}
