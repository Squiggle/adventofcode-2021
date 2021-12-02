namespace adventofcode2021.Day01
{
  public class DepthFinder
  {
    public static void Run()
    {
      Console.WriteLine("============\nDepth Finder\n============");
      var source = File
        .ReadAllLines("Day01/input.txt")
        .Select(input => int.Parse(input))
        .ToArray();

      var tally = RollingTallyOfIncrements(source);
      Console.WriteLine($"Running tally: {tally}");


      var windowedSums = source.SlidingWindow(3)
        .Where(w => w.Length == 3)
        .Select(w => w.Sum())
        .ToArray();
      var tallyOfWindows = RollingTallyOfIncrements(windowedSums);

      Console.WriteLine($"Windowed tally: {tallyOfWindows}");
    }

    static int RollingTallyOfIncrements(int[] values)
    {
      int? prev = null;
      var tally = 0;
      foreach (var value in values)
      {
        if (prev.HasValue)
        {
          tally += (prev.Value - value) switch
          {
            < 0 => 1,
            >= 0 => 0
          };
        }
        prev = value;
      }
      return tally;
    }
  }
}