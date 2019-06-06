using Game.Penguins.Core.Interfaces.Game.Actions;
using Game.Penguins.Core.Interfaces.Game.GameBoard;
using Game.Penguins.Core.Interfaces.Game.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Penguins
{
    public static class BoardExtensions
    {
        /// <summary>
        /// Init a new board
        /// </summary>
        /// <param name="board"></param>
        public static void InitBoard(this Cell[,] board, int sizeX, int sizeY)
        {
            var points = Enumerable.Repeat(1, 34).ToList();
            points.AddRange(Enumerable.Repeat(2, 20).ToList());
            points.AddRange(Enumerable.Repeat(3, 10).ToList());

            var random = new Random();

            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    var pointIndex = random.Next(0, points.Count);
                    board[i, j] = new Cell()
                    {
                        CellType = CellType.Fish,
                        FishCount = points[pointIndex],
                        X = i,
                        Y = j
                    };
                    points.RemoveAt(pointIndex);
                }
            }
        }

        /// <summary>
        /// Returns cell with penguins for a user
        /// </summary>
        /// <param name="board"></param>
        /// <param name="playerIdentifier"></param>
        /// <returns></returns>
        public static IList<Cell> GetMyPenguins(this Cell[,] board, Guid playerIdentifier)
        {
            var result = new List<Cell>();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j].CellType == CellType.FishWithPenguin && board[i, j].CurrentPenguin.Player.Identifier == playerIdentifier)
                        result.Add(board[i, j]);
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="board"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static IList<Cell> GetAvailableCells(this Cell[,] board, Cell origin)
        {
            var result = new List<Cell>();

            for (int i = 0; i <= 5; i++)
                result.AddRange(board.GetAvailableCells(origin, (Direction)i));

            return result;
        }

        /// <summary>
        /// Returns all available cell for a penguins from an origin cell
        /// </summary>
        /// <param name="board"></param>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static IList<Cell> GetAvailableCells(this Cell[,] board, Cell origin, Direction direction)
        {
            var result = new List<Cell>();

            Cell destination = null;
            int xDest = 0;
            switch (direction)
            {
                case Direction.Left:
                    if (origin.X > 0)
                    {
                        destination = board[origin.X - 1, origin.Y];
                    }
                    break;
                case Direction.Right:
                    if (origin.X < 7)
                    {
                        destination = board[origin.X + 1, origin.Y];
                    }
                    break;
                case Direction.TopLeft:
                    xDest = (origin.Y % 2 == 0) ? origin.X - 1 : origin.X;
                    if (xDest >= 0 && origin.Y > 0)
                    {
                        destination = board[xDest, origin.Y - 1];
                    }
                    break;
                case Direction.TopRight:
                    xDest = (origin.Y % 2 == 0) ? origin.X : origin.X + 1;
                    if (xDest < 8 && origin.Y > 0)
                    {
                        destination = board[xDest, origin.Y - 1];
                    }
                    break;
                case Direction.BottomLeft:
                    xDest = (origin.Y % 2 == 0) ? origin.X - 1 : origin.X;
                    if (xDest >= 0 && origin.Y < 7)
                    {
                        destination = board[xDest, origin.Y + 1];
                    }
                    break;
                case Direction.BottomRight:
                    xDest = (origin.Y % 2 == 0) ? origin.X : origin.X + 1;
                    if (xDest < 8 && origin.Y < 7)
                    {
                        destination = board[xDest, origin.Y + 1];
                    }
                    break;
            }

            if (destination != null && destination.CellType == CellType.Fish)
            {
                result.Add(destination);
                result.AddRange(board.GetAvailableCells(destination, direction));
            }

            return result;
        }

        /// <summary>
        /// Check if we can place a penguin on a cell
        /// </summary>
        /// <param name="board"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool CanPlacePenguin(this Cell[,] board, int x, int y)
        {
            return board[x, y].CellType == CellType.Fish && board[x, y].FishCount != 3;
        }
    }

    public enum Direction
    {
        Left = 0,
        Right,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    public class IA
    {
        private readonly Random random = new Random();
        private readonly CustomGame game;
        private readonly Guid playerIdentifier;

        private Cell[,] Board
        {
            get
            {
                return game.BoardObject.BoardObject;
            }
        }

        public IA(Guid playerIdentifier, CustomGame game)
        {
            this.playerIdentifier = playerIdentifier;
            this.game = game;
        }

        public void PlacePenguin()
        {
            // Place a penguin on the map for the IA :
            var nextX = random.Next(0, 8);
            var nextY = random.Next(0, 8);

            while (!Board.CanPlacePenguin(nextX, nextY))
            {
                nextX = random.Next(0, 8);
                nextY = random.Next(0, 8);
            }

            game.PlacePenguinManual(nextX, nextY);
        }

        public Tuple<Cell,Cell> Move()
        {
            var cellswithPenguin = Board.GetMyPenguins(playerIdentifier);

            var preferedCell = cellswithPenguin.Select(e => new
            {
                Origin = e,
                PreferedCell = GetPreferedCell(e)
            })
            .Where(e => e.PreferedCell != null)
            .OrderByDescending(e => e.PreferedCell.FishCount)
            .FirstOrDefault();
            
            return new Tuple<Cell, Cell>(preferedCell.Origin, preferedCell.PreferedCell);
        }

        private Cell GetPreferedCell(Cell origin)
        {
            var avalableCells = Board.GetAvailableCells(origin);

            if (avalableCells.Count > 0)
            {
                // We search items with 3 fish first
                if (avalableCells.Any(e => e.FishCount == 3))
                    return avalableCells.FirstOrDefault(e => e.FishCount == 3);

                if (avalableCells.Any(e => e.FishCount == 2))
                    return avalableCells.FirstOrDefault(e => e.FishCount == 2);

                // No prefered rules, we select randomly an available cell
                return avalableCells[random.Next(0, avalableCells.Count)];
            }

            return null;
        }
    }

    public class Player : IPlayer
    {
        private Guid identifier;
        public Guid Identifier
        {
            get
            {
                return identifier;
            }
            set
            {
                identifier = value;
                if (StateChanged != null)
                    StateChanged.Invoke(this, null);
            }
        }

        private PlayerType playerType;
        public PlayerType PlayerType
        {
            get
            {
                return playerType;
            }
            set
            {
                playerType = value;
                if (StateChanged != null)
                    StateChanged.Invoke(this, null);
            }
        }

        private PlayerColor color;
        public PlayerColor Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                if (StateChanged != null)
                    StateChanged.Invoke(this, null);
            }
        }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                if (StateChanged != null)
                    StateChanged.Invoke(this, null);
            }
        }

        private int points;
        public int Points
        {
            get
            {
                return points;
            }
            set
            {
                points = value;
                if (StateChanged != null)
                    StateChanged.Invoke(this, null);
            }
        }

        private int penguins;
        public int Penguins
        {
            get
            {
                return penguins;
            }
            set
            {
                penguins = value;
                if (StateChanged != null)
                    StateChanged.Invoke(this, null);
            }
        }

        public IA IA { get; set; }

        public event EventHandler StateChanged;
    }

    public class Penguin : IPenguin
    {
        public IPlayer Player
        {
            get
            {
                return PlayerObject;
            }
        }

        public Player PlayerObject { get; set; }
    }

    public class Cell : ICell
    {
        private CellType cellType;
        public CellType CellType
        {
            get
            {
                return cellType;
            }
            set
            {
                cellType = value;
                if (StateChanged != null)
                    StateChanged.Invoke(this, null);
            }
        }

        private int fishCount;
        public int FishCount
        {
            get
            {
                return fishCount;
            }
            set
            {
                fishCount = value;
                if (StateChanged != null)
                    StateChanged.Invoke(this, null);
            }
        }

        public IPenguin CurrentPenguin
        {
            get
            {
                return CurrentPenguinObject;
            }
        }

        private Penguin currentPenguinObject;
        public Penguin CurrentPenguinObject
        {
            get
            {
                return currentPenguinObject;
            }
            set
            {
                currentPenguinObject = value;
                if (StateChanged != null)
                    StateChanged.Invoke(this, null);
            }
        }

        public int X { get; set; }
        public int Y { get; set; }

        public event EventHandler StateChanged;
    }

    public class Board : IBoard
    {
        ICell[,] IBoard.Board
        {
            get
            {
                return BoardObject;
            }
        }

        public Cell[,] BoardObject { get; set; }
    }

    public class CustomGame : IGame
    {
        private readonly Random random = new Random();

        public IBoard Board
        {
            get
            {
                return BoardObject;
            }
        }

        public Board BoardObject { get; set; }

        public NextActionType NextAction { get; set; }

        public IPlayer CurrentPlayer
        {
            get
            {
                return CurrentPlayerObject;
            }
        }

        private int currentPlayerIndex = 0;
        public Player CurrentPlayerObject { get; set; }

        public IList<IPlayer> Players
        {
            get
            {
                return PlayersObject.OfType<IPlayer>().ToList();
            }
        }

        private int penguinsByPlayer = 4;
        public List<Player> PlayersObject { get; set; } = new List<Player>();

        public event EventHandler StateChanged;

        public IPlayer AddPlayer(string playerName, PlayerType playerType)
        {
            var identifier = Guid.NewGuid();

            PlayersObject.Add(new Player()
            {
                Identifier = identifier,
                Name = playerName,
                PlayerType = playerType,
                Penguins = 0,
                Points = 0,
                IA = playerType == PlayerType.Human ? null : new IA(identifier, this)
            });

            return PlayersObject.Last();
        }

        public void Move()
        {
            if (CurrentPlayerObject.PlayerType != PlayerType.Human)
            {
                var aiMove = CurrentPlayerObject.IA.Move();
                if(aiMove != null)
                {
                    MovePenguinOnMap(aiMove.Item1, aiMove.Item2);
                }
            }

            CheckActions();
        }

        public void MoveManual(ICell origin, ICell destination)
        {
            MovePenguinOnMap(origin, destination);

            // Check blocked penguins :
            CheckBlockedPenguins();
            
            CheckActions();
        }

        public void PlacePenguin()
        {
            if (CurrentPlayerObject.PlayerType != PlayerType.Human)
                CurrentPlayerObject.IA.PlacePenguin();
        }

        public void PlacePenguinManual(int x, int y)
        {
            if (BoardObject.BoardObject[x, y].CellType != CellType.Fish)
                return;

            BoardObject.BoardObject[x, y].CellType = CellType.FishWithPenguin;
            BoardObject.BoardObject[x, y].CurrentPenguinObject = new Penguin()
            {
                PlayerObject = CurrentPlayerObject
            };

            CurrentPlayerObject.Penguins++;

            CheckActions();
        }

        public void StartGame()
        {
            InitPlayers();

            BoardObject = new Board()
            {
                BoardObject = new Cell[8, 8]
            };
            BoardObject.BoardObject.InitBoard(8, 8);

            CheckActions();
        }

        #region Init game

        private void MovePenguinOnMap(ICell origin, ICell destination)
        {
            var originCell = origin as Cell;
            var destinationCell = destination as Cell;

            destinationCell.CurrentPenguinObject = originCell.CurrentPenguinObject;
            destinationCell.CellType = CellType.FishWithPenguin;

            CurrentPlayerObject.Points += originCell.FishCount;

            originCell.CellType = CellType.Water;
            originCell.FishCount = 0;
            originCell.CurrentPenguinObject = null;

            // Check blocked penguins :
            CheckBlockedPenguins();
        }
                
        private void InitPlayers()
        {
            penguinsByPlayer = PlayersObject.Count == 2 ? 4 : (PlayersObject.Count == 3 ? 3 : 2);
            foreach (var player in PlayersObject)
            {
                player.Penguins = 0;
                player.Color = (PlayerColor)PlayersObject.IndexOf(player);
            }
        }

        private void CheckActions()
        {
            CheckNextPlayer();

            CheckNextAction();

            if (StateChanged != null)
                StateChanged.Invoke(this, null);
        }

        private void CheckNextPlayer()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % PlayersObject.Count;
            CurrentPlayerObject = PlayersObject[currentPlayerIndex];
        }

        private void CheckNextAction()
        {
            // Need to place more penguins ?
            if (PlayersObject.Any(e => e.Penguins < penguinsByPlayer))
            {
                NextAction = NextActionType.PlacePenguin;
                return;
            }

            if(!Players.Any(e => BoardObject.BoardObject.GetMyPenguins(e.Identifier).Count > 0))
            { 
                // No more penguins, we can stop the game :
                NextAction = NextActionType.Nothing;
            }
            else
            {
                NextAction = NextActionType.MovePenguin;
                // We check the current player available actions :
                var penguinCells = BoardObject.BoardObject.GetMyPenguins(CurrentPlayer.Identifier);
                if (penguinCells.Count == 0)
                    CheckActions();
            }
        }

        private void CheckBlockedPenguins()
        {
            foreach(var player in Players)
            {
                var penguinCells = BoardObject.BoardObject.GetMyPenguins(player.Identifier);
                foreach (var penguinCell in penguinCells)
                {
                    // We check if the destination cell is specific (no available cell in a next round) :
                    if (BoardObject.BoardObject.GetAvailableCells(penguinCell).Count == 0)
                    {
                        // No more available cells, we can remove the penguin :
                        CurrentPlayerObject.Points += penguinCell.FishCount;
                        penguinCell.CellType = CellType.Water;
                        penguinCell.FishCount = 0;
                        penguinCell.CurrentPenguinObject = null;
                    }
                }
            }
        }

        #endregion
    }

    class FakeClasses
    {
    }
}
