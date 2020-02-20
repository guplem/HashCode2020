using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PracticeRoundSimpleAlgorithm
{
    public class Solver
    {
        public static Random rnd = new Random();
        public Solution SolveProblem(int problemDesiredSlices, Pizza[] pizzasInProblem)
        {
            Solution bestSolution = new Solution(new List<Pizza>(), problemDesiredSlices);
            int startIndex = pizzasInProblem.Length-1;
            List<Pizza> pizzasToExclude = new List<Pizza>(); // Not used yet
            List<Pizza> basePizzaList = new List<Pizza>(); // Not used yet
            
            while (bestSolution.diference != 0 && startIndex >= 0)
            {
                Solution solution = GetSolutionWithBitToSmallAlgorithm(problemDesiredSlices, pizzasInProblem, basePizzaList, pizzasToExclude, startIndex);
                if (solution.diference < bestSolution.diference) bestSolution = solution;
                Console.WriteLine("Index " + startIndex + " - Best: " + bestSolution.diference + " - Found: " + solution);
                startIndex--;
            }

            Console.WriteLine("SOLUTION FOUND: " + bestSolution.slices + "/" + problemDesiredSlices + " (dif = " + bestSolution.diference + ")." + " Pizzas: " + bestSolution.qttyOfPizzas);

            return bestSolution;
        }







        // Quick algorithm (big to small)
        public static Solution GetSolutionWithBitToSmallAlgorithm(int desiredSlices, Pizza[] pizzasInProblem, List<Pizza> basePizzaList, List<Pizza> pizzasToExclude, int startIndex)
        {
            int indexOfTheLastAddedPizza = startIndex;
            int currentIndexOfPizzaToCheck = startIndex;
            Solution solution = new Solution(new List<Pizza>(basePizzaList), desiredSlices);

            while (desiredSlices != solution.slices)
            {
                if (pizzasToExclude.Contains(pizzasInProblem[currentIndexOfPizzaToCheck]))
                {
                    currentIndexOfPizzaToCheck = DecreaseInLoop(currentIndexOfPizzaToCheck, pizzasInProblem.Length);
                    continue;
                }

                if (solution.AddPizza(pizzasInProblem[currentIndexOfPizzaToCheck]))
                    if (solution.IsValidSolution())
                        indexOfTheLastAddedPizza = currentIndexOfPizzaToCheck;
                    else
                        solution.RemovePizza(pizzasInProblem[currentIndexOfPizzaToCheck]);

                currentIndexOfPizzaToCheck = DecreaseInLoop(currentIndexOfPizzaToCheck, pizzasInProblem.Length);
                
                if (currentIndexOfPizzaToCheck == indexOfTheLastAddedPizza)
                    break;
            }

            return solution;
        }
        private static int DecreaseInLoop(int number, int exclusiveMaxValue)
        {
            return number-1 < 0 ? exclusiveMaxValue-1 : number-1;
        }

    }
}