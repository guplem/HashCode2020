using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Threading;
using System.Windows.Forms;

namespace PracticeRound
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.Write("Write the input file's name: ");
            string filename = Console.ReadLine();
            
            /*switch (problemToSolve)
            {
                case -1: break;
                case 0: latestFilenameUsed = "b_small.in"; break;
                case 1: latestFilenameUsed = "c_medium.in"; break;
                case 2: latestFilenameUsed = "d_quite_big.in"; break;
                case 3: latestFilenameUsed = "e_also_big.in"; break;
                default:
            }*/
            
            
            string[] fileLines = ReadFileLines(filename);
            int problemDesiredSlices = Int32.Parse(fileLines[0].Split(' ')[0]);
            int numberOfPizzasWrittenInTheFile = Int32.Parse(fileLines[0].Split(' ')[1]);
            Pizza[] pizzasInFile = GetPizzasFromStringLine(fileLines[1]);
            if (pizzasInFile.Length != numberOfPizzasWrittenInTheFile)
                throw new Exception("The number of pizzas read is different than the informed/written.");
            
            Solver solver = new Solver();
            Solution solution =  solver.SolveProblem(problemDesiredSlices, pizzasInFile);
            
            SaveSolution(solution, filename);
        }


        /*private static Thread[] threads;
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
*/
        
        private static string[] ReadFileLines(string filename)
        {
            return System.IO.File.ReadAllText(filename).Split('\n');
        }
        
        private static Pizza[] GetPizzasFromStringLine(string allPizzasInfoAsString)
        {
            int[] slicesPerPizza = Array.ConvertAll(allPizzasInfoAsString.Split(' '), s => int.Parse(s));
            Pizza[] pizzas = new Pizza[slicesPerPizza.Length];
            for (int i = 0; i < slicesPerPizza.Length; i++)
                pizzas[i] = new Pizza(i, slicesPerPizza[i]);
            return pizzas;
        }
        
        private static void SaveSolution(Solution solution, string problemName)
        {
            try
            {
                string[] lines = { solution.qttyOfPizzas.ToString(), solution.pizzasIdString};
                System.IO.File.WriteAllLines("Solution to " + problemName +  " - Dif " + solution.diference + ".txt", lines);
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
            }
        }
        
        //private static string latestFilenameUsed = String.Empty;

        

        




        
        
        
        
        
        
        
        
        

        

        
    }
}