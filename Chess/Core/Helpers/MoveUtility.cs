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
    public static Move? GetMoveFromSAN(string san, Board.Board board)
    {
        san = san.Replace("+", "").Replace("#", ""); // Remove check and checkmate symbols

        if (san == "O-O")
        {
            // Kingside castling
            return board.isWhiteToMove
                ? new Move(4, 6, MoveFlag.CastleFlag, Piece.WhiteKing)
                : new Move(60, 62, MoveFlag.CastleFlag, Piece.BlackKing);
        }
        if (san == "O-O-O")
        {
            // Queenside castling
            return board.isWhiteToMove
                ? new Move(4, 2, MoveFlag.CastleFlag, Piece.WhiteKing)
                : new Move(60, 58, MoveFlag.CastleFlag, Piece.BlackKing);
        }

        if (san == "0-1")
        {
            Console.WriteLine("Black wins");
            return null;
        }
        if (san == "1-0")
        {
            Console.WriteLine("White wins");
            return null;
        }

        bool isCapture = san.Contains('x');
        san = san.Replace("x", "");

        byte targetSquare = BoardUtility.IndexFromName(san.Substring(san.Length - 2));
        byte piece = board.isWhiteToMove ? Piece.WhitePawn : Piece.BlackPawn;
        char pieceChar = ' ';

        if (char.IsUpper(san[0]))
        {
            pieceChar = san[0];
            piece = Piece.GetPieceFromSymbol(pieceChar);
            piece = Piece.MakePiece(Piece.GetType(piece), board.isWhiteToMove);
            san = san.Substring(1);
        }

        IEnumerable<byte> possibleStarts;
        if (san.Length == 3 && char.IsLetter(san[0])) // Disambiguating move (e.g., Nbd7)
        {
            char disambiguator = san[0];
            possibleStarts = GetPossibleStartingSquares(piece, targetSquare, board.Squares)
                .Where(s => BoardUtility.GetFile(s) == disambiguator - 'a');
        }
        else if (san.Length == 3 && char.IsDigit(san[0])) // Disambiguating move (e.g., R1a3)
        {
            char disambiguator = san[0];
            possibleStarts = GetPossibleStartingSquares(piece, targetSquare, board.Squares)
                .Where(s => BoardUtility.GetRank(s) == disambiguator - '1');
        }
        else
        {
            possibleStarts = GetPossibleStartingSquares(piece, targetSquare, board.Squares);
        }

        byte startingSquare;
        try
        {
            startingSquare = possibleStarts.First();
        }
        catch (InvalidOperationException)
        {
            throw new InvalidOperationException($"No valid starting square found for move: {san}");
        }

        MoveFlag flag = MoveFlag.NoFlag;
        if (isCapture) flag = MoveFlag.CaptureFlag;
        if (san.Contains('='))
        {
            flag = MoveFlag.NoFlag;
            piece = Piece.GetPieceFromSymbol(san[san.Length - 1]);
        }

        return new Move(startingSquare, targetSquare, flag, piece);
    }

    private static IEnumerable<byte> GetPossibleStartingSquares(byte piece, byte targetSquare, byte[] squares)
    {
        for (byte i = 0; i < 64; i++)
        {
            if (squares[i] == piece)
            {

            }
            if (squares[i] == piece && IsValidMove(Piece.GetType(piece), Piece.GetColor(piece), i, targetSquare, squares))
            {
                yield return i;
            }
        }
    }
    public static byte GetStartingSquare(byte piece, byte targetSquare, byte[] squares)
    {
        var startSquare = Piece.GetType(piece) switch
        {
            PieceType.Pawn => (Func<byte>)(() => FindPawnStartSquare(piece, targetSquare, squares)),
            PieceType.Bishop => (Func<byte>)(() => FindPieceOnDiagonals(piece, targetSquare, squares)),
            PieceType.Rook => (Func<byte>)(() => FindPieceOnStraightLines(piece, targetSquare, squares)),
            PieceType.Knight => (Func<byte>)(() => FindKnight(piece, targetSquare, squares)),
            PieceType.Queen => (Func<byte>)(() =>
            {
                byte diagonalSquare = FindPieceOnDiagonals(piece, targetSquare, squares);
                if (diagonalSquare != 255) return diagonalSquare;
                return FindPieceOnStraightLines(piece, targetSquare, squares);
            }),
            PieceType.King => (Func<byte>)(() => FindKing(piece, targetSquare, squares)),
            _ => throw new NotImplementedException()
        };
        return startSquare.Invoke();
    }

    private static byte FindPawnStartSquare(byte piece, byte targetSquare, byte[] squares)
    {
        byte rank = BoardUtility.GetRank(targetSquare);
        byte file = BoardUtility.GetFile(targetSquare);
        PieceColor color = Piece.GetColor(piece);
        int direction = color == PieceColor.White ? 1 : -1;

        // Check one square behind
        byte oneSquareBehind = (byte)((rank - direction) * 8 + file);
        if (IsValidSquare(oneSquareBehind) && squares[oneSquareBehind] == piece)
        {
            return oneSquareBehind;
        }

        // Check two squares behind (for initial double move)
        byte twoSquaresBehind = (byte)((rank - 2 * direction) * 8 + file);
        if (IsValidSquare(twoSquaresBehind) && squares[twoSquaresBehind] == piece)
        {
            // Ensure it's a valid double move (pawn on its starting rank)
            if ((color == PieceColor.White && rank == 3) || (color == PieceColor.Black && rank == 4))
            {
                return twoSquaresBehind;
            }
        }

        // Check diagonal captures
        for (int fileOffset = -1; fileOffset <= 1; fileOffset += 2)
        {
            byte captureSquare = (byte)((rank - direction) * 8 + (file + fileOffset));
            if (IsValidSquare(captureSquare) && squares[captureSquare] == piece)
            {
                return captureSquare;
            }
        }

        throw new InvalidOperationException("No valid pawn move found");
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
                if (!IsValidSquare((byte)currentSquare)) break;
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
            if (IsValidSquare((byte)currentSquare))
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
            if (IsValidSquare((byte)currentSquare))
            {
                if (Math.Abs(BoardUtility.GetFile((byte)currentSquare) - BoardUtility.GetFile(targetSquare)) <= 1)
                {
                    if (squares[currentSquare] == piece) return (byte)currentSquare;
                }
            }
        }
        return 255; // Not found
    }


    private static bool IsValidSquare(byte square)
    {
        return square >= 0 && square < 64;
    }

    private static bool IsValidMove(PieceType pieceType, PieceColor pieceColor, byte startSquare, byte targetSquare, byte[] squares)
    {
        if (startSquare == targetSquare) return false;

        int rankDiff = Math.Abs(BoardUtility.GetRank(targetSquare) - BoardUtility.GetRank(startSquare));
        int fileDiff = Math.Abs(BoardUtility.GetFile(targetSquare) - BoardUtility.GetFile(startSquare));

        switch (pieceType)
        {
            case PieceType.Pawn:
                int direction = pieceColor == PieceColor.White ? 1 : -1;
                if (fileDiff == 0) // Moving forward
                {
                    if (rankDiff == 1 && squares[targetSquare] == (int)PieceType.None)
                        return true;
                    if (rankDiff == 2 && BoardUtility.GetRank(startSquare) == (pieceColor == PieceColor.White ? 1 : 6)
                        && squares[targetSquare] == (int)PieceType.None
                        && squares[startSquare + 8 * direction] == (int)PieceType.None)
                        return true;
                }
                else if (fileDiff == 1 && rankDiff == 1) // Capture
                {
                    return squares[targetSquare] != (int)PieceType.None && Piece.GetColor(squares[targetSquare]) != pieceColor;
                }
                return false;

            case PieceType.Knight:
                return (rankDiff == 2 && fileDiff == 1) || (rankDiff == 1 && fileDiff == 2);

            case PieceType.Bishop:
                return rankDiff == fileDiff && IsClearDiagonal(startSquare, targetSquare, squares);

            case PieceType.Rook:
                return (rankDiff == 0 || fileDiff == 0) && IsClearStraightLine(startSquare, targetSquare, squares);

            case PieceType.Queen:
                return (rankDiff == fileDiff && IsClearDiagonal(startSquare, targetSquare, squares)) ||
                       ((rankDiff == 0 || fileDiff == 0) && IsClearStraightLine(startSquare, targetSquare, squares));

            case PieceType.King:
                return rankDiff <= 1 && fileDiff <= 1;

            default:
                return false;
        }
    }

    private static bool IsClearDiagonal(byte startSquare, byte targetSquare, byte[] squares)
    {
        int rankStep = BoardUtility.GetRank(targetSquare) > BoardUtility.GetRank(startSquare) ? 1 : -1;
        int fileStep = BoardUtility.GetFile(targetSquare) > BoardUtility.GetFile(startSquare) ? 1 : -1;
        int current = startSquare + 8 * rankStep + fileStep;
        int end = targetSquare;

        while (current != end)
        {
            if (squares[current] != (int)PieceType.None)
                return false;
            current += 8 * rankStep + fileStep;
        }

        return true;
    }

    private static bool IsClearStraightLine(byte startSquare, byte targetSquare, byte[] squares)
    {
        int step = startSquare < targetSquare ?
            (BoardUtility.GetRank(startSquare) == BoardUtility.GetRank(targetSquare) ? 1 : 8) :
            (BoardUtility.GetRank(startSquare) == BoardUtility.GetRank(targetSquare) ? -1 : -8);

        int current = startSquare + step;
        int end = targetSquare;

        while (current != end)
        {
            if (squares[current] != (int)PieceType.None)
                return false;
            current += step;
        }

        return true;
    }
}
