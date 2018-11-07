using System.Collections.Generic;
using System;
namespace Engine.Model {
	public static class StaticPieces {
		private static Dictionary<string,Func<string,Piece>> registration;
		static StaticPieces() {
			registration = new Dictionary<string, Func<string,Piece>>();
		}
		public static void AddRegistration(string abbreviation, Func<string,Piece> generator) {
			registration.Add(abbreviation.ToLowerInvariant(), generator);
			registration.Add(abbreviation.ToUpperInvariant(), generator);
		}
		public static void RemoveRegistration(string abbreviation) {
			registration.Remove(abbreviation.ToLowerInvariant());
			registration.Remove(abbreviation.ToUpperInvariant());
		}
		public static Piece CreatePiece(string abbreviation) {
			return registration[abbreviation](abbreviation);
		}
	}
}