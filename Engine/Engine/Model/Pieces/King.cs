using System;
using System.Collections.Generic;
namespace Engine.Model.Pieces {
	public class King : Piece {
		public static King CreateKing(string abb) {
			King ret = new King();
			if (abb.Equals("K")) {
				ret.White = true;
			} else if (abb.Equals("k")) {
				ret.White = false;
			} else {
				throw new Exception("Mapping Error!");
			}
			return ret;
		}
		static King() {
			StaticPieces.AddRegistration("K", CreateKing);
		}
		public King() : base("King", "K", true) {
			Royal = true;
		}
		public King(King other) : base(other) {}
		public override List<Tuple<int, int>> ValidMoves(Board board, in int col, in int row) {
			if (board[col, row] is King && board[col, row].White == White) {
				List<Tuple<int, int>> ret = new List<Tuple<int, int>>();
				for (int dc = -1; dc <= 1; dc++) {
					for (int dr = -1; dr <= 1; dr++) {
						//0,0 wäre keine Bewegung
						if ((!(dc == 0 && dr == 0)) && board.IsAccessible(col + dc, row + dr) && (board.IsFree(col + dc, row + dc) || board[col + dc, row + dr].White != White)) {
							ret.Add(new Tuple<int, int>(col + dc, row + dr));
						}
					}
				}
				return ret;
			} else {
				throw new Exception("King-Piece of color " + (White ? "white" : "black") + " expected.");
			}
		}
		public override List<Tuple<int, int>> ProtectedTeammates(Board board, in int col, in int row) {
			if (board[col, row] is King && board[col, row].White == White) {
				List<Tuple<int, int>> ret = new List<Tuple<int, int>>();
				for (int dc = -1; dc <= 1; dc++) {
					for (int dr = -1; dr <= 1; dr++) {
						//0,0 wäre keine Bewegung
						if ((!(dc == 0 && dr == 0)) && board.IsAccessible(col + dc, row + dr) && !board.IsFree(col + dc, row + dc) && board[col + dc, row + dr].White == White) {
							ret.Add(new Tuple<int, int>(col + dc, row + dr));
						}
					}
				}
				return ret;
			} else {
				throw new Exception("King-Piece of color " + (White ? "white" : "black") + " expected.");
			}
		}
	}
}
