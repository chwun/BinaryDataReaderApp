using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace BinaryDataReaderApp.Models
{
	/// <summary>
	/// Class for a binary data template
	/// </summary>
	public class BinaryDataTemplate
	{
		/// <summary>
		/// Name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// List of binary parts in this template
		/// </summary>
		public List<BinaryPart> Parts { get; private set; }

		/// <summary>
		/// Creates new instance of BinaryDataTemplate
		/// </summary>
		/// <param name="name">Name of this template</param>
		public BinaryDataTemplate(string name)
		{
			Name = name;
			Parts = new List<BinaryPart>();
		}

		/// <summary>
		/// Reads template from given XML provider
		/// </summary>
		/// <param name="templateXMLProvider">Object providing binary data template from XML</param>
		public bool ReadFromXML(IBinaryDataTemplateXMLProvider templateXMLProvider)
		{
			Parts.Clear();

			try
			{
				XElement xmlData = templateXMLProvider.GetXMLData();
				Name = xmlData.Attribute("Name").Value;

				foreach (XElement element in xmlData.Elements())
				{
					ParsePart(element, Parts);
				}

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// Parses template data for binary part from given XML
		/// </summary>
		/// <param name="element">XElement containing template data for binary part</param>
		/// <param name="partsList">current list of binary parts</param>
		private void ParsePart(XElement element, List<BinaryPart> partsList)
		{
			if (element.Name == "Section")
			{
				ParseSection(element, partsList);
			}
			else if (element.Name == "Value")
			{
				ParseValue(element, partsList);
			}
		}

		/// <summary>
		/// Parses template data for binary section from given XML
		/// </summary>
		/// <param name="element">XElement containing template data for binary section</param>
		/// <param name="partsList">current list of binary parts</param>
		private void ParseSection(XElement element, List<BinaryPart> partsList)
		{
			long id = long.Parse(element.Attribute("ID").Value);
			string name = element.Attribute("Name").Value;

			LoopType loopType = (LoopType)Enum.Parse(typeof(LoopType), element.Attribute("LoopType")?.Value ?? "NONE");
			long loopCountReference = long.Parse(element.Attribute("LoopCountReference")?.Value ?? "0");
			int loopCount = int.Parse(element.Attribute("LoopCount")?.Value ?? "0");

			BinarySection binarySection = new BinarySection(id, name);

			if (loopType != LoopType.NONE)
			{
				binarySection.LoopSettings = new LoopSettings()
				{
					Type = loopType,
					LoopCountFixed = loopCount,
					LoopCountReference = loopCountReference
				};
			}

			partsList.Add(binarySection);

			foreach (XElement childElement in element.Elements())
			{
				ParsePart(childElement, binarySection.Parts);
			}
		}
		/// <summary>
		/// Parses template data for binary value from given XML
		/// </summary>
		/// <param name="element">XElement containing template data for binary value</param>
		/// <param name="partsList">current list of binary parts</param>
		private void ParseValue(XElement element, List<BinaryPart> partsList)
		{
			long id = long.Parse(element.Attribute("ID").Value);
			string name = element.Attribute("Name").Value;
			string type = element.Attribute("Type").Value;

			switch (type)
			{
				case "byte":
					BinaryValue<byte> binaryValue_byte = new BinaryValue<byte>(id, name);
					partsList.Add(binaryValue_byte);
					break;

				case "short":
					BinaryValue<short> binaryValue_short = new BinaryValue<short>(id, name);
					partsList.Add(binaryValue_short);
					break;

				case "ushort":
					BinaryValue<ushort> binaryValue_ushort = new BinaryValue<ushort>(id, name);
					partsList.Add(binaryValue_ushort);
					break;

				case "int":
					BinaryValue<int> binaryValue_int = new BinaryValue<int>(id, name);
					partsList.Add(binaryValue_int);
					break;

				case "uint":
					BinaryValue<uint> binaryValue_uint = new BinaryValue<uint>(id, name);
					partsList.Add(binaryValue_uint);
					break;

				default:
					break;
			}
		}
	}
}