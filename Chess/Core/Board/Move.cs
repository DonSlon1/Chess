namespace Chess.Core.Board;

public enum MoveFlag
{
	NoFlag = 0b0000,
	EnPassantCaptureFlag = 0b0001,
	CastleFlag = 0b0010,
	PawnTwoUpFlag = 0b0011,

	PromoteToQueenFlag = 0b0100,
	PromoteToKnightFlag = 0b0101,
	PromoteToRookFlag = 0b0110,
	PromoteToBishopFlag = 0b0111,
}

public class Move
{
    private readonly ushort _move;

	// Masks
	private const ushort StartSquareMask  = 0b0000000000111111;
	private const ushort TargetSquareMask = 0b0000111111000000;
	private const ushort FlagMask         = 0b1111000000000000;

	public Move(int startSquare, int targetSquare, MoveFlag flag)
	{
		_move = (ushort)(startSquare | (targetSquare << 6) | ((int)flag << 12));
	}

	public int StartSquare => _move & StartSquareMask;
	public int TargetSquare => (_move & TargetSquareMask) >> 6;
	public MoveFlag Flag => (MoveFlag)((_move & FlagMask)>>12);
	public bool IsPromotion => Flag >= MoveFlag.PromoteToQueenFlag;
	public bool IsCastle => Flag == MoveFlag.CastleFlag;

}
