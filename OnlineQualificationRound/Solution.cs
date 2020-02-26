using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineQualificationRound
{
    public class Solution
    {
        public Dictionary<Library, List<Book>> libraries { get; private set; }
        public List<Library> sortedLibraries { get; private set; }
        public int totalDaysAvailable { get; private set; }
        public int totalSignUpDays { get; private set; }

        public HashSet<Book> unsortedScannedBooks
        {
            get
            {
                if (_unsortedScannedBooks == null)
                    _unsortedScannedBooks = GetUnsortedScannedBooks();
                return _unsortedScannedBooks;
            }
            private set { }
        }

        private HashSet<Book> _unsortedScannedBooks = null;
        
        public Solution(int totalDaysAvailable)
        {
            this.totalDaysAvailable = totalDaysAvailable;
            totalSignUpDays = 0;
            libraries = new Dictionary<Library, List<Book>>();
            sortedLibraries = new List<Library>();
        }

        

        public void AddLibrary(Library library, List<Book> libraryBooks)
        {
            if (sortedLibraries.Contains(library))
                return;

            List<Book> booksToScan = new List<Book>(libraryBooks);

            if (booksToScan.Count > 0)
            {
                libraries.Add(library, booksToScan);
                sortedLibraries.Add(library);
                totalSignUpDays += library.signUpTime;
                _unsortedScannedBooks = null;
            }
        }

        public int GetScore()
        {
            Utils.WriteLine("  > Calculating score... ");

            int totalScore = 0;
            HashSet<Book> scannedBooks = unsortedScannedBooks;
            foreach (Book book in scannedBooks)
                totalScore += book.score;

            return totalScore;
        }

        private HashSet<Book> GetUnsortedScannedBooks()
        {
            int currentSignUpTime = -1;
            Library currentSignUpLibrary = null;
            Dictionary<Library, Queue<Book>> signUpLibraries = new Dictionary<Library, Queue<Book>>();
            HashSet<Book> scannedBooks = new HashSet<Book>();

            int indexOfLastRegisteredLibrary = -1;
            
            for (int currentDay = 0; currentDay < totalDaysAvailable; currentDay++)
            {
                /*if (currentDay % 100 == 0)
                {
                    Utils.WriteLine("    Getting the scanned books of the day " + Utils.GetFormated(currentDay) + "/" + Utils.GetFormated(totalDaysAvailable) );
                    Utils.GoBackOneLine();  
                }*/

                
                if (currentSignUpTime <= 0)
                {
                    if (currentSignUpLibrary != null)
                        signUpLibraries.Add(currentSignUpLibrary, new Queue<Book>(libraries[currentSignUpLibrary]));

                    indexOfLastRegisteredLibrary++;
                    if (indexOfLastRegisteredLibrary < sortedLibraries.Count)
                    {
                        currentSignUpLibrary = sortedLibraries.ElementAt(indexOfLastRegisteredLibrary);
                        currentSignUpTime = currentSignUpLibrary.signUpTime;
                    }
                    else
                    {
                        currentSignUpLibrary = null;
                    }
                }
                currentSignUpTime--;

                
                bool anyRemainingBookToScan = false;
                
                // Calculating daily books score
                HashSet<Library> scannedLibraries = new HashSet<Library>();
                foreach (KeyValuePair<Library, Queue<Book>> pair in signUpLibraries)
                {
                    for (int b = 0; b < pair.Key.scannedBooksPerDay || pair.Value.Count < 0; b++)
                    {
                        Book book = pair.Value.FirstOrDefault();
                        if (book != null)
                        {
                            /*countedBookNumber ++;
                            if (countedBookNumber % 100 == 0)
                            {
                                Utils.WriteLine("    Counting book " + Utils.GetFormated(countedBookNumber) + "/" + Utils.GetFormated(totalNumberOfBooks) );
                                Utils.GoBackOneLine();
                            }*/
                            
                            scannedBooks.Add(pair.Value.Dequeue()); // No need to check if already exists because it is a hash set
                        }
                    }

                    if (pair.Value.Count > 0)
                        anyRemainingBookToScan = true;
                    else
                        scannedLibraries.Add(pair.Key);
                }

                foreach (Library libraryToRemove in scannedLibraries)
                    signUpLibraries.Remove(libraryToRemove);

                if (!anyRemainingBookToScan)
                {
                    if (currentSignUpTime > 0)
                        currentDay += currentSignUpTime;
                    currentSignUpTime = 0;
                }
                
                
            }

            return scannedBooks;
        }
        
    }
}

        
    
