using Chess.Core.Board;

namespace Chess.Core.Helpers;

public class MoveUtility
{
    public static string GetSANFromMove(Move move)
    {
	    var s = $"Move from {move.StartSquare} to {move.TargetSquare}";
        throw new NotImplementedException();
    }

	/// <summary>
	/// Get move from the given name in SAN notation (e.g. "Nxf3", "Rad1", "O-O", etc.)
	/// The given board must contain the position from before the move was made
	/// </summary>
    public static Move GetMoveFromSAN(string san)
    {
	    san = san.Replace("+", ""); // Remove check symbol
	    san = san.Replace("#", ""); // Remove checkmate symbol
	    san = san.Replace("x", ""); // Remove capture symbol
	    Console.WriteLine(san);
	    return new Move(0,0,MoveFlag.NoFlag,0);
	    throw new NotImplementedException();
    }
}
