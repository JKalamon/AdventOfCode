using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var piccoloCloud2WebApiData = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Speerasdasdason", "SpeeronCheckInServer_PiccoloCloud2222", String.Empty)?.ToString() ?? String.Empty;
			Console.WriteLine(piccoloCloud2WebApiData);
		}
	}
}
