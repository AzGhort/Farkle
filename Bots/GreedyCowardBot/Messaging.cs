using System;
using System.Collections.Generic;
using Farkle;

namespace GreedyCowardAI
{
    internal enum MessageType
    {
        Comment,
        StartGame,
        StartTurn,
        EndGame,
        EndTurn,
        SuccessfulAction,
        FailedAction,
        MyScore,
        OpponentScore,
        DicesRolled
    }

    internal class Message
    {
        // can be either score or dice indices
        public List<int> MessageParams = new List<int>();
        public MessageType Type;

        public Message(string messageLiteral)
        {
            if (string.IsNullOrEmpty(messageLiteral))
            {
                Type = MessageType.Comment;
            }
            else
            {
                var tokens = messageLiteral.Split(" ");
                switch (tokens[0])
                {
                    case CommunicationConstants.StartGameInfo:
                        Type = MessageType.StartGame;
                        break;
                    case CommunicationConstants.EndGameInfo:
                        Type = MessageType.EndGame;
                        break;
                    case CommunicationConstants.StartTurnInfo:
                        Type = MessageType.StartTurn;
                        break;
                    case CommunicationConstants.EndTurnInfo:
                        Type = MessageType.EndTurn;
                        break;
                    case CommunicationConstants.SuccessInfo:
                        Type = MessageType.SuccessfulAction;
                        break;
                    case CommunicationConstants.FailureInfo:
                        Type = MessageType.FailedAction;
                        break;
                    case CommunicationConstants.CurrentTurnScoreInfo:
                        Type = MessageType.MyScore;
                        MessageParams.Add(int.Parse(tokens[1]));
                        break;
                    case CommunicationConstants.OtherPlayerTurnInfo:
                        Type = MessageType.OpponentScore;
                        MessageParams.Add(int.Parse(tokens[1]));
                        break;
                    case CommunicationConstants.CommentStart:
                        Type = MessageType.Comment;
                        break;
                    case CommunicationConstants.DicesInfo:
                        Type = MessageType.DicesRolled;
                        for (var i = 1; i < tokens.Length; i++)
                            if (int.TryParse(tokens[i], out var value))
                                MessageParams.Add(value);
                        break;
                    default:
                        Type = MessageType.Comment;
                        break;
                }
            }
        }

        public static void SendActionOrder(PlayerAction action, List<int> messageParams)
        {
            switch (action)
            {
                case PlayerAction.Keep:
                    Console.Write(CommunicationConstants.KeepOrder);
                    foreach (var t in messageParams)
                    {
                        Console.Write(" ");
                        Console.Write(t);
                    }

                    Console.WriteLine();
                    break;
                case PlayerAction.Score:
                    Console.WriteLine(CommunicationConstants.ScoreOrder);
                    break;
            }
        }
    }
}