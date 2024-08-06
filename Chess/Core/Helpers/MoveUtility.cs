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
    public static Move GetMoveFromSAN(string san,Board.Board board)
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
		    var piece = board.isWhiteToMove ? Piece.WhitePawn : Piece.BlackPawn;
		    var targetSquare = BoardUtility.IndexFromName(san);
		    var startingSquare = GetStartingSquare(piece, targetSquare, board.Squares);
		    Console.WriteLine(startingSquare);
            return new Move(startingSquare, targetSquare, MoveFlag.NoFlag, piece);
        }
	    else if (san.Length == 3)
	    {
		    var piece = Piece.GetPieceFromSymbol(san[0]);
		    var targetSquare = BoardUtility.IndexFromName(san.Substring(1, 2));
		    var startingSquare = GetStartingSquare(piece, targetSquare, board.Squares);
		    Console.WriteLine(startingSquare);
            return new Move(startingSquare, targetSquare, MoveFlag.NoFlag, piece);
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
        var startSquare = Piece.GetType(piece) switch
        {
            PieceType.Pawn => (Func<byte>)(() =>
            {
                byte rank = BoardUtility.GetRank(targetSquare);
                byte file = BoardUtility.GetFile(targetSquare);
                int direction = Piece.GetColor(piece) == PieceColor.White ? -1 : 1;
                int startRank = rank + direction;
                int endRank = Piece.GetColor(piece) == PieceColor.White ? 0 : 7;

                for (int i = startRank; i != endRank + direction; i += direction)
                {
                    byte square = (byte)(i * 8 + file);
                    if (squares[square] == piece)
                    {
                        return square;
                    }
                }
                throw new InvalidOperationException("No pawn found");
            }),
            PieceType.Bishop => (Func<byte>)(() =>
            {
                return FindPieceOnDiagonals(piece, targetSquare, squares);
            }),
            PieceType.Rook => (Func<byte>)(() =>
            {
                return FindPieceOnStraightLines(piece, targetSquare, squares);
            }),
            PieceType.Knight => (Func<byte>)(() =>
            {
                return FindKnight(piece, targetSquare, squares);
            }),
            PieceType.Queen => (Func<byte>)(() =>
            {
                byte diagonalSquare = FindPieceOnDiagonals(piece, targetSquare, squares);
                if (diagonalSquare != 255) return diagonalSquare;
                return FindPieceOnStraightLines(piece, targetSquare, squares);
            }),
            PieceType.King => (Func<byte>)(() =>
            {
                return FindKing(piece, targetSquare, squares);
            }),
            _ => throw new NotImplementedException()
        };
        return startSquare.Invoke();
    }

    public static int[] GetAvailableStartingSquares(byte piece, byte targetSquare, byte[] squares)
    {
        List<int> availableSquares = new List<int>();
        PieceType pieceType = Piece.GetType(piece);
        PieceColor pieceColor = Piece.GetColor(piece);

        for (byte i = 0; i < 64; i++)
        {
            if (squares[i] == piece)
            {
                if (IsValidMove(pieceType, pieceColor, i, targetSquare, squares))
                {
                    availableSquares.Add(i);
                }
            }
        }

        return availableSquares.ToArray();
    }

    private static byte FindPieceOnDiagonals(byte piece, byte targetSquare, byte[] squares)
    {
        int[] directions = { -9, -7, 7, 9 };
        return FindPieceInDirections(piece, targetSquare, squares, directions);
    }

    private static byte FindPieceOnStraightLines(byte piece, byte targetSquare, byte[] squares)
    {
        int[] directions = { -8, -1, 1, 8 };
        return FindPieceInDirections(piece, targetSquare, squares, directions);
    }

    private static byte FindPieceInDirections(byte piece, byte targetSquare, byte[] squares, int[] directions)
    {
        foreach (int direction in directions)
        {
            int currentSquare = targetSquare;
            while (true)
            {
                currentSquare += direction;
                if (currentSquare < 0 || currentSquare > 63) break;
                if (Math.Abs(BoardUtility.GetFile((byte)currentSquare) - BoardUtility.GetFile((byte)(currentSquare - direction))) > 1) break;
                if (squares[currentSquare] == piece) return (byte)currentSquare;
                if (squares[currentSquare] != 0) break;
            }
        }
        return 255; // Not found
    }

    private static byte FindKnight(byte piece, byte targetSquare, byte[] squares)
    {
        int[] knightMoves = { -17, -15, -10, -6, 6, 10, 15, 17 };
        foreach (int move in knightMoves)
        {
            int currentSquare = targetSquare + move;
            if (currentSquare >= 0 && currentSquare < 64)
            {
                if (Math.Abs(BoardUtility.GetFile((byte)currentSquare) - BoardUtility.GetFile(targetSquare)) <= 2)
                {
                    if (squares[currentSquare] == piece) return (byte)currentSquare;
                }
            }
        }
        return 255; // Not found
    }

    private static byte FindKing(byte piece, byte targetSquare, byte[] squares)
    {
        int[] kingMoves = { -9, -8, -7, -1, 1, 7, 8, 9 };
        foreach (int move in kingMoves)
        {
            int currentSquare = targetSquare + move;
            if (currentSquare >= 0 && currentSquare < 64)
            {
                if (Math.Abs(BoardUtility.GetFile((byte)currentSquare) - BoardUtility.GetFile(targetSquare)) <= 1)
                {
                    if (squares[currentSquare] == piece) return (byte)currentSquare;
                }
            }
        }
        return 255; // Not found
    }

    private static bool IsValidMove(PieceType pieceType, PieceColor pieceColor, byte startSquare, byte targetSquare, byte[] squares)
    {
        // Implement move validation logic for each piece type
        // This is a simplified version and may need to be expanded based on your specific rules
        switch (pieceType)
        {
            case PieceType.Pawn:
                int direction = pieceColor == PieceColor.White ? -1 : 1;
                int rankDiff = BoardUtility.GetRank(targetSquare) - BoardUtility.GetRank(startSquare);
                int fileDiff = Math.Abs(BoardUtility.GetFile(targetSquare) - BoardUtility.GetFile(startSquare));
                return (rankDiff == direction && fileDiff == 0) || (rankDiff == 2 * direction && fileDiff == 0 && BoardUtility.GetRank(startSquare) == (pieceColor == PieceColor.White ? 6 : 1));
            case PieceType.Bishop:
                return Math.Abs(BoardUtility.GetRank(targetSquare) - BoardUtility.GetRank(startSquare)) == Math.Abs(BoardUtility.GetFile(targetSquare) - BoardUtility.GetFile(startSquare));
            case PieceType.Rook:
                return BoardUtility.GetRank(targetSquare) == BoardUtility.GetRank(startSquare) || BoardUtility.GetFile(targetSquare) == BoardUtility.GetFile(startSquare);
            case PieceType.Knight:
                int rankDiffKnight = Math.Abs(BoardUtility.GetRank(targetSquare) - BoardUtility.GetRank(startSquare));
                int fileDiffKnight = Math.Abs(BoardUtility.GetFile(targetSquare) - BoardUtility.GetFile(startSquare));
                return (rankDiffKnight == 2 && fileDiffKnight == 1) || (rankDiffKnight == 1 && fileDiffKnight == 2);
            case PieceType.Queen:
                return IsValidMove(PieceType.Bishop, pieceColor, startSquare, targetSquare, squares) || IsValidMove(PieceType.Rook, pieceColor, startSquare, targetSquare, squares);
            case PieceType.King:
                int rankDiffKing = Math.Abs(BoardUtility.GetRank(targetSquare) - BoardUtility.GetRank(startSquare));
                int fileDiffKing = Math.Abs(BoardUtility.GetFile(targetSquare) - BoardUtility.GetFile(startSquare));
                return rankDiffKing <= 1 && fileDiffKing <= 1;
            default:
                return false;
        }
    }
}
