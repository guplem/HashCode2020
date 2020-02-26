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
            //List<Library> librariesSortedByPotentialScore = allLibraries.OrderByDescending(i => i).ToList();
            //Utils.WriteLine("  Libraries sorted in descending order by potential punctuation.");
            
            //Build up the solution
            Solution solution = new Solution(totalDaysAvailable);
            Utils.WriteLine("Adding libraries to solution...");

            int debugInfoIteration = 5;
            for (int l = 0; l < allLibraries.Length; l++)
            {
                if (l % debugInfoIteration == 0)
                {
                    Utils.WriteLine("    Calculating remaining libraries " + Utils.GetFormated(l) + "/" + Utils.GetFormated(allLibraries.Length) + " ... (Remaining days to sign up new libraries: " + Utils.GetFormated(solution.totalDaysAvailable-solution.totalSignUpDays) + ")");
                    Utils.GoBackOneLine();
                }
                Library[] remainingLibraries = allLibraries.Except(solution.libraries.Keys).ToArray();
                
                if (l % debugInfoIteration == 0)
                    Utils.WriteLine("    Updating all libraries' books... Total progress: " + Utils.GetFormated(l) + "/" + Utils.GetFormated(allLibraries.Length) + "(Remaining days to sign up new libraries: " + Utils.GetFormated(solution.totalDaysAvailable-solution.totalSignUpDays) + ")");
                for (int i = 0; i < remainingLibraries.Length; i++)
                {
                    /*if (l % debugInfoIteration == 0 && i % debugInfoIteration == 0)
                    {
                        Utils.WriteLine("    Updating library " + i + "/" + remainingLibraries.Length);
                        Utils.GoBackOneLine();
                        Utils.ClearCurrentConsoleLine();
                    }*/
                    remainingLibraries[i].UpdateBooksTo(remainingLibraries[i].books.Except(solution.unsortedScannedBooks).ToList());
                }
                if (l % debugInfoIteration == 0)
                    Utils.GoBackOneLine();
                
                
                if (l % debugInfoIteration == 0)
                {
                    Utils.WriteLine("    Selecting best library to scan... Total progress: " + Utils.GetFormated(l) + "/" + Utils.GetFormated(allLibraries.Length) + "(Remaining days to sign up new libraries: " + Utils.GetFormated(solution.totalDaysAvailable-solution.totalSignUpDays) + ")");
                    Utils.GoBackOneLine();
                }
                Library topLibrary = remainingLibraries.OrderByDescending(i => i).ToList().ElementAt(0);
                
                
                if (l % debugInfoIteration == 0)
                {
                    Utils.WriteLine("    Adding library... Total progress: " + Utils.GetFormated(l) + "/" + Utils.GetFormated(allLibraries.Length) + "(Remaining days to sign up new libraries: " + Utils.GetFormated(solution.totalDaysAvailable-solution.totalSignUpDays) + ")");
                    Utils.GoBackOneLine();
                }
                solution.AddLibrary(topLibrary, topLibrary.books);
                
                if (l % debugInfoIteration == 0)
                {
                    Utils.WriteLine("    Library added. Total progress: " + Utils.GetFormated(l) + "/" + Utils.GetFormated(allLibraries.Length) + "(Remaining days to sign up new libraries: " + Utils.GetFormated(solution.totalDaysAvailable-solution.totalSignUpDays) + ")");
                    Utils.GoBackOneLine();
                }
                
                if (solution.totalDaysAvailable <= solution.totalSignUpDays)
                    break;
            }

            return solution;
        }
    }
}