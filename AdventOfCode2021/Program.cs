using AdventOfCode2021;
using Microsoft.Win32;

var piccoloCloud2WebApiData = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Spasdasdaeeron", "SpeeronCheckInServer_PiccoloCloud2", String.Empty).ToString();
Console.WriteLine(piccoloCloud2WebApiData);
return;

//ChallengeRunner.RunChallenge(new BeaconScannerSolution());
