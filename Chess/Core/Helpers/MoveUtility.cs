using System.Runtime.CompilerServices;
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
	    if (san.Equals("0-1"))
	    {
			return new Move(0,0,MoveFlag.NoFlag,Piece.BlackBishop);
	    }
	    if (san.Equals("O-O") || san.Equals("O-O-O"))
		    throw new NotImplementedException();

	    if (san.Length == 2)
	    {
		    Console.WriteLine("Pawn move");
	    }
	    else if (san.Length == 3)
	    {
		    var piece = Piece.GetPieceFromSymbol(san[0]);
		    var targetSquare = BoardUtility.IndexFromName(san.Substring(1, 2));
	    }
	    else if (san.Length == 4)
	    {
		    Console.WriteLine("Piece move with conflicts");
	    }

			return new Move(0,0,MoveFlag.NoFlag,Piece.BlackBishop);
	    //Console.WriteLine(san);
	    throw new NotImplementedException();
    }

	public static byte GetStartingSquare(byte piece, byte targetSquare, byte[] squares)
	{
		return 0;
	}
}
