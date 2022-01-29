using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace foodstuff
{
    public class FoodItem {
        public string name{ get; set; }
        public List<string> pros{ get; set; }
        public List<string> cons{ get; set; }
        public List<string> vits{ get; set; }

    }

    class Program
    {
        public List<foodItem> foods;



        static void Main(string[] args)
        {
            int choice = -1;
            Console.WriteLine("Meals...");
            choice = PrintMenu();
            if ( choice == 1 ) {
                return;
            } else if ( choice == 2 ) {
                //Make new Meal
                BuildMeal();
            }
        }

        static int PrintMenu() {
            Console.WriteLine("#############################");
            Console.WriteLine("###### Choose: ##############");
            Console.WriteLine("### 1. Enter new meal #######");
            Console.WriteLine("### 2. Enter NEW food item. #")
            Console.WriteLine("### 2. Exit. ################");
            Console.Write("### Enter choice (1, 2, ...): ");
            string response = Console.ReadLine();
            int choice = -1;
            try {
                choice = int.Parse(response);
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
            return choice;
        }

        static void BuildMeal() {
            Console.WriteLine("Let's build a meal.");
            Console.WriteLine("Printing current foods: ");
            ListFoods();
        }

        static void ListFoods() {

        }
    }

    
}
