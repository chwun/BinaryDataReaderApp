using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;

namespace BinaryDataReaderApp.Models
{
	/// <summary>
	/// Class for a binary data template
	/// </summary>
	public class BinaryDataTemplate : ModelBase
	{
		private string name;
		private ObservableCollection<BinaryPart> parts;
		private string filePattern;
		private List<IntToStringConverter> converters;

		/// <summary>
		/// Name
		/// </summary>
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// List of binary parts in this template
		/// </summary>
		public ObservableCollection<BinaryPart> Parts
		{
			get
			{
				return parts;
			}
			private set
			{
				parts = value;
				OnPropertyChanged();
			}
		}

		public string FilePattern
		{
			get
			{
				return filePattern;
			}
			set
			{
				filePattern = ConvertFilePatternToRegex(value);
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Creates new instance of BinaryDataTemplate
		/// </summary>
		/// <param name="name">Name of this template</param>
		public BinaryDataTemplate(string name)
		{
			Name = name;
			Parts = new ObservableCollection<BinaryPart>();
			converters = new List<IntToStringConverter>();
		}

		/// <summary>
		/// Saves the template to XML
		/// </summary>
		/// <param name="xmlAccess">object providing access to an XML file</param>
		/// <returns>true, if saving successful</returns>
		public bool SaveToXML(IXMLAccess xmlAccess)
		{
			XElement xmlData = new XElement(Constants.TemplateXML_Root);
			xmlData.Add(new XAttribute(Constants.TemplateXML_Name, Name));
			xmlData.Add(new XAttribute(Constants.TemplateXML_FilePattern, FilePattern));

			xmlData.Add(CreateXML_Template());
			xmlData.Add(CreateXML_Converters());

			try
			{
				xmlAccess.WriteXMLData(xmlData);
				return true;
			}
			catch
			{
				return false;
			}
		}

		private XElement CreateXML_Template()
		{
			XElement xmlTemplate = new XElement(Constants.TemplateXML_Template);

			foreach (var part in Parts)
			{
				xmlTemplate.Add(CreateXML_Part(part));
			}

			return xmlTemplate;
		}

		private XElement CreateXML_Part(BinaryPart part)
		{
			if (part is BinarySection section)
			{
				return CreateXML_Section(section);
			}
			else if (part is BinaryValue value)
			{
				return CreateXML_Value(value);
			}
			else
			{
				throw new NotImplementedException();
			}
		}

		private XElement CreateXML_Section(BinarySection section)
		{
			XElement xmlSection = new XElement(Constants.TemplateXML_BinarySection);

			xmlSection.Add(new XAttribute(Constants.TemplateXML_ID, section.ID));
			xmlSection.Add(new XAttribute(Constants.TemplateXML_Name, section.Name));

			if (section.LoopSettings != null)
			{
				xmlSection.Add(new XAttribute(Constants.TemplateXML_LoopType, section.LoopSettings.Type.ToString()));
				xmlSection.Add(new XAttribute(Constants.TemplateXML_LoopCountReference, section.LoopSettings.LoopCountReference));
				xmlSection.Add(new XAttribute(Constants.TemplateXML_LoopCount, section.LoopSettings.LoopCountFixed));
			}

			foreach (var part in section.Parts)
			{
				xmlSection.Add(CreateXML_Part(part));
			}

			return xmlSection;
		}

		private XElement CreateXML_Value(BinaryValue value)
		{
			XElement xmlValue = new XElement(Constants.TemplateXML_BinaryValue);
			xmlValue.Add(new XAttribute(Constants.TemplateXML_ID, value.ID));
			xmlValue.Add(new XAttribute(Constants.TemplateXML_Name, value.Name));
			xmlValue.Add(new XAttribute(Constants.TemplateXML_Type, value.ValueType.ToString()));

			if (value.Converter != null)
			{
				xmlValue.Add(new XAttribute(Constants.TemplateXML_Converter, value.Converter.Name));
			}

			return xmlValue;
		}

		private XElement CreateXML_Converters()
		{
			XElement xmlConverters = new XElement(Constants.TemplateXML_Converters);

			foreach (var converter in converters)
			{
				switch (converter)
				{
					case EnumToStringConverter c:
						xmlConverters.Add(CreateXML_EnumStringConverter(c));
						break;

					default:
						break;
				}
			}

			return xmlConverters;
		}

		private XElement CreateXML_EnumStringConverter(EnumToStringConverter c)
		{
			XElement xmlConverter = new XElement(Constants.TemplateXML_EnumStringConverter);
			xmlConverter.Add(new XAttribute(Constants.TemplateXML_Name, c.Name));

			foreach (var mapping in c.GetMappings())
			{
				XElement xmlEnumValue = new XElement(Constants.TemplateXML_EnumValue);
				xmlEnumValue.Add(new XAttribute(Constants.TemplateXML_Value, mapping.Key));
				xmlEnumValue.Add(new XAttribute(Constants.TemplateXML_Text, mapping.Value));

				xmlConverter.Add(xmlEnumValue);
			}

			return xmlConverter;
		}

		/// <summary>
		/// Reads template data from given XML
		/// </summary>
		/// <param name="xmlAccess">object providing access to an XML file</param>
		public bool ReadFromXML(IXMLAccess xmlAccess)
		{
			Parts.Clear();

			try
			{
				XElement xmlData = xmlAccess.ReadXMLData();
				Name = xmlData.Attribute(Constants.TemplateXML_Name).Value;
				FilePattern = xmlData.Attribute(Constants.TemplateXML_FilePattern)?.Value ?? "";

				XElement xmlDataConverters = xmlData.Element(Constants.TemplateXML_Converters);
				if (xmlDataConverters != null)
				{
					foreach (XElement elementConverter in xmlDataConverters.Elements())
					{
						ParseConverter(elementConverter);
					}
				}

				XElement xmlDataTemplate = xmlData.Element(Constants.TemplateXML_Template);
				foreach (XElement element in xmlDataTemplate.Elements())
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

		private string ConvertFilePatternToRegex(string pattern)
		{
			if (!string.IsNullOrWhiteSpace(pattern))
			{
				return pattern.Replace("*", ".*").Replace("?", ".?");
			}
			else
			{
				return "";
			}
		}

		private void ParseConverter(XElement element)
		{
			if (element.Name == Constants.TemplateXML_EnumStringConverter)
			{
				ParseEnumStringConverter(element);
			}
		}

		private void ParseEnumStringConverter(XElement element)
		{
			string name = element.Attribute(Constants.TemplateXML_Name).Value;
			IntToStringConverter converter = new EnumToStringConverter(name);

			foreach (XElement elementEnumValue in element.Elements(Constants.TemplateXML_EnumValue))
			{
				int value = int.Parse(elementEnumValue.Attribute(Constants.TemplateXML_Value).Value);
				string text = elementEnumValue.Attribute(Constants.TemplateXML_Text).Value;

				converter.AddMapping(value, text);
			}

			converters.Add(converter);
		}

		/// <summary>
		/// Parses template data for binary part from given XML
		/// </summary>
		/// <param name="element">XElement containing template data for binary part</param>
		/// <param name="partsList">current list of binary parts</param>
		private void ParsePart(XElement element, ObservableCollection<BinaryPart> partsList)
		{
			if (element.Name == Constants.TemplateXML_BinarySection)
			{
				ParseSection(element, partsList);
			}
			else if (element.Name == Constants.TemplateXML_BinaryValue)
			{
				ParseValue(element, partsList);
			}
		}

		/// <summary>
		/// Parses template data for binary section from given XML
		/// </summary>
		/// <param name="element">XElement containing template data for binary section</param>
		/// <param name="partsList">current list of binary parts</param>
		private void ParseSection(XElement element, ObservableCollection<BinaryPart> partsList)
		{
			long id = long.Parse(element.Attribute(Constants.TemplateXML_ID).Value);
			string name = element.Attribute(Constants.TemplateXML_Name).Value;

			LoopType loopType = (LoopType)Enum.Parse(typeof(LoopType), element.Attribute(Constants.TemplateXML_LoopType)?.Value ?? LoopType.NONE.ToString(), true);
			long loopCountReference = long.Parse(element.Attribute(Constants.TemplateXML_LoopCountReference)?.Value ?? "0");
			int loopCount = int.Parse(element.Attribute(Constants.TemplateXML_LoopCount)?.Value ?? "0");

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
		private void ParseValue(XElement element, ObservableCollection<BinaryPart> partsList)
		{
			long id = long.Parse(element.Attribute(Constants.TemplateXML_ID).Value);
			string name = element.Attribute(Constants.TemplateXML_Name).Value;
			string type = element.Attribute(Constants.TemplateXML_Type).Value.ToLower();
			string converterName = element.Attribute(Constants.TemplateXML_Converter)?.Value ?? "";
			IntToStringConverter converter = !string.IsNullOrWhiteSpace(converterName) ? converters.Where(x => x.Name == converterName).FirstOrDefault() : null;

			BinaryValueType valueType = (BinaryValueType)Enum.Parse(typeof(BinaryValueType), type, true);
			BinaryValue binaryValue = new BinaryValue(id, name, valueType, converter);
			partsList.Add(binaryValue);
		}
	}
}