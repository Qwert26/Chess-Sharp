using System;
using System.Collections.Generic;
namespace Engine.Model.Pieces {
	/// <summary>
	/// Modelliert einen weißen bzw. schwarzen Turm.
	/// </summary>
	public class Rook : SlidingPiece {
		public static readonly Tuple<int, int>[] directions = { new Tuple<int, int>(1, 0), new Tuple<int, int>(0, 1), new Tuple<int, int>(-1, 0), new Tuple<int, int>(0, -1) };
		public static Rook CreateRook(string abb) {
			Rook ret = new Rook();
			if (abb.Equals("R")) {
				ret.White = true;
			} else if (abb.Equals("r")) {
				ret.White = false;
			} else {
				throw new Exception("Mapping Error!");
			}
			return ret;
		}
		static Rook() {
			StaticPieces.AddRegistration("R", CreateRook);
		}
		public Rook() : base("Rook","R",true) {
			Value = 500;
		}
		public Rook(Rook other) : base(other) {}
		public override Tuple<int, int>[] getDirections() {
			return directions;
		}
		public override PieceStatus CurrentStatus(Board board, in int col, in int row) {
			return CurrentStatus<Rook>(board, col, row);
		}
	}
}