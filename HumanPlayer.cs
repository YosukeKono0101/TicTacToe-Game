using System;
using System.IO;

namespace Final_Assignment
{
    public class HumanPlayer : Player
    {
        // Property for storing Name and NumberType
        public string Name { get; }
        public NumberType NumberType { get; }

        // Constructor of HumanPlayer
        public HumanPlayer(string name, NumberType number_type)
        {
            Name = name;
            NumberType = number_type;
        }

        // Make a move for HumanPlayer
        public void make_move(Board board, History move_history)
        {
            // Initialize rows, columns, and numbers with default values
            int row = -1, col = -1, number = -1;
            // Check if the move is valid
            bool valid_move = false;

            // Keep promopting for input until the user makes a valid move
            while (!valid_move)
            {
                // Prompt the user for input
                Console.WriteLine($"{Name}, enter your move (row col number(1-9)) or \"help\" to see help guide:");
                Console.WriteLine("Rows and columns are numbered 1, 2, and 3.");
                // Read input and split it into an array of strings
                string[] input = Console.ReadLine().Split();

                // Check if the user input "help"
                if (input[0] == "help")
                {
                    // Display the help guide
                    HelpSystem.get_help();
                    // Prompt the user for input again
                    continue;
                }

                // Parse input values for row, col, and number
                row = int.Parse(input[0]) - 1;
                col = int.Parse(input[1]) - 1;

                number = int.Parse(input[2]);

                // Check if the move is valid
                valid_move = check_if_move_valid(board, row, col, number);
            }

            // If the move is valid, update the board and move history
            board.place_number(row, col, number);
            move_history.add_move(new MoveData(row, col, number));
        }

        //Check if the move is valid
        public bool check_if_move_valid(Board board, int row, int col, int number)
        {
            // Check if the position is within board
            if (row < 0 || row >= 3 || col < 0 || col >= 3)
            {
                Console.WriteLine("Invalid position, please try again.");
                return false;
            }

            // Check if the cell is already occupied
            if (!board.check_if_cell_empty(row, col))
            {
                Console.WriteLine("The cell is already occupied, please try again.");
                return false;
            }

            // Check if the number is valid for the player's number type(odd, even)
            if (NumberType == NumberType.Odd && number % 2 == 0 || NumberType == NumberType.Even && number % 2 != 0)
            {
                Console.WriteLine("The number you entered is invalid for your number type(Odd/Even) , please try again.");
                return false;
            }

            // Check if the number is within the valid range of 1-9
            if (number < 1 || number > 9)
            {
                Console.WriteLine("Invalid number. The available number range is 1-9, please try again.");
                return false;
            }

            // Check if the number has already been put on the board
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Empty cell
                    if (board.check_if_cell_empty(i, j))
                    {
                        continue;
                    }
                    // Check if the number has already been put on the board
                    if (board.check_if_cell_empty(i, j) == false && board.cell[i, j] == number)
                    {
                        Console.WriteLine("The number has already been placed on the board, please try again.");
                        return false;
                    }
                }
            }

            // Return true if the move is valid
            return true;
        }
    }
}

