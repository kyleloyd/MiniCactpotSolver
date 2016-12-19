using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    public class Solution
    {
        public Selection MaximumValue { get; set; }
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
            //A list containing the numbers that have not been uncovered
            _remainingNumbers = new List<int>();

            //Adds numbers that have not already been uncovered to _remainingNumbers
            for (var counter = 1; counter <= 9; counter++)
            {
                if (!values.Contains(counter))
                {
                    _remainingNumbers.Add(counter);
                }
            }

            //A list containing all possible selections for the provided game board
            var selections = new List<Selection>();

            selections.Add(_rowOne = new Selection(new List<int>() { values[0], values[1], values[2] }, "Top Row"));
            selections.Add(_rowTwo = new Selection(new List<int>() { values[3], values[4], values[5] }, "Center Row"));
            selections.Add(_rowThree = new Selection(new List<int>() { values[6], values[7], values[8] }, "Bottom Row"));

            selections.Add(_columnOne = new Selection(new List<int>() { values[0], values[3], values[6] }, "Left Column"));
            selections.Add(_columnTwo = new Selection(new List<int>() { values[1], values[4], values[7] }, "Center Column") );
            selections.Add(_columnThree = new Selection(new List<int>() { values[2], values[5], values[8] }, "Right Column"));

            selections.Add(_topLeftToBottomRight = new Selection(new List<int>() { values[0], values[4], values[8] }, "Top-Left to Bottom-Right Diagonal"));
            selections.Add(_topRightToBottomLeft = new Selection(new List<int>() { values[2], values[4], values[6] }, "Top-Right to Bottom-Left Diagonal"));

            foreach (var current in selections)
            {
                switch (current.NumberOfEmpty)
                {
                    case 1:
                        GetPossibleRewardsForOneMissing(current);
                        break;
                    case 2:
                        GetPossibleRewardsForTwoMissing(current);
                        break;
                    case 3:
                        GetPossibleRewardsForThreeMissing(current);
                        break;
                    default:
                        GetPossibleRewardsForZeroMissing(current);
                        break;
                }

                CalculateAdditionalSelectionValues(current);
            }

            MaximumValue = selections.Where(x => x.MaximumValue == selections.Max(y => y.MaximumValue)).First();
            MaximumAverage = selections.Where(x => x.Average == selections.Max(y => y.Average)).First();
            var minValues = new List<int>(selections.Select(x => x.MinimumValue));
        }

        private void GetPossibleRewardsForZeroMissing(Selection current)
        {
            var selectionSum = current.Values.Sum();
            current.PossibleSums.Add(selectionSum);
            current.MaximumValue = current.Average = current.MinimumValue = _rewards[selectionSum];
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
        }

        private void GetPossibleRewardsForThreeMissing(Selection current)
        {
            for (var firstNumber = 1; firstNumber <= 9; firstNumber++)
            {
                if (_remainingNumbers.Contains(firstNumber))
                {
                    for (var secondNumber = 1; secondNumber <= 9; secondNumber++)
                    {
                        if (_remainingNumbers.Contains(secondNumber) && secondNumber != firstNumber)
                        {
                            for (var thirdNumber = 1; thirdNumber <= 9; thirdNumber++)
                            {
                                if (_remainingNumbers.Contains(thirdNumber) && thirdNumber != firstNumber && thirdNumber != secondNumber)
                                {
                                    current.PossibleSums.Add(firstNumber + secondNumber + thirdNumber);
                                }
                            }
                        }
                    }
                }
            }
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
