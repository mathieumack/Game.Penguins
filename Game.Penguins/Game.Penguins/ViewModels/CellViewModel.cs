using Game.Penguins.Core.Interfaces.Game.GameBoard;
using Game.Penguins.Core.Interfaces.Game.Players;
using Game.Penguins.Framework;

namespace Game.Penguins.ViewModels
{
    public class CellViewModel : ViewModel
    {
        public int X { get; private set; }

        public int Y { get; private set; }

        public string XY
        {
            get
            {
                return X + ";" + Y;
            }
        }

        public ICell Cell { get; set; }

        public bool IsWater { get; private set; }

        public bool IsIce { get; private set; }

        private bool isSelectedFirst;
        public bool IsSelectedFirst
        {
            get => isSelectedFirst;
            set
            {
                if (isSelectedFirst != value)
                {
                    isSelectedFirst = value;
                    RaisePropertyChanged(nameof(IsSelectedFirst));
                }
            }
        }

        private bool isSelectedSecond;
        public bool IsSelectedSecond
        {
            get => isSelectedSecond;
            set
            {
                if (isSelectedSecond != value)
                {
                    isSelectedSecond = value;
                    RaisePropertyChanged(nameof(IsSelectedSecond));
                }
            }
        }

        #region Fish points

        public bool IsOnePoint { get; private set; }

        public bool IsTwoPoint { get; private set; }

        public bool IsThreePoint { get; private set; }

        #endregion

        #region Colors

        public bool IsBlue { get; private set; }

        public bool IsYellow { get; private set; }

        public bool IsGreen { get; private set; }

        public bool IsRed { get; private set; }

        #endregion

        public CellViewModel(int x, int y, ICell cell)
            : base()
        {
            X = x;
            Y = y;
            this.Cell = cell;
            RefreshState();
            cell.StateChanged += Cell_StateChanged;
        }

        private void Cell_StateChanged(object sender, System.EventArgs e)
        {
            RefreshState();
        }

        private void RefreshState()
        {
            IsIce = Cell.CellType == CellType.Fish || Cell.CellType == CellType.FishWithPenguin;
            IsWater = Cell.CellType == CellType.Water;

            IsOnePoint = Cell.FishCount == 1;
            IsTwoPoint = Cell.FishCount == 2;
            IsThreePoint = Cell.FishCount == 3;

            IsBlue = Cell.CurrentPenguin != null && Cell.CurrentPenguin.Player.Color == PlayerColor.Blue;
            IsYellow = Cell.CurrentPenguin != null && Cell.CurrentPenguin.Player.Color == PlayerColor.Yellow;
            IsGreen = Cell.CurrentPenguin != null && Cell.CurrentPenguin.Player.Color == PlayerColor.Green;
            IsRed = Cell.CurrentPenguin != null && Cell.CurrentPenguin.Player.Color == PlayerColor.Red;

            RaisePropertyChanged(nameof(IsIce));
            RaisePropertyChanged(nameof(IsWater));

            RaisePropertyChanged(nameof(IsOnePoint));
            RaisePropertyChanged(nameof(IsTwoPoint));
            RaisePropertyChanged(nameof(IsThreePoint));

            RaisePropertyChanged(nameof(IsBlue));
            RaisePropertyChanged(nameof(IsYellow));
            RaisePropertyChanged(nameof(IsGreen));
            RaisePropertyChanged(nameof(IsRed));
        }
    }
}
