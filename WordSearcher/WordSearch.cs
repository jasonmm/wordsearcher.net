using System;
using Microsoft.Extensions.Logging;

namespace WordSearcher
{
    class WordSearch
    {
        private const string alphabet = "abcdefghijklmnopqrstuvwxyz";
        private int[,] directions = {
            {-1, 0, 1, 1, 1, 0, -1, -1},            // x
            {-1, -1, -1, 0, 1, 1, 1, 0}            // y
        };

        public string Title { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public string SortType { get; set; }
        public bool ShowDate { get; set; }
        public bool UppercaseWords { get; set; }
        public string[] WordList { get; set; }
        public int MaxPlacementTries { get; set; }

        private readonly ILogger<WordSearcher.Controllers.WordSearcherController> logger;

        public WordSearch(CreateGridRequest request, ILogger<WordSearcher.Controllers.WordSearcherController> l)
        {
            Title = request.Title;
            Rows = request.Rows;
            Columns = request.Columns;
            SortType = request.SortType;
            ShowDate = request.ShowDate;
            UppercaseWords = request.UppercaseWords;
            WordList = request.WordList;
            logger = l;
        }

        public char[,] BuildGrid()
        {
            var random = new Random();

            var grid = InitGrid();
            foreach (string word in WordList)
            {
                if (word.Length == 0)
                {
                    continue;
                }

                var numTries = 0;
                while (numTries < MaxPlacementTries)
                {
                StartTryPlacement:
                    numTries++;

                    logger.LogDebug("Try #" + numTries);

                    // Pick a random row, column, and direction.
                    var randRow = random.Next(0, Rows - 1);
                    var randCol = random.Next(0, Columns - 1);
                    var dirIndex = random.Next(0, 7);

                    // Get the x and y directions
                    var dx = directions[0, dirIndex];
                    var dy = directions[1, dirIndex];

                    // Check to see if the word will fit in the word search grid.
                    var endRow = randRow + (dy * word.Length);
                    var endCol = randCol + (dx * word.Length);
                    if (endRow < 0 || endRow >= Rows || endCol < 0 || endCol >= Columns)
                    {
                        logger.LogDebug($"{endRow},{endCol} out of range.");
                        continue;
                    }

                    // Check to see if placing this word here will work with the other words already placed in the grid.
                    for (int i = 0; i < word.Length; i++)
                    {
                        if (gridSquareIsTaken(randRow, randCol, word[i], grid))
                        {
                            logger.LogDebug($"{endRow},{endCol} is taken.");
                            goto StartTryPlacement;
                        }
                        randRow += dy;
                        randCol += dx;
                    }

                    // Place the word in the grid.  The word is placed in
                    //  the grid "backwards".  We also record the position of
                    //  the word here so that we always know where this word
                    //	is in the grid.
                    var endPoint = new Point
                    {
                        X = randCol - dx,
                        Y = randRow - dy
                    };
                    for (int i = word.Length - 1; i >= 0; i--)
                    {
                        randRow -= dy;
                        randCol -= dx;
                        grid[randRow, randCol] = char.ToUpper(word[i]);
                    }
                    var startPoint = new Point
                    {
                        X = randCol,
                        Y = randRow
                    };
                    break;
                }
                if (numTries >= MaxPlacementTries)
                {
                    throw new Exception("Error: Unable to place '" + word + "' in the word search grid (" + Rows + "," + Columns + ") after " + numTries + " of " + MaxPlacementTries + " tries.  Try using fewer words or a larger grid");
                }
            }

            // Fill in the rest of the grid with random letters.
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns; x++)
                {
                    if (grid[y, x] == ' ')
                    {
                        grid[y, x] = char.ToUpper(alphabet[random.Next(25)]);
                    }
                }
            }

            return grid;
        }

        private bool gridSquareIsTaken(int row, int column, char ch, char[,] grid)
        {
            return grid[row, column] != ' ' && grid[row, column] != ch;
        }

        private char[,] InitGrid()
        {
            var grid = new char[Rows, Columns];
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    grid[r, c] = ' ';
                }
            }
            return grid;
        }
    }
}