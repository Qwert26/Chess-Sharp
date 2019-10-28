using System;
using Engine.Model;
using Engine.Model.Pieces;
using Engine.Utils;
namespace Engine.Analysis {
	public static class StaticPoints {
		/// <summary>
		/// Startpunkte von Standard-Schach.
		/// </summary>
		public readonly static int START_POINTS_DEFAULT_CHESS = 3900;
		/// <summary>
		/// Punkte, mit dem alle Bauern auf allen Spalten gleich viel Wert sind, wenn sie auf derselben Zeile stehen.
		/// </summary>
		public readonly static int POINTS_FOR_EVEN_PAWNS = 1400;
		/// <summary>
		/// 
		/// </summary>
		public readonly static double EVEN_PAWNS_RELATIVE = 14.0 / 39.0;
		/// <summary>
		///Spielabhänginge Startpunkte in Centi-Bauern von einer Seite.
		/// </summary>
		public static int startPoints;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="board"></param>
		/// <returns></returns>
		public static Tuple<int, int, int> CurrentBoardValue(Board board) {
			int white = 0, black = 0;
			for (int col = 0; col < board.Columns; col++) {
				for (int row = 0; row < board.Rows; row++) {
					if (!board.IsFree(col, row)) {
						if (board[col, row].White) {
							white += board[col, row].Value;
						} else {
							black += board[col, row].Value;
						}
					}
				}
			}
			return new Tuple<int, int, int>(white, black, white + black);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="board">Aktuelles Spielbrett</param>
		/// <param name="col">Aktuelle Spalte</param>
		/// <param name="row">Aktuelle Zeile</param>
		/// <param name="relativePoints">Relativer Wert des Teams im Vergleich zum Start</param>
		/// <returns></returns>
		public static double PointMultiplierPawn(Board board, int col, in int row, in double relativePoints) {
			if (!board.IsFree(col, row) && board[col, row] is Pawn) {
				int startRow = board.PawnStartRank(board[col, row].White);
				int distance = Math.Abs(row - startRow);
				double relativeDistance = Math.Clamp(distance / (board.Rows / 2.0), 0, 1);
				if (col >= board.Columns/2) {
					col = board.Columns - 1 - col;
				}
				double relativeCenter = col / (board.Columns / 2.0);
				double inverseLerpedPoints;
				double outerColoumnStartRow, outerColumnDistantRow;
				double innerColoumnStartRow, innerColumnDistantRow;
				double evenColoumnsStartRow = 1.0, evenColoumnsDistantRow = 1.2;
				double lerpedColoumnStartRow, lerpedColoumnDistantRow;
				double lerpedRow, lerpedEvenRow;
				if (relativePoints >= EVEN_PAWNS_RELATIVE) {
					outerColoumnStartRow = 0.9;
					innerColoumnStartRow = 1.1;
					outerColumnDistantRow = 1.06;
					innerColumnDistantRow = 1.4;

					inverseLerpedPoints = StaticMath.InverseLerpUnclamped(EVEN_PAWNS_RELATIVE, 1, relativePoints);

					lerpedColoumnStartRow = StaticMath.LerpUnclamped(outerColoumnStartRow, innerColoumnStartRow, relativeCenter);
					lerpedColoumnDistantRow = StaticMath.LerpUnclamped(outerColumnDistantRow, innerColumnDistantRow, relativeCenter);

					lerpedRow = StaticMath.LerpUnclamped(lerpedColoumnStartRow, lerpedColoumnDistantRow, relativeDistance);
					lerpedEvenRow = StaticMath.LerpUnclamped(evenColoumnsStartRow, evenColoumnsDistantRow, relativeDistance);

					return StaticMath.LerpUnclamped(lerpedEvenRow, lerpedRow, inverseLerpedPoints);
				} else {
					outerColoumnStartRow = 1.2;
					innerColoumnStartRow = 0.9;
					outerColumnDistantRow = 1.45;
					innerColumnDistantRow = 1.05;

					inverseLerpedPoints = StaticMath.InverseLerpUnclamped(0, EVEN_PAWNS_RELATIVE, relativePoints);

					lerpedColoumnStartRow = StaticMath.LerpUnclamped(outerColoumnStartRow, innerColoumnStartRow, relativeCenter);
					lerpedColoumnDistantRow = StaticMath.LerpUnclamped(outerColumnDistantRow, innerColumnDistantRow, relativeCenter);

					lerpedRow = StaticMath.LerpUnclamped(lerpedColoumnStartRow, lerpedColoumnDistantRow, relativeDistance);
					lerpedEvenRow = StaticMath.LerpUnclamped(evenColoumnsStartRow, evenColoumnsDistantRow, relativeDistance);

					return StaticMath.LerpUnclamped(lerpedRow, lerpedEvenRow, inverseLerpedPoints);
				}
			} else {
				throw new Exception("Pawn-Piece expected!");
			}
		}
		/// <summary>
		/// Berechnet einen Punktemultiplikator für den Springer abhängig davon, wo er sich auf dem Brett befindet.
		/// </summary>
		/// <param name="board"></param>
		/// <param name="col"></param>
		/// <param name="row"></param>
        /// <param name="linear"></param>
		/// <returns></returns>
		public static double PointMultiplierKnightPositional(Board board, in int col, in int row, bool linear=true) {
			int rows = board.Rows;
			int fields;
			//Ist das Brett ein Zylinder?
			if (board.Zylindrical) {
				//Oberer oder unterer Rand?
				if (row == 0 || row == rows - 1) {
					fields = 4;
				} else if (row == 1 || row == rows - 2) {
					fields = 6;
				} else {
					fields = 8;
				}
			} else {
				int columns = board.Columns;
				//Oberer oder unterer Rand?
				if (row == 0 || row == rows - 1) {
					//linker oder rechter Rand?
					if (col == 0 || col == columns - 1) {
						fields = 2; //Ist in einer Ecke
					} else if (col == 1 || col == columns - 2) {
						fields = 3; //Ist neben einer Ecke
					} else {
						fields = 4; //Zwei Felder von einer Ecke entfernt
					}
				} else if (row == 1 || row == rows - 2) {
					//linker oder rechter Rand?
					if (col == 0 || col == columns - 1) {
						fields = 3; //Ist neben einer Ecke
					} else if (col == 1 || col == columns - 2) {
						fields = 4; //Ist diagonal von einer Ecke
					} else {
						fields = 6; //Zwei Felder vom Rand und mindestens 3 Felder von einer Ecke
					}
				} else {
					//linker oder rechter Rand?
					if (col == 0 || col == columns - 1) {
						fields = 4; //Zwei Felder von einer Ecke entfernt
					} else if (col == 1 || col == columns - 2) {
						fields = 6; //Zwei Felder vom Rand und mindestens 3 Felder von einer Ecke
					} else {
						fields = 8; //Ist im "Zentrum"
					}
				}
			}
            return linear ? StaticMath.Recode(2, 8, 0.7, 1, fields) : Math.Pow(10.0 / 7.0, StaticMath.Recode(2, 8, 0, 1, fields));
		}
		/// <summary>
		/// Berechnet einen Punktemultiplikator für den Springer abhängig davon, wie geschlossen ein Spielbrett ist.
		/// </summary>
		/// <param name="board">Aktuelles Brett</param>
		/// <param name="white">Nullbarer Wahrheitswert, der angibt ob weiß(true), schwarz(false) oder beide(null) Teams betrachtet werden sollen.</param>
		/// <returns></returns>
		public static double PointMultiplierKnightCloseness(Board board, bool? white = null) {
			bool[,] closeness = AssessCloseness(board);
			int closeCount = 0;
			if (white.HasValue) {
				for (int col = 0; col < board.Columns; col++) {
					closeCount += closeness[col, white.Value ? 1 : 0] ? 1 : 0;
				}
				return StaticMath.Recode(0, board.Columns, 1, 1.5, closeCount);
			} else {
				for (int col = 0; col < board.Columns; col++) {
					closeCount += closeness[col, 0] ? 1 : 0;
					closeCount += closeness[col, 1] ? 1 : 0;
				}
				return StaticMath.Recode(0, board.Columns * 2, 1, 1.5, closeCount);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="board">Das Brett, dessen Geschlossenheit beurteilt werden soll.</param>
		/// <returns>Eine Wahrheitsmatrix, die für jede Spalte angibt, ob diese für eine Farbe geschlossen oder offen ist.</returns>
		public static bool[,] AssessCloseness(Board board) {
			bool[,] ret = new bool[board.Columns, 2];
			for (int col = 0; col < board.Columns; col++) {
				ret[col, 0] = ret[col, 1] = false;
				for (int row = 0; row < board.Rows; row++) {
					if (!board.IsFree(col, row) && board[col, row] is Pawn) {
						ret[col, board[col, row].White ? 1 : 0] = true;
					}
				}
			}
			return ret;
		}
		/// <summary>
		/// Berechnet einen Punktemultiplikator für Läufer, Türme und Königinnen abhängig davon, wie geschlossen ein Spielbrett ist.
		/// </summary>
		/// <param name="board">Aktuelles Brett</param>
		/// <param name="white">Nullbarer Wahrheitswert, der angibt ob weiß(true), schwarz(false) oder beide(null) Teams betrachtet werden sollen.</param>
		/// <returns></returns>
		public static double PointMultiplierSlidersCloseness(Board board, bool? white = null) {
			bool[,] closeness = AssessCloseness(board);
			int closeCount = 0;
			if (white.HasValue) {
				for (int col = 0; col < board.Columns; col++) {
					closeCount += closeness[col, white.Value ? 1 : 0] ? 1 : 0;
				}
				return StaticMath.Recode(0, board.Columns, 1.1, 1, closeCount);
			} else {
				for (int col = 0; col < board.Columns; col++) {
					closeCount += closeness[col, 0] ? 1 : 0;
					closeCount += closeness[col, 1] ? 1 : 0;
				}
				return StaticMath.Recode(0, board.Columns * 2, 1.1, 1, closeCount);
			}
		}
	}
}