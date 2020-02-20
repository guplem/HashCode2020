using System.Collections.Generic;
using System.Linq;

namespace OnlineQualificationRound
{
    public class Solution
    {
        public readonly Dictionary<Library, List<Book>> libraries = new Dictionary<Library, List<Book>>();
        public List<Library> sortedLibraries = new List<Library>();

        public void AddLibrary(Library library, List<Book> libraryBooks)
        {
            libraries.Add(library, new List<Book>(libraryBooks));
            sortedLibraries.Add(library);
        }

        public int GetScore(int totalDaysAvailable)
        {
            // TODO
            int totalScore = 0;
            int currentSignUpTime = 0;
            Library currentSignUpLibrary = null;
            Dictionary<Library, List<Book>> signUpLibraries = new Dictionary<Library, List<Book>>();
            List<Book> scannedBooks = null;

            for (int i = 0; i < totalDaysAvailable; i++)
            {
                if (currentSignUpTime <= 0)
                {
                    if (currentSignUpLibrary != null)
                        signUpLibraries.Add(currentSignUpLibrary, libraries[currentSignUpLibrary]);
                    currentSignUpLibrary = null;
                }

                if (currentSignUpLibrary == null)
                {
                    currentSignUpLibrary = sortedLibraries.ElementAt(0);
                    currentSignUpTime = currentSignUpLibrary.signUpTime;
                }

                currentSignUpTime--;


                foreach (KeyValuePair<Library, List<Book>> pair in signUpLibraries)
                {
                    for (int b = 0; b < pair.Key.scannedBooksPerDay; b++)
                    {
                        Book scanningBook = pair.Value.ElementAt(0);
                        pair.Value.RemoveAt(0);

                        if (!scannedBooks.Contains(scanningBook))
                        {
                            totalScore += scanningBook.score;
                            scannedBooks.Add(scanningBook);
                        }
                    }
                }

            }

            return totalScore;
        }
    }
}

        
    
