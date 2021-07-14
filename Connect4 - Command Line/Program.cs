using System;
using System.Threading;

namespace Connect4
{
    /// <summary>
    /// Helper class to track a players data and progress
    /// </summary>
    public class PlayerData
    {
        /// <summary>
        /// Optional name for a player
        /// </summary>
        private string playerName;
        public string PlayerName { get { return playerName; } set { playerName = value; } }

        /// <summary>
        /// Player ID to determine which players turn it is
        /// </summary>
        private int playerID;
        public int PlayerID { get{ return playerID; } set { playerID = value; } }

        /// <summary>
        /// Players shape
        /// </summary>
        private char playerShape;
        public char PlayerShape { get { return playerShape; } set { playerShape = value; } }

        /// <summary>
        /// Tracks how many times a player has won
        /// </summary>
        private int wins;
        public int Wins { get { return wins; } set { wins = value; } }

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="p_playerName"></param>
        /// <param name="p_playerID"></param>
        public PlayerData(string p_playerName, char p_shape, int p_ID)
        {
            PlayerName = p_playerName;
            PlayerShape = p_shape;
            PlayerID = p_ID;
        }
    }

    /// <summary>
    /// Helper class to define tiles which have positions and can be occupied
    /// </summary>
    public class Tile
    {
        private bool isOccupied;
        public bool IsOccupied { get { return isOccupied; } set { isOccupied = value; } }

        private int[,] position;
        public int[,] Position { get { return position; } set { position = value; } }
        private char shape;
        public char Shape { get{ return shape; } set { shape = value; } }

        /// <summary>
        /// CTOR
        /// </summary>
        public Tile()
        {
            Shape = ' ';
        }
    }

    class Program
    {
        #region Board Info
        /// <summary>
        /// Information about the board including tiles, and the board dimensions
        /// </summary>
        private static Tile[,] tiles;
        private static int boardWidth;
        private static int boardHeight;
        #endregion

        #region Player Information
        /// <summary>
        /// Information about the players and whose turn it is
        /// </summary>
        private static PlayerData player1;
        private static PlayerData player2;
        private static PlayerData[] players;
        private static int currentTurn;
        #endregion

        #region Utility
        /// <summary>
        /// Milliseconds used for pausing the game for presentation purposes
        /// </summary>
        private static int milliseconds;
        #endregion

        // Entry point of the game
        static void Main(string[] args)
        {
            // Set the players and board to some default values
            player1 = new PlayerData($"Player 1 (X)", 'X', 0);
            player2 = new PlayerData($"Player 2 (O)", 'O', 1);
            players = new PlayerData[] { player1, player2 };
            boardWidth = 7;
            boardHeight = 6;

            Console.WriteLine("Welcome to Connect 4! \n");

            // Start the game
            Restart();
        }

        /// <summary>
        /// Method to set the player turn and the tiles based on the board width and height
        /// </summary>
        private static void SetupGame()
        {
            // Sets a random player to go first to make things fair
            currentTurn = new Random().Next() % 2;
            Console.WriteLine("Tossing a coin...");

            milliseconds = 1000;
            Thread.Sleep(milliseconds);

            // A little fun message to the player to simulate a coin toss
            string result;
            if(currentTurn == 0)
            {
                result = $"The coin landed on heads. {players[currentTurn].PlayerName} goes first";
            }
            else
            {
                result = $"The coin landed on tails. {players[currentTurn].PlayerName} goes first";
            }
            Console.WriteLine($"{result} \n");


            // Create new tiles based on the board dimensions
            tiles = new Tile[boardHeight, boardWidth];
            for(int i = 0; i < boardHeight; i++)
            {
                for(int j = 0; j < boardWidth; j++)
                {
                    tiles[i,j] = new Tile();
                    tiles[i,j].IsOccupied = false;
                    tiles[i, j].Position = new int[i, j];
                }
            }

            // Update the board after creating it
            UpdateBoard();
        }

