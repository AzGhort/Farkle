using System.Collections.Generic;

namespace Farkle
{
    /// <summary>
    ///     Represents types of player's actions.
    /// </summary>
    internal enum PlayerAction
    {
        Keep,
        Score,
        Invalid
    }

    /// <summary>
    ///     Represents full action of the player.
    /// </summary>
    internal class PlayerResponse
    {
        public List<Dice> Dices;
        public PlayerAction Order;
    }

    /// <summary>
    ///     Class containing constants used in communication with players.
    /// </summary>
    internal class CommunicationConstants
    {
        // messages from players
        public const string KeepOrder = "KEEP";

        public const string ScoreOrder = "SCORE";

        // messages from the game
        public const string EndGameInfo = "GAME OVER";
        public const string StartGameInfo = "START";
        public const string OtherPlayerTurnInfo = "OTHER PLAYER SCORE";
        public const string StartTurnInfo = "PLAY";
        public const string EndTurnInfo = "TURN OVER";
        public const string SuccessInfo = "SUCCESS";
        public const string FailureInfo = "FAILURE";
        public const string BadOrderInfo = "INVALID ACTION";
        public const string CurrentTurnScoreInfo = "CURRENT TURN SCORE";
    }
}