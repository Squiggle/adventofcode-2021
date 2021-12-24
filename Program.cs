using adventofcode2021.Day01;
using adventofcode2021.Day03;

var days = new Action[] {
  DepthFinder.Run,
  Pilot.Run,
  SystemsDiagnostics.Run
};

void RunDay(int day)
{
  Console.WriteLine($"==== Day {day} ====");
  days[day]();
}

if (args.Length == 0)
{
  RunDay(days.Length - 1);
}
else
{
  RunDay(Convert.ToInt32(args[0]) + 1);
}
