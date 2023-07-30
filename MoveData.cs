using System;
using System.IO;

namespace Final_Assignment
{
    // This class represents a move made in the game
    public class MoveData
    {
        // Property for storing the row number where the move is made.
        public int Row { get; set; }

        // Property for storing the column number where the move is made.
        public int Col { get; set; }

        // Property for storing the number type in the move.
        public int Number { get; set; }

        // Constructor of the MoveData class, which initializes a new move with the available row, column, and number.
        public MoveData(int row, int col, int number)
        {
            // Set the Row property with the value provided.
            Row = row;
            // Set the Col property with the value provided.
            Col = col;
            // Set the Number property with the value provided.
            Number = number;
        }
    }

}

