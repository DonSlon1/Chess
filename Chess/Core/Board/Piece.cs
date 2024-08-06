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

    public const byte WhiteKing = (byte)King | (byte)White;
    public const byte WhiteQueen = (byte)Queen | (byte)White;
    public const byte WhiteRook = (byte)Rook | (byte)White;
    public const byte WhiteBishop = (byte)Bishop | (byte)White;
    public const byte WhiteKnight = (byte)Knight | (byte)White;
    public const byte WhitePawn = (byte)Pawn | (byte)White;
    public const byte BlackKing = (byte)King | (byte)Black;
    public const byte BlackQueen = (byte)Queen | (byte)Black;
    public const byte BlackRook = (byte)Rook | (byte)Black;
    public const byte BlackBishop = (byte)Bishop | (byte)Black;
    public const byte BlackKnight = (byte)Knight | (byte)Black;
    public const byte BlackPawn = (byte)Pawn | (byte)Black;

    private const byte ColorMask = 0b1000;
    private const byte PieceMask = 0b0111;

    public static byte MakePiece(PieceType pieceType, PieceColor pieceColor) => (byte) ((byte) pieceType | (byte) pieceColor);
    public static byte MakePiece(PieceType pieceType,bool isWhite) => MakePiece(pieceType, isWhite ? White : Black);
    public static PieceColor GetColor(int piece) => (PieceColor) (ColorMask & piece);
    public static PieceType GetType(int piece) => (PieceType) (PieceMask & piece);

    public static char GetPieceSymbol(byte piece)
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

    public static byte GetPieceFromSymbol(char symbol)
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
