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
            Solution solution = new Solution(totalDaysAvailable);
            Utils.WriteLine("Adding libraries to solution...");
            int libCount = 1;
            
            foreach (Library library in librariesSortedByPotentialScore)
            {
                if (libCount % 10 == 0)
                {
                    Utils.WriteLine("    Adding library " + Utils.GetFormated(libCount) + "/" + Utils.GetFormated(librariesSortedByPotentialScore.Count) + " ... (Remaining days to sign up new libraries: " + Utils.GetFormated(solution.totalDaysAvailable-solution.totalSignUpDays) + ")");

                    Utils.GoBackOneLine();
                }

                solution.AddLibrary(library, library.books, true);


                if (solution.totalDaysAvailable <= solution.totalSignUpDays)
                    break;
                
                libCount++;
            }
                

            return solution;
        }
    }
}