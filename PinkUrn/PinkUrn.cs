namespace PinkUrn;

public class PinkUrn
{
  public readonly int InitialRedBalls;
  public readonly int InitialWhiteBalls;
  public int RedBalls { get; set; }
  public int WhiteBalls { get; set; }
  public int TotalBalls { get => RedBalls + WhiteBalls; }

  public PinkUrn(int redBalls, int whiteBalls)
  {
    ArgumentOutOfRangeException.ThrowIfLessThan(redBalls, 1);
    ArgumentOutOfRangeException.ThrowIfNegative(whiteBalls);
    InitialRedBalls = RedBalls = redBalls;
    InitialWhiteBalls = WhiteBalls = whiteBalls;
  }

  public PinkUrn(double probability, int scalar = 3)
  {
    // Checking probability is on the unit interval (0% - 100%)
    ArgumentOutOfRangeException.ThrowIfNegative(probability);
    ArgumentOutOfRangeException.ThrowIfGreaterThan(probability, 1);
    ArgumentOutOfRangeException.ThrowIfLessThan(scalar, 2);

    // Convert the probability into a number of red and white balls that 
    // follows a negative hypergeometric distribution with the same mean as a
    // negative binomial distribution with that probability.
    var (numerator, denominator) = FractionConverter.ToFraction(probability);
    int totalBalls = scalar * denominator - 1;
    InitialWhiteBalls = WhiteBalls = totalBalls - scalar * numerator + 1;
    InitialRedBalls = RedBalls = totalBalls - InitialWhiteBalls;
  }

  public bool GetResult(int index)
  {
    // Less than because the range on random.Next = [0,totalBalls), thus the 
    // return value can be thought of as the zero-index of the ball.
    if (index < RedBalls)
    {
      // We have selected a red ball, so reset and return true
      Reset();
      return true;
    }
    else
    {
      // We selected a white ball and we remove it from the urn
      WhiteBalls -= 1;
      return false;
    }
  }

  public bool GetResult(double time)
  {
    int index = (int)(time * TotalBalls);
    return GetResult(index);
  }

  public void Reset()
  {
    RedBalls = InitialRedBalls;
    WhiteBalls = InitialWhiteBalls;
  }
}
