using Game.Penguins.Core.Interfaces.Game.Players;
using Game.Penguins.Extensions;
using Game.Penguins.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Penguins.ViewModels
{
    class WelcomeScreenViewModel
        : ViewModel
        , IApplicationContentView
    {
        public string Name => "Welcome";

        #region Players configuration

        private string player1Name = "Player 1";
        public string Player1Name
        {
            get => player1Name;
            set
            {
                if (player1Name != value)
                {
                    player1Name = value;

                    ValidateStringValue(value, nameof(Player1Name));

                    RaisePropertyChanged(nameof(Player1Name));
                }
            }
        }

        public string Player1Type { get; set; } = "Human";

        private string player2Name = "Player 2";
        public string Player2Name
        {
            get => player2Name;
            set
            {
                if (player2Name != value)
                {
                    player2Name = value;

                    ValidateStringValue(value, nameof(Player2Name));

                    RaisePropertyChanged(nameof(Player2Name));
                }
            }
        }

        public string Player2Type { get; set; }

        private string player3Name = "Player 3";
        public string Player3Name
        {
            get => player3Name;
            set
            {
                if (player3Name != value)
                {
                    player3Name = value;

                    ValidateStringValue(value, nameof(Player3Name));

                    RaisePropertyChanged(nameof(Player3Name));
                }
            }
        }

        public string Player3Type { get; set; }

        private string player4Name = "Player 4";
        public string Player4Name
        {
            get => player4Name;
            set
            {
                if (player4Name != value)
                {
                    player4Name = value;

                    ValidateStringValue(value, nameof(Player4Name));

                    RaisePropertyChanged(nameof(Player4Name));
                }
            }
        }

        public string Player4Type { get; set; }

        private ConfigurationPlayer GetConfigurationPlayer(string playerName, string playerType)
        {
            return new ConfigurationPlayer()
            {
                PlayerName = playerName,
                PlayerType = playerType.ToPlayerType()
            };
        }

        #endregion

        #region Validation

        private void ValidateStringValue(string value, string propertyName)
        {
            ClearValidationErrors(propertyName);

            if (string.IsNullOrEmpty(value))
                AddValidationError(propertyName, "Value must not be null or empty.");
        }

        private bool ValidateForm()
        {
            ValidateStringValue(Player1Name, nameof(Player1Name));
            ValidateStringValue(Player2Name, nameof(Player2Name));
            ValidateStringValue(Player3Name, nameof(Player3Name));
            ValidateStringValue(Player4Name, nameof(Player4Name));

            RaisePropertyChanged(nameof(Player1Name));
            RaisePropertyChanged(nameof(Player2Name));
            RaisePropertyChanged(nameof(Player3Name));
            RaisePropertyChanged(nameof(Player4Name));

            return !GetAllErrors().Any();
        }

        #endregion

        #region Navigation

        public string PreviousButtonContent => "";

        public string NextButtonContent => "Start";

        public bool HasPreviousView => false;

        public bool HasNextView => true;

        public IApplicationContentView GetPreviousView()
        {
            throw new InvalidOperationException();
        }

        public IApplicationContentView GetNextView()
        {
            if (ValidateForm())
            {
                var players = new List<ConfigurationPlayer>()
                {
                    GetConfigurationPlayer(Player1Name, Player1Type),
                    GetConfigurationPlayer(Player2Name, Player2Type)
                };
                if(Player3Type != "None")
                    players.Add(GetConfigurationPlayer(Player3Name, Player3Type));
                if (Player4Type != "None")
                    players.Add(GetConfigurationPlayer(Player4Name, Player4Type));

                return new CurrentGameViewModel(players);
            }

            return null;
        }

        #endregion
    }
}
