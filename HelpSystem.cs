using System;
namespace Final_Assignment
{
    public class HelpSystem
    {
        // Display help guide to the user
        public static void get_help()
        {
            // Displays game instructions
            Console.WriteLine("< Numerical Tic-Tac-Toe Instruction >");
            Console.WriteLine("1. On a 3x3 board, players take turns placing numbers from 1 to 9.");
            Console.WriteLine("2. Player 1 uses odd numbers and player 2 uses even numbers.");
            Console.WriteLine("3. Each number may only be used once.");
            Console.WriteLine("4. The first player to place 15 points in a horizontal, vertical, or diagonal line wins.");
            Console.WriteLine("5. Rows and columns are numbered 1, 2, and 3.");
            Console.WriteLine("6. To make a move, enter the row, column, and number separated by spaces (e.g., '1 2 3').");
            Console.WriteLine("7. Rows and columns are numbered 1, 2, and 3 from top left to bottom right.");
            Console.WriteLine("8. If at any time you need help, type 'help'.");
        }
    }
}

