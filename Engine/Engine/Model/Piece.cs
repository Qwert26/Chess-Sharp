using System;
using System.Collections.Generic;

namespace Engine.Model
{
    public abstract class Piece : IEquatable<Piece>
    {
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
		public abstract PieceStatus CurrentStatus(Board board, in int col, in int row);

        public override bool Equals(object obj)
        {
            return Equals(obj as Piece);
        }

        public bool Equals(Piece other)
        {
            return other != null &&
                   White == other.White &&
                   Royal == other.Royal &&
                   Value == other.Value &&
                   Abbreviation == other.Abbreviation;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(White, Royal, Value, Abbreviation);
        }

        public static bool operator ==(Piece piece1, Piece piece2)
        {
            return EqualityComparer<Piece>.Default.Equals(piece1, piece2);
        }

        public static bool operator !=(Piece piece1, Piece piece2)
        {
            return !(piece1 == piece2);
        }
    }
}