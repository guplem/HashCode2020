using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineQualificationRound
{
    public class Solver
    {
        public Solution SolveProblem(int totalDaysAvailable, Book[] allBooks, Library[] allLibraries)
        {
            Utils.WriteLine("Solving problem... ");

            // Sort libraries sorted in descending order by potential punctuation.
            List<Library> librariesSortedByPotentialScore = allLibraries.OrderByDescending(i => i).ToList();
            Utils.WriteLine("  Libraries sorted in descending order by potential punctuation.");
            
            //Build up the solution
            Solution bestSolution = new Solution(totalDaysAvailable);
            Utils.WriteLine("Adding libraries to solution...");
            int libCount = 1;
            
            foreach (Library library in librariesSortedByPotentialScore)
            {
                if (libCount % 10 == 0)
                {
                    Utils.WriteLine("    Adding library " + Utils.GetFormated(libCount) + "/" + Utils.GetFormated(librariesSortedByPotentialScore.Count) + " ... (Remaining days to sign up new libraries: " + Utils.GetFormated(bestSolution.totalDaysAvailable-bestSolution.totalSignUpDays) + ")");

                    Utils.GoBackOneLine();
                }

                bestSolution.AddLibrary(library, library.books, true);


                if (bestSolution.totalDaysAvailable <= bestSolution.totalSignUpDays)
                    break;
                
                libCount++;
            }
                
            // Genetic iteration
            Random rnd = new Random();
            int iterationCount = 0;
            Utils.WriteLine("Current score: " + Utils.GetFormated(bestSolution.GetScore()) + " (iteration " + iterationCount + "). Start solution.");
            while(!Console.KeyAvailable)
            {
                iterationCount++;
                Solution mutatedSolution = new Solution(totalDaysAvailable);

                Queue<Library> randomSortedLibraries = new Queue<Library>(allLibraries.OrderBy(x => rnd.Next()).ToList());
                Queue<Library> bestSolutionLibraries = new Queue<Library>(bestSolution.sortedLibraries);

                for (int l = 0; l < allLibraries.Length; l++)
                {
                    Library libToAd;
                    if (rnd.NextDouble() >= 0.1) // Add from best solution
                    {
                        do {
                            libToAd = bestSolutionLibraries.Dequeue();
                        } while (!mutatedSolution.AddLibrary(libToAd, libToAd.books, false));
                    }
                    else // add from the random sorted libraries
                    {
                        do {
                            libToAd = randomSortedLibraries.Dequeue();
                        } while (!mutatedSolution.AddLibrary(libToAd, libToAd.books, false));
                    }
                    
                    if (mutatedSolution.totalSignUpDays >= mutatedSolution.totalDaysAvailable)
                        break;
                }

                int bestSolutionScore = bestSolution.GetScore();
                int mutationScore = mutatedSolution.GetScore();
                if (mutationScore > bestSolutionScore) {
                    bestSolution = mutatedSolution;
                    Utils.WriteLine("Current score: " + Utils.GetFormated(mutationScore) + " (iteration " + iterationCount + ")  IMPROVED!");
                } else {
                    Utils.WriteLine("Current score: " + Utils.GetFormated(mutationScore) + " (iteration " + iterationCount + ")");
                    Utils.GoBackOneLine();
                }
            }
            
            return bestSolution;
        }
    }
}