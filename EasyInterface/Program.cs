using Solver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            var values = new List<int>();
            for (var counter = 1; counter <= 3; counter++)
            {
                while (true)
                {
                    Console.Write($"Line {counter}, separated by spaces (Put zeros for unrevealed):");
                    if (!GenerateValueList(ref values))
                    {
                        continue;
                    }

                    break;
                }
            }

            var solution = new Solution(values);
        }

        private static bool GenerateValueList(ref List<int> values)
        {
            var input = Console.ReadLine();
            var splitInput = input.Split(' ');

            if (splitInput.Count() != 3)
            {
                Console.WriteLine("Not enough entries, try again!\n");
                return false;
            }

            var intsToAdd = new List<int>();
            foreach (var digit in splitInput)
            {
                int result;
                if (!int.TryParse(digit, out result))
                {
                    Console.WriteLine("Invalid entry, try again!\n");
                    return false;
                }

                if (result > 9 || result < 0)
                {
                    Console.WriteLine("Invalid entry, try again!\n");
                    return false;
                }

                if (values.Contains(result) || intsToAdd.Contains(result))
                {
                    Console.WriteLine("Duplicate entry, try again!\n");
                    return false;
                }

                intsToAdd.Add(result);
            }

            values.AddRange(intsToAdd);
            return true;
        }
    }
}
