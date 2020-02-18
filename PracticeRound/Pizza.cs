using System.Collections.Generic;

namespace PracticeRound
{
    public class Pizza
    {
        public readonly int pizzaNumber;
        public readonly int pizzaSize;

        public Pizza(int pizzaNumber, int pizzaSize)
        {
            this.pizzaNumber = pizzaNumber;
            this.pizzaSize = pizzaSize;
        }

        public override string ToString()
        {
            return pizzaNumber + ": " + pizzaSize;
        }


        protected bool Equals(Pizza other)
        {
            return pizzaNumber == other.pizzaNumber;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Pizza) obj);
        }

        public override int GetHashCode()
        {
            return pizzaNumber;
        }

        private sealed class PizzaNumberRelationalComparer : IComparer<Pizza>
        {
            public int Compare(Pizza x, Pizza y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (ReferenceEquals(null, y)) return 1;
                if (ReferenceEquals(null, x)) return -1;
                return x.pizzaNumber.CompareTo(y.pizzaNumber);
            }
        }

        public static IComparer<Pizza> PizzaNumberComparer { get; } = new PizzaNumberRelationalComparer();
    }
}