using Game.Penguins.Core.Interfaces.Game.Players;
using Game.Penguins.Framework;

namespace Game.Penguins.ViewModels
{
    public class PlayerViewModel : ViewModel
    {
        private readonly IPlayer player;

        private int points;
        public int Points
        {
            get => points;
            set
            {
                if (points != value)
                {
                    points = value;
                    RaisePropertyChanged(nameof(Points));
                }
            }
        }

        private string playerName;
        public string PlayerName
        {
            get => playerName;
            set
            {
                if (playerName != value)
                {
                    playerName = value;
                    RaisePropertyChanged(nameof(PlayerName));
                }
            }
        }

        public PlayerViewModel(IPlayer player)
            : base()
        {
            this.player = player;
            RefreshState();
            player.StateChanged += Player_StateChanged;
        }

        private void Player_StateChanged(object sender, System.EventArgs e)
        {
            RefreshState();
        }

        private void RefreshState()
        {
            PlayerName = player.Name;
            Points = player.Points;
        }
    }
}
