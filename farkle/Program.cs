using System;
using System.Collections.Generic;
using System.Threading;

namespace farkleapp
{
    class Program
    {
        static void Main(string[] args)
        {
            int numPlayers;
            List<int> playerScores;
            List<String> playerNames = new List<String>();

            Console.WriteLine("FARKLE");
            
            //Get number of players, player names, initialize score list
            PlayerSetup();
            
            //BEGIN GAME
            int turnNumber = 1;
            bool playing = true;
            while (playing) {
                for (int i = 0; i < numPlayers; i++) {
                    playerScores[i] += PlayerTurn(turnNumber, playerNames[i], playerScores[i]);
                }

                Console.Write("Tired of playing? (y/n): ");
                string stopPlaying = Console.ReadLine();
                stopPlaying = stopPlaying.Substring(0,1);
                stopPlaying.ToLower();
                if (stopPlaying.Equals("y")) {
                    playing = false;
                    Console.WriteLine("Hope you had fun. \nExiting...");
                }
                turnNumber += 1;
            }

            int DiceRoll() {
                Random rand = new Random();
                int diceRoll = rand.Next(1, 7);
                return diceRoll;
            }

            List<int> FarkleRoll(int numRolls) {
                List<int> farkleRoll = new List<int>();
                for (int i = 0; i < numRolls; i++) {
                    farkleRoll.Add(DiceRoll());
                }
                return farkleRoll;
            }

            List<String> GetPlayerNames(int num) {
                List<String> names = new List<String>();
                for (int i = 0; i < num; i++) {
                    Console.Write("\nEnter name of Player " + (i+1) + ": ");
                    string name = Console.ReadLine();
                    names.Add(name);
                }
                return names;
            }

            void PlayerSetup() {
                bool validPlayerInput = false;
                numPlayers = -1;
                while (!validPlayerInput)
                {
                    try
                    {
                        Console.Write("\nHow many players?: ");
                        String inputPlayers = Console.ReadLine();
                        numPlayers = int.Parse(inputPlayers);
                        //Console.WriteLine(numPlayers);
                        validPlayerInput = true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception caught: " + e);
                    }
                }
                Console.WriteLine("# Players Accepted!\n");
                Thread.Sleep(250);
                Console.WriteLine("Number of Players: " + numPlayers);
                Thread.Sleep(250);
                playerNames = new List<String>(GetPlayerNames(numPlayers));
                playerScores = new List<int>();
                for (int j = 0; j < numPlayers; j++)
                    playerScores.Add(0);

                Console.WriteLine("\nPlayers are set.\nLet's begin!!!");
            }

            int PlayerTurn(int turn, string player, int score) {
                Console.WriteLine("\n" + player + "'s turn!");
                Console.WriteLine("TURN: " + turn + ", PLAYER: " + player);
                Console.WriteLine(player + "'s current score is: " + score + ".");
                Console.WriteLine("Press Enter to roll!");
                Console.Read();
                List<int> turnRoll = new List<int>(FarkleRoll(6));
                Console.WriteLine(DisplayRoll(turnRoll));
                EvaluateRoll(turnRoll);

                return 0;
            }

            string DisplayRoll(List<int> roll) {
                roll.Sort();
                string preRow = " ";
                string topRow = "|";
                string midRow = "|";
                string lowRow = "|";

                string preRow1 = " 1  ";
                string topRow1 = "   |";
                string midRow1 = " o |";
                string lowRow1 = "   |";

                string preRow2 = " 2  ";
                string topRow2 = "o  |";
                string midRow2 = "   |";
                string lowRow2 = "  o|";

                string preRow3 = " 3  ";
                string topRow3 = "  o|";
                string midRow3 = " o |";
                string lowRow3 = "o  |";

                string preRow4 = " 4  ";
                string topRow4 = "o o|";
                string midRow4 = "   |";
                string lowRow4 = "o o|";

                string preRow5 = " 5  ";
                string topRow5 = "o o|";
                string midRow5 = " o |";
                string lowRow5 = "o o|";

                string preRow6 = " 6  ";
                string topRow6 = "o o|";
                string midRow6 = "o o|";
                string lowRow6 = "o o|";

                for (int i = 0; i < roll.Count; i++) {
                    if (roll[i].Equals(1)) {
                        preRow += preRow1;
                        topRow += topRow1;
                        midRow += midRow1;
                        lowRow += lowRow1;
                    } else if (roll[i].Equals(2)) {
                        preRow += preRow2;
                        topRow += topRow2;
                        midRow += midRow2;
                        lowRow += lowRow2;
                    } else if (roll[i].Equals(3)) {
                        preRow += preRow3;
                        topRow += topRow3;
                        midRow += midRow3;
                        lowRow += lowRow3;
                    } else if (roll[i].Equals(4)) {
                        preRow += preRow4;
                        topRow += topRow4;
                        midRow += midRow4;
                        lowRow += lowRow4;
                    } else if (roll[i].Equals(5)) {
                        preRow += preRow5;
                        topRow += topRow5;
                        midRow += midRow5;
                        lowRow += lowRow5;
                    } else if (roll[i].Equals(6)) {
                        preRow += preRow6;
                        topRow += topRow6;
                        midRow += midRow6;
                        lowRow += lowRow6;
                    } else {
                        string errString = "Error in Dice Roll. Not an int 1-6.";
                        return errString;
                    }
                }
                
                string result = preRow + "\n" + topRow + "\n" + midRow + "\n" + lowRow + "\n";
                return result;
            }

            void EvaluateRoll(List<int> roll) {
                List<string> choices = new List<string>();
                List<int> numOnes = new List<int>(CountNums(roll, 1));
                List<int> numFives = new List<int>(CountNums(roll, 5));
                List<int> numPairs = new List<int>(CountPairs(roll));
                List<int> numTrios = new List<int>(CountTrios(roll));

                Console.WriteLine("Ones: " + numOnes[0]);
                //todo: display dice images with 1 on them
                Console.WriteLine("Fives: " + numFives[0]);
                //todo: display dice images with 5 on them
                Console.WriteLine("Trios: " + numTrios[0]);
                //todo: display triples
                Console.WriteLine("3 Pairs?: " + ThreePairs(roll));
                //todo: display pairs
                Console.WriteLine("Straight (1-6)?: " + IsStraight(roll).ToString());
            }

            List<int> CountNums(List<int> roll, int key) {
                int numNums = 0;
                string indexString = "";
                for (int i = 0; i < roll.Count; i++) {
                    if (roll[i].Equals(key)) {
                        numNums++;
                        int nonZeroIndex = i+1;
                        indexString += nonZeroIndex.ToString();
                    }
                        
                }
                List<int> result = new List<int>();
                result.Add(numNums);
                if (indexString.Length > 0) {
                    result.Add(int.Parse(indexString));
                } else {
                    result.Add(0);
                }
                
                return result;
            }

            List<int> CountPairs(List<int> roll) {
                int numPairs = 0;
                List<int> result = new List<int>();
                string pairIndexes = "";
                List<bool> beenFound = new List<bool>();
                for (int i = 0; i < roll.Count; i++) {
                    beenFound.Add(false);
                }
                for (int i = 0; i < roll.Count; i++) {
                    for (int j = 1; j < roll.Count; j++) {
                        if (roll[i].Equals(roll[j]) && !beenFound[i] && !beenFound[j] && !i.Equals(j)) {
                            numPairs++;
                            int nonZeroI = i + 1;
                            int nonZeroJ = j + 1;
                            pairIndexes += nonZeroI.ToString();
                            pairIndexes += nonZeroJ.ToString();
                            beenFound[i] = true;
                            beenFound[j] = true;

                        }
                    }
                }
                result.Add(numPairs);
                if (pairIndexes.Length > 0) {
                    result.Add(int.Parse(pairIndexes));
                } else {
                    result.Add(0);
                }
                
                return result;
            }

            List<int> CountTrios(List<int> roll) {
                int numTrios = 0;
                List<int> result = new List<int>();
                string trioIndexes = "";
                List<bool> beenFound = new List<bool>();
                for (int i = 0; i < roll.Count; i++) {
                    beenFound.Add(false);
                }
                for (int i = 0; i < roll.Count; i++) {
                    for (int j = 1; j < roll.Count; j++) {
                        for (int k = 2; k < roll.Count; k++)
                        if (roll[i].Equals(roll[j]) && roll[j].Equals(roll[k]) && roll[i].Equals(roll[k]) && !beenFound[i] && !beenFound[j] && !beenFound[k] && !i.Equals(j) && !j.Equals(k) && !i.Equals(k) ) {
                            numTrios++;
                            int nonZeroI = i + 1;
                            int nonZeroJ = j + 1;
                            int nonZeroK = k + 1;
                            trioIndexes += nonZeroI.ToString();
                            trioIndexes += nonZeroJ.ToString();
                            trioIndexes += nonZeroK.ToString();
                            beenFound[i] = true;
                            beenFound[j] = true;
                            beenFound[k] = true;
                        }
                    }
                }
                result.Add(numTrios);
                if (trioIndexes.Length > 0) {
                    result.Add(int.Parse(trioIndexes));
                } else {
                    result.Add(0);
                }

                return result;
            }

            bool IsStraight(List<int> roll) {
                if (roll.Count.Equals(6) && CountPairs(roll)[0].Equals(0)) {
                    return true;
                } else {
                    return false;
                }
            }

            bool ThreePairs(List<int> roll) {
                if (roll.Count.Equals(6) && CountPairs(roll)[0].Equals(3))
                    return true;
                else
                    return false;
            }
        }
        
    }
}
