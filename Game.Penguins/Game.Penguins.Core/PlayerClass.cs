using Game.Penguins.Core.Interfaces.Game.Players;
using System;
using System.Collections.Generic;

namespace Game.Penguins
{
    public class PlayerClass : IPlayer
    {
        public Guid Identifier { get; set; }

		public PlayerType PlayerType { get; set; }

		public PlayerColor Color { get; set; }

		public string Name { get; set; }

		public int Points { get; set; }

		public int Penguins { get; set; }

		public event EventHandler StateChanged;

		public PlayerClass(PlayerType playerType, PlayerColor playerColor, string name)
		{
			PlayerType = playerType;
			Color = playerColor;
			Name = name;

			Identifier = Guid.NewGuid();
			Points = 0;
			Penguins = 4;
		}

		public void ChangeState()
		{
			StateChanged.Invoke(this, null);
		}
	}
}
