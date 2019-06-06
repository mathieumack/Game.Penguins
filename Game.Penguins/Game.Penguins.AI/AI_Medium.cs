using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Penguins.Core.Interfaces.Game.GameBoard;
using Game.Penguins.Core.Interfaces.Game.Players;


namespace Game.Penguins
{
	public class AI_Medium
	{
		public Random r { get; }

		public AI_Medium()
		{
			r = new Random();
		}

		private void NextPlayer(IList<IPlayer> Players, IPlayer CurrentPlayer)
		{
			int nextPlayerIndex = Players.IndexOf(CurrentPlayer) + 1;

			if (nextPlayerIndex == Players.Count())
			{
				nextPlayerIndex = 0;
			}

			CurrentPlayer = Players[nextPlayerIndex];
		}
		private Cell SearchCell(IBoard Board, ICell cellToFind)
		{
			for (int i = 0; i < Board.Board.GetLength(0); i++)
			{
				for (int j = 0; j < Board.Board.GetLength(1); j++)
				{
					if (Board.Board[i, j] == cellToFind)
					{
						return (Cell)Board.Board[i, j];
					}
				}
			}

			return null;
		}
		private bool IsInAvalaibleDeplacement(List<List<Cell>> avalaibleDeplacement, ICell destination)
		{
			for (int i = 0; i < avalaibleDeplacement.Count; i++)
			{
				if (avalaibleDeplacement[i].Exists(e => e == destination))
				{
					return true;
				}
			}

			return false;
		}
		private List<List<Cell>> FindAvalaibleDeplacement(IBoard Board, Cell origin, Cell dest)
		{
			List<List<Cell>> avalaibleDeplacement = new List<List<Cell>>();
			int[] cellIndexOrigin = SearchIndexOfCell(Board, origin);

			avalaibleDeplacement.Add(FindAvalaibleLigne(Board, cellIndexOrigin));
			avalaibleDeplacement.Add(FindAvalaibleDiagGauche(Board, cellIndexOrigin));
			avalaibleDeplacement.Add(FindAvalaibleDiagDroite(Board, cellIndexOrigin));

			avalaibleDeplacement = RemoveUnreachableCellInLigne(Board, avalaibleDeplacement, cellIndexOrigin);
			avalaibleDeplacement = RemoveUnreachableCellInDiagGauche(Board, avalaibleDeplacement, cellIndexOrigin);
			avalaibleDeplacement = RemoveUnreachableCellInDiagDroite(Board, avalaibleDeplacement, cellIndexOrigin);

			return avalaibleDeplacement;
		}
		private int[] SearchIndexOfCell(IBoard Board, Cell cell)
		{
			int[] index = new int[2];

			for (int i = 0; i < Board.Board.GetLength(0); i++)
			{
				for (int j = 0; j < Board.Board.GetLength(1); j++)
				{
					if (Board.Board[i, j] == cell)
					{
						index[0] = i;
						index[1] = j;

						return index;
					}
				}
			}

			return null;
		}
		private List<List<Cell>> RemoveUnreachableCellInLigne(IBoard Board, List<List<Cell>> avalaibleDeplacement, int[] cellIndexOrigin)
		{
			int[] rm = new int[2];
			int[] rm1 = new int[2];

			for (int i = (avalaibleDeplacement[0].Count - 1); i > 0; i--)
			{
				if (avalaibleDeplacement[0][i].CellType != CellType.Fish)
				{
					if (SearchIndexOfCell(Board, avalaibleDeplacement[0][i])[0] > cellIndexOrigin[0])
					{
						rm[0] = i;
						rm[1] = avalaibleDeplacement[0].Count - i;
					}
					else if (SearchIndexOfCell(Board, avalaibleDeplacement[0][i])[0] < cellIndexOrigin[0])
					{
						rm1[0] = 0;
						rm1[1] = i + 1;
						i = 0;
					}
				}
			}

			avalaibleDeplacement[0].RemoveRange(rm[0], rm[1]);
			avalaibleDeplacement[0].RemoveRange(rm1[0], rm1[1]);

			return avalaibleDeplacement;
		}
		private List<List<Cell>> RemoveUnreachableCellInDiagGauche(IBoard Board, List<List<Cell>> avalaibleDeplacement, int[] cellIndexOrigin)
		{
			int[] rm = new int[2];
			int[] rm1 = new int[2];

			for (int i = (avalaibleDeplacement[1].Count - 1); i > 0; i--)
			{
				if (avalaibleDeplacement[1][i].CellType != CellType.Fish)
				{
					if (SearchIndexOfCell(Board, avalaibleDeplacement[1][i])[1] > cellIndexOrigin[1])
					{
						rm[0] = i;
						rm[1] = avalaibleDeplacement[1].Count - i;
					}
					else if (SearchIndexOfCell(Board, avalaibleDeplacement[1][i])[1] < cellIndexOrigin[1])
					{
						rm1[0] = 0;
						rm1[1] = i + 1;
						i = 0;
					}
				}
			}

			avalaibleDeplacement[1].RemoveRange(rm[0], rm[1]);
			avalaibleDeplacement[1].RemoveRange(rm1[0], rm1[1]);

			return avalaibleDeplacement;
		}
		private List<List<Cell>> RemoveUnreachableCellInDiagDroite(IBoard Board, List<List<Cell>> avalaibleDeplacement, int[] cellIndexOrigin)
		{
			int[] rm = new int[2];
			int[] rm1 = new int[2];

			for (int i = (avalaibleDeplacement[2].Count - 1); i > 0; i--)
			{
				if (avalaibleDeplacement[2][i].CellType != CellType.Fish)
				{
					if (SearchIndexOfCell(Board, avalaibleDeplacement[2][i])[1] < cellIndexOrigin[1])
					{
						rm[0] = i;
						rm[1] = avalaibleDeplacement[2].Count - i;
					}
					else if (SearchIndexOfCell(Board, avalaibleDeplacement[2][i])[1] > cellIndexOrigin[1])
					{
						rm1[0] = 0;
						rm1[1] = i + 1;
						i = 0;
					}
				}
			}

			avalaibleDeplacement[2].RemoveRange(rm[0], rm[1]);
			avalaibleDeplacement[2].RemoveRange(rm1[0], rm1[1]);

			return avalaibleDeplacement;
		}
		private List<Cell> FindAvalaibleDiagDroite(IBoard Board, int[] cellIndexOrigin)
		{
			List<Cell> deplacementDiagDroite = new List<Cell>();

			for (int i = 0; i < Board.Board.GetLength(0); i++)
			{
				for (int j = 0; j < Board.Board.GetLength(1); j++)
				{
					if ((j % 2 == 0 && cellIndexOrigin[1] % 2 == 0) || (j % 2 == 1 && cellIndexOrigin[1] % 2 == 1))
					{
						if (j < cellIndexOrigin[1])
						{
							if (i == cellIndexOrigin[0] + ((cellIndexOrigin[1] - j) / 2))
							{
								deplacementDiagDroite.Add((Cell)Board.Board[i, j]);
							}
						}
						else if (j > cellIndexOrigin[1])
						{
							if (i == cellIndexOrigin[0] - ((j - cellIndexOrigin[1]) / 2))
							{
								deplacementDiagDroite.Add((Cell)Board.Board[i, j]);
							}
						}
					}
					else if ((j % 2 == 1 && cellIndexOrigin[1] % 2 == 0))
					{
						if (j < cellIndexOrigin[1])
						{
							if (i == cellIndexOrigin[0] + (Decimal.Floor((cellIndexOrigin[1] - j) / 2)))
							{
								deplacementDiagDroite.Add((Cell)Board.Board[i, j]);
							}
						}
						else if (j > cellIndexOrigin[1])
						{
							if (i == cellIndexOrigin[0] - (Decimal.Floor((j - cellIndexOrigin[1]) / 2) + 1))
							{
								deplacementDiagDroite.Add((Cell)Board.Board[i, j]);
							}
						}
					}
					else if ((j % 2 == 0 && cellIndexOrigin[1] % 2 == 1))
					{
						if (j < cellIndexOrigin[1])
						{
							if (i == cellIndexOrigin[0] + (Decimal.Floor((cellIndexOrigin[1] - j) / 2) + 1))
							{
								deplacementDiagDroite.Add((Cell)Board.Board[i, j]);
							}
						}
						else if (j > cellIndexOrigin[1])
						{
							if (i == cellIndexOrigin[0] - (Decimal.Floor((j - cellIndexOrigin[1]) / 2)))
							{
								deplacementDiagDroite.Add((Cell)Board.Board[i, j]);
							}
						}
					}
				}
			}

			deplacementDiagDroite = deplacementDiagDroite.OrderByDescending(e => SearchIndexOfCell(Board, e)[1]).ToList();
			return deplacementDiagDroite;
		}
		private List<Cell> FindAvalaibleDiagGauche(IBoard Board, int[] cellIndexOrigin)
		{
			List<Cell> deplacementDiagGauche = new List<Cell>();

			for (int i = 0; i < Board.Board.GetLength(0); i++)
			{
				for (int j = 0; j < Board.Board.GetLength(1); j++)
				{
					if ((j % 2 == 0 && cellIndexOrigin[1] % 2 == 0) || (j % 2 == 1 && cellIndexOrigin[1] % 2 == 1))
					{
						if (j < cellIndexOrigin[1])
						{
							if (i == cellIndexOrigin[0] - ((cellIndexOrigin[1] - j) / 2))
							{
								deplacementDiagGauche.Add((Cell)Board.Board[i, j]);
							}
						}
						else if (j > cellIndexOrigin[1])
						{
							if (i == cellIndexOrigin[0] + ((j - cellIndexOrigin[1]) / 2))
							{
								deplacementDiagGauche.Add((Cell)Board.Board[i, j]);
							}
						}
					}
					else if ((j % 2 == 1 && cellIndexOrigin[1] % 2 == 0))
					{
						if (j < cellIndexOrigin[1])
						{
							if (i == cellIndexOrigin[0] - (Decimal.Floor((cellIndexOrigin[1] - j) / 2) + 1))
							{
								deplacementDiagGauche.Add((Cell)Board.Board[i, j]);
							}
						}
						else if (j > cellIndexOrigin[1])
						{
							if (i == cellIndexOrigin[0] + (Decimal.Floor((j - cellIndexOrigin[1]) / 2)))
							{
								deplacementDiagGauche.Add((Cell)Board.Board[i, j]);
							}
						}
					}
					else if ((j % 2 == 0 && cellIndexOrigin[1] % 2 == 1))
					{
						if (j < cellIndexOrigin[1])
						{
							if (i == cellIndexOrigin[0] - (Decimal.Floor((cellIndexOrigin[1] - j) / 2)))
							{
								deplacementDiagGauche.Add((Cell)Board.Board[i, j]);
							}
						}
						else if (j > cellIndexOrigin[1])
						{
							if (i == cellIndexOrigin[0] + (Decimal.Floor((j - cellIndexOrigin[1]) / 2) + 1))
							{
								deplacementDiagGauche.Add((Cell)Board.Board[i, j]);
							}
						}
					}
				}
			}

			return deplacementDiagGauche;
		}
		private List<Cell> FindAvalaibleLigne(IBoard Board, int[] cellIndexOrigin)
		{
			List<Cell> deplacementLigne = new List<Cell>();

			for (int i = 0; i < Board.Board.GetLength(0); i++)
			{
				for (int j = 0; j < Board.Board.GetLength(1); j++)
				{
					if (j == cellIndexOrigin[1] && i != cellIndexOrigin[0])
					{
						deplacementLigne.Add((Cell)Board.Board[i, j]);
					}
				}
			}

			return deplacementLigne;
		}
		private bool RemovePenguins(IBoard Board, Cell origin, PlayerClass currentPlayer)
		{
			int[] originIndex = SearchIndexOfCell(Board, origin);
			int x = originIndex[0];
			int y = originIndex[1];
			bool token = false;
			Console.Write("penguin in cell [{0},{1}]", x, y);

			if (y>0)
			{
				if (Board.Board[x, y - 1].CurrentPenguin != null || Board.Board[x, y - 1].CellType == CellType.Water)
				{
					token = true;
				}
			}
			if (y<7)
			{
				if (Board.Board[x, y + 1].CurrentPenguin != null || Board.Board[x, y + 1].CellType == CellType.Water)
				{
					token = true;
				}
			}
			if (x<7)
			{
				if (Board.Board[x + 1, y].CurrentPenguin != null || Board.Board[x+1, y].CellType == CellType.Water)
				{
					token = true;
				}
			}
			if (x>0)
			{
				if (Board.Board[x - 1, y].CurrentPenguin != null || Board.Board[x-1, y].CellType == CellType.Water)
				{
					token = true;
				}
			}
			if (y%2 != 0) /// line number is odd
			{
				if (x < 7 && y > 0)
				{
					if (Board.Board[x + 1, y - 1].CurrentPenguin != null || Board.Board[x + 1, y - 1].CellType == CellType.Water)
					{
						token = true;
					}
				}
				if (x < 7 && y < 7)
				{
					if (Board.Board[x + 1, y + 1].CurrentPenguin != null || Board.Board[x + 1, y + 1].CellType == CellType.Water)
					{
						token = true;
					}
				}
			}
			if (y%2 == 0) /// line number is even
			{
				if (x > 0 && y > 0)
				{
					if (Board.Board[x - 1, y - 1].CurrentPenguin != null || Board.Board[x - 1, y - 1].CellType == CellType.Water)
					{
						token = true;
					}
				}
				if (x > 0 && y < 7)
				{
					if (Board.Board[x - 1, y + 1].CurrentPenguin != null || Board.Board[x - 1, y + 1].CellType == CellType.Water)
					{
						token = true;
					}
				}

			}
			if (token==true)
			{
				Console.WriteLine(" needs to be deleted");
				currentPlayer.Penguins--;
				//remove penguin
			}
			else
			{
				Console.WriteLine("doesn't need to be deleted");
			}
			return token;

		}
		public void Move(IBoard Board, IPlayer CurrentPlayer, Dictionary<IPlayer, List<Cell>> AIPenguins)
		{
			bool movePenguins = false;

			do
			{
				int random = r.Next(0, CurrentPlayer.Penguins);
				Cell start = SearchCell(Board, AIPenguins[CurrentPlayer][random]);
				Cell end = SearchCell(Board, Board.Board[r.Next(0, 8), r.Next(0, 8)]);
				List<List<Cell>> avalaibleDeplacement = FindAvalaibleDeplacement(Board, start, end);

				if (start.CurrentPenguin.Player == CurrentPlayer && end.CellType == CellType.Fish && IsInAvalaibleDeplacement(avalaibleDeplacement, end))
				{
					PlayerClass currentPlayer = (PlayerClass)CurrentPlayer;
					currentPlayer.Points += start.FishCount;
					currentPlayer.ChangeState();

					start.CellType = CellType.Water;
					start.FishCount = 0;
					start.CurrentPenguin = null;

					end.CellType = CellType.FishWithPenguin;
					end.CurrentPenguin = new Penguins(CurrentPlayer);

					start.ChangeState();
					end.ChangeState();
					AIPenguins[currentPlayer].Remove(start);
					if (!RemovePenguins(Board, end, currentPlayer))
					{
						AIPenguins[currentPlayer].Add(end);
					}
					movePenguins = true;
				}
			} while (!movePenguins);

		}
		
		public void PlacePenguin(IBoard Board, IPlayer CurrentPlayer, Dictionary<IPlayer, List<Cell>> AIPenguins)
		{
			List<Cell> AvailableCell = new List<Cell>();

			foreach (Cell tempcell in Board.Board)
			{
				if (tempcell.FishCount==1)
				{
					AvailableCell.Add(tempcell);
				}
			}
			bool token = false;
			do
			{
				/// Choix aleatoire de la case.
				int index = r.Next(0, AvailableCell.Count());
				Cell cell = AvailableCell[index];
				if (cell.CurrentPenguin == null)
				{
					cell.CellType = CellType.FishWithPenguin;
					cell.CurrentPenguin = new Penguins(CurrentPlayer);

					AIPenguins[CurrentPlayer].Add(cell);
					cell.ChangeState();
					token = true;
				}
				else
				{
					AvailableCell.Remove(cell);
				}
				

			} while (!token);
			

		}
	}
}
