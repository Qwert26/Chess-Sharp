using System;
using System.Collections.Generic;
namespace Engine.Model {
	public class Board {
		private Piece[,] pieces;
		/// <summary>
		/// K: white king-side
		/// Q: white queen-side
		/// k: black king-side
		/// q: black queen-side
		/// Andere Buchstaben stehen für Position der Türme
		/// </summary>
		private string castleRights;
		public Board(int cols, int rows) {
			pieces = new Piece[cols, rows];
			Zylindrical = false;
			castleRights = "";
		}
		public bool Zylindrical { get; set; }
		public int MaximumMovedistance {
			get {
				return Math.Max(pieces.GetLength(0), pieces.GetLength(1))-1;
			}
		}
		public int Columns {
			get => pieces.GetLength(0);
		}
		public int Rows {
			get => pieces.GetLength(1);
		}
		public Piece this[int col, int row] {
			get {
				if (Zylindrical) {
					if (col < 0) {
						col = pieces.GetLength(0) + col;
					}
					col %= pieces.GetLength(0);
				}
				return pieces[col, row];
			}
			private set {
				if (Zylindrical) {
					if (col < 0) {
						col = pieces.GetLength(0) + col;
					}
					col %= pieces.GetLength(0);
				}
				pieces[col, row] = value;
			}
		}
		public Piece this[Tuple<int, int> pos] {
			get {
				return this[pos.Item1, pos.Item2];
			}
			private set {
				this[pos.Item1, pos.Item2] = value;
			}
		}
		public void SetDefaultCastleRights() {
			castleRights = "KkQq";
		}
		public void SetRookbasedCastleRights() {
			castleRights = "";
			for (int col = 0; col < pieces.GetLength(0); col++) {
				if (pieces[col, 0] is Pieces.Rook) {
					castleRights += ('A' + col);
				}
				if (pieces[col, pieces.GetLength(1) - 1] is Pieces.Rook) {
					castleRights += ('a' + col);
				}
			}
		}
		public int PawnStartRank(bool white) {
			if (white) {
				return 1;
			} else {
				return pieces.GetLength(1) - 2;
			}
		}
		public bool IsFree(int col, int row) {
			return this[col, row] == null;
		}
		public bool IsAccessible(int col, int row) {
			if (0 <= row && row < pieces.GetLength(1)) {
				if (Zylindrical) {
					return true;
				} else if (0 <= col && col < pieces.GetLength(0)) {
					return true;
				} else {
					return false;
				}
			} else {
				return false;
			}
		}
		/// <summary>
		/// Führt den gegebenen Zug durch und gibt die Figur zurück, die auf dem Zielfeld stand.
		/// </summary>
		/// <param name="toMove"></param>
		/// <param name="moveTo"></param>
		/// <returns></returns>
		public Piece ExecuteMove(Tuple<int, int> toMove, Tuple<int, int> moveTo) {
			if (IsFree(toMove.Item1, toMove.Item2)) {
				throw new Exception("There is no Piece to move!");
			} else {
				if (IsFree(moveTo.Item1, moveTo.Item2)) {
					this[moveTo] = this[toMove];
					pieces[toMove.Item1, toMove.Item2] = null;
					return null;
				} else if (this[moveTo].White != this[toMove].White) {
					Piece ret = this[moveTo];
					this[moveTo] = this[toMove];
					this[toMove] = null;
					return ret;
				} else {
					throw new Exception("Taking own piece is not allowed!");
				}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="white">Soll das weiße Team betrachtet werden?</param>
		/// <returns></returns>
		public Dictionary<Tuple<int, int>, PieceStatus> CollectPieceStatus(bool white) {
			Dictionary<Tuple<int, int>, PieceStatus> ret = new Dictionary<Tuple<int, int>, PieceStatus>();
			for (int col = 0; col < pieces.GetLength(0); col++) {
				for (int row = 0; row < pieces.GetLength(1); row++) {
					if (!IsFree(col, row) && pieces[col, row].White == white) {
						ret.Add(new Tuple<int, int>(col, row), pieces[col, row].CurrentStatus(this, col, row));
					}
				}
			}
			return ret;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Dictionary<Tuple<int, int>, PieceStatus> CollectPieceStatus() {
			Dictionary<Tuple<int, int>, PieceStatus> ret = new Dictionary<Tuple<int, int>, PieceStatus>();
			for (int col = 0; col < pieces.GetLength(0); col++) {
				for (int row = 0; row < pieces.GetLength(1); row++) {
					if (!IsFree(col, row)) {
						ret.Add(new Tuple<int, int>(col, row), pieces[col, row].CurrentStatus(this, col, row));
					}
				}
			}
			return ret;
		}
	}
}