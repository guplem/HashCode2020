using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineQualificationRound
{
    public class Solution
    {
        public readonly Dictionary<Library, List<Book>> libraries = new Dictionary<Library, List<Book>>();
        public readonly List<Library> sortedLibraries = new List<Library>();
        public readonly int totalDaysAvailable;

        public Solution(int totalDaysAvailable)
        {
            this.totalDaysAvailable = totalDaysAvailable;
        }

        public void AddLibrary(Library library, List<Book> libraryBooks, bool removePreviouslyScannedBooks)
        {
            List<Book> booksToScan = new List<Book>(libraryBooks);
            
            // Remove the book from the list if it has been already been scanned by another library to improve the final score
            if (removePreviouslyScannedBooks)
            {
                HashSet<Book> scannedBooks = GetUnsortedScannedBooks();
                foreach (Book book in scannedBooks)
                    if (booksToScan.Contains(book))
                        booksToScan.Remove(book);
            }

            libraries.Add(library, booksToScan);
            sortedLibraries.Add(library);
        }

        public int GetScore()
        {
            Console.WriteLine("    Calculating score... ");

            int totalScore = 0;
            HashSet<Book> scannedBooks = GetUnsortedScannedBooks();
            foreach (Book book in scannedBooks)
                totalScore += book.score;

            return totalScore;
        }

        private HashSet<Book> GetUnsortedScannedBooks()
        {
            int currentSignUpTime = 0;
            Library currentSignUpLibrary = null;
            Dictionary<Library, HashSet<Book>> signUpLibraries = new Dictionary<Library, HashSet<Book>>();
            HashSet<Book> scannedBooks = new HashSet<Book>();

            for (int i = 0; i < totalDaysAvailable; i++)
            {
                if (currentSignUpTime <= 0)
                {
                    if (currentSignUpLibrary != null)
                        signUpLibraries.Add(currentSignUpLibrary, new HashSet<Book>(libraries[currentSignUpLibrary]));
                    currentSignUpLibrary = null;
                }

                if (currentSignUpLibrary == null)
                {
                    foreach (Library sortLib in sortedLibraries)
                        if (!signUpLibraries.ContainsKey(sortLib))
                        {
                            currentSignUpLibrary = sortLib;
                            currentSignUpTime = currentSignUpLibrary.signUpTime;
                            break;
                        }
                }
                
                currentSignUpTime--;
                
                foreach (KeyValuePair<Library, HashSet<Book>> pair in signUpLibraries)
                    for (int b = 0; b < pair.Key.scannedBooksPerDay; b++)
                    {
                        Book book = pair.Value.FirstOrDefault();
                        if (book != null)
                        {
                            pair.Value.Remove(book);
                            scannedBooks.Add(book);
                        }
                    }
                
                /*
                foreach (KeyValuePair<Library, List<Book>> pair in signUpLibraries)
                    for (int b = 0; b < pair.Key.scannedBooksPerDay; b++)
                    {
                        Book scanningBook = pair.Value.ElementAt(0);
                        pair.Value.RemoveAt(0);

                        //if (!scannedBooks.Contains(scanningBook))
                            scannedBooks.Add(scanningBook);
                    }
                 */
            }

            return scannedBooks;
        }
        
        /*private List<Book> GetSortedScannedBooks()
        {
            int currentSignUpTime = 0;
            Library currentSignUpLibrary = null;
            Dictionary<Library, List<Book>> signUpLibraries = new Dictionary<Library, List<Book>>();
            List<Book> scannedBooks = new List<Book>();

            for (int i = 0; i < totalDaysAvailable; i++)
            {
                if (currentSignUpTime <= 0)
                {
                    if (currentSignUpLibrary != null)
                        signUpLibraries.Add(currentSignUpLibrary, new List<Book>(libraries[currentSignUpLibrary]));
                    currentSignUpLibrary = null;
                }

                if (currentSignUpLibrary == null)
                {
                    foreach (Library sortLib in sortedLibraries)
                        if (!signUpLibraries.ContainsKey(sortLib))
                        {
                            currentSignUpLibrary = sortLib;
                            currentSignUpTime = currentSignUpLibrary.signUpTime;
                            break;
                        }
                }
                
                currentSignUpTime--;
                
                foreach (KeyValuePair<Library, List<Book>> pair in signUpLibraries)
                    for (int b = 0; b < pair.Key.scannedBooksPerDay; b++)
                    {
                        Book scanningBook = pair.Value.ElementAt(0);
                        pair.Value.RemoveAt(0);

                        if (!scannedBooks.Contains(scanningBook))
                            scannedBooks.Add(scanningBook);
                    }
                
            }

            return scannedBooks;
        }*/
    }
}

        
    
