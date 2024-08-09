using System.Text;
using System.Text.RegularExpressions;

namespace Chess.Core.Helpers;

using Chess.Core.Board;
public static class PgnUtitlity
{
    public static string CreatePgn(Board originalBoard)
    {
        Move[] moves = originalBoard.AllGameMoves.ToArray();
        StringBuilder sb = new();
        Board board = new();
        for (int i = 0; i < moves.Length; i++)
        {
            if (i%2 == 0)
            {
                sb.Append($"{i/2 + 1}. ");
            }
            sb.Append(MoveUtility.GetSANFromMove(moves[i],board));
            sb.Append(" ");
            board.MakeMove(moves[i]);
        }

        if (originalBoard.GameOver)
        {
            sb.Append(originalBoard.WhiteWon ? "1-0" : "0-1");
        }
        return sb.ToString();
    }
    public static Board ParsePgn(string pgn)
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
                if (newMove is not null)
                {
                    board.MakeMove(newMove);
                }
                else
                {
                    if (move == "0-1")
                    {
                        board.GameOver = true;
                        board.WhiteWon = false;
                    }
                    else if (move == "1-0")
                    {
                        board.GameOver = true;
                        board.WhiteWon = true;
                    }
                }
            }
        }

        return board;
    }

}
