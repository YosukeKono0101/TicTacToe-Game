using System;
using System.IO;

namespace Final_Assignment
{
    public class Board
    {
        // Two-dementional array to represent the cell of the game board
        public int[,] cell;
        // Constant integer to represent the size of the board(3 * 3)
        public const int size = 3;

        // Constructor
        public Board()
        {
            // Initialize the cell array
            cell = new int[size, size];
        }

        // Display the board
        public void display()
        {
            // Iterate board rows
            for (int i = 0; i < size; i++)
            {
                // Iterate board columns
                for (int j = 0; j < size; j++)
                {
                    // Display a cell value if it's not 0
                    // if it's 0 display "-"
                    if (cell[i, j] != 0)
                    {
                        Console.Write(cell[i, j].ToString());
                    }
                    else
                    {
                        Console.Write("-");
                    }

                    // Display "|" between the cell, not applied to the last row                 
                    if (j < size - 1)
                    {
                        Console.Write(" | ");
                    }
                }
                // Move to the next line after display all cells in the row
                Console.WriteLine();

                // Display a line between the rows, not applied to the last row               
                if (i < size - 1)
                {
                    Console.WriteLine("---------");
                }
            }
        }

        //Check if the board is full or if there is a winning line
        public bool confirm_win()
        {
            return check_board_full() || check_winning_line() != null;
        }

        //Check if the cell is empty
        public bool check_if_cell_empty(int row, int col)
        {
            return cell[row, col] == 0;
        }

        // Place the number in the cell at the row and column
        public void place_number(int row, int col, int number)
        {
            cell[row, col] = number;
        }

        // Check if the board is full
        public bool check_board_full()
        {
            //Iterate over all rows
            for (int i = 0; i < size; i++)
            {
                // Iterate over all columns
                for (int j = 0; j < size; j++)
                {
                    // Return false if the current row and column cells are empty
                    if (check_if_cell_empty(i, j))
                    {
                        return false;
                    }
                }
            }

            // If the method did not return false, the board is full and returns true
            return true;
        }

        public NumberType? check_winning_line()
        {
            // Iterate over each row and column
            for (int i = 0; i < size; i++)
            {
                // Check if the row is filled and the total is 15
                if (is_row_filled(i) && calculate_row(i) == 15)
                {
                    // Check if the first cell in the row is odd
                    if (cell[i, 0] % 2 == 1)
                    {
                        return NumberType.Odd;
                    }
                    else
                    {
                        return NumberType.Even;
                    }
                }
                // Check if the column is filled and the total is 15
                if (is_column_filled(i) && calculate_column(i) == 15)
                {
                    // Check if the first cell in the column is odd
                    if (cell[0, i] % 2 == 1)
                    {
                        return NumberType.Odd;
                    }
                    else
                    {
                        return NumberType.Even;
                    }
                }
            }
            // Check if the diagonal line1 is filled and the total is 15
            if (is_diagonal_line1_filled() && calculate_diagonal_line1() == 15)
            {
                // Check if the first cell in the diagonal is odd
                if (cell[0, 0] % 2 == 1)
                {
                    return NumberType.Odd;
                }
                else
                {
                    return NumberType.Even;
                }
            }
            // Check if the diagonal line2 is filled and the total is 15
            if (is_diagonal_line2_filled() && calculate_diagonal_line2() == 15)
            {
                // Check if the first cell in the diagonal is odd
                if (cell[0, 2] % 2 == 1)
                {
                    return NumberType.Odd;
                }
                else
                {
                    return NumberType.Even;
                }
            }
            // If there is no winning line, return null
            return null;
        }

        // Check if the row is filled
        public bool is_row_filled(int row)
        {
            return cell[row, 0] != 0 && cell[row, 1] != 0 && cell[row, 2] != 0;
        }

        // Check if the column is filled
        public bool is_column_filled(int col)
        {
            return cell[0, col] != 0 && cell[1, col] != 0 && cell[2, col] != 0;
        }

        // Check if the diagonal line1 is filled
        public bool is_diagonal_line1_filled()
        {
            return cell[0, 0] != 0 && cell[1, 1] != 0 && cell[2, 2] != 0;
        }

        // Check if the diagonal line2 is filled
        public bool is_diagonal_line2_filled()
        {
            return cell[0, 2] != 0 && cell[1, 1] != 0 && cell[2, 0] != 0;
        }


        // Calculates the sum of the row
        public int calculate_row(int row)
        {
            return cell[row, 0] + cell[row, 1] + cell[row, 2];
        }


        // Calculates the sum of the column
        public int calculate_column(int col)
        {
            return cell[0, col] + cell[1, col] + cell[2, col];
        }


        // Calculates the sum of the diagonal line1
        public int calculate_diagonal_line1()
        {
            return cell[0, 0] + cell[1, 1] + cell[2, 2];
        }


        // Calculates the sum of the diagonal line2
        public int calculate_diagonal_line2()
        {
            return cell[0, 2] + cell[1, 1] + cell[2, 0];
        }
    }
}

