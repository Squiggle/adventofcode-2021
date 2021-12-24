namespace adventofcode2021.Day03
{
  public class SystemsDiagnostics
  {
    public static void Run()
    {
      // process as a collection of bit arrays
      var input = File.ReadAllLines("Day03/test.txt");

      var numericalReadings = input
        .Select(value => Convert.ToInt32(value, 2))
        .ToArray();
      var valueLength = input[0].Length;

      PowerConsumption(numericalReadings, valueLength);
      LifeSupport(numericalReadings, valueLength);
    }


    static void PowerConsumption(int[] readings, int lengthOfRecord)
    {
      // array of most prevalent bits in each column
      var mostPrevalent = lengthOfRecord
        .Enumerate()
        .Select(index => MostPrevalentInCol(readings, index, lengthOfRecord));
      // lest prevalent is an inversion of the most prevalent
      var leastPrevalent = mostPrevalent.Select(x => 1 ^ x);

      // janky-ass conversion 🥳
      var gamma = Convert.ToInt16(EnumerableToString(mostPrevalent), 2);
      var epsilon = Convert.ToInt16(EnumerableToString(leastPrevalent), 2);
      Console.WriteLine("Part 1 - Power Consumption");
      Console.WriteLine($"Gamma: {gamma}");
      Console.WriteLine($"Epsilon: {epsilon}");
      Console.WriteLine($"Result: {gamma * epsilon}");
    }

    static void LifeSupport(int[] readings, int lengthOfRecord)
    {

      var reduceByComparitor = (Func<int, int, bool> comparitor, int defaultValue) =>
      {
        var accumulator = readings.ToArray();
        var index = 0;
        while (accumulator.Length > 1)
        {
          Console.WriteLine(String.Join(" - ", accumulator.Select(x => Convert.ToString(x, 2).PadLeft(5, '0'))));
          var mostPrevalent = MostPrevalentInColOrDefault(accumulator, index, lengthOfRecord, defaultValue);
          Console.WriteLine($"Selecting only entries {(comparitor(1, mostPrevalent) ? 1 : 0)} at position {lengthOfRecord - index - 1}");
          accumulator = accumulator
            .Where(value => comparitor(BitAt(value, lengthOfRecord - index - 1), mostPrevalent))
            .ToArray();
          index++;
        }
        return accumulator.First();
      };

      var oxygenGeneratorRating = reduceByComparitor((int bitAtIndex, int mostPrevalent) => bitAtIndex == mostPrevalent, 1);
      Console.WriteLine(oxygenGeneratorRating);

      // var co2ScrubberRatingEntry = reduceByComparitor((int bitAtIndex, int mostPrevalent) => bitAtIndex != mostPrevalent);
      // Console.WriteLine(EnumerableToString(co2ScrubberRatingEntry));
      // var co2ScrubberRating = Convert.ToInt32(EnumerableToString(co2ScrubberRatingEntry), 2);

      Console.WriteLine("Part 2 - Life Support");
      Console.WriteLine($"Oxygen generator rating: {oxygenGeneratorRating}");


      var co2ScrubberRating = reduceByComparitor((int bitAtIndex, int mostPrevalent) => bitAtIndex != mostPrevalent, 0);
      Console.WriteLine($"CO2 scrubber rating: {co2ScrubberRating}");
      Console.WriteLine($"Result: {oxygenGeneratorRating * co2ScrubberRating}");
    }

    /// <summary>
    /// Returns the bit (0 or 1) at the given index
    /// - bitAt(4, 3) is equivalent to bitAt(0xb100, 3) will be 1
    /// - bitAt(22, 5) is equivalent to bitAt(0x10110, 5) will be 1
    /// uses RightShift operator to ensure the least significant digit is returned
    /// when using 'mod 2' (returns 1 for odd values, 0 for even)
    /// </summary>
    static int BitAt(int value, int negativeIndex) => (value >> negativeIndex) % 2;


    /// <summary>
    /// Returns the most prevalent bit in the given column
    /// 0-indexed from LHS.
    /// </summary>
    static int MostPrevalentInCol(int[] readings, int col, int colLength) =>
        readings.Sum(value => BitAt(value, colLength - col - 1)) > (readings.Length / 2)
          ? 1
          : 0;

    static int MostPrevalentInColOrDefault(int[] readings, int col, int colLength, int defaultValue)
    {
      var totalSum = readings.Sum(value => BitAt(value, colLength - col - 1));
      // Console.WriteLine($"MostPrevalentInColOrDefault average:{totalSum - (readings.Length / 2)} default:{defaultValue}");
      if (totalSum - (Convert.ToDouble(readings.Length) / 2) == 0)
      {
        Console.WriteLine($"Given equal items in col {col} - selecting default {defaultValue}");
      }
      return (totalSum - (Convert.ToDouble(readings.Length) / 2)) switch
      {
        < 0 => 0,
        > 0 => 1,
        _ => defaultValue // 1s and 0s are equal - pick the default supplied
      };
      // var grouping = readings
      //   .GroupBy(value => BitAt(value, colLength - col - 1))
      //   .ToDictionary(k => k.Key, k => k.Count())
      //   .OrderByDescending(kvp => kvp.Value)
      //   .ToArray();
      // return grouping[0].Value == grouping[1].Value
      //   ? defaultValue
      //   : grouping.First().Key;
    }

    // .OrderByDescending(kvp => kvp.Value);
    // ? 1
    // : 0;

    static int SumOfCols(int[] readings, int col, int colLength) =>
        readings.Sum(value => BitAt(value, colLength - col - 1));

    static string EnumerableToString(IEnumerable<int> arr) => new string(arr.Select(x => x.ToString()[0]).ToArray());
  }
}