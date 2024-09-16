namespace PinkUrnModel;

internal class FractionConverter
{
  /// <summary>
  /// Converts a double into a fraction representation.
  /// </summary>
  /// <param name="decimalValue">The value to convert to a fraction</param>
  /// <returns>two integers (numerator and denominator) where numerator/denominator ~= decimalValue</returns>
  public static (int numerator, int denominator) ToFraction(double decimalValue)
  {
    // Getting decimal positions without using string manipulation
    double partial = decimalValue - Math.Floor(decimalValue);
    int decimalPositions = 0;
    while (partial > 0)
    {
      decimalPositions++;
      partial *= 10;
      partial -= Math.Floor(partial);
    }

    long denominator = (long)Math.Pow(10, decimalPositions);
    long numerator = (long)(decimalValue * denominator);
    long gcd = GreatestCommonDevisor(numerator, denominator);
    return ((int)(numerator / gcd), (int)(denominator / gcd));
  }

  private static long GreatestCommonDevisor(long a, long b)
  {
    while (b != 0)
    {
      long temp = b;
      b = a % b;
      a = temp;
    }
    return a;
  }

}