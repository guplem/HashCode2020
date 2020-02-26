using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineQualificationRound
{
    public class Library : IComparable<Library>
    {
        public readonly int id;
        public readonly int signUpTime;
        public readonly int scannedBooksPerDay;
        public List<Book> books { get; private set; }
        
        private readonly int daysAvaliableInProblem;
        
        private int _potentialScore = -1;
        public int potentialScore
        {
            get
            {
                if (_potentialScore < 0)
                    _potentialScore = CalculatePotentialScore();
                return _potentialScore;
            }
        }

        private int CalculatePotentialScore()
        {
            int totalScore = 0;
            int availableDays = daysAvaliableInProblem - signUpTime;
            int bookCounter = 0;
            for (int d = 0; d < availableDays; d++)
            {
                for (int b = 0; b < scannedBooksPerDay; b++)
                {
                    totalScore += books.ElementAt(bookCounter).score;
                    bookCounter++;
                    
                    if (bookCounter >= books.Count)
                        return totalScore;
                }
            }


            return totalScore;
        }

        public Library(int id, int signUpTime, int scannedBooksPerDay, List<Book> booksInLibrary, int daysAvaliableInProblem)
        {
            this.id = id;
            this.signUpTime = signUpTime;
            this.scannedBooksPerDay = scannedBooksPerDay;
            UpdateBooksTo(booksInLibrary);
            this.daysAvaliableInProblem = daysAvaliableInProblem;
        }

        public void UpdateBooksTo(List<Book> newBooksList)
        {
            books = new List<Book>(newBooksList.ToArray().OrderByDescending(i => i));
            _potentialScore = -1;
        }
        
        public int CompareTo(Library other)
        {
            return potentialScore - other.potentialScore;
        }

        protected bool Equals(Library other)
        {
            return id == other.id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Library) obj);
        }

        public override int GetHashCode()
        {
            return id;
        }
    }
}