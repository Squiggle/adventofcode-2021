namespace adventofcode2021
{
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

    public static IEnumerable<int> Enumerate(this int count)
    {
      for (var i = 0; i < count; i++)
      {
        yield return i;
      }
    }
  }
}