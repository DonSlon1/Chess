using System.Text.RegularExpressions;

namespace Chess.Core.Helpers;

using Chess.Core.Board;
public class PGNUtitlity
{
    public static string CreatePGN(Board board)
    {
        throw new NotImplementedException();
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

                board.MakeMove(MoveUtility.GetMoveFromSAN(move, board));
                Console.WriteLine(board.ToString());
            }
        }

        return board;
    }

}
