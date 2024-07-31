using System.Text;
using Chess.Core.Board;

namespace Chess.Core.Helpers;

public class BoardUtility
{
	public const string fileNames = "abcdefgh";
	public const string rankNames = "12345678";

	public static int IndexFromName(string name)
	{
		if (name.Length != 2) throw new ArgumentException("Invalid field name");
		name = name.ToLower();
		var fileName = name[0];
		var rankName = name[1];
		if (!fileNames.Contains(fileName)) throw new ArgumentException("Invalid file name");
		if (!rankNames.Contains(rankName)) throw new ArgumentException("Invalid rank name");

		return fileNames.IndexOf(fileName) * 8 + rankNames.IndexOf(rankName);
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
				int piece = board.Squares[squareIndex];
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
		return $"{fileNames[index.Value / 8]}{rankNames[index.Value % 8]}";
	}
	public static string NameFromCoord(int fileIndex, int rankIndex)
	{
		return $"{fileNames[fileIndex]}{rankNames[rankIndex]}";
	}
}
