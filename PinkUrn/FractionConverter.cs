class FractionConverter
{
  public static (int numerator, int denominator) ToFraction(double decimalValue)
  {
    int decimalPositions = GetDecimalPositions(decimalValue);
    long denominator = (long)Math.Pow(10, decimalPositions);
    long numerator = (long)(decimalValue * denominator);
    long gcd = GreatestCommonDevisor(numerator, denominator);
    return ((int)(numerator / gcd), (int)(denominator / gcd));
  }

  private static int GetDecimalPositions(double value)
  {
    string valueString = value.ToString().TrimEnd('0');
    int decimalIndex = valueString.IndexOf('.');
    return decimalIndex == -1 ? 0 : valueString.Length - decimalIndex - 1;
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