using System;
using System.IO;

namespace Final_Assignment
{
    public class GameController
    {
        public Board board;
        public Player player1;
        public Player player2;
        public Player current_player;
        public History move_history;

        public GameController(GameMode game_mode)
        {
            // Initialize a new Board object
            board = new Board();

            // Create the first player as a human player with odd numbers
            player1 = new HumanPlayer("Player 1", NumberType.Odd);

            // Check if the game mode is Human vs Human
            if (game_mode == GameMode.Human_vs_Human)
            {
                // If yes, create the second player as a human player with even numbers
                player2 = new HumanPlayer("Player 2", NumberType.Even);
            }
            else
            {
                // If not, create the second player as a computer player with even numbers
                player2 = new ComputerPlayer("Computer", NumberType.Even);
            }

            // Set the current player to player1
            current_player = player1;

            // Initialize a new History object to keep track of moves
            move_history = new History();
        }


        public void initialize_game()
        {
            // Keep looping until there's a winner or the board is full
            while (!board.confirm_win())
            {
                // Clear the previous output to display the updated board
                Console.Clear();

                // Display the state of the board
                board.display();

                // Let the current player make their move and update the move history
                current_player.make_move(board, move_history);

                // If the game is not over yet, handle undo and redo actions and switch players
                if (!board.confirm_win())
                {
                    // Check if the user wants to undo or redo any moves
                    handle_undo_and_redo(board);

                    // switch players for the next turn
                    switch_players();
                }
                else
                {
                    // If the game is over, switch players so the actual winner is displayed
                    switch_players();
                }
            }

            // Clear the previous output and display the board
            Console.Clear();
            board.display();

            // Check if there's a winning line and identify the winner
            NumberType? winner_number_type = board.check_winning_line();
            if (winner_number_type != null)
            {
                // If there's a winner, displar the name of the winner
                string winner_name = winner_number_type == NumberType.Odd ? player1.Name : player2.Name;
                Console.WriteLine($"{winner_name} wins!");
            }
            else
            {
                // If there's no winner, display the message below
                Console.WriteLine("It's a draw!");
            }

            // Prompt the user to enter any ket to exit
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        public void handle_undo_and_redo(Board board)
        {
            // Check if the current player is a human player
            if (current_player is HumanPlayer)
            {
                bool continue_game = false;
                while (!continue_game)
                {
                    // Prompt the user for input
                    Console.WriteLine("Type 'undo' to undo a move, 'redo' to redo a move, or press enter to continue the game.");
                    string input = Console.ReadLine().ToLower();

                    // Check if the user wants to undo a move
                    if (input == "undo")
                    {
                        // Get the move to undo from the move history
                        MoveData move = move_history.undo_move();
                        if (move != null)
                        {
                            // Remove the number from the board
                            board.place_number(move.Row, move.Col, 0);
                            // Switch to the player who made the move
                            switch_players();

                            // Ask the user if they want to redo the move they just undid
                            Console.WriteLine("Do you want to redo the move you just undid? Type 'yes' to redo, or press enter to continue the game.");
                            input = Console.ReadLine().ToLower();

                            if (input == "yes")
                            {
                                // Redo the move
                                move = move_history.redo_move();
                                if (move != null)
                                {
                                    // Place the number back on the board
                                    board.place_number(move.Row, move.Col, move.Number);
                                    // Switch to the next player
                                    switch_players();
                                }
                            }
                        }
                        else
                        {
                            // Display a message if there are no moves to undo
                            Console.WriteLine("No moves to undo.");
                            continue_game = true;
                        }
                    }
                    // If the user wants to redo a move
                    else if (input == "redo")
                    {
                        // Get the move to redo from the move history
                        MoveData move = move_history.redo_move();
                        if (move != null)
                        {
                            // Place the number back on the board
                            board.place_number(move.Row, move.Col, move.Number);
                            // Switch to the next player
                            switch_players();
                        }
                        else
                        {
                            // Display a message if there are no moves to redo
                            Console.WriteLine("No moves to redo.");
                        }
                    }
                    else
                    {
                        continue_game = true;
                    }
                }
            }
        }

        public void switch_players()
        {
            // Check if the current player is player1
            if (current_player == player1)
            {
                // Switch to player2 if the current player is player1
                current_player = player2;
            }
            else
            {
                // Switch to player1 if the current player is player2
                current_player = player1;
            }
        }
    }
}

