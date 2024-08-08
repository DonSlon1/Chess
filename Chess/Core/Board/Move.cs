namespace Chess.Core.Board;

public enum MoveFlag
{
	NoFlag = 0b0000,
	EnPassantCaptureFlag = 0b0001,
	CastleFlag = 0b0010,
	PawnTwoUpFlag = 0b0011,
	CaptureFlag = 0b0110,

	PromoteToQueenFlag = 0b1100,
	PromoteToKnightFlag = 0b1101,
	PromoteToRookFlag = 0b1110,
	PromoteToBishopFlag = 0b1111,
}

/// <summary>
/// Encodes move information into a single 32-bit integer.
/// </summary>
/// <param name="startSquare">The starting square of the move (0-63).</param>
/// <param name="targetSquare">The target square of the move (0-63).</param>
/// <param name="flag">The flag indicating special move types (e.g., capture, promotion).</param>
/// <param name="piece">The piece being moved.</param>
/// <param name="isPossibleMoreMoves">Indicates if there are more possible starting squares for this move.</param>
/// <param name="isCheck">Indicates if this move results in a check.</param>
/// <param name="isCheckMate">Indicates if this move results in a checkmate.</param>
public class Move(
	int startSquare,
	int targetSquare,
	MoveFlag flag,
	int piece,
	bool isPossibleMoreMoves = false,
	bool isCheck = false,
	bool isCheckMate = false)
	: IEquatable<Move>
{
    private readonly int _move = startSquare | (targetSquare << 6) | ((int)flag << 12) | (piece << 16) | ((isPossibleMoreMoves?1:0) << 20) |((isCheck?1:0) << 21) | ((isCheckMate?1:0) << 22);

	// Masks
	private const int StartSquareMask                 = 0b00000000000000000111111;
	private const int TargetSquareMask                = 0b00000000000111111000000;
	private const int FlagMask                        = 0b00000001111000000000000;
	private const int PieceTypeMask                   = 0b00011110000000000000000;
	private const int PossibleMoreStartingSquaresMask = 0b00100000000000000000000;
	private const int CheckFlagMask                   = 0b01000000000000000000000;
	private const int CheckMateFlagMask               = 0b10000000000000000000000;

	public byte StartSquare => (byte)(_move & StartSquareMask);
	public byte TargetSquare => (byte)((_move & TargetSquareMask) >> 6);
	public MoveFlag Flag => (MoveFlag)((_move & FlagMask)>>12);
	public byte Piece => (byte)((_move & PieceTypeMask) >> 16);
	public bool IsPromotion => Flag >= MoveFlag.PromoteToQueenFlag;
	public bool IsCastle => Flag == MoveFlag.CastleFlag;
	public bool PossibleMoreStartingSquares => (_move & PossibleMoreStartingSquaresMask) != 0;
	public bool IsCheck => (_move & CheckFlagMask) != 0;
	public bool IsCheckMate => (_move & CheckMateFlagMask) != 0;
	public PieceType PromotionPieceType
	{
		get
		{
			if (!IsPromotion) throw new InvalidOperationException("Move is not a promotion move");
			return Flag switch
			{
				MoveFlag.PromoteToQueenFlag => PieceType.Queen,
				MoveFlag.PromoteToKnightFlag => PieceType.Knight,
				MoveFlag.PromoteToRookFlag => PieceType.Rook,
				MoveFlag.PromoteToBishopFlag => PieceType.Bishop,
				_ => throw new InvalidOperationException("Invalid promotion flag")
			};
		}
	}

	public virtual bool Equals(Move? move) => _move == move?._move;
	public override bool Equals(object? obj) => Equals(obj as Move);

	public override int GetHashCode() => _move.GetHashCode();

	public static bool operator ==(Move? a, Move? b)
	{
		if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;
		return a.Equals(b);
	}

	public static bool operator !=(Move? a, Move? b) => !(a == b);

}
