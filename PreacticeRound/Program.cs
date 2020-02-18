using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Threading;
using System.Windows.Forms;

namespace PizzaProblem
{
    internal class Program
    {
        private static Thread[] threads;
        private static int problemToSolve = -1;
        public static Random rnd = new Random();
        
        public static void Main(string[] args)
        {
            Console.Write("Write the input file's name: ");
            latestFilenameUsed = Console.ReadLine();
            if (String.Compare(latestFilenameUsed, "all", StringComparison.OrdinalIgnoreCase) == 0)
            {
                problemToSolve = 0;
                SolveProblem();
            }
            else
            {
                Console.Write("Write the quantity of threads to execute: ");
                threads = new Thread[Int32.Parse(Console.ReadLine() ?? throw new Exception("The read input is not a number."))];
                SolveProblemWithMultitreading();
            }
        }

        private static void SolveProblemWithMultitreading()
        {
            if (threads!= null)
                for (int t = 0; t < threads.Length; t++)
                {
                    Console.WriteLine("Initializing thread");
                    threads[t] = new Thread(SolveProblem);
                    threads[t].Start();
                }
        }

        public static void FinishAllThreads()
        {
            if (threads!= null)
                for (int t = 0; t < threads.Length; t++)
                    threads[t].Abort();
        }

        public static Pizza[] pizzasInProblem;

        public static void SolveProblem()
        {
            Console.WriteLine("Solving problem...");
            
            // Build up the problem
            string[] fileLines = ReadFileLines();
            int problemDesiredSlices = Int32.Parse(fileLines[0].Split(' ')[0]);
            int problemNumberOfPizzas = Int32.Parse(fileLines[0].Split(' ')[1]);
            Console.WriteLine("Max. slices: " + problemDesiredSlices);
            Console.WriteLine("Number of pizzas: " + problemNumberOfPizzas);
            // Store each pizza's information
            pizzasInProblem = GetPizzasFromStringLine(fileLines[1]);
            if (pizzasInProblem.Length != problemNumberOfPizzas)
                throw new Exception("The number of pizzas read is different than the informed");
            
            
            
            
            Console.WriteLine(" ------- Calculating solutions...  ------- ");
            // Look for the solution using method X
            Solution solution = new Solution(new List<Pizza>(), problemDesiredSlices);
            /*switch (algorithmToUse) {
                case 0:
                    solution = GetSolution0(problemDesiredSlices, pizzasInProblem, new List<Pizza>(), new List<Pizza>()); break;
                case 1:*/
                    solution = GetSolution1(problemDesiredSlices, pizzasInProblem);/* break;
            }*/

            
            
            
            
            // Print the result
            Console.WriteLine("SOLUTION FOUND: " + solution.GetNumberOfSlices() + "/" + problemDesiredSlices + " (dif = " + solution.diference + ")." + " Pizzas: " + solution.qttyOfPizzas);
            SaveSolution(solution);
            


            //Solve the next one if desired
            if (problemToSolve >= 0)
            {
                problemToSolve++;
                SolveProblem();
            }
            else
            {
                //End all threads
                FinishAllThreads();
            }

        }

        private static void SaveSolution(Solution solution)
        {
            try
            {
                string[] lines = { solution.qttyOfPizzas.ToString(), solution.pizzasString};
                System.IO.File.WriteAllLines("Solution to " + latestFilenameUsed +  " - Dif " + solution.diference + ".txt", lines);
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
            }
        }
        
        private static string latestFilenameUsed = String.Empty;
        private static string[] ReadFileLines()
        {
            switch (problemToSolve)
            {
                case -1: break;
                case 0: latestFilenameUsed = "b_small.in"; break;
                case 1: latestFilenameUsed = "c_medium.in"; break;
                case 2: latestFilenameUsed = "d_quite_big.in"; break;
                case 3: latestFilenameUsed = "e_also_big.in"; break;
                default: FinishAllThreads(); return null; break;
            }
                
            return System.IO.File.ReadAllText(latestFilenameUsed).Split('\n');
        }
        
