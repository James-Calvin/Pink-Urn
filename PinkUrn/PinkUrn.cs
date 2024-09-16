namespace PinkUrn;

/// <summary>
/// Represents an urn model with red (event) and white (non-event) balls which simulates a random event with a given probability, but with more statistically consistent experience.
/// When a white (non-event) ball is observed, it is not replaced. When a red (event) ball is observed, the urn resets.
/// </summary>
public class PinkUrn
{
  /// <summary>
  /// Gets the initial number of red (event) balls in the urn.
  /// </summary>
  public readonly int InitialRedBalls;

  /// <summary>
  /// Gets the initial number of white (non-event) balls in the urn.
  /// </summary>
  public readonly int InitialWhiteBalls;

  /// <summary>
  /// Gets or sets the current number of red (event) balls in the urn.
  /// </summary>
  public int RedBalls { get; set; }

  /// <summary>
  /// Gets or sets the current number of white (non-event) balls in the urn.
  /// </summary>
  public int WhiteBalls { get; set; }

  /// <summary>
  /// Gets the total number of balls currently in the urn.
  /// </summary>
  public int TotalBalls { get => RedBalls + WhiteBalls; }

  /// <summary>
  /// Initializes a new instances of the <see cref="PinkUrn"/> class with the specified number of red (event) and white (non-event) balls. 
  /// </summary>
  /// <param name="redBalls">The initial number of red (event) balls in the urn.</param>
  /// <param name="whiteBalls">The initial number of white (non-event) balls in the urn.</param>
  /// <exception cref="ArgumentOutOfRangeException">Thrown if redBalls is less than 1 or whiteBalls is negative.</exception>
  public PinkUrn(int initialRedBalls, int initialWhiteBalls)
  {
    // Ensure there is at least one event ball.
    ArgumentOutOfRangeException.ThrowIfLessThan(initialRedBalls, 1);
    // Ensure there is a non-negative number of white balls.
    ArgumentOutOfRangeException.ThrowIfNegative(initialWhiteBalls);

    // Initialize the urn with the given parameters
    InitialRedBalls = RedBalls = initialRedBalls;
    InitialWhiteBalls = WhiteBalls = initialWhiteBalls;
  }

  /// <summary>
  /// Initializes a new instances of the <see cref="PinkUrn"/> such that the mean non-event streak is equal to the mean non-event streak of a constant probability event.
  /// </summary>
  /// <param name="probability">The probability </param>
  /// <param name="scalar"></param>
  public PinkUrn(double probability, int scalar = 3)
  {
    // Checking probability is on the unit interval (0% - 100%)
    ArgumentOutOfRangeException.ThrowIfNegative(probability);
    ArgumentOutOfRangeException.ThrowIfGreaterThan(probability, 1);
    // There is a degenerate case. When the numerator of the fraction 
    // representing the probability is 1 and the scalar multiplier is 1 then
    // an invalid urn state is created where the number of total balls is 
    // equal to the number of white balls. To avoid this degenerate case, we
    // simply enforce the scalar to be at least 2.
    ArgumentOutOfRangeException.ThrowIfLessThan(scalar, 2);

    // Convert the probability into a number of red and white balls that 
    // follows a negative hypergeometric distribution with the same mean as a
    // negative binomial distribution with that probability. This is done by
    // taking a probability and converting it to an integer numerator and
    // denominator, and then putting the parameters of the urn in the terms of
    // the integer representations
    var (numerator, denominator) = FractionConverter.ToFraction(probability);

    // This formula is derived by substituting the total number of balls in
    // such a way to remove the denominator when equating the two means
    int totalBalls = scalar * denominator - 1;

    // This is the number of non-event (confusingly called successes for 
    // the negative geometric distribution) after equating the mean to the 
    // mean of the constant probability event.
    InitialWhiteBalls = WhiteBalls = scalar * (denominator - numerator);

    // The number of event balls is the remaining total balls after 
    // subtracting the calculated number of white balls
    InitialRedBalls = RedBalls = totalBalls - InitialWhiteBalls;
  }

  /// <summary>
  /// Returns whether or not the ball at index is a red (event) ball.
  /// The indices are sorted such that red balls appear first.
  /// </summary>
  /// <param name="index">The index of the ball to observe.</param>
  /// <returns><c>true</c> is a red ball is selected; otherwise <c>false</c></returns>
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

  /// <summary>
  /// Allows use of a unit interval to index the urn. 
  /// The indices are sorted such that red (event) balls appear first.
  /// </summary>
  /// <param name="time">A double representing time as a fraction of the total balls.</param>
  /// <returns><c>true</c> if a red ball is selected; otherwise, <c>false</c></returns>
  public bool GetResult(double time)
  {
    // "time" is used as the variable name as respect to linear interpolation
    // of which this is reminiscent 
    int index = (int)(time * TotalBalls);
    return GetResult(index);
  }

  /// <summary>
  /// Resets the urn to its initial configuration
  /// </summary>
  public void Reset()
  {
    RedBalls = InitialRedBalls;
    WhiteBalls = InitialWhiteBalls;
  }
}
