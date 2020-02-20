using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace OnlineQualificationRound
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.Write("Write the input file's name: ");
            string filename = Console.ReadLine();
            string[] fileLines = ReadFileLines(filename);
            
            
            
            int totalBooksNumber = Int32.Parse(fileLines[0].Split(' ')[0]);
            int totalLibrariesNumber = Int32.Parse(fileLines[0].Split(' ')[1]);
            
            int totalDaysAvailable = Int32.Parse(fileLines[0].Split(' ')[2]);
            Console.WriteLine("Storing books... ");
            Book[] allBooks = GetBookssFromStringLine(fileLines[1], totalBooksNumber);
            Console.WriteLine("Storing libraries... ");
            Library[] allLibraries = GetLibrariesFromLines(fileLines.Skip(2).ToArray(), allBooks, totalLibrariesNumber, totalDaysAvailable);
            

            
            
            Solver solver = new Solver();
            Solution solution =  solver.SolveProblem(totalDaysAvailable, allBooks, allLibraries.ToList());
            
            SaveSolution(solution, filename, totalDaysAvailable);
        }


        private static string[] ReadFileLines(string filename)
        {
            return System.IO.File.ReadAllText(filename).Split('\n');
        }
        
        private static Book[] GetBookssFromStringLine(string allPizzasInfoAsString, int totalBooksNumber)
        {
            int[] scorePerBook = Array.ConvertAll(allPizzasInfoAsString.Split(' '), s => int.Parse(s));
            Book[] books = new Book[scorePerBook.Length];
            for (int i = 0; i < scorePerBook.Length; i++)
                books[i] = new Book(i, scorePerBook[i]);
            
            if (books.Length != totalBooksNumber)
                throw new Exception("The number of books read was not the same as the number of books informed as being part of the whole problem.");
                
            return books;
        }
        
        private static Library[] GetLibrariesFromLines(string[] linesWithLibrariesInfo, Book[] allBooks, int totalLibrariesNumber, int daysAvailableInProblem)
        {
            List<Library> libraries = new List<Library>();
            int numberOfBooks = -1;
            int signUpTime = -1;
            int scannedBooksPerDay = -1;
            List<Book> booksInLibrary = new List<Book>();
            
            for (int l = 0; l < linesWithLibrariesInfo.Length; l++)
            {
                if (string.IsNullOrEmpty(linesWithLibrariesInfo[l]))
                    break;
                
                if (l % 2 == 0) { // First line
                    numberOfBooks = Int32.Parse(linesWithLibrariesInfo[l].Split(' ')[0]);
                    signUpTime = Int32.Parse(linesWithLibrariesInfo[l].Split(' ')[1]);
                    scannedBooksPerDay = Int32.Parse(linesWithLibrariesInfo[l].Split(' ')[2]);
                } 
                else { // Second line
                    int[] booksId = Array.ConvertAll(linesWithLibrariesInfo[l].Split(' '), s => int.Parse(s));
                    foreach (int bookId in booksId)
                            if (allBooks[bookId].id == bookId)
                                booksInLibrary.Add(allBooks[bookId]);
                            else 
                                throw new Exception("The books array is not sorted by the book id.");
                    
                    if (numberOfBooks != booksInLibrary.Count)
                        throw new Exception("The number of books read was not the same as the number of books informed as being part of the library. Read " + booksInLibrary.Count + ", expected " + numberOfBooks);
                    else
                        libraries.Add(new Library(libraries.Count(), signUpTime, scannedBooksPerDay, booksInLibrary, daysAvailableInProblem));
                    
                    //Reset 
                    numberOfBooks = -1;
                    signUpTime = -1;
                    scannedBooksPerDay = -1;
                    booksInLibrary = new List<Book>();
                }
            }
            
            if (libraries.Count != totalLibrariesNumber)
                throw new Exception("The number of libraries read was not the same as the number of libraries informed as being part of the whole problem.");

            return libraries.ToArray();
        }
        
        private static void SaveSolution(Solution solution, string problemName, int totalDaysAvaliable)
        {
            Console.WriteLine("Saving solution...");
            
            List<string> lines = new List<string>();
                
            lines.Add(solution.libraries.Count.ToString());
            
            foreach (KeyValuePair<Library, List<Book>> keyValuePair in solution.libraries)
            {
                lines.Add(keyValuePair.Key.id.ToString() + " " + keyValuePair.Value.Count.ToString());
                lines.Add(keyValuePair.Value.Aggregate("", (current, pizza) => current + (pizza.id + " ")));
            }
            
            try {
                System.IO.File.WriteAllLines("Solution to " + problemName +  " - Score " + solution.GetScore(totalDaysAvaliable) + ".txt", lines.ToArray());
                Console.WriteLine("Solution saved.\n \n");
            } catch (IOException e) { Console.WriteLine(e);  }
            
            
        }
        
    }
    
    
}