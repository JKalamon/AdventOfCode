using MoreLinq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
  internal static class PassportProcessingSolution
  {
    public static void Solve()
    {
      var inputList = File.ReadAllText("4PassportProcessing/input.txt");
      var passports = inputList.Split("\r\n\r\n").Select(x => x.Replace("\r\n", " ").Trim());

      var passportLowValList = passports.Select(x => new PassportLowValidation(x));
      var passportList = passports.Select(x => new Passport(x));
      Console.WriteLine($"First part {passportLowValList.Count(x => x.IsValid())}");

      Console.WriteLine($"Second part {passportList.Count(x => x.IsValid())}");
      //Console.WriteLine($"Solution First Part {NumberOfTrees(inputList, 3, 1)}");
    }

    public record PassportLowValidation(string Input)
    {
      public string? BirthYear => GetValue("byr:([#a-z0-9]+)($|\\s)");

      public string? IssueYear => GetValue("iyr:([#a-z0-9]+)($|\\s)");

      public string? ExpirationYear => GetValue("eyr:([#a-z0-9]+)($|\\s)");

      public string? Height => GetValue("hgt:([#a-z0-9]+)($|\\s)");

      public string? HairColor => GetValue("hcl:([#a-z0-9]+)($|\\s)");

      public string? EyeColor => GetValue("ecl:([#a-z0-9]+)($|\\s)");

      public string? PassportId => GetValue("pid:([#a-z0-9]+)($|\\s)");

      public string? CountryId => GetValue("cid:([#a-z0-9]+)($|\\s)");

      public bool IsValid()
      {
        return HairColor != null && EyeColor != null && PassportId != null && Height != null && ExpirationYear != null && IssueYear != null && BirthYear != null;
      }

      private string? GetValue(string pattern)
      {
        var reg = new Regex(pattern);
        if (reg.IsMatch(this.Input))
          return reg.Match(this.Input).Groups[1].Value;

        return null;
      }
    }

    public record Passport(string Input)
    {
      public int BirthYear => int.Parse(GetValue("byr:(\\d{4})($|\\s)") ?? "0");

      public int IssueYear => int.Parse(GetValue("iyr:(\\d{4})($|\\s)") ?? "0");

      public int ExpirationYear => int.Parse(GetValue("eyr:(\\d{4})($|\\s)") ?? "0");

      public string? Height => GetValue("hgt:(\\d+(cm|in))($|\\s)");

      public string? HairColor => GetValue("hcl:(#[a-f0-9]{6})($|\\s)");

      public string? EyeColor => GetValue("ecl:(amb|blu|brn|gry|grn|hzl|oth)($|\\s)");

      public string? PassportId => GetValue("pid:(\\d{9})($|\\s)");

      public string? CountryId => GetValue("cid:(\\d+)($|\\s)");

      public bool IsValid()
      {
        
        if (HairColor == null || EyeColor == null || PassportId == null || Height == null)
          return false;

        if (BirthYear < 1920 || BirthYear > 2002)
          return false;

        if (IssueYear < 2010 || IssueYear > 2020)
          return false;

        if (ExpirationYear < 2020 || ExpirationYear > 2030)
          return false;

        var height = int.Parse(Height!.Replace("cm", String.Empty).Replace("in", String.Empty));
        if (Height!.EndsWith("cm") && (height < 150 || height > 193))
          return false;

        if (Height!.EndsWith("in") && (height < 59 || height > 76))
          return false;

        return true;
      }

      private string? GetValue(string pattern)
      {
        var reg = new Regex(pattern);
        if (reg.IsMatch(this.Input))
          return reg.Match(this.Input).Groups[1].Value;

        return null;
      }
    }
  }
}
