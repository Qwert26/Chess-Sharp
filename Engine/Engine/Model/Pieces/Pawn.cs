using System;
using System.Collections.Generic;
namespace Engine.Model.Pieces {
	public class Pawn : Piece {
		private static Pawn CreatePawn(string abb) {
			Pawn ret = new Pawn();
			if (abb.Equals("P"))
			{
				ret.White = true;
			}
			else if (abb.Equals("p"))
			{
				ret.White = false;
			}
			else {
				throw new Exception("Mapping Error!");
			}
			return ret;
		}
		static Pawn() {
			StaticPieces.AddRegistration("P", CreatePawn);
		}
		public Pawn() : base("Pawn","P",true) {}
		public override List<Tuple<int,int>> ValidMoves(Board board, in int col, in int row) {
			if (board[col,row]is Pawn) {
				int startRow = board.PawnStartRank(White);
				List<Tuple<int, int>> ret = new List<Tuple<int, int>>();
				if (White) {
					if (startRow == row && board.IsFree(col,row+2)) {//Start mit Doppelzug
						ret.Add(new Tuple<int, int>(col, row + 2));
					}
					if (board.IsFree(col, row + 1)) {//Standard
						ret.Add(new Tuple<int, int>(col, row + 1));
					}
					if (board.IsAccessible(col + 1, row + 1) && !board.IsFree(col + 1, row + 1) && !board[col + 1, row + 1].White) {//Angriff rechts
						ret.Add(new Tuple<int, int>(col + 1, row + 1));
					}
					if (board.IsAccessible(col - 1, row + 1) && !board.IsFree(col - 1, row + 1) && !board[col - 1, row + 1].White) {//Angriff links
						ret.Add(new Tuple<int, int>(col - 1, row + 1));
					}
				} else {

				}
				return ret;
			} else {
				throw new Exception("Myself expected.");
			}
		}
	}
}
