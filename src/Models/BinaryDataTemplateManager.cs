using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BinaryDataReaderApp.Models
{
	public class BinaryDataTemplateManager
	{
		private string templateDirectory;
		private Dictionary<string, BinaryDataTemplate> templates;
		private Dictionary<string, DateTime?> filesTimestamps;

		public BinaryDataTemplateManager(string templateDirectory)
		{
			this.templateDirectory = templateDirectory;
			templates = new Dictionary<string, BinaryDataTemplate>();
			filesTimestamps = new Dictionary<string, DateTime?>();
		}

		public BinaryDataTemplate GetMatchingTemplate(string binaryFilename)
		{
			RefreshTemplatesDirectory();

			try
			{
				foreach (BinaryDataTemplate template in templates.Values)
				{
					if (!string.IsNullOrWhiteSpace(template.FilePattern))
					{
						Match match = Regex.Match(binaryFilename, template.FilePattern);
						if (match.Success)
						{
							return template;
						}
					}
				}
			}
			catch
			{
			}

			return null;
		}

		public BinaryDataTemplate GetTemplate(string templateFile)
		{
			if (!string.IsNullOrWhiteSpace(templateFile))
			{
				if (filesTimestamps.ContainsKey(templateFile))
				{
					DateTime fileLastWrite = GetFileLastWriteTimestamp(templateFile);

					if (fileLastWrite > filesTimestamps[templateFile])
					{
						ReadTemplateFile(templateFile);
					}
				}
				else
				{
					ReadTemplateFile(templateFile);
				}

				return templates[templateFile];
			}
			else
			{
				return null;
			}
		}

		private void RefreshTemplatesDirectory()
		{
			List<string> files = GetChangedTemplateFiles(templateDirectory);
			foreach (string file in files)
			{
				ReadTemplateFile(file);
			}
		}

		private List<string> GetChangedTemplateFiles(string directory)
		{
			List<string> changedFiles = new List<string>();

			try
			{
				string[] files = Directory.GetFiles(directory, "*.xml");

				foreach (string file in files)
				{
					DateTime fileLastWrite = GetFileLastWriteTimestamp(file);

					if (filesTimestamps.ContainsKey(file) && (filesTimestamps[file] != null))
					{
						if (fileLastWrite > filesTimestamps[file])
						{
							// file has changed:
							changedFiles.Add(file);
						}
					}
					else
					{
						changedFiles.Add(file);
					}
				}
			}
			catch
			{}

			return changedFiles;
		}

		private DateTime GetFileLastWriteTimestamp(string file)
		{
			try
			{
				return File.GetLastWriteTimeUtc(file);
			}
			catch
			{
				return DateTime.UtcNow;
			}
		}

		private void ReadTemplateFile(string file)
		{
			XMLAccess xmlProvider = new XMLAccess(file);
			BinaryDataTemplate template = new BinaryDataTemplate("new template");

			if (template.ReadFromXML(xmlProvider))
			{
				templates[file] = template;
				filesTimestamps[file] = DateTime.UtcNow;
			}
			else
			{
				templates[file] = null;
				filesTimestamps[file] = null;
			}
		}
	}
}