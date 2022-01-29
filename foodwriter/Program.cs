using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

/*
    20220128: Status
        Finished all of creating food list and writing to file
        Need to think about how to read these
            Reading them in just to display them really
        Also need to have a delete option
        Also need to have a modify option
        Need to modify code so it shows the list as its being created, so user can see


*/


namespace foodwriter
{
    public class FoodItem {
        public string name{ get; set; }
        public List<string> pros{ get; set; }
        public List<string> cons{ get; set; }
        public List<string> vits{ get; set; }
    }

    public class FoodGrouping {
        public string name{ get; set; }
        public List<FoodItem> foodItems{ get; set; }

    }

    class Program
    {
        static string listDir = "lists\\";
        static void Main(string[] args)
        {
            List<FoodItem> masterFoods = new List<FoodItem>();
            Console.WriteLine("Hi!");
            bool validResponse = false;
            while (!validResponse) { // Loop until proper 'y' or 'n' response
                Console.Write("Ready to start creating foods? (y/n): ");
                string response = Console.ReadLine();
                if (ResponseValid(response))
                    if (response.Equals("y")) {
                        validResponse = true;
                        
                        string listName = "";
                        bool validListName = false;
                        while (!validListName) {
                            Console.Write("What is the name of the food list?: ");
                            listName = Console.ReadLine();
                            if (listName.Length > 0) {
                                validListName = true;
                            } else {
                                Console.WriteLine("ERROR: Invalid response. Please try again. ");
                            }
                        }
                        bool doneCreatingFoodList = false;
                        while (!doneCreatingFoodList) {
                            masterFoods.Add(CreateFood());
                            Console.Write("Want to add another food? (y/n): ");
                            string sss = Console.ReadLine();
                            if (ResponseValid(sss)) {
                                if(sss.Equals("y")) {
                                    Console.WriteLine("Let's add another food. ");
                                } else if (sss.Equals("n")) {
                                    Console.WriteLine("Done adding foods.");
                                    doneCreatingFoodList = true;
                                    //Write out files list
                                    WriteList(listName, masterFoods);
                                } else {
                                    Console.WriteLine("ERROR: Invalid response. Please try again.");
                                }
                            }
                        }
                        
                    }
                    else { //"n": don't want to start creating foods :(
                        Console.WriteLine("Aw, no foods. :(");
                        validResponse = true;
                        return;
                    } 
                        
                else 
                    Console.WriteLine("ERROR: Invalid Response. Try Again.");
            }
        }

        static void WriteList(string listName, List<FoodItem> foods) {
            //Serialize food list in JSON and output
            string jsonString = JsonSerializer.Serialize(foods);
            Console.WriteLine(jsonString);
            string filename = listName + ".json";
            File.WriteAllText(Program.listDir + filename + ".json", jsonString);
            Console.WriteLine("Done writing food list to file. Thank you." );
        }

        static bool ResponseValid(string s) {
            s = s.ToLower();
            if (s.Equals("y") || s.Equals("n")) {
                return true;
            } 
            return false;
        }

        static FoodItem CreateFood() {
            //Create food object, maybe add to list?
            Console.WriteLine("It's food creatin' time!");
            string newName = GetName();
            List<string> newPros = GetPros();
            List<string> newCons = GetCons();
            List<string> newVits = GetVitamins();
            FoodItem newFoodItem = new FoodItem {
                name = newName,
                pros = newPros,
                cons = newCons,
                vits = newVits
            };
            return newFoodItem;
        }

        static string GetName() {
            string s = "";
            bool validResponse = false;
            while (!validResponse) {
                Console.Write("What's the name of the food?: ");
                s = Console.ReadLine();
                if (s.Length > 0)
                    validResponse = true;
            }
            return s;
        }

        static List<string> GetPros() {
            String s = "";
            bool doneYet = false;
            List<string> pros = new List<string>();
            bool validResponse;
            while (!doneYet) {
                validResponse = false;
                while (!validResponse) {
                    Console.Write("Enter a positive thing about this food item!: ");
                    s = Console.ReadLine();
                    if (s.Length > 0) {
                        validResponse = true;
                        pros.Add(s);
                    } else {
                        Console.WriteLine("ERROR: Invalid response. Please try again. ");
                    }
                }
                Console.WriteLine("Added item to Pros list");
                Console.Write("Would you like to add another positive thing? (y/n): ");
                string ss = Console.ReadLine();
                validResponse = false;
                while (!validResponse) {
                    if (ResponseValid(ss)) {
                        if(ss.Equals("n")) {
                            doneYet = true;
                            validResponse = true;
                        } else if (ss.Equals("y")) {
                            Console.WriteLine("Let's enter another positive thing about this food item. ");
                            validResponse = true;
                        } else {
                            Console.WriteLine("ERROR: Invalid response. Please try again. ");
                        }
                    }
                }
            }
            return pros;
        }

        static List<string> GetCons() {
            String s = "";
            bool doneYet = false;
            List<string> cons = new List<string>();
            bool validResponse;
            while (!doneYet) {
                validResponse = false;
                while (!validResponse) {
                    Console.Write("Enter a negative thing about this food item!: ");
                    s = Console.ReadLine();
                    if (s.Length > 0) {
                        validResponse = true;
                        cons.Add(s);
                    } else {
                        Console.WriteLine("ERROR: Invalid response. Please try again. ");
                    }
                }
                Console.WriteLine("Added item to Cons list");
                Console.Write("Would you like to add another negative thing? (y/n): ");
                string ss = Console.ReadLine();
                validResponse = false;
                while (!validResponse) {
                    if (ResponseValid(ss)) {
                        if(ss.Equals("n")) {
                            doneYet = true;
                            validResponse = true;
                        } else if (ss.Equals("y")) {
                            Console.WriteLine("Let's enter another negative thing about this food item. ");
                            validResponse = true;
                        } else {
                            Console.WriteLine("ERROR: Invalid response. Please try again. ");
                        }
                    }
                }
            }
            return cons;
        }

        static List<string> GetVitamins() {
            Console.WriteLine("Let's get the vitamins!");
            String s = "";
            bool doneYet = false;
            List<string> vits = new List<string>();
            bool validResponse;
            while (!doneYet) {
                validResponse = false;
                while (!validResponse) {
                    Console.Write("Enter a vitamin that this food item has. : ");
                    s = Console.ReadLine();
                    if (s.Length > 0) {
                        validResponse = true;
                        vits.Add(s);
                    } else {
                        Console.WriteLine("ERROR: Invalid response. Please try again. ");
                    }
                }
                Console.WriteLine("Added item to Vitamins list");
                Console.Write("Would you like to add another vitamin this food has? (y/n): ");
                string ss = Console.ReadLine();
                validResponse = false;
                while (!validResponse) {
                    if (ResponseValid(ss)) {
                        if(ss.Equals("n")) {
                            doneYet = true;
                            validResponse = true;
                        } else if (ss.Equals("y")) {
                            Console.WriteLine("Let's enter another vitamin that this food item has. ");
                            validResponse = true;
                        } else {
                            Console.WriteLine("ERROR: Invalid response. Please try again. ");
                        }
                    }
                }
            }
            return vits;
        }
    }
}
