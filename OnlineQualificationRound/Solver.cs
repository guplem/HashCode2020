using System;

namespace OnlineQualificationRound
{
    public class Solver
    {
        public Solution SolveProblem(int totalDaysAvaliable, Book[] allBooks, Library[] allLibraries)
        {
            // Sort array in ascending order. 
            Array.Sort(allLibraries);
            Array.Reverse(allLibraries); // Then in descending (by potential value)
            
            Solution solution = new Solution();
            
            foreach (Library library in allLibraries)
                solution.AddLibrary(library, library.books);

            return solution;
        }
    }
}