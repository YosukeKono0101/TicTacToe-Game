using System;
using System.IO;

namespace Final_Assignment
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("*************************");
            Console.WriteLine("* Numerical Tic-Tac-Toe *");
            Console.WriteLine("*************************");
            Console.WriteLine();
            HelpSystem.get_help();
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("<There are two game modes, choose one from below>");
            Console.WriteLine("************************");
            Console.WriteLine("* 1. Human vs Human    *");
            Console.WriteLine("* 2. Human vs Computer *");
            Console.WriteLine("************************");

            // Variable 'game_mode' of type 'GameMode' and initialize it to 'None'.
            GameMode game_mode = GameMode.None;

            // User's input
            int input;

            // Loop until the user selects a valid game mode (Human_vs_Human or Computer_vs_Human).
            while (game_mode != GameMode.Human_vs_Human && game_mode != GameMode.Computer_vs_Human)
            {
                // Read the user's input from the console and parse it as an integer.
                input = int.Parse(Console.ReadLine());

                // Assign the game mode based on the user's input.
                if (input == 1)
                {
                    game_mode = GameMode.Human_vs_Human;
                }
                else
                {
                    game_mode = GameMode.Computer_vs_Human;
                }
            }
            // Instance of the 'GameController' class with the game mode that the user chose.
            GameController game = new GameController(game_mode);

            // Initialize the game.
            game.initialize_game();

        }
    }                                        
}

