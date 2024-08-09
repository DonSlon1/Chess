using System.Text;
using Chess.Core.Board;

namespace Chess.Core.Helpers;

public abstract class BoardUtility
{
	private const string FileNames = "abcdefgh";
	private const string RankNames = "12345678";

	public static byte IndexFromName(string name)
	{
		if (name.Length != 2) throw new ArgumentException("Invalid field name");
		name = name.ToLower();
		var fileName = name[0];
		var rankName = name[1];
		if (!FileNames.Contains(fileName)) throw new ArgumentException("Invalid file name");
		if (!RankNames.Contains(rankName)) throw new ArgumentException("Invalid rank name");

		return (byte)(RankNames.IndexOf(rankName) * 8 + FileNames.IndexOf(fileName));
	}

	public static string CreateDiagram(Board.Board board, bool blackAtTop = true, bool includeFen = true, bool includeZobristKey = true)
	{
		// Thanks to ernestoyaquello
		System.Text.StringBuilder result = new();
		//int lastMoveSquare = board.AllGameMoves.Count > 0 ? board.AllGameMoves[^1].TargetSquare : -1;

		for (int y = 0; y < 8; y++)
		{
			int rankIndex = blackAtTop ? 7 - y : y;
			result.AppendLine("+---+---+---+---+---+---+---+---+");

			for (int x = 0; x < 8; x++)
			{
				int fileIndex = blackAtTop ? x : 7 - x;
				int squareIndex = IndexFromCoord(fileIndex, rankIndex);
				//bool highlight = squareIndex == lastMoveSquare;
				byte piece = board.Squares[squareIndex];
				// if (highlight)
				// {
				// 	result.Append($"|({Piece.GetPieceSymbol(piece)})");
				// }
				// else
				// {
					result.Append($"| {Piece.GetPieceSymbol(piece)} ");
				// }


				if (x == 7)
				{
					// Show rank number
					result.AppendLine($"| {rankIndex + 1}");
				}
			}

			if (y == 7)
			{
				// Show file names
				result.AppendLine("+---+---+---+---+---+---+---+---+");
				const string fileNames = "  a   b   c   d   e   f   g   h  ";
				const string fileNamesRev = "  h   g   f   e   d   c   b   a  ";
				result.AppendLine(blackAtTop ? fileNames : fileNamesRev);
				result.AppendLine();

				if (includeFen)
				{
					result.AppendLine($"Fen         : {FenUtility.CurrentFen(board)}");
				}
				if (includeZobristKey)
				{
					//result.AppendLine($"Zobrist Key : {board.ZobristKey}");
				}
			}
		}

		return result.ToString();
	}

	public static int IndexFromCoord(int fileIndex, int rankIndex)
	{
		return fileIndex + rankIndex * 8;
	}
	public static string NameFromIndex(int? index)
	{
		if (index == null) return "-";
		return $"{FileNames[index.Value % 8]}{RankNames[index.Value / 8]}";
	}
	public static string NameFromCoord(int fileIndex, int rankIndex)
	{
		return $"{FileNames[fileIndex]}{RankNames[rankIndex]}";
	}
	public static byte GetRank(byte squareIndex)
	{
		return (byte)(squareIndex / 8);
	}
	public static byte GetFile(byte squareIndex)
	{
		return (byte)(squareIndex % 8);
	}
}
