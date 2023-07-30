using System;
using System.IO;

namespace Final_Assignment
{
    public class History
    {
        // List of MoveData objects to store the moves made in the game
        public List<MoveData> moves;
        // List of MoveData objects to store the moves that can be redone after undoing
        public List<MoveData> redo_moves;

        // Constructor of the History class, initializing the moves and redo_moves lists
        public History()
        {
            moves = new List<MoveData>();
            redo_moves = new List<MoveData>();
        }

        // Add a move to the moves list and clear the redo_moves list
        public void add_move(MoveData move)
        {
            // Add the new move to the list of moves
            moves.Add(move);
            // Remove the redo moves list
            redo_moves.Clear();
        }

        // Undo the last move, remove it from the moves list, and add it to the redo_moves list
        public MoveData undo_move()
        {
            if (moves.Count == 0) return null;

            MoveData move = moves[moves.Count - 1];
            moves.Remove(move);
            redo_moves.Add(move);
            return move;
        }

        // Redo the last move, remove it from the redo_moves list, and add it back to the moves list
        public MoveData redo_move()
        {
            if (redo_moves.Count == 0) return null;

            MoveData move = redo_moves[redo_moves.Count - 1];
            redo_moves.Remove(move);
            moves.Add(move);
            return move;
        }
    }
}