        private static Pizza[] GetPizzasFromStringLine(string allPizzasInfoAsString)
        {
            int[] slicesPerPizza = Array.ConvertAll(allPizzasInfoAsString.Split(' '), s => int.Parse(s));
            Pizza[] pizzas = new Pizza[slicesPerPizza.Length];
            for (int i = 0; i < slicesPerPizza.Length; i++)
                pizzas[i] = new Pizza(i, slicesPerPizza[i]);
            return pizzas;
        }
        
        private static List<Pizza> GetPizzasNotInSolution(Pizza[] pizzas, Solution solution)
        {
            List<Pizza> pizzasNotInSolution = new List<Pizza>();
            for (int p = 0; p < pizzas.Length; p++)
                if (!solution.Contains(pizzas[p]))
                    pizzasNotInSolution.Add(pizzas[p]);

            return pizzasNotInSolution;
        }

        private static Solution GetBestSolution(int desiredSlices, List<Solution> solutions)
        {
            Solution bestSolution = new Solution(new List<Pizza>(), desiredSlices);
            foreach (Solution solution in solutions)
            {
                if (solution.slices > bestSolution.slices && solution.slices <= desiredSlices)
                    bestSolution = solution;
            }

            return bestSolution;
        }

        
        
        
        
        
        
        
        
        

        // Quick algorithm (big to small)
        public static Solution GetSolution0(int desiredSlices, Pizza[] pizzasInProblem, List<Pizza> basePizzaList, List<Pizza> pizzasToExclude)
        {
            Solution tempSolution = new Solution(new List<Pizza>(basePizzaList), desiredSlices);

            int indexOfPizzaToAdd = (rnd.NextDouble() < 0.2) ? rnd.Next(pizzasInProblem.Length) : pizzasInProblem.Length - 1;

            while (desiredSlices != tempSolution.slices && indexOfPizzaToAdd >= 0)
            {
                if (pizzasToExclude.Contains(pizzasInProblem[indexOfPizzaToAdd]))
                {
                    indexOfPizzaToAdd--;
                    continue;
                }
                
                tempSolution.AddPizza(pizzasInProblem[indexOfPizzaToAdd]);

                //tempSolution = new Solution(selectedPizzas, desiredSlices);
                if (!tempSolution.IsValidSolution())
                    tempSolution.RemovePizza(pizzasInProblem[indexOfPizzaToAdd]);

                indexOfPizzaToAdd--;
            }

            return tempSolution;
        }

        
        
        
        
        
        
        

        // "Genetic" (random evolution with 1 child)
        private static Solution GetSolution1(int desiredSlices, Pizza[] pizzasInProblem)
        {
            Solution bestSolution = GetSolution0(desiredSlices, pizzasInProblem, new List<Pizza>(), new List<Pizza>());

            int iterationCounter = 0;
            while(!Console.KeyAvailable)
            {
                if (bestSolution.slices == desiredSlices)
                    break;

                
                // Obtinc una mutació de la millor
                Solution mutatedSolution = bestSolution.GetMutation(GetPizzasNotInSolution(pizzasInProblem, bestSolution));
                Console.WriteLine("Iteration " + iterationCounter + 
                                  "\nMutated solution dif = " + mutatedSolution.diference /*+ " Slices " + mutatedSolution.slices + " Desired " + mutatedSolution.desiredSlices + " pizzas: " + mutatedSolution.pizzasString*/ + 
                                  "\n   Best solution dif = " + bestSolution.diference/* + " Slices " + bestSolution.slices + " Desired " + bestSolution.desiredSlices + " pizzas: " + bestSolution.pizzasString*/);

                
                // Em guardo la millor solució
                Solution[] tempSol = { bestSolution, mutatedSolution };
                bestSolution = GetBestSolution(desiredSlices, tempSol.ToList());

                
                iterationCounter++;
            }

            return bestSolution;
        }

        
    }
}