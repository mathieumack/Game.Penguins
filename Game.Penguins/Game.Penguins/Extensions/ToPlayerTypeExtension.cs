using Game.Penguins.Core.Interfaces.Game.Players;

namespace Game.Penguins.Extensions
{
    public static class ToPlayerTypeExtension
    {
        /// <summary>
        /// Convert a player type string (Combobox item) to the enumeration
        /// </summary>
        /// <param name="playerType"></param>
        /// <returns></returns>
        public static PlayerType ToPlayerType(this string playerType)
        {
            switch (playerType)
            {
                case "Human":
                    return PlayerType.Human;
                case "AI - Easy":
                    return PlayerType.AIEasy;
                case "AI - Medium":
                    return PlayerType.AIMedium;
                case "AI - Hard":
                    return PlayerType.AIHard;
                default:
                    return PlayerType.Human;
            }
        }
    }
}
