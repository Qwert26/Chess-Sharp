using System;
namespace Engine.Utils {
	public static class StaticMath {
		public static double LerpUnclamped(double a, double b, double t) {
			return t * a + (1 - t) * b;
		}
		public static double InverseLerpUnclamped(double a, double b, double result) {
			return (result - a) / (b - a);
		}
		public static double Recode(double aIn, double bIn, double aOut, double bOut, double valueIn, out double t) {
			return LerpUnclamped(aOut, bOut, t = InverseLerpUnclamped(aIn, bIn, valueIn));
		}
	}
}