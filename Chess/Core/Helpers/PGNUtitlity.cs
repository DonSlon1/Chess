using System.Text.RegularExpressions;

namespace Chess.Core.Helpers;

using Chess.Core.Board;
public class PGNUtitlity
{
    public static string CreatePGN(Board board)
    {
        throw new NotImplementedException();
    }

    public static List<Move> ParsePGN(string pgn)
    {
        string[] movesLines = Regex.Split(pgn, @"\d+\.");
        foreach (var movesLine in movesLines)
        {
            var moves = movesLine.Trim().Split(" ");
            foreach (var move in moves)
            {
                MoveUtility.GetMoveFromSAN(move);
            }
        }

        return new List<Move>();
    }

}
