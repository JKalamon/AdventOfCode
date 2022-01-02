using MoreLinq;

namespace AdventOfCode2021
{
  internal class TrenchMapSolution : IChallenge
  {
    public string Title => "--- Day 20: Trench Map ---";

    public DateTime DateTime => new(2021, 12, 20);

    private IEnumerable<string> commands = File.ReadAllLines("20TranchMap/input.txt");

    public enum PixelType
    {
      Dark = 0,
      Lit = 1,
    }

    public record Point(int X, int Y);

    public record Pixel(PixelType Type, Point Coordinates);

    public record Image(IEnumerable<Pixel> Pixels)
    {
      public static Image FromLines(IEnumerable<string> lines)
      {
        var pixelList = new List<Pixel>();
        for (int y = 0; y < lines.Count(); y++)
        {
          var line = lines.ElementAt(y);
          for (int x = 0; x < line.Length; x++)
          {
            if (line[x] == '#')
            {
              pixelList.Add(new Pixel(PixelType.Lit, new(x, y)));
            }
          }
        }

        return new Image(pixelList);
      }

      public IEnumerable<Pixel> Pixels { get; private set; } = Pixels.Where(x => x.Type == PixelType.Lit);

      public PixelType DefaultPixelType = PixelType.Dark;

      public void Enhance(PixelType[] algorithm)
      {
        var maxVal = Convert.ToInt32("111111111", 2);
        var xMin = this.Pixels.Min(pix => pix.Coordinates.X) - 1;
        var xMax = this.Pixels.Max(pix => pix.Coordinates.X) + 1;
        var yMin = this.Pixels.Min(pix => pix.Coordinates.Y) - 1;
        var yMax = this.Pixels.Max(pix => pix.Coordinates.Y) + 1;

        var isChangingDefaultType = (this.DefaultPixelType == PixelType.Dark && algorithm[0] == PixelType.Lit) || (this.DefaultPixelType == PixelType.Lit && algorithm[maxVal] == PixelType.Dark);
        var pixelTypeToSave = isChangingDefaultType ? this.DefaultPixelType : GetOtherPixelType(this.DefaultPixelType);
        var newListOfPixels = new List<Pixel>();

        for (int y = yMin; y <= yMax; y++)
        {
          for (int x = xMin; x <= xMax; x++)
          {
            var point = new Point(x, y);
            if (pixelTypeToSave == algorithm[this.GetPixelValue(point)])
            {
              newListOfPixels.Add(new Pixel(pixelTypeToSave, point));
            }
          }
        }

        this.Pixels = newListOfPixels;
        this.DefaultPixelType = GetOtherPixelType(pixelTypeToSave);
      }

      public void Draw()
      {
        var xMin = 0; //this.Pixels.Min(pix => pix.Coordinates.X);
        var xMax = this.Pixels.Max(pix => pix.Coordinates.X);
        var yMin = 0;// this.Pixels.Min(pix => pix.Coordinates.Y);
        var yMax = this.Pixels.Max(pix => pix.Coordinates.Y);


        
        for (int y = yMin; y <= yMax; y++)
        {
          for (int x = xMin; x <= xMax; x++)
          {
            var point = new Point(x, y);
            
            if(this.Pixels.Any(x => x.Coordinates == point))
            {
              Console.Write(this.DrawPixelType(this.GetOtherPixelType(this.DefaultPixelType)));
            }
            else
            {
              Console.Write(this.DrawPixelType(this.DefaultPixelType));
            }
          }
          Console.WriteLine();
        }

      }

      public PixelType GetOtherPixelType(PixelType p)
        => p == PixelType.Dark ? PixelType.Lit : PixelType.Dark;

      public string DrawPixelType(PixelType p)
        => p == PixelType.Dark ? "." : "#";

      public int GetPixelValue(Point p)
      {
        Point[] points = new Point[] {
          new(p.X - 1, p.Y - 1), new(p.X, p.Y - 1), new(p.X + 1, p.Y - 1),
          new(p.X - 1, p.Y), new(p.X, p.Y), new(p.X + 1, p.Y),
          new(p.X - 1, p.Y + 1), new(p.X, p.Y + 1), new(p.X + 1, p.Y + 1)
        };

        var binaryString = string.Empty;
        points.ForEach(x =>
        {
          var pixelValue = this.Pixels.FirstOrDefault(y => y.Coordinates == x) ?? new Pixel(this.DefaultPixelType, x);
          binaryString += ((int)pixelValue.Type).ToString();
        });

        return Convert.ToInt32(binaryString, 2);
      }
    }

    public object SolvePart1()
    {
      var algorithm = this.commands.First().Select(x => x == '.' ? PixelType.Dark : PixelType.Lit);

      var image = Image.FromLines(this.commands.Skip(2));
      image.Draw();

      Console.WriteLine("");
      Console.WriteLine("----------");
      Console.WriteLine("");
      image.Enhance(algorithm.ToArray());
      image.Draw();
      image.Enhance(algorithm.ToArray());
      return image.Pixels.Count();
    }

    public object? SolvePart2()
    {
      var algorithm = this.commands.First().Select(x => x == '.' ? PixelType.Dark : PixelType.Lit);

      var image = Image.FromLines(this.commands.Skip(2));
      for (int i = 0; i < 50; i++)
      {
        image.Enhance(algorithm.ToArray());
        Console.WriteLine($"Finished enhance {i + 1}");
      }      
      
      return image.Pixels.Count();
    }
  }
}

