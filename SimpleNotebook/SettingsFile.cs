using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNotebook
{
	public class SettingsFile
	{

		public SettingsFile(string path)
		{
			SettingsPath = path;
			ReadSettingsFile();
		}

		public string SettingsPath { get; private set; }
		public int MaxFileObjectSize { get; private set; }
		public string[] IgnoredFileExtensions { get; private set; }
		public string StartupPath { get; private set; }
		public string[] IncludedFiles { get; private set; }

		public void ReadSettingsFile()
		{
			string[] settingsfile = System.IO.File.ReadAllLines(SettingsPath);

			foreach (string line in settingsfile)
			{
				string[] parts = line.Split(@":-");

				switch (parts[0].ToLower().Trim())
				{
					case "ignoredfiles":
						IgnoredFileExtensions = parts[1].Trim().Split(',');
						break;
					case "maxfilesize":
						MaxFileObjectSize = Convert.ToInt32(parts[1].Trim());
						break;
					case "startuppath":
						StartupPath = string.Concat(parts[1].Where(c => !char.IsWhiteSpace(c)));
						break;
					case "includedfiles":
						IncludedFiles = parts[1].Trim().Split(',');
						break;
				}

			}
		}


	}
}
