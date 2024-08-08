using System.Text;
using System.Text.RegularExpressions;

namespace Chess.Core.Helpers;

using Chess.Core.Board;
public class PGNUtitlity
{
    public static string CreatePGN(Board board)
    {
        StringBuilder sb = new();
        for (int i = 0; i < board.AllGameMoves.Count; i++)
        {
            if (i%2 == 0)
            {
                sb.Append($" {i/2 + 1}. ");
                sb.Append(MoveUtility.GetSANFromMove(board.AllGameMoves[i]));
                sb.Append(" ");
                if (board.AllGameMoves[i+1] != null)
                {
                    sb.Append(MoveUtility.GetSANFromMove(board.AllGameMoves[i+1]));
                }
            }
        }
        return sb.ToString();
    }
    public static Board ParsePGN(string pgn)
    {
        Board board = new();
        string[] movesLines = Regex.Split(pgn, @"\d+\.");
        foreach (var movesLine in movesLines)
        {
            var moves = movesLine.Trim().Split(" ");
            foreach (var move in moves)
            {
                if (move == "") continue;
                var newMove = MoveUtility.GetMoveFromSAN(move, board);
                if (newMove != null)
                {
                    board.MakeMove(newMove);
                    //Console.WriteLine(board.ToString());
                }
            }
        }

        return board;
    }

}