        /// <summary>
        /// Method to accept player input and attempt to update the game state of placing a piece
        /// </summary>
        private static void Play()
        {
            Console.WriteLine("Please select a row to drop your piece \n");
            string rowKey = Console.ReadLine();
            int rowNumber;
            milliseconds = 2000;

            // Check for valid input to see if the row exists
            if (int.TryParse(rowKey, out rowNumber))
            {
                rowNumber = int.Parse(rowKey);
                // Players will pick Row 1, but the index is 0, so subtract one from their choice
                rowNumber -= 1;

                //Check the input is a valid row
                if (rowNumber < boardWidth && rowNumber > - 1)
                {
                    // Row is full, inform the player and replay
                    if (tiles[0,rowNumber].IsOccupied)
                    {
                        Console.WriteLine($"ERROR! Row {rowNumber + 1} is full, please try a different row..");
                        Thread.Sleep(milliseconds);
                        UpdateBoard();
                        Play();
                    }
                    else
                    {
                        // Drop the piece if the row isn't full
                        DropPiece(rowNumber, players[currentTurn].PlayerShape);
                    }
                }
                else
                {
                    // Inform the player they selected an invalid row and replay
                    Console.WriteLine($"ERROR! {rowNumber + 1} is invalid, please try again..\n");
                    Console.WriteLine("\n");
                    Thread.Sleep(milliseconds);
                    UpdateBoard();
                    Play();
                }
            }

        }

        /// <summary>
        /// Method that places a piece by going through the row from bottom to top, check if 
        /// the tile is occupied or not
        /// </summary>
        /// <param name="p_row"></param>
        /// <param name="p_shape"></param>
        private static void DropPiece(int p_row, char p_shape)
        {
            for(int i = boardHeight - 1; i >= 0; i--)
            {
                Tile tile = tiles[i, p_row];
                if(tile != null && tile.IsOccupied == false)
                {
                    tiles[i, p_row].IsOccupied = true;
                    tiles[i, p_row].Shape = p_shape;
                    break;
                }
            }

            // New line for tidiness 
            Console.WriteLine("\n");

            // Check for a win after placing each piece
            if (CheckForWin() == true)
            {
                Win();
            }

            // Check if the board is full before continuing to play
            bool fullBoard = CheckIsBoardFull();
            if (fullBoard) TieGame();

            // No winner and the board isn't full, keep playing and update the turn counter
            else
            {
                // Update/Reset turn counter
                currentTurn++;
                if (currentTurn > 1)
                {
                    currentTurn = 0;
                }


                UpdateBoard();
                Play();
            }
        }

        /// <summary>
        /// Redraws and updates information about the board
        /// </summary>
        private static void UpdateBoard()
        {
            string rowsHeader = "  1    2    3    4    5    6    7";
            Console.Write(rowsHeader);

            for (int i = 0; i < boardHeight; i++)
            {
                Console.WriteLine();
                for (int j = 0; j < boardWidth; j++)
                {
                    Tile tile = tiles[i, j];
                    Console.Write($"[ {tile.Shape} ]");
                }
            }

            Console.WriteLine("\n");
            Console.WriteLine($"Current player is {players[currentTurn].PlayerName} \n");
        }

