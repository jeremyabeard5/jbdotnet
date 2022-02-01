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
        static string listDir = "sts\\";
        static List<FoodItem> masterFoods = new List<FoodItem>();
        static void Main(string[] args)
        {
            
            Console.WriteLine("\nHi!");

            DisplayCurrentFoods();

            int choice = DisplayFirstMenu();

            TakeFirstAction(choice);

            Console.WriteLine("Thanks for playing!");

        }

        static void CreateFoodList() {
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
                            Console.Write("\nWhat is the name of the food list?: ");
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

        static void TakeFirstAction(int c) {
            //c = choice
            if (c==1) {
                //Create
                CreateFoodList();
            } else if (c==2) {
                //Edit

            } else if (c==3) {
                //Delete

            } else if (c==4) {
                //Display
                DisplayFullFood();
            } else if (c==5) {
                //Quit
                Console.WriteLine("Quitting...");
                return;
                
            } else {
                Console.WriteLine("Invalid integer input. Please try again, choose 1, 2, 3, 4, or 5. Thank you.");
            }
        }

        static void DisplayFullFood() {
            FoodItem food = DisplayAndGetCurrentFood();
            
            Console.WriteLine("\nDisplaying Food: " + food.name);
            Console.WriteLine("  Food Pros:");
            for (int i = 0; i < food.pros.Count; i++) {
                Console.WriteLine("    " + food.pros[i]);
            }
            Console.WriteLine("  Food Cons: ");
            for (int i = 0; i < food.cons.Count; i++) {
                Console.WriteLine("    " + food.cons[i]);
            }
            Console.WriteLine("  Food Vitamins/Minerals: ");
            for (int i = 0; i < food.vits.Count; i++) {
                Console.WriteLine("    " + food.vits[i]);
            }
            
        }

        static string LoadJson(string listName) {
            Console.WriteLine("listName: " + listName);
            //string text1 = Path.Combine("lists", (listName + ".json"));
            string text1 = Path.Combine("C:", "Users", "jerem", "OneDrive");
            string text2 = Path.Combine("Documents", "Coding", "GitStuff", "jbdotnet");
            string text3 = Path.Combine("foodwriter", "sts", listName + ".json");
            string text = Path.Combine(text1, text2, text3);
            return File.ReadAllText("sts\\" + listName + ".json"); //"C:\\Users\\jerem\\OneDrive\\Documents\\Coding\\GitStuff\\jbdotnet\\foodwriter\\lists\\turkey.json"); //"lists\\turkey.json");//Program.listDir + listName + ".json");
        }

        static FoodItem DisplayAndGetCurrentFood() {
            int foodChoice = -1;
            Console.WriteLine("\nCurrent Foods Available");
            string[] allfiles = GetDirectory("sts");
            List<string> savedFoods = allfiles.ToList<string>();
            savedFoods = RemoveFilePaths(savedFoods);
            savedFoods = RemoveExtensions(savedFoods);
            for (int i = 0; i < savedFoods.Count; i++) {
                Console.WriteLine((i+1) + ". " + savedFoods[i]);
            }

            Console.Write("Enter a number (1-" + savedFoods.Count + ") to choose a food: ");
            string r = Console.ReadLine();
            foodChoice = IntResponseValid(r)-1;

            string listToLoad = savedFoods[foodChoice];
            string jsonList = LoadJson(listToLoad); 
            FoodItem food = JsonSerializer.Deserialize<FoodItem>(jsonList);

            return food;
        }

        static void DisplayCurrentFoods() {
            Console.WriteLine("\nCurrent Foods Available");
            string[] allfiles = GetDirectory("sts");
            List<string> savedFoods = allfiles.ToList<string>();
            savedFoods = RemoveFilePaths(savedFoods);
            savedFoods = RemoveExtensions(savedFoods);
            for (int i = 0; i < savedFoods.Count; i++) {
                Console.WriteLine((i+1) + ". " + savedFoods[i]);
            }
        }

        static string[] GetDirectory(string dir) {
            return Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);
        }

        static List<string> RemoveFilePaths(List<string> filesAndPaths) {
            List<string> justFiles = new List<string>();
            foreach (string fileAndPath in filesAndPaths) {
                justFiles.Add(fileAndPath.Substring(fileAndPath.LastIndexOf("\\")+1));
            }
            return justFiles;
        }

        static List<string> RemoveExtensions(List<string> filesAndExtensions) {
            List<string> filesResult = new List<string>();
            foreach (var fileAndExtension in filesAndExtensions) {
                string justFile = fileAndExtension.Split('.')[0];
                filesResult.Add(justFile);
                //Console.WriteLine(justFile);
            }
            return filesResult;
        }

        static int DisplayFirstMenu() {
            
            Console.WriteLine("\nWhat would you like to do? ");
            Console.WriteLine("### 1. Create new foods");
            Console.WriteLine("### 2. Edit existing food");
            Console.WriteLine("### 3. Delete existing food");
            Console.WriteLine("### 4. Display existing food");
            Console.WriteLine("### 5. Quit. ");
            Console.Write("### What's your choice? (1-5): ");

            string r = Console.ReadLine();
            int choice = IntResponseValid(r);
            
            if (choice==1) {
                //Create new food
                Console.WriteLine("Let's create new food objects");
            } else if (choice==2) {
                //Edit existing food
                Console.WriteLine("Let's edit an existing food object");
            } else if (choice==3) {
                //Delete existing food
                Console.WriteLine("Let's delete an existing food objects");
            } else if (choice==4) {
                // Display existing food
                Console.WriteLine("Let's display an existing food objects");
            } else if (choice==5) {
                // Exit
                Console.WriteLine("Let's exit");
            } else {
                Console.WriteLine("ERROR: Invalid menu choice. Please try again.");
            }
            

            return choice;
        }

        static void WriteList(string listName, List<FoodItem> foods) {
            //Serialize food list in JSON and output
            foreach (var food in foods) {
                string jsonString = JsonSerializer.Serialize(food);
                Console.WriteLine(jsonString);
                string filename = RemoveSpaces(food.name) + ".json";
                File.WriteAllText(Program.listDir + filename, jsonString);
            }
            //string jsonString = JsonSerializer.Serialize(foods);
            //Console.WriteLine(jsonString);
            //string filename = listName + ".json";
            //File.WriteAllText(Program.listDir + filename + ".json", jsonString);
            Console.WriteLine("Done writing food list to file. Thank you." );
        }

        static string RemoveSpaces(string s) {
            return string.Concat(s.Where(c => !char.IsWhiteSpace(c)));
        }

        static bool ResponseValid(string s) {
            s = s.ToLower();
            if (s.Equals("y") || s.Equals("n")) {
                return true;
            } 
            return false;
        }

        static int IntResponseValid(string s) {
            int response = -1;
            bool validIntResponse = false;
            while (!validIntResponse) {
                try {
                    response = int.Parse(s);
                    validIntResponse = true;
                } catch (Exception e) {
                    Console.WriteLine("ERROR: Invalid integer input");
                    Console.WriteLine(e.ToString());
                    Console.WriteLine("Try again");
                }
            }
            
            return response;
        }

        static FoodItem CreateFood() {
            //Create food object, maybe add to list?
            Console.WriteLine("\nIt's food creatin' time!");
            string newName = GetName();
            List<string> newVits = GetVitamins();
            List<string> newCons = GetCons();
            List<string> newPros = GetPros();
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
                    Console.Write("\nEnter a non-vitmin-related positive thing about this food item!: ");
                    s = Console.ReadLine();
                    if (s.Length > 0) {
                        validResponse = true;
                        pros.Add(s);
                    } else {
                        Console.WriteLine("ERROR: Invalid response. Please try again. ");
                    }
                }
                Console.WriteLine("Added item to Pros list");
                
                validResponse = false;
                while (!validResponse) {
                    Console.Write("\nWould you like to add another positive thing? (y/n): ");
                    string ss = Console.ReadLine();
                    if (ResponseValid(ss)) {
                        if(ss.Equals("n")) {
                            doneYet = true;
                            validResponse = true;
                        } else if (ss.Equals("y")) {
                            Console.WriteLine("\nLet's enter another non-vitamin-related positive thing about this food item. ");
                            validResponse = true;
                        } else {
                            Console.WriteLine("ERROR: Invalid response. Please try again. ");
                        }
                    } else {
                        Console.WriteLine("ERROR: Invalid response. Please try again.");
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
            Console.WriteLine("Let's get the vitamins/minerals!");
            String s = "";
            bool doneYet = false;
            List<string> vits = new List<string>();
            bool validResponse;
            while (!doneYet) {
                validResponse = false;
                while (!validResponse) {
                    Console.Write("Enter a vitamin/mineral that this food item has. : ");
                    s = Console.ReadLine();
                    if (s.Length > 0) {
                        validResponse = true;
                        vits.Add(s);
                    } else {
                        Console.WriteLine("ERROR: Invalid response. Please try again. ");
                    }
                }
                Console.WriteLine("Added item to Vitamins/Minerals list");
                Console.Write("Would you like to add another vitamin/mineral this food has? (y/n): ");
                string ss = Console.ReadLine();
                validResponse = false;
                while (!validResponse) {
                    if (ResponseValid(ss)) {
                        if(ss.Equals("n")) {
                            doneYet = true;
                            validResponse = true;
                        } else if (ss.Equals("y")) {
                            Console.WriteLine("Let's enter another vitamin/mineral that this food item has. ");
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
