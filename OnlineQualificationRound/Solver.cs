using System;
using System.Collections.Generic;

namespace OnlineQualificationRound
{
    public class Solver
    {
        public Solution SolveProblem(int totalDaysAvailable, Book[] allBooks, List<Library> allLibraries)
        {
            Console.WriteLine("Solving problem... ");

            // Sort libraries sorted in descending order by potential punctuation.
            List<Library> librariesSortedByPriority = new List<Library>();
            
            Console.Write("Remaining libraries to sort: ");
            while (allLibraries.Count > 0)
            {
                Console.Write(allLibraries.Count + ", ");
                Library topLibrary = null;
                
                foreach (Library library in allLibraries)
                    if (topLibrary == null || library.potentialScore > topLibrary.potentialScore)
                        topLibrary = library;

                if (topLibrary != null)
                {
                    allLibraries.Remove(topLibrary);
                    librariesSortedByPriority.Add(topLibrary);                    
                }
            }
            Console.WriteLine("Libraries sorted in descending order by potential punctuation.");
            
            //Build up the solution
            Solution solution = new Solution();
            foreach (Library library in allLibraries)
                solution.AddLibrary(library, library.books);

            return solution;
        }
    }
}