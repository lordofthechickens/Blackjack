using System.Diagnostics;

namespace Blackjack
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string? value = "Start";
            Console.WriteLine("Welcome to Blackjack! Press any key to start.");
            Console.ReadKey(true);
            while (value != "QUIT")
            {
                playerIsBust = false;
                dealerIsBust = false;
                
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Starting new hand...\n");
                Console.ResetColor();
                GameDeck Deck = new();
                //Draw cards to player and dealer
                Deck.DrawCardTo(Deck.PlayerCards);
                Deck.DrawCardTo(Deck.DealerCards);
                Deck.DrawCardTo(Deck.PlayerCards);
                Deck.DrawCardTo(Deck.DealerCards);
                //Display dealer cards
                Console.WriteLine($"Dealer's cards: ? | {Deck.DealerCards[1]} \n");
                
                //Loop asking if player wants to draw
                while (!playerIsBust)
                {
                    //Display Player cards
                    Console.WriteLine($"Your cards: {GameDeck.CardsAsString(Deck.PlayerCards,false)}");
                    Console.WriteLine($"Sum of your cards is {GameDeck.SumToMessage(GameDeck.SumOf(Deck.PlayerCards))}");
                    Console.WriteLine("Would you like to draw another card? (Y/N) \n");
                    if (!ReadYesOrNo())
                    {
                        break;
                    }
                    else
                    {
                        Deck.DrawCardTo(Deck.PlayerCards);
                    }
                    //Check if player has bust.
                    if (GameDeck.SumOf(Deck.PlayerCards).Count == 0)
                    {
                        playerIsBust = true;
                    }
                    
                }
                //End player loop

                //Start dealer loop. If dealer sum >= 17, stand.
                if (!playerIsBust)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Dealer's turn...");
                    Console.ResetColor();
                }

                while (!dealerIsBust && !playerIsBust)
                {
                    if (GameDeck.SumOf(Deck.DealerCards)[0] >= 17)
                    {
                        Console.WriteLine("Dealer stands. \n");
                        break;
                    }
                    Console.WriteLine($"Dealer has drawn {Deck.DrawCardTo(Deck.DealerCards)}");

                    // if statement to check for bust.
                    if (GameDeck.SumOf(Deck.DealerCards).Count == 0)
                    {
                        dealerIsBust = true;
                    }

                }
                //End dealer loop
                //Display who won.
                if (playerIsBust)
                {
                    Console.WriteLine($"Your cards: {GameDeck.CardsAsString(Deck.PlayerCards, false)}");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Player busts. Dealer wins!");
                }
                else if (dealerIsBust)
                {
                    Console.WriteLine($"Dealer's cards: {GameDeck.CardsAsString(Deck.DealerCards, false)}");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\nDealer busts. Player wins!");
                }
                else
                {
                    //Three conditions. (1) dealer wins by num. (2) player wins by num. (3) decision by card count.
                    Console.WriteLine($"Dealer's cards: {GameDeck.CardsAsString(Deck.DealerCards,false)}");
                    Console.WriteLine($"Dealer's sum: {GameDeck.SumOf(Deck.DealerCards)[0]}\n");
                    Console.WriteLine($"Your cards: {GameDeck.CardsAsString(Deck.PlayerCards, false)}");
                    Console.WriteLine($"Your sum: {GameDeck.SumOf(Deck.PlayerCards)[0]}\n");
                    //Dealer wins by score
                    Console.ForegroundColor = ConsoleColor.Blue;
                    if (GameDeck.SumOf(Deck.DealerCards)[0] > GameDeck.SumOf(Deck.PlayerCards)[0])
                    {
                        Console.WriteLine("Dealer wins by score.");
                    }
                    else if (GameDeck.SumOf(Deck.DealerCards)[0] < GameDeck.SumOf(Deck.PlayerCards)[0])
                    {
                        Console.WriteLine("You won by score!");
                    }
                    else
                    {
                        if (Deck.DealerCards.Count < Deck.PlayerCards.Count)
                        {
                            Console.WriteLine("Score is tied. Dealer wins by card count.");
                        }
                        else if (Deck.DealerCards.Count > Deck.PlayerCards.Count)
                        {
                            Console.WriteLine("Score is tied. You win by card count.");
                        }
                        else
                        {
                            Console.WriteLine("Score and card count are tied. You and the Dealer tie.");
                        }
                    }
                }
                Console.ResetColor();

                //Reassign 'value' to ask if the player wants to continue
                Console.WriteLine("Press Enter to play again. Type 'QUIT' to terminate the program.");
                value = Console.ReadLine();
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Program terminating. Thanks for playing!\n");
            Console.ResetColor();

        }

        internal static bool playerIsBust;
        internal static bool dealerIsBust;

        public static bool ReadYesOrNo()
        {
            while (true)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Y)
                {
                    return true;
                }
                else if (key == ConsoleKey.N)
                {
                    return false;
                }
            }
        }

        

        internal class GameDeck
        {
            internal GameDeck()
            {
                MainDeck = ['A', 'A', 'A', 'A', 'K', 'K', 'K', 'K', 'Q', 'Q', 'Q', 'Q',
                    'J', 'J', 'J', 'J', 10, 10, 10, 10, 9, 9, 9, 9, 8, 8, 8, 8,
                    7, 7, 7, 7, 6, 6, 6, 6, 5, 5, 5, 5, 4, 4, 4, 4, 3, 3, 3, 3, 2, 2, 2, 2];
                _totalCards = 52;

                PlayerCards = [];
                DealerCards = [];
            }

            private readonly List<object> MainDeck;
            private static readonly List<object> nonAceCards = [2, 3, 4, 5, 6, 7, 8, 9, 10, 'J', 'Q', 'K'];
            private readonly Random rnd = new();
            private int _totalCards;

            public List<object> PlayerCards { get; private set; }
            public List<object> DealerCards { get; private set; }



            /// <summary> Selects a random card in the deck, removes it, updates the card count, adds the card to the list argument, and returns the card.</summary>
            /// <returns>Char or Int representing the drawn card</returns>
            public object DrawCardTo(List<object> CardList)
            {
                Debug.Assert(MainDeck != null && _totalCards > 0);
                int index = rnd.Next(0, _totalCards);
                object selectedCard = MainDeck[index];
                MainDeck.RemoveAt(index);
                _totalCards--;
                CardList.Add(selectedCard);
                return selectedCard;
            }
            /// <summary> Calculates valid hand totals and returns them in a 0–2 element list.</summary>
            /// <remarks> If the list is empty, the hand is bust.</remarks>
            public static List<int> SumOf(List<object> inputCards)
            {
                List<int> sumList = [];
                int sum = 0;
                int value = 2;
                //gets the sum of the static (non-ace) cards
                foreach (object card in nonAceCards)
                {
                    int count = inputCards.Count(x => EqualityComparer<object>.Default.Equals(x,card));
                    sum += value * count;

                    if (value < 10)
                    {
                        value++;
                    }
                }
                //Account for and add the aces to the sum. If sum is over 21, add nothing to list.
                int aceCount = inputCards.Count(x => EqualityComparer<object>.Default.Equals(x, 'A'));
                if (aceCount == 0)
                {
                    if (sum <= 21)
                    {
                        sumList.Add(sum);
                    }
                }
                else if (aceCount == 1)
                {
                    if (sum + 11 <= 21)
                    {
                        sumList.Add(sum + 11);
                    }
                    if (sum + 1 <= 21)
                    {
                        sumList.Add(sum + 1);
                    }
                }
                else
                {
                    sum += aceCount - 1;
                    if (sum + 11 <= 21)
                    {
                        sumList.Add(sum + 11);
                    }
                    if (sum + 1 <= 21)
                    {
                        sumList.Add(sum + 1);
                    }
                }
                return sumList;
            }
            
            internal static string SumToMessage(List<int> sumList)
            {
                if (sumList.Count == 0)
                {
                    //Currently unreachable in implementation.
                    return "over 21.";
                }
                else if (sumList.Count == 1)
                {
                    return $"{sumList[0]}.";
                }
                else
                {
                    return $"{sumList[0]} or {sumList[1]}.";
                }
            }

            /// <summary> Converts a list of cards into a string separated by '|' </summary>
            /// <param name="cardList"> The hand to be displayed.</param>
            /// <param name="omitFirstCard"> Whether the first card should be visible (Used for dealer)</param>
            /// <returns>String showing the cards in the list</returns>
            internal static string CardsAsString(List<object> cardList, bool omitFirstCard)
            {
                string output = "";
                if (omitFirstCard)
                {
                    output += "?";
                }
                else
                {
                    output += $"{cardList[0]}";
                }
                for (int i = 1; i < cardList.Count; i++)
                {
                    output += $" | {cardList[i]}";
                }
                return output;
            }
        }
    }
}
