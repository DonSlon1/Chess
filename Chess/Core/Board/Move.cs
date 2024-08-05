namespace Chess.Core.Board;

public enum MoveFlag
{
	NoFlag = 0b0000,
	EnPassantCaptureFlag = 0b0001,
	CastleFlag = 0b0010,
	CheckFlag = 0b1000,
	PawnTwoUpFlag = 0b0011,

	PromoteToQueenFlag = 0b1100,
	PromoteToKnightFlag = 0b1101,
	PromoteToRookFlag = 0b1110,
	PromoteToBishopFlag = 0b1111,
}

public class Move: IEquatable<Move>
{
    private readonly int _move;

	// Masks
	private const int StartSquareMask  = 0b00000000000000111111;
	private const int TargetSquareMask = 0b00000000111111000000;
	private const int FlagMask         = 0b00001111000000000000;
	private const int PieceTypeMask    = 0b11110000000000000000;

	public Move(int startSquare, int targetSquare, MoveFlag flag,int piece)
	{
		_move = startSquare | (targetSquare << 6) | ((int)flag << 12) | (piece << 16);
	}

	public int StartSquare => _move & StartSquareMask;
	public int TargetSquare => (_move & TargetSquareMask) >> 6;
	public MoveFlag Flag => (MoveFlag)((_move & FlagMask)>>12);
	public int Pice => (_move & PieceTypeMask) >> 16;
	public bool IsPromotion => Flag >= MoveFlag.PromoteToQueenFlag;
	public bool IsCastle => Flag == MoveFlag.CastleFlag;
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
