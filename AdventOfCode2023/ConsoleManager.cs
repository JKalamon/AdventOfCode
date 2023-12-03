using MoreLinq;

namespace AdventOfCode2023;

internal static class ConsoleManager
{
  private readonly static int StartLineY = 8;
  public static void SetTitle(string title, DateTime date)
  {
    var oldColor = Console.ForegroundColor;
    //// Draw frame
    Console.ForegroundColor = ConsoleColor.Green;
    Console.SetCursorPosition(0, 0);
    WriteLine('/', '=', '\\');
    WriteLine('|', ' ', '|');
    WriteLine('|', ' ', '|');
    WriteLine('|', ' ', '|');
    WriteLine('|', ' ', '|');
    WriteLine('\\', '=', '/');


    Console.ForegroundColor = ConsoleColor.Cyan;
    WriteStringCenter(title, 2);
    Console.ForegroundColor = ConsoleColor.Magenta;
    WriteStringCenter(date.ToString("dd.MM.yyyy"), 3);

    Console.SetCursorPosition(0, StartLineY);
    Console.ForegroundColor = oldColor;
  }

  public static void DrawImage(IEnumerable<string> image)
  {
    var defaultColor = Console.ForegroundColor;
    var colors = new Dictionary<char, ConsoleColor>()
    {
      ['X'] = ConsoleColor.DarkRed,
      ['|'] = ConsoleColor.Blue,
      ['+'] = ConsoleColor.Yellow,
      ['`'] = ConsoleColor.DarkYellow,
      ['~'] = ConsoleColor.DarkYellow,
      ['/'] = ConsoleColor.Green,
      ['\\'] = ConsoleColor.DarkGreen,
      ['('] = ConsoleColor.Red,
      [')'] = ConsoleColor.Red,
      ['O'] = ConsoleColor.Gray,
      ['*'] = ConsoleColor.Yellow,
      [','] = ConsoleColor.DarkGreen,
      ['.'] = ConsoleColor.DarkGreen,
      ['&'] = ConsoleColor.Cyan,
    };

    var imageWidth = image.Max(x => x.Length);
    var startPositionX = (Console.BufferWidth / 2 - imageWidth) / 2 - 1;
    Console.SetCursorPosition(startPositionX, StartLineY);

    image.ForEach((x, index) =>
    {
      x.ForEach(pix =>
      {
        var color = colors.ContainsKey(pix) ? colors[pix] : ConsoleColor.Gray;
        Console.ForegroundColor = color;
        Console.Write(pix);
      });

      Console.SetCursorPosition(startPositionX, StartLineY + index + 1);
    });
  }

  public static void WriteResult(string result, int resultNumber, TimeSpan elapsed, long bytes)
  {
    var left = Console.CursorLeft;
    var top = Console.CursorTop;
    Console.SetCursorPosition(Console.BufferWidth / 2 + 4, StartLineY + (resultNumber * 4));		
		Console.ForegroundColor = resultNumber == 1 ? ConsoleColor.Yellow : ConsoleColor.DarkCyan;
    Console.WriteLine($"Result for part {resultNumber}: {result}");
		
		Console.ForegroundColor = ConsoleColor.Gray;
    Console.SetCursorPosition(Console.BufferWidth / 2 + 4, StartLineY + 1 + (resultNumber * 4));
		Console.WriteLine($"Time: {elapsed.TotalSeconds:F3} seconds");
		Console.SetCursorPosition(Console.BufferWidth / 2 + 4, StartLineY + 2 + (resultNumber * 4));
		Console.WriteLine($"Memory: {bytes / 1024} KB");

		Console.SetCursorPosition(left, top);
  }

  private static void WriteCharacter(char input, int numberOfTimes = 1)
  {
    for (int i = 0; i < numberOfTimes; i++)
      Console.Write(input);
  }

  private static void WriteStringCenter(string line, int lineNumber)
  {
    Console.SetCursorPosition((Console.BufferWidth - line.Length) / 2 - 1, lineNumber);
    Console.Write(line);
  }

  private static void WriteLine(char startChar, char lineChar, char endChar)
  {
    Console.Write(startChar);
    WriteCharacter(lineChar, Console.BufferWidth - 2);
    Console.WriteLine(endChar);
  }
}
