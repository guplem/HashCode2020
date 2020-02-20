using System.Collections.Generic;

namespace OnlineQualificationRound
{
    public class Solution
    {
        public readonly Dictionary<Library, List<Book>> libraries = new Dictionary<Library, List<Book>>();

        public void AddLibrary(Library library, List<Book> libraryBooks)
        {
            libraries.Add(library, new List<Book>(libraryBooks));
        }

        public int GetScore(int totalDaysAvaliable)
        {
            // TODO
            return -1;
        }
    }
}