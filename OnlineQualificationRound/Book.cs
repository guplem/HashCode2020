using System;
using System.Collections.Generic;

namespace OnlineQualificationRound
{
    public class Book : IComparable<Book>
    {
        public readonly int id;
        public readonly int score;
        
        public Book(int id, int score)
        {
            this.id = id;
            this.score = score;
        }

        protected bool Equals(Book other)
        {
            return id == other.id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Book) obj);
        }

        public override int GetHashCode()
        {
            return id;
        }
        
        public int CompareTo(Book other)
        {
            return score-other.score;
        }
    }
}