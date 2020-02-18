using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace PizzaProblem
{
    public class Solution
    {
        public readonly int desiredSlices;
        public int diference { get { return desiredSlices - slices; } private set { } }
        private List<Pizza> pizzas = new List<Pizza>();
        
        public string pizzasString {
            get {             
                List<Pizza> sortedList = GetSortedPizzaList();
                return sortedList.Aggregate("", (current, pizza) => current + (pizza.pizzaNumber + " "));
            }
        }
        public int qttyOfPizzas
        {
            get { return pizzas.Count; }
        }
        public int slices { get ; private set; }

        public Solution(List<Pizza> pizzas, int desiredSlices)
        {
            this.pizzas = new List<Pizza>(pizzas);
            this.desiredSlices = desiredSlices;
            slices = GetNumberOfSlices();
        }

        public int GetNumberOfSlices()
        {
            if (pizzas == null)
                return 0;

            int count = 0;
            foreach (Pizza pizza in pizzas)
                count += pizza.pizzaSize;
            return count;
        }

        public override string ToString()
        {
            return "Slices: " + slices.ToString();
        }

        public bool AddPizza(Pizza pizza)
        {
            if (pizzas.Contains(pizza))
                return false;
            
            pizzas.Add(pizza);
            slices = GetNumberOfSlices();
            return true;
        }
        
        public bool RemovePizza(Pizza pizza)
        {
            if (!pizzas.Contains(pizza))
                return false;
            
            bool ret = pizzas.Remove(pizza);
            slices = GetNumberOfSlices();
            return ret;
        }


        public Solution GetMutation(List<Pizza> possiblePizzasToAdd)
        {
            List<Pizza> newPizzaList = new List<Pizza>(pizzas);
            Solution newSolution = new Solution(newPizzaList, desiredSlices);

            int pizzasToRemove = Program.rnd.Next(pizzas.Count+1);
            //Console.WriteLine("MUTATION removing " + pizzasToRemove + " pizzas. ");
            List<Pizza> removedPizzas = new List<Pizza>();
            for (int i = 0; i < pizzasToRemove; i++)
            {
                removedPizzas.Add(newSolution.pizzas.ElementAt(Program.rnd.Next(0, newSolution.pizzas.Count)));
                newSolution.pizzas.Remove(removedPizzas.ElementAt(removedPizzas.Count-1));
            }

            return Program.GetSolution0(desiredSlices, Program.pizzasInProblem, newSolution.pizzas, removedPizzas);
        }

        public bool Contains(Pizza pizza)
        {
            return pizzas.Contains(pizza);
        }

        public List<Pizza> GetSortedPizzaList()
        {
            return pizzas.OrderBy(p => p.pizzaNumber).ToList();
        }

        public bool IsValidSolution()
        {
            return slices <= desiredSlices;
        }
    }
}