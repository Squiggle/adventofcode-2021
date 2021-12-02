public class Pilot
{
  public static void Run()
  {
    var directions = Directions(File.ReadAllLines("Day02/input.txt"));

    Console.WriteLine("==========\nPiloting\n==========");

    var horizontal = 0;
    var vertical = 0;
    foreach (var direction in directions)
    {
      (horizontal, vertical) = BasicMovement(direction.direction)(direction.value, horizontal, vertical);
    }

    Console.WriteLine($"Horizontal: {horizontal} Vertical: {vertical}");
    Console.WriteLine($"Result: {horizontal * vertical}");

    Console.WriteLine("==========\nPiloting 2\n==========");

    var h2 = 0;
    var v2 = 0;
    var aim = 0;
    foreach (var direction in directions)
    {
      (h2, v2, aim) = MoveWithAim(direction.direction)(direction.value, h2, v2, aim);
    }

    Console.WriteLine($"Horizontal: {h2} Vertical: {v2}");
    Console.WriteLine($"Result: {h2 * v2}");
  }

  static Func<int, int, int, (int, int)> BasicMovement(Direction direction) =>
      direction switch
      {
        Direction.Forward => (int value, int horizontal, int vertical) => (horizontal + value, vertical),
        Direction.Up => (int value, int horizontal, int vertical) => (horizontal, vertical - value),
        Direction.Down => (int value, int horizontal, int vertical) => (horizontal, vertical + value),
        _ => throw new Exception($"No direction {direction}")
      };

  static Func<int, int, int, int, (int, int, int)> MoveWithAim(Direction direction) =>
      direction switch
      {
        Direction.Forward => (int value, int horizontal, int vertical, int aim) => (horizontal + value, vertical + (value * aim), aim),
        Direction.Up => (int value, int horizontal, int vertical, int aim) => (horizontal, vertical, aim - value),
        Direction.Down => (int value, int horizontal, int vertical, int aim) => (horizontal, vertical, aim + value),
        _ => throw new Exception($"No direction {direction}")
      };

  enum Direction
  {
    Forward,
    Down,
    Up
  }

  record Movement(Direction direction, int value);

  record Position(int horizontal, int vertical);

  static IEnumerable<Movement> Directions(IEnumerable<string> commands) =>
    commands
      .Select(command => command.Split(' '))
      .Select(parts => new Movement(ParseMovement(parts[0]), int.Parse(parts[1])));

  static Direction ParseMovement(string command) =>
     command switch
     {
       "forward" => Direction.Forward,
       "down" => Direction.Down,
       "up" => Direction.Up,
       _ => throw new Exception($"Unrecognised Direction {command}")
     };
}

