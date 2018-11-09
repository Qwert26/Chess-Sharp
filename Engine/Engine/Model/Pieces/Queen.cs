using System;
using System.Collections.Generic;
namespace Engine.Model.Pieces {
	/// <summary>
	/// Modellierte eine weiße bzw. schwarze Königin
	/// </summary>
	public class Queen : SlidingPiece {
		public static readonly Tuple<int, int>[] directions = {
			new Tuple<int, int>(1,0),
			new Tuple<int, int>(1,1),
			new Tuple<int, int>(0,1),
			new Tuple<int, int>(-1,1),
			new Tuple<int, int>(-1,0),
			new Tuple<int, int>(-1,-1),
			new Tuple<int, int>(0,-1),
			new Tuple<int, int>(1,-1)
		};
		public static Queen CreateQueen(string abb) {
			Queen ret = new Queen();
			if (abb.Equals("Q")) {
				ret.White = true;
			} else if (abb.Equals("q")) {
				ret.White = false;
			} else {
				throw new Exception("Mapping Error!");
			}
			return ret;
		}
		static Queen() {
			StaticPieces.AddRegistration("Q", CreateQueen);
		}
		public Queen() : base("Queen", "Q", true) {
			Value = 900;
		}
		public Queen(Piece other) : base(other) {}
		public override Tuple<int, int>[] getDirections() {
			return directions;
		}
		public override List<Tuple<int, int>> ValidMoves(Board board, in int col, in int row) {
			return ValidMoves<Queen>(board, col, row);
		}
	}
}