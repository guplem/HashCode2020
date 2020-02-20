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
                
                int bookCounter = 0;
                for (int d = 0; d < daysAvaliableInProblem - signUpTime; d++)
                    for (int b = 0; b < scannedBooksPerDay; b++)
                    {
                        try
                        {
                            totalScore += books.ElementAt(bookCounter).score;
                            bookCounter++;
                        } catch (ArgumentOutOfRangeException) { }
                    }

                return totalScore;
        }

        public Library(int id, int signUpTime, int scannedBooksPerDay, List<Book> booksInLibrary, int daysAvaliableInProblem)
        {
            this.id = id;
            this.signUpTime = signUpTime;
            this.scannedBooksPerDay = scannedBooksPerDay;
            this.books = booksInLibrary.ToArray().OrderByDescending(i => i).ToList();
            this.daysAvaliableInProblem = daysAvaliableInProblem;
        }

        public int CompareTo(Library other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return potentialScore.CompareTo(other.potentialScore);
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