using System;
using System.Collections.Generic;
namespace Engine.Model {
	public abstract class Piece {
		public string name;
		private string abbreviation;
		public Piece(Piece other) {
			name = other.name;
			abbreviation = other.abbreviation;
			White = other.White;
			Royal = other.Royal;
			Value = other.Value;
		}
		public Piece(string name, string abb, bool white) {
			this.name = name;
			abbreviation = abb;
			White = white;
			Royal = false;
			Value = 0;
		}
		public bool White { get; set; }
		public bool Royal { get; set; }
		/// <summary>
		/// Relativer Standard-Wert in Centi-Bauern. 100 ist der Wert für einen Bauern.
		/// </summary>
		public int Value { get; set; }
		public string Abbreviation {
			get {
				if (White) {
					return abbreviation.ToUpperInvariant();
				} else {
					return abbreviation.ToLowerInvariant();
				}
			}
			set {
				abbreviation = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="board"></param>
		/// <param name="col"></param>
		/// <param name="row"></param>
		/// <returns></returns>
		public abstract List<Tuple<int,int>> ValidMoves(Board board, in int col, in int row);
	}
}