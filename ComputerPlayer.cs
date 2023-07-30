using System;
using System.IO;

namespace Final_Assignment
{
    public class ComputerPlayer : Player
    {
        public Random random;
        // Properties for storing Name and NumberType
        public string Name { get; }
        public NumberType NumberType { get; }

        // Constructor of ComputerPlayer
        public ComputerPlayer(string name, NumberType number_type)
        {
            Name = name;
            NumberType = number_type;
            random = new Random();
        }

        // Make a move for ComputerPlayer
        public void make_move(Board board, History move_history)
        {
            // Variables for row, column, and number
            int row, col, number;

            //  Create random rows, columns and numbers
            row = random.Next(0, 3);
            col = random.Next(0, 3);
            number = get_random_number();

            // Keep creating random rows, columns and numbers until the empty cell is found
            while (!board.check_if_cell_empty(row, col) || !check_if_num_valid(number, board))
            {
                row = random.Next(0, 3);
                col = random.Next(0, 3);
                number = get_random_number();
            }

            // Display a message of what move the computer has made
            Console.WriteLine($"{Name} placed {number} at {row}, {col}");

            // Update the board and move history
            board.place_number(row, col, number);
            move_history.add_move(new MoveData(row, col, number));
        }

        // Check if a given number is valid
        public bool check_if_num_valid(int number, Board board)
        {
            // Loop each cell of the board
            for (int i = 0; i < Board.size; i++)
            {
                for (int j = 0; j < Board.size; j++)
                {
                    // Return false if the cell contains the number 
                    if (board.cell[i, j] == number)
                    {
                        return false;
                    }
                }
            }

            // Return true if the number is not found in any cell
            return true;
        }
        //Generate random number
        public int get_random_number()
        {
            // determine the starting number based on the player's number type(odd or even)
            int start_number = (NumberType == NumberType.Odd) ? 1 : 2;
            // Set the increment value to 2 to generate odd or even numbers
            int increment = 2;
            // Generate a random number within the range of valid numbers(1-9)
            return start_number + increment * random.Next(0, (10 - start_number) / increment);
        }
    }
}

