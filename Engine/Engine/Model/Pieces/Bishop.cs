using System;
using System.Collections.Generic;
namespace Engine.Model.Pieces {
	/// <summary>
	/// Modelliert einen weißen bzw. schwarzen Läufer.
	/// </summary>
	public class Bishop : SlidingPiece {
		public static readonly Tuple<int, int>[] directions = { new Tuple<int, int>(1, 1), new Tuple<int, int>(1, -1), new Tuple<int, int>(-1, -1), new Tuple<int, int>(-1, 1) };
		public static Bishop CreateBishop(string abb) {
			Bishop ret = new Bishop();
			if (abb.Equals("B")) {
				ret.White = true;
			} else if (abb.Equals("b")) {
				ret.White = false;
			} else {
				throw new Exception("Mapping Error!");
			}
			return ret;
		}
		static Bishop() {
			StaticPieces.AddRegistration("B", CreateBishop);
		}
		public Bishop() : base("Bishop", "B", true) {
			Value = 300;
		}
		public Bishop(Bishop other) : base(other) {}
		public override List<Tuple<int, int>> ValidMoves(Board board, in int col, in int row) {
			return ValidMoves<Bishop>(board, col, row);
		}
		public override Tuple<int, int>[] getDirections() {
			return directions;
		}
	}
}