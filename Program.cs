var source = File
  .ReadAllLines("input.txt")
  .Select(input => int.Parse(input))
  .ToArray();

/**
Part one
Running tally
*/

int RollingTallyOfIncrements(int[] values)
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

var tally = RollingTallyOfIncrements(source);
Console.WriteLine($"Running tally: {tally}");

/**
Part two
Rolling window of three
*/


var windowedSums = source.SlidingWindow(3)
  .Where(w => w.Length == 3)
  .Select(w => w.Sum())
  .ToArray();
var tallyOfWindows = RollingTallyOfIncrements(windowedSums);

Console.WriteLine($"Windowed tally: {tallyOfWindows}");

static class EnumerableExtensions
{
  public static IEnumerable<int[]> SlidingWindow(this int[] source, int windowSize)
  {
    int[] window = { };
    int index = 0;
    foreach (var value in source)
    {
      if (index >= windowSize)
      {
        window = window.Skip(1).ToArray();
      }
      window = window.Append(value).ToArray();
      yield return window;
      index++;
    }
  }
}