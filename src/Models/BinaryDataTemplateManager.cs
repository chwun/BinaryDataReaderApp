using System.IO;
using System.Text.RegularExpressions;

namespace BinaryDataReaderApp.Models;

public class BinaryDataTemplateManager
{
	private readonly Dictionary<string, DateTime?> filesTimestamps;
	private readonly string templateDirectory;
	private readonly Dictionary<string, BinaryDataTemplate> templates;

	public BinaryDataTemplateManager(string templateDirectory)
	{
		this.templateDirectory = templateDirectory;
		templates = new();
		filesTimestamps = new();
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

		return null;
	}

	private void RefreshTemplatesDirectory()
	{
		var files = GetChangedTemplateFiles(templateDirectory);
		foreach (string file in files)
		{
			ReadTemplateFile(file);
		}
	}

	private List<string> GetChangedTemplateFiles(string directory)
	{
		var changedFiles = new List<string>();

		try
		{
			string[] files = Directory.GetFiles(directory, "*.xml");

			foreach (string file in files)
			{
				DateTime fileLastWrite = GetFileLastWriteTimestamp(file);

				if (filesTimestamps.TryGetValue(file, out var fileTimestamp) && fileTimestamp != null)
				{
					if (fileLastWrite > fileTimestamp)
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
		{
		}

		return changedFiles;
	}

	private static DateTime GetFileLastWriteTimestamp(string file)
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
		XMLAccess xmlProvider = new(file);
		BinaryDataTemplate template = new("new template");

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