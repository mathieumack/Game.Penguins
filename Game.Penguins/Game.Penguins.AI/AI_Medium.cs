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
		private Random r;
		public AI_Medium()
		{
			r = new Random();
		}
		public void Move()
		{
			//throw new NotImplementedException();
			bool movePenguins = false;
			int temoin = 0;


			do
			{
				Random r = new Random();

				Cell start = SearchCell(AIPenguins[CurrentPlayer][r.Next(0, CurrentPlayer.Penguins)]);
				Cell end = SearchCell(Board.Board[r.Next(0, 8), r.Next(0, 8)]);
				List<List<Cell>> avalaibleDeplacement = FindAvalaibleDeplacement(start, end);

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

					NextAction = NextActionType.MovePenguin;
					NextPlayer();
					StateChanged(this, null);
					start.ChangeState();
					end.ChangeState();
					AIPenguins[currentPlayer].Remove(start);
					AIPenguins[currentPlayer].Add(end);
					movePenguins = true;

				}
			} while (!movePenguins);
		}

		public void PlacePenguin()
		{
			if (CurrentPlayer.PlayerType == PlayerType.AIMedium)
			{
				/// Choix aleatoire de la case.
				int x;
				int y;
				Cell cell;

				bool penguinPlaced = false;

				while (!penguinPlaced)
				{
					x = r.Next(0, 8);
					y = r.Next(0, 8);

					cell = (Cell)Board.Board[x, y];

					if (cell.FishCount == 1 && cell.CurrentPenguin == null)
					{
						cell.CellType = CellType.FishWithPenguin;
						cell.CurrentPenguin = new Penguins(CurrentPlayer);

						NextAction = NextActionType.PlacePenguin;
						if (Turn == Players.Count() * PenguinsByPlayer)
						{
							NextAction = NextActionType.MovePenguin;
						}

						Turn++;
						AIPenguins[CurrentPlayer].Add(cell);
						NextPlayer();
						cell.ChangeState();
						StateChanged.Invoke(this, null);
						penguinPlaced = true;
					}
				}
			}
		}

	}

}