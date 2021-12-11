using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace MovieLog
{
    public class MovieList {
        public string listname { get; set; }
        public string creator { get; set; }
        public string directory { get; set; }
        public List<Movie> items { get; set; }
        
    }


    public class Movie {
        public string filename { get; set; }
        public string filepath { get; set; }
        public bool watched { get; set; }

    }

    

    public class Program
    {
        //String moviedir = "F:\\Movies";
        static string moviedir = "C:\\Users\\jerem\\Videos\\JB Temp Project Files";
        static string listDir = "lists\\";
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("Movie Log");
            Console.WriteLine("Welcome to movie remembrall (C)");
            
            bool choiceValid = false;
            int choiceInt = -1;
            while (!choiceValid) {
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("   1. Create New Movie List");
                Console.WriteLine("   2. Load Movie List");
                Console.WriteLine("   3. Exit");
                Console.Write("Choice: ");
                try {
                    string choice = Console.ReadLine();
                    if (choice.Equals("1") || choice.Equals("2") || choice.Equals("3")) {
                        choiceValid = true;
                        choiceInt = int.Parse(choice);
                    } else {
                        Console.WriteLine("ERROR: Invalid input. Enter 1, 2, or 3.");
                    }
                } catch (Exception e) {
                    Console.WriteLine(e.ToString());
                }
            }

            if (choiceInt.Equals(1)) {
                Console.WriteLine("Choice 1 selected.");
                Console.WriteLine("Let's build a movie list together.");
                BuildList();
            } else if (choiceInt.Equals(2)) {
                Console.WriteLine("Choice 2 selected.");
                Console.WriteLine("Let's load an existing movie list.");
                LoadList();
            } else {
                //exit
                Console.WriteLine("\nFinished.");
            }

        }

        static void BuildList() {
            Console.WriteLine("What is your name?");
            Console.Write("Creator Name: ");
            string creatorname = Console.ReadLine();
            if (creatorname.Length < 1)
                creatorname = "NO CREATOR NAME";

            Console.WriteLine("Thank you " + creatorname + ".");
            Console.WriteLine("What do you want to call your list?");
            Console.Write("List Name: ");
            string listkey = Console.ReadLine();
            if (listkey.Length < 1)
                listkey = "NO LIST NAME";

            List<string> desiredFiles = new List<string>();
            List<string> desiredFilesOnly = new List<String>();
            List<string>[] directoryOutput = new List<string>[2];
            directoryOutput = ReadFromDirectory();
            desiredFiles = directoryOutput[0];
            desiredFilesOnly = directoryOutput[1];

            List<Movie> movies = new List<Movie>();
            List<bool> watchedAlready = new List<bool>();
            for (int i = 0; i < desiredFilesOnly.Count; i++) { 
                bool watchedYet = GetMovie(desiredFilesOnly[i], i, desiredFilesOnly.Count);
                Movie newMovie = new Movie { //Create new movie object based on user input if they've watched it
                    filename = desiredFilesOnly[i],
                    filepath = desiredFiles[i],
                    watched = watchedYet
                };
                //Add new movie object to movie list
                movies.Add(newMovie);
            }
            MovieList movieListObject = new MovieList {
                listname = listkey,
                creator = creatorname,
                directory = moviedir,
                items = movies
            };
            //Now movies list is complete, can write to json 
            string jsonString = JsonSerializer.Serialize(movieListObject);
            Console.WriteLine(jsonString);
            string filename = listkey + ".json";
            //File.WriteAllText((moviedir + "\\" + filename), jsonString);
            SaveJson(filename, jsonString);
            //File.WriteAllText("lists\\" + filename, jsonString);
            Console.WriteLine("\nDone creating movie list: " + listkey + ". THank you!");
        }

        static void LoadList() {
            Console.WriteLine("Getting current lists...");
            string[] allfiles = GetDirectory("lists");
            List<string> savedLists = allfiles.ToList<string>();
            savedLists = RemoveFilePaths(savedLists);
            savedLists = RemoveExtensions(savedLists);
            Console.WriteLine("\nSAVED LISTS");
            for (int i = 0; i < savedLists.Count; i++) {
                Console.WriteLine((i+1) + ". " + savedLists[i]);
            }

            Console.WriteLine("\n Enter name of list to load.");
            Console.Write("List name: ");
            string listToLoad = Console.ReadLine();
            string jsonList = LoadJson(listToLoad); 
            MovieList movieList = JsonSerializer.Deserialize<MovieList>(jsonList);
            List<Movie> oldLoadedList = movieList.items;
            

            Console.WriteLine("Thank you.");
            Console.WriteLine("List " + listToLoad + " loaded. Found " + movieList.items.Count + " movies.");
            Console.WriteLine("List Contents:");
            for (int i = 0; i < movieList.items.Count; i++) {
                Console.WriteLine((i+1) + ". " + movieList.items[i].filename);
                Console.WriteLine("   Watched Already: " + movieList.items[i].watched);
            }


            bool choiceValid = false;
            int choiceInt = -1;
            while (!choiceValid) {
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("1. Scan directory for new movies");
                Console.WriteLine("2. Mark movie as watched/not watched");
                Console.WriteLine("3. Display List Contents");
                Console.WriteLine("4. Done.");
                Console.Write("Choice: ");
                try {
                    string choice = Console.ReadLine();
                    if (choice.Equals("1") || choice.Equals("2") || choice.Equals("3")) {
                        choiceValid = true;
                        choiceInt = int.Parse(choice);
                    } else {
                        Console.WriteLine("ERROR: Invalid input. Enter 1, 2, or 3.");
                    }
                } catch (Exception e) {
                    Console.WriteLine(e.ToString());
                }
            }

            if (choiceInt.Equals(1)) {
                List<Movie> newMovieList = new List<Movie>();
                Console.WriteLine("Choice 1 selected.");
                Console.WriteLine("Scanning for new movies...");
                Console.WriteLine("WARNING: This may remove any items which are not in directory anymore.");
                //Scan for any new movies based on string
                List<string>[] directoryContents = new List<string>[2];
                //directoryContents[1] matches with movie.filename
                List<bool> alreadyExistsInOldList = new List<bool>();
                directoryContents = ReadFromDirectory(); //0: desiredFiles 1: desiredFilesOnly
                //go thru every element of the new scan of directory (directoryContents), and check if it already exists in the old scan (oldLoadedList)
                //compile List<bool> that corresponds with new scan (directoryContents), with true if item already exists
                //after this, will go thru all false items and ask user if they watched
                for (int i = 0; i < directoryContents[1].Count; i++) {
                    bool found = false;
                    for (int j = 0; j < oldLoadedList.Count; j++) {
                        Console.WriteLine(directoryContents[1][i]);
                        Console.WriteLine(oldLoadedList[j].filename);
                        if (directoryContents[1][i].Equals(oldLoadedList[j].filename)) {
                            alreadyExistsInOldList.Add(true);
                            found = true;
                        } 
                    }
                    if (!found) {
                        alreadyExistsInOldList.Add(false);
                    }
                }

                List<string> moviesToAdd = new List<string>();
                List<int> indexesOfMoviesToAdd = new List<int>();
                for (int i = 0; i < alreadyExistsInOldList.Count; i++) {
                    if (!alreadyExistsInOldList[i]) {
                        moviesToAdd.Add(directoryContents[1][i]);
                        indexesOfMoviesToAdd.Add(i);
                    } else {
                        newMovieList.Add(oldLoadedList[i]);
                    }
                }
                
                
                //prompt for watch status of movies
                Console.WriteLine("NEW FILES TO BE PROCESSED");
                for (int i = 0; i < moviesToAdd.Count; i++) {
                    Console.WriteLine((i+1) + ": " + moviesToAdd[i]);
                }

                //moviesToAdd has same length as indexesOfMoviesToAdd
                for (int i = 0; i < moviesToAdd.Count; i++) {
                    bool movieWatched = GetMovie(moviesToAdd[i], i, moviesToAdd.Count);
                    Movie updatedMovie = new Movie {
                        filename = moviesToAdd[i],
                        filepath = directoryContents[0][indexesOfMoviesToAdd[i]],
                        watched = movieWatched
                    };
                    newMovieList.Add(updatedMovie);
                }
                
                //Now all new movies have been added 
                newMovieList.Sort((x, y) => x.filename.CompareTo(y.filename));

                //save list
                MovieList movieListObject = new MovieList {
                    listname = movieList.listname,
                    creator = movieList.creator,
                    directory = movieList.directory,
                    items = newMovieList
                };
                //Now movies list is complete, can write to json 
                string jsonString = JsonSerializer.Serialize(movieListObject);
                Console.WriteLine(jsonString);
                string filename = movieList.listname + ".json";
                //File.WriteAllText((moviedir + "\\" + filename), jsonString);
                SaveJson(filename, jsonString);
                //File.WriteAllText("lists\\" + filename, jsonString);
                Console.WriteLine("\nDone refreshing movie list: " + movieList.listname + ". Thank you!");


            } else if (choiceInt.Equals(2)) {
                Console.WriteLine("Choice 2 selected.");
                Console.WriteLine("Which movie to edit status of?");
                //Display all movies from list
                for (int i = 0; i < oldLoadedList.Count; i++) {
                    Console.WriteLine((i+1) + ". " + oldLoadedList[i].filename);
                }
                //Prompt for which movie (based on index/#) to edit
                Console.Write("\nEnter number of movie to change status: ");
                string movieIndexStr = Console.ReadLine();
                int movieIndex = int.Parse(movieIndexStr);
                movieIndex--;
                //ADDD EERRRROR CHECKKKING
                Movie movieToChange = oldLoadedList[movieIndex];
                if (movieToChange.watched) {
                    Console.WriteLine("Movie " + movieToChange.filename + " has been watched.");
                    Console.Write("Change to not watched? (y/n): ");
                    string confirm = Console.ReadLine();
                    //add error checking
                    if (confirm.ToLower().Equals("y")) {
                        oldLoadedList[movieIndex].watched = false;
                    }
                } else {
                    Console.WriteLine("Movie " + movieToChange.filename + " has NOT been watched.");
                    Console.Write("Change to watched? (y/n): ");
                    string confirm = Console.ReadLine();
                    //add error checking
                    if (confirm.ToLower().Equals("y")) {
                        oldLoadedList[movieIndex].watched = true;
                    }
                }
                                
                //Save file again
                MovieList movieListObject = new MovieList {
                    listname = movieList.listname,
                    creator = movieList.creator,
                    directory = movieList.directory,
                    items = oldLoadedList
                };
                //Now movies list is complete, can write to json 
                string jsonString = JsonSerializer.Serialize(movieListObject);
                Console.WriteLine(jsonString);
                string filename = movieList.listname + ".json";
                //File.WriteAllText((moviedir + "\\" + filename), jsonString);
                SaveJson(filename, jsonString);
                //File.WriteAllText("lists\\" + filename, jsonString);
                Console.WriteLine("\nDone editing item in movie list: " + movieList.listname + ". Thank you!");

                
            } else if (choiceInt.Equals(3)) {
                Console.WriteLine("Choice 3 Selected");
                Console.WriteLine("Displaying List " + movieList.listname);
                for (int i = 0; i < movieList.items.Count; i++) {
                    Console.WriteLine((i+1) + ". " + movieList.items[i].filename);
                    Console.WriteLine("   Watched Already: " + movieList.items[i].watched);
                }
                
            } else {
                //exit
                Console.WriteLine("\nFinished.");
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
                Console.WriteLine(justFile);
            }
            return filesResult;
        }

        static void SaveJson(string filename, string jsonText) {
            File.WriteAllText(Program.listDir + filename, jsonText);
        }

        static string LoadJson(string listName) {
            return File.ReadAllText(Program.listDir + listName + ".json");
        }

        static List<string>[] ReadFromDirectory() {
            

            Console.WriteLine(moviedir);
            Console.WriteLine("Reading files from following directory:\n" + moviedir + "\n");
            //Get array of all files in drive/directory
            string[] allfiles = GetDirectory(moviedir);
            //initiate list for file extensions
            List<string> fileExtensions = new List<string>();
            for (int i = 0; i < allfiles.Length; i++) {
                //Print all files, build file extension list
                Console.WriteLine(i + ": " + allfiles[i]);
                //Console.WriteLine(allfiles[i].Substring(allfiles[i].LastIndexOf(".")));
                fileExtensions.Add(allfiles[i].Substring(allfiles[i].LastIndexOf(".")).ToLower());
            }
            //Make file extension list unique
            Console.WriteLine("\nFile Extensions Present in Directory:");
            List<string> uniqueFileExtensions = fileExtensions.Distinct().ToList();
            foreach (string fileExtension in uniqueFileExtensions) {
                Console.WriteLine(fileExtension);
            }

            //Create list of desired file extensions to grab
            List<string> videoFileExtensions = new List<string> {".mp4", ".avi", ".mkv", ".m4v", ".mov"};

            //Get all files which are of desired file extension
            List<string> desiredFiles = new List<string>();
            for (int i = 0; i < allfiles.Length; i++) {
                for (int j = 0; j < videoFileExtensions.Count; j++) {
                    if (fileExtensions[i].Equals(videoFileExtensions[j])) {
                        desiredFiles.Add(allfiles[i]);
                    }
                }
            }

            //Print all desired files, full file path
            Console.WriteLine("\n\nDESIRED FILES");
            for (int i = 0; i < desiredFiles.Count; i++) {
                Console.WriteLine(i + ": " + desiredFiles[i]);
            }


            //Build list of just filenames, print all desired files, just filenames
            List<string> desiredFilesOnly = RemoveFilePaths(desiredFiles); 
            Console.WriteLine("\n\nDESIRED FILES, FILENAMES ONLY");
            for (int i = 0; i < desiredFilesOnly.Count; i++) {
                Console.WriteLine(i + ": " + desiredFilesOnly[i]);
            }

            List<string>[] result = new List<string>[2];
            result[0] = desiredFiles;
            result[1] = desiredFilesOnly;
            return result;
        }

        static bool GetMovie(string movieString, int currentIndex, int totalCount) {
            string input = "";
            bool validInput = false;
            while (!validInput) {
                Console.WriteLine("\nMovie File(" + (currentIndex+1).ToString() + "/" + totalCount + "): " + movieString);
                Console.Write("Have you watched this movie yet? (y/n): ");
                input = Console.ReadLine();
                if (input.Length > 0) {
                    if (input.Substring(0,1).ToLower().Equals("y") || input.Substring(0,1).ToLower().Equals("n")) {
                        validInput = true;
                    } 
                }
                if (!validInput)
                    Console.WriteLine("**Invalid input. Please enter y or n**");
            }

            bool watchedAlready = false;
            if (input.Substring(0, 1).ToLower().Equals("y")) {
                watchedAlready = true;
            } else {
                watchedAlready = false;
            } 
            return watchedAlready;
        }
    }
}