        /// <summary>
        /// Method that checks for vertical, horizontal, and up/down diagonals to see if
        /// there is a match of 4 for the current players shape in any of those directions
        /// </summary>
        /// <returns></returns>
        private static bool CheckForWin()
        {
            bool didWin = false;
            char shape = players[currentTurn].PlayerShape;

            // Check for Horizontal (___) wins
            for(int i = 0; i < boardHeight; i++)
            {
                // Stops at boardWidth - 3 so the board can check within 3 spaces for a win
                for(int j = 0; j < boardWidth - 3; j++)
                {
                    if (tiles[i, j].Shape == shape && tiles[i,j + 1].Shape == shape
                        && tiles[i, j + 2].Shape == shape && tiles[i, j + 3].Shape == shape)
                    {
                        didWin = true;
                        return didWin;
                    }
                }
            }

            // Check for Vertical (|) wins
            for(int i = 0; i < boardHeight - 3; i++)
            {
                for(int j = 0; j < boardWidth; j ++)
                {
                    if (tiles[i, j].Shape == shape && tiles[i + 1, j].Shape == shape
                        && tiles[i + 2, j].Shape == shape && tiles[i + 3, j].Shape == shape)
                    {
                        didWin = true;
                        return didWin;
                    }
                }
            }

            // Check for Downward Diagonals (\) wins
            for (int i = 0; i < boardHeight - 3; i++)
            {
                for (int j = 0; j < boardWidth - 3; j++)
                {
                    if (tiles[i, j].Shape == shape && tiles[i + 1, j + 1].Shape == shape
                        && tiles[i + 2, j + 2].Shape == shape && tiles[i + 3, j + 3].Shape == shape)
                    {
                        didWin = true;
                        return didWin;
                    }
                }
            }


            // Finally, check for Upward Diagonals (/) wins 
            for (int i = 3; i < boardWidth - 1; i++)
            {
                for (int j = 0; j < boardHeight - 3; j++)
                {

                    if (tiles[i, j].Shape == shape && tiles[i - 1, j + 1].Shape == shape
                        && tiles[i - 2, j + 2].Shape == shape && tiles[i - 3, j + 3].Shape == shape)
                    {
                        didWin = true;
                        return didWin;
                    }
                }
            }

            // Return false because no wins were found
            return didWin;
        }
        

        /// <summary>
        /// Checks if the board is full and ends the game if it is (TIE)
        /// </summary>
        /// <returns></returns>
        private static bool CheckIsBoardFull()
        {
            int filledTiles = 0;
            for (int i = 0; i < boardHeight; i++)
            {
                for (int j = 0; j < boardWidth; j++)
                {
                    Tile tile = tiles[i, j];
                    if(tile.IsOccupied)
                    {
                        filledTiles++;
                    }
                }
            }

            if (filledTiles == boardHeight * boardWidth)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Displays the win state of the game if the current player has a match of 4 or more
        /// and updates the players win count
        /// </summary>
        /// <param name="p_winner"></param>
        private static void Win()
        {
            // Update the display to reflect the current winner
            Console.WriteLine("=========    GAME OVER!   =========");
            string rowsHeader = "  1    2    3    4    5    6    7";
            Console.Write(rowsHeader);

            for (int i = 0; i < boardHeight; i++)
            {
                Console.WriteLine();
                for (int j = 0; j < boardWidth; j++)
                {
                    Tile tile = tiles[i, j];
                    Console.Write($"[ {tile.Shape} ]");
                }
            }

            Console.WriteLine();
            Console.WriteLine($"{players[currentTurn].PlayerName} is the winner!\n");

            players[currentTurn].Wins++;

            // Prompt the players to play again
            Console.WriteLine("Play again? [Y/N]");
            string confirmationInput = Console.ReadLine();
            string restartKey = "y";

            // Restart if the players hit Y, otherwise, quit
            if(confirmationInput.ToLower() == restartKey)
            {
                Restart();
            }
        }

        /// <summary>
        /// Method that informs the players that the board is full and ends the game in a tie
        /// </summary>
        private static void TieGame()
        {
            Console.WriteLine($"Board was full without finding a winner, game is a tie. \n");
            Console.WriteLine("Restarting...");
            milliseconds = 3000;
            Thread.Sleep(milliseconds);
            Restart();
        }

        /// <summary>
        /// Restarts the game, updates the win counts display, and updates the board 
        /// </summary>
        private static void Restart()
        {
            // Display wins for the players
            Console.WriteLine($"{player1.PlayerName} wins: {player1.Wins}");
            Console.WriteLine($"{player2.PlayerName} wins: {player2.Wins}  \n");

            SetupGame();
            Play();
        }
    }
}
