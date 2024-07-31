using static Chess.Core.Board.PieceColor;
using static Chess.Core.Board.PieceType;

namespace Chess.Core.Board;

public enum PieceType
{
    None = 0,
    Pawn = 1,
    Knight = 2,
    Bishop = 3,
    Rook = 4,
    Queen = 5,
    King = 6,
}

public enum PieceColor
{
    Black = 0,
    White = 8,
}

public static class Piece
{
    private const int ColorMask = 0b1000;
    private const int PieceMask = 0b0111;

    public static int MakePiece(PieceType pieceType, PieceColor pieceColor) => (int) pieceType | (int) pieceColor;
    public static int MakePiece(PieceType pieceType,bool isWhite) => MakePiece(pieceType, isWhite ? White : Black);
    public static PieceColor GetColor(int piece) => (PieceColor) (ColorMask & piece);
    public static PieceType GetType(int piece) => (PieceType) (PieceMask & piece);

    public static char GetPieceSymbol(int piece)
    {
        var pieceSymbol = GetType(piece) switch
        {
            King => 'K',
            Queen => 'Q',
            Knight => 'N',
            Bishop => 'B',
            Rook => 'R',
            Pawn => 'P',
            _ => ' '
        };
        return GetColor(piece) == White ? pieceSymbol : char.ToLower(pieceSymbol);
    }

    public static int GetPieceFromSymbol(char symbol)
    {
        var pieceSymbol = char.ToUpper(symbol);
        var pieceType = pieceSymbol switch
        {
            'Q' => Queen,
            'K' => King,
            'N' => Knight,
            'B' => Bishop,
            'R' => Rook,
            'P' => Pawn,
            _ => None
        };
        return MakePiece(pieceType, char.IsUpper(symbol));
    }


}
