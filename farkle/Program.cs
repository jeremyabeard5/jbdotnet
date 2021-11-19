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
                    if (playerScores[i] > 10000) {
                        Console.WriteLine("GAME OVER! Player " + i + ", " + playerNames[i] + " won! With a score of " + playerScores[i] + ". Thanks for playing!");
                        playing = false;
                    }
                    if (playing) {
                        playerScores[i] += PlayerTurn(turnNumber, playerNames[i], playerScores[i]);
                    }
                    
                }

                Console.Write("\nTired of playing? (y/n): ");
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
                //The following 6 lines of code are for hard-setting a roll for playtesting
                //farkleRoll[0] = 1; //For Debugging
                //farkleRoll[1] = 4; //For Debugging
                //farkleRoll[2] = 4; //For Debugging
                //farkleRoll[3] = 4;//For Debugging
                //farkleRoll[4] = ;//For Debugging
                //farkleRoll[5] = ;//For Debugging
                farkleRoll.Sort();
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

            int PlayerTurn(int turn, string player, int score)
            {
                Console.WriteLine("\n" + player + "'s turn!");
                Console.WriteLine("TURN: " + turn + ", PLAYER: " + player);
                Console.WriteLine(player + "'s current score is: " + score + ".");
                Console.WriteLine("Press Enter to roll!");
                Console.Read();
                int turnScore = 0;
                int numDice = 6;
                List<int> turnRoll = new List<int>();
                bool playerPlaying = true;
                while (playerPlaying)
                {
                    Console.WriteLine("\nTURN: " + turn + ", PLAYER: " + player);
                    Console.WriteLine(player + "'s score before this turn is: " + score + ".");
                    Console.WriteLine(player + "'s banked points this turn is: " + turnScore + ".");
                    turnRoll = new List<int>(FarkleRoll(numDice));

                    Console.WriteLine(DisplayRoll(turnRoll));

                    List<int> choices = new List<int>();
                    choices = EvaluateRoll(turnRoll); //(ex. [3, 2, 1, 0, 0])

                    List<String> letras = new List<String>();
                    letras.Add("A");
                    letras.Add("B");
                    letras.Add("C");
                    letras.Add("D");
                    letras.Add("E");

                    List<String> choicesString = new List<String>();
                    choicesString.Add("1's");
                    choicesString.Add("5's");
                    choicesString.Add("Trios");
                    choicesString.Add("Three Pairs");
                    choicesString.Add("Straight (1-6)");

                    //Populate list of possible choices (choiceOptions) for scores to bank
                    int letterIndex = 0;
                    List<String> choiceOptions = new List<String>();
                    for (int i = 0; i < choices.Count; i++)
                    {
                        if (choices[i] > 0)
                        {
                            choiceOptions.Add(choicesString[i]);
                            letterIndex++;
                        }
                    }

                    //If there are no options, then you farkle.
                    if (choiceOptions.Count.Equals(0))
                    {
                        Console.WriteLine("\nFARKLE FARKLE FARKLE FARKLE FARKLE FARKLE\n");
                        Console.WriteLine("You FARKLED. No options to bank!");
                        Console.WriteLine("You scored 0 for this turn. Total Score: " + score + ". Next player's turn!");
                        return 0;
                    }

                    bool validChoice = false;
                    string choice = "";
                    while (!validChoice)
                    {
                        Console.WriteLine("\nWhat would you like to take for scoring? (Enter one letter for choice)");
                        for (int i = 0; i < choiceOptions.Count; i++)
                        {
                            Console.WriteLine(letras[i] + ". " + choiceOptions[i]);
                        }
                        Console.Write("\nChoice: ");
                        choice = Console.ReadLine();
                        choice = choice.ToUpper();

                        //ADD ERROR CHECKING
                        validChoice = true;

                        if (!validChoice)
                        {
                            Console.WriteLine("ERROR: Invalid selection. Enter one letter for choice. Try again.");
                        }
                    }
                    //User Selection validated
                    //Now determine score based on choice
                    //choice list's maximum length is length of choiceOptions list 
                    
                    List<String> choiceList = new List<String>();
                    for (int i = 0; i < choice.Length; i++) {
                        choiceList.Add(choice.Substring(i,1));
                    }
                    

                    //letra : full list of letters
                    //choicesString : full list of possible farkle bank options
                    //choiceOptions : based on the roll, current farkle bank options available
                        //letra[i] corresponds with choiceOptions[i] but they have different lengths most cases
                    //choiceList : list of what user chose from choiceOptions, 
                    int choiceIndex = -1;
                    for (int numChc = 0; numChc < choiceList.Count; numChc++) {
                        int currentBankScore = 0;
                        string currentChoiceLetter = choiceList[numChc];
                        string currentChoiceText = "";
                        for (int i = 0; i < choiceOptions.Count; i++) {
                            if (currentChoiceLetter.Equals(letras[i])) {
                                choiceIndex = i;
                                currentChoiceText = choiceOptions[i];
                                Console.WriteLine("Choice: " + letras[choiceIndex] + ". " + choiceOptions[choiceIndex]);
                            }
                        }

                        if (currentChoiceText.Equals("1's")) {
                            //choices = EvaluateRoll
                            

                            
                            int bankedOnes = 1;
                            List<int> oneSummary = CountNums(turnRoll, 1);
                            if (oneSummary[0] > 1) {
                                bool validInput = false;
                                while (!validInput) {
                                    Console.Write("How many 1's will you bank? (Max " + choices[0] + "): ");
                                    string bankInput = Console.ReadLine();
                                    //ERROR CHECKING
                                    try {
                                        bankedOnes = int.Parse(bankInput);
                                    } catch (Exception e) {
                                        Console.WriteLine("ERROR: Invalid input.");
                                    }
                                    if (bankedOnes < 0 || bankedOnes > choices[0]) {
                                        Console.WriteLine("ERROR: Selection is too many or too few 1's ");
                                    } else {
                                        validInput = true;
                                    }
                                }
                            }
                            //now we have bankedOnes, which is how many 1's user wants to bank
                            
                            currentBankScore = ScoreNums(1, choices, turnRoll, bankedOnes);
                            Console.WriteLine("Banked " + bankedOnes + " 1's.");
                            List<int> numOnes = new List<int>(CountNums(turnRoll, 1));
                            string oneIndexesStr = numOnes[1].ToString();
                            List<int> oneIndexes = new List<int>();
                            for (int i = 0; i < oneIndexesStr.Length; i++) {
                                oneIndexes.Add(int.Parse(oneIndexesStr.Substring(i,1)) - 1);
                            }
                            int j = oneIndexes.Count-1;
                            for ( int i = 0; i < bankedOnes; i++) {
                                turnRoll.RemoveAt(oneIndexes[j]);
                                numDice--;
                                j--;
                            }

                            
                        }
                        else if (currentChoiceText.Equals("5's")) {
                            bool validInput = false;
                            int bankedFives = 1;
                            while (!validInput) {
                                if (choices[1] > 1) {
                                    Console.Write("How many 5's will you bank? (Max " + choices[1] + "): ");
                                    string bankInput = Console.ReadLine();
                                    //ERROR CHECKING
                                    try
                                    {
                                        bankedFives = int.Parse(bankInput);
                                    } catch (Exception e) {
                                        Console.WriteLine("ERROR: Invalid 5 number input.");
                                    }
                                }
                                if (bankedFives < 0 || bankedFives > choices[1]) {
                                    Console.WriteLine("ERROR: Selection is too many or too few 5's ");
                                } else {
                                    validInput = true;
                                }
                            }
                            currentBankScore = ScoreNums(5, choices, turnRoll, bankedFives);
                            Console.WriteLine("Banked " + bankedFives + " 5's.");
                            List<int> numFives = new List<int>(CountNums(turnRoll, 5));
                            string fiveIndexesStr = numFives[1].ToString();
                            List<int> fiveIndexes = new List<int>();
                            for (int i = 0; i < fiveIndexesStr.Length; i++) {
                                fiveIndexes.Add(int.Parse(fiveIndexesStr.Substring(i,1)) - 1);
                            }
                            int j = fiveIndexes.Count-1;
                            for ( int i = 0; i < bankedFives; i++) {
                                turnRoll.RemoveAt(fiveIndexes[j]);
                                numDice--;
                                j--;
                            }
                        }
                        else if (currentChoiceText.Equals("Trios")) {
                            List<int> trioSummary = CountTrios(turnRoll);
                            //trioSummary is int list of 3 ints: numTrios, trioNumsInt, and trioIndexes
                            //trioSummary[0]: # of trio sets, eg 0, 1, or 2 (length=1)
                            //trioSummary[1]: # on the dice involved in trio eg 3, or 32, or 1, or 56 (length=0, 1 or 2) 
                            //trioSummary[2]: index (1-6, not 0-5) of all trios eg 134, or 123456 (length=0, 3, or 6)
                            //choices example = [3, 2, 1, 0, 0], # of trios is in middle, index 2
                            if (trioSummary[0].Equals(1)) {
                                int trioKey = trioSummary[1];
                                string trioIndex = trioSummary[2].ToString();
                                List<int> trioPlaces = new List<int>();
                                for (int i = 0; i < trioIndex.Length; i++) { //can just hardcode to 3 prolly
                                    trioPlaces.Add(int.Parse(trioIndex.Substring(i,1)));
                                }
                                //now we have trioPlaces which is list with indexes (1-6) of trio dice places
                                Console.WriteLine("You banked one trio! (" + trioKey + ")");
                                if (trioKey.Equals(1)) {
                                    Console.WriteLine("Banked 1000 points!");
                                    currentBankScore = 1000;
                                } else {
                                    Console.WriteLine("Banked " + (100*trioKey) + " points!");
                                    currentBankScore = (100*trioKey);
                                }
                                for (int i = trioPlaces.Count-1; i > -1; i--) { //can just hardcode to 3 prolly
                                    turnRoll.RemoveAt(trioPlaces[i]-1);
                                    numDice--;
                                }
                            } else if (trioSummary[0].Equals(2)) {
                                string trioSummary1 = trioSummary[1].ToString();
                                int trioKey1 = int.Parse(trioSummary1.Substring(0,1));
                                int trioKey2 = int.Parse(trioSummary1.Substring(1,1));
                                string trioIndexTotal = trioSummary[2].ToString();
                                string trioIndex1 = trioIndexTotal.Substring(0,3);
                                string trioIndex2 = trioIndexTotal.Substring(3,3);
                                List<int> trioPlaces1 = new List<int>();
                                List<int> trioPlaces2 = new List<int>();
                                for (int i = 0; i < trioIndex1.Length; i++) { //can just hardcode to 3 prolly
                                    trioPlaces1.Add(int.Parse(trioIndex1.Substring(i,1)));
                                    trioPlaces2.Add(int.Parse(trioIndex2.Substring(i,1)));
                                }
                                //now we have trioPlaces which is list with indexes (1-6) of trio dice places
                                
                                Console.WriteLine("What trio would you like to bank?\nA. " + trioKey1 + "\nB. " + trioKey2 + "\nC. Both");
                                Console.Write("Choice (A, B, or C) : ");
                                string trioInput = Console.ReadLine();
                                if (trioInput.Equals("A")) {
                                    Console.WriteLine("You banked one trio! (" + trioKey1 + ")");
                                    Console.WriteLine("Banked " + (100*trioKey1) + " points!");
                                    for (int i = 0; i < trioPlaces1.Count; i++) { //can just hardcode to 3 prolly
                                        turnRoll.RemoveAt(trioPlaces1[i]-1);
                                        numDice--;
                                    }
                                    currentBankScore = (100*trioKey1);
                                } else if (trioInput.Equals("B")) {
                                    Console.WriteLine("You banked one trio! (" + trioKey2 + ")");
                                    Console.WriteLine("Banked " + (100*trioKey2) + " points!");
                                    for (int i = 0; i < trioPlaces2.Count; i++) { //can just hardcode to 3 prolly
                                        turnRoll.RemoveAt(trioPlaces2[i]-1);
                                    }
                                    currentBankScore = (100*trioKey2);
                                } else {
                                    Console.WriteLine("You banked 2 trios! (" + trioKey1 + " and " + trioKey2 + ")");
                                    Console.WriteLine("Banked " + (100*(trioKey1 + trioKey2)) + " points!");
                                    turnRoll.Clear();
                                    currentBankScore = (100*(trioKey1+trioKey2));
                                }
                            }
                        }
                        else if (currentChoiceText.Equals("Three Pairs")) {
                            currentBankScore = 750;
                            Console.WriteLine("You banked three pairs!");
                            Console.WriteLine("Banked " + currentBankScore + " points.");
                            turnRoll.Clear();
                        }
                        else if (currentChoiceText.Equals("Straight (1-6)")) {
                            currentBankScore = 1000;
                            Console.WriteLine("You banked a straight!");
                            Console.WriteLine("Banked " + currentBankScore + " points.");
                            turnRoll.Clear();
                        }

                        turnScore += currentBankScore;

                    }






                    
                    

                    //End of current roll (not end of turn yet)
                    Console.WriteLine("\nCurrent Turn Score: " + turnScore);
                    Console.WriteLine("Dice Remaining: " + numDice);
                    if (turnRoll.Count.Equals(0)) {
                        Console.Write("All dice banked! Roll again? (y/n) :");
                        string rollagain = Console.ReadLine();
                        //ADD ERROR CHECKING
                        if (rollagain.Equals("y")) {
                            //dice = 6, roll again
                            numDice = 6;
                        } else if (rollagain.Equals("n")) {
                            playerPlaying = false;
                            Console.WriteLine("Turn finished!");
                            Console.WriteLine("You scored " + turnScore + " this turn!");
                            Console.WriteLine("Total Score: " + (score + turnScore));
                            return turnScore;
                        }
                    } else {
                        Console.WriteLine("\nRoll remaining dice? (y/n): ");
                        String wantToContinue = Console.ReadLine();
                        //ADD ERROR CHECKING
                        if (wantToContinue.Equals("n")) {
                            //Bank points
                            playerPlaying = false;
                            Console.WriteLine("Turn finished!");
                            Console.WriteLine("You scored " + turnScore + " this turn!");
                            Console.WriteLine("Total Score: " + (score + turnScore));
                            return turnScore;
                        } else {
                            //Continue rolling
                        }
                    }
                }
                return turnScore;
            }

            string DisplayRoll(List<int> roll) {
                
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
  
            List<int> EvaluateRoll(List<int> roll) {
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

                List<int> result = new List<int>();
                result.Add(numOnes[0]);
                result.Add(numFives[0]);
                result.Add(numTrios[0]);
                if (ThreePairs(roll)) {
                    result.Add(1);
                } else {
                    result.Add(0);
                }
                if (IsStraight(roll)) {
                    result.Add(1);
                } else {
                    result.Add(0);
                }

                return result;
                //RESULT is list of 5 integers: (# of 1's, # of 5's, # of Trios, Three-Pair (1/0), Straight (1/0))
                
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

            //Returns list of 3 ints
            //[# of trios (1 or 2), trio Numbers [1-6][1-6], non-zero trio indexes [1-6][1-6][1-6][1-6][1-6][1-6]
            List<int> CountTrios(List<int> roll) {
                //roll is something like <1, 2, 2, 4, 4, 4>
                int numTrios = 0;
                List<int> result = new List<int>();
                string trioIndexes = "";
                string trioNums = "";
                List<bool> beenFound = new List<bool>();
                for (int i = 0; i < roll.Count; i++) {
                    beenFound.Add(false);
                }
                for (int i = 0; i < roll.Count; i++) {
                    for (int j = 1; j < roll.Count; j++) {
                        for (int k = 2; k < roll.Count; k++) {
                            if (roll[i].Equals(roll[j]) && roll[j].Equals(roll[k]) && roll[i].Equals(roll[k]) && !beenFound[i] && !beenFound[j] && !beenFound[k] && !i.Equals(j) && !j.Equals(k) && !i.Equals(k)) {
                                int nonZeroI = i + 1;
                                int nonZeroJ = j + 1;
                                int nonZeroK = k + 1;
                                trioIndexes += nonZeroI.ToString();
                                trioIndexes += nonZeroJ.ToString();
                                trioIndexes += nonZeroK.ToString();
                                beenFound[i] = true;
                                beenFound[j] = true;
                                beenFound[k] = true;
                                trioNums += roll[i].ToString();
                                numTrios++;
                            }
                        }
                    }
                }
                int trioNumsInt = 0;
                if (!trioNums.Length.Equals(0)) {
                    trioNumsInt = int.Parse(trioNums);;
                } 
                
                result.Add(numTrios);
                result.Add(trioNumsInt);
                if (trioIndexes.Length > 0) {
                    result.Add(int.Parse(trioIndexes));
                } else {
                    result.Add(0);
                }

                return result;
                //result is int list of 3 ints: numTrios, trioNumsInt, and trioIndexes
                //numTrios: # of trio sets, eg 0, 1, or 2 (length=1)
                //trioNumsInt: # of dice involved in trio eg 3, or 32, or 1, or 56 (length=0, 1 or 2) 
                //trioIndexes: index (1-6, not 0-5) of all trios eg 134, or 123456 (length=0, 3, or 6)
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
        
            int ScoreNums(int num, List<int> rollSummary, List<int> roll, int numsToBank) {
                List<int> numCount = CountNums(roll, num);
                if (num.Equals(1)) {
                    return 100*numsToBank;
                }
                if (num.Equals(5)) {
                    return 50*numsToBank;
                }
                return 0;
            }

            int ScoreTrios(List<int> roll, int numTriosToBank) {
                List<int> trioSummary = CountTrios(roll);
                if (numTriosToBank.Equals(1)) {
                    Console.Write("Which # trio to bank? (ex. 5): ");
                    string intInput = Console.ReadLine();
                    bool validInput = false;
                    int trioBankChoice = -1;
                    while(!validInput) {
                        if (intInput.Length > 1 || intInput.Length.Equals(0)) {
                            Console.WriteLine("ERROR: Invalid input. Enter one number indicating trio.");
                        }
                        else {
                            try {
                                trioBankChoice = int.Parse(intInput); //ex. 5 (for 555)
                                validInput = true;
                            } catch (Exception e) {
                                Console.WriteLine("ERROR: incorrect input. Enter integer (1-6) of trio (1, 2, etc.)");
                            }
                            //numTriosToBank: either 1 or 2 (int with length 1)
                            //trioSummary is int list of 3 ints: numTrios, trioNumsInt, and trioIndexes
                            //trioSummary[0]: # of trio sets, eg 0, 1, or 2 (length=1)
                            //trioSummary[1]: # of dice involved in trio eg 3, or 32, or 1, or 56 (length=0, 1 or 2) 
                            //trioSummary[2]: index (1-6, not 0-5) of all trios eg 134, or 123456 (length=0, 3, or 6)
                            string trios = trioSummary[0].ToString();
                            string trioNums = trioSummary[1].ToString();
                            string trioIndices = trioSummary[2].ToString();

                            for (int i = 0; i < trioNums.Length; i++) { //go thru all trios (maybe 5, then 2 for example)
                                int trioNum = int.Parse(trioNums.Substring(i,1));
                                if (trioNum.Equals(trioBankChoice)) { 
                                    // get indexes of trio
                                    // trioNum index 0 = trioIndices[0, 1, 2]
                                    // trioNum index 1 = trioIndices[3, 4, 5]
                                    // index of first number = 3x trioNum index
                                    int trioNumsFoundIndex = i;
                                    int firstTrioNumIndex = 3*i;
                                    if (trioNum.Equals(1)) {
                                        return 1000;
                                        //roll.RemoveAt(i);
                                        
                                    }

                                    //remove dice from roll

                                    //determine what # they are (trioNum)
                                    //Multiply to get unit score
                                    //Return unit score
                                }
                            }
                        }
                    }
                } else {
                    //they chose 2 trios
                    //get points, remove all
                }
                return 0;
            }

            

        }
        
    }
}
