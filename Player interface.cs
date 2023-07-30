using System;
using System.IO;

namespace Final_Assignment
{
    public interface Player
    {
        // Property for storing the player's name (player1, player2, or computer)
        string Name { get; }

        // Property for storing the player's number type (odd or even)
        NumberType NumberType { get; }

        // Move method with board and history object as parameters
        void make_move(Board board, History move_history);
    }
}

