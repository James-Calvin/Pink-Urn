# Pink Urn

The **Pink Urn** is a "luck normalization" technique. It uses specialized probability urn model that improves the consistency of random event distributions, such as those found in MMO games. It transforms a simple constant probability event model into a negative hypergeometric distribution, which helps smooth out streaks of successes and failures, creating a more consistent player experience all based on a simple probability.

## How It Works

In the Pink Urn model:

- The urn contains a mix of **red (event) balls** and **white (non-event) balls**.
- When a white ball is drawn, it is not replaced, increasing the chances of drawing a red ball on subsequent trials.
- When a red ball is drawn, the urn resets, and the process begins again.

This simulates a system where, while events are still random, players are less likely to experience long streaks of failures (or successes), providing a more consistent experience. The Pink Urn model approximates the mean success rate of a constant probability event but with a smaller variance.

## Features

- **Configurable Probability**: Specify a desired success probability, and the urn model will approximate it using a discrete negative hypergeometric distribution.
- **Reduced Variance**: The Pink Urn reduces the variability of streaks in random outcomes. Success and failure streaks are shorter on average than in a standard random model.
- **Deterministic Results**: The system operates deterministically once initialized with its parameters.
- **Resets After Event**: When a successful event occurs, the urn resets to its initial state, ensuring fairness over multiple trials.

## Usage

The Pink Urn class can be used to model events with more consistent probabilities in games or simulations.

### Example

```csharp
using PinkUrnModel;

class Program
{
    static void Main()
    {
        // Create a Pink Urn with an average 0.02 probability of success
        PinkUrn urn = new PinkUrn(0.02);

        // Simulate 10 trials
        Random random = new Random();
        for (int i = 0; i < 10; i++)
        {
            bool result = urn.GetResult(random.NextDouble());
            Console.WriteLine(result ? "Success" : "Failure");
        }
    }
}
```

### API

- **`PinkUrn(int redBalls, int whiteBalls)`**  
  Initializes the Pink Urn with a specific number of red (event) and white (non-event) balls.

  ```csharp
  PinkUrn urn = new PinkUrn(5, 100);
  ```

- **`PinkUrn(double probability, int scalar = 3)`**  
  Initializes the Pink Urn with a given probability, automatically calculating the red and white ball counts.

  ```csharp
  PinkUrn urn = new PinkUrn(0.02);
  ```

- **`bool GetResult(int index)`**  
  Returns whether or not the event occurs at the given index. The result is deterministic based on the current state of the urn.

  ```csharp
  bool success = urn.GetResult(randomIndex);
  ```

- **`bool GetResult(double time)`**  
  Returns the event result based on a time value in the unit interval [0, 1], where the urn’s total number of balls is indexed.

  ```csharp
  bool success = urn.GetResult(0.75);
  ```

- **`void Reset()`**  
  Resets the urn to its initial state.

  ```csharp
  urn.Reset();
  ```

## Simulation Results

We simulated the Pink Urn model and a simple constant probability model over 50,000 players performing 40,000 trials each using a 0.02 probability of success. Here are the results:

| Model    | Successes  | Failures      | Mean Probability | Mean Failure Streak | Failure Streak Std. Dev. | Best Failure Streak |
| -------- | ---------- | ------------- | ---------------- | ------------------- | ------------------------ | ------------------- |
| Simple   | 39,992,769 | 1,960,007,231 | 0.0199963845     | 49.00904038         | 49.44142142              | 988                 |
| Pink Urn | 39,986,123 | 1,960,013,877 | 0.0199930615     | 49.01735227         | 34.99427645              | 147                 |

The Pink Urn model has a lower standard deviation of successes, failures, and failure streaks, meaning that it creates a more consistent experience across players. Players in the Pink Urn model also experience shorter extreme failure streaks, reducing frustration caused by long runs of bad luck.

## Installation

To install the Pink Urn library via NuGet:

```bash
dotnet add package PinkUrn --version 1.0.0
```

Or search for `PinkUrn` in your NuGet package manager.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
