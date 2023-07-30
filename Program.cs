using System;
using System.IO;

namespace NumericalTicTacToe
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

    /*public class FileHandler
    {
        // Save the game state to a file
        public static void save_game(Board board, string fileName)
        {
            
        }
        // Load a game state from a filea
        public static Board load_game(string fileName)
        {
            
        }         
    }*/

    public interface Player
    {
        // Property for storing the player's name (player1, player2, or computer)
        string Name { get; }

        // Property for storing the player's number type (odd or even)
        NumberType NumberType { get; }

        // Move method with board and history object as parameters
        void make_move(Board board, History move_history);
    }

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

    // Enumeration with three possible values.
    public enum GameMode
    {
        None = 0,
        Human_vs_Human = 1,
        Computer_vs_Human = 2
    }

    // Enumeration with two possible values.
    public enum NumberType
    {
        Odd,
        Even
    }
}

