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
        private List<EnumStringConverter> converters;

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
            converters = new List<EnumStringConverter>();
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
                FilePattern = xmlData.Attribute("FilePattern")?.Value ?? "";

                XElement xmlDataConverters = xmlData.Element("Converters");
                if (xmlDataConverters != null)
                {
                    foreach (XElement elementConverter in xmlDataConverters.Elements())
                    {
                        ParseConverter(elementConverter);
                    }
                }

                XElement xmlDataTemplate = xmlData.Element("Template");
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
            if (element.Name == "EnumStringConverter")
            {
                ParseEnumStringConverter(element);
            }
        }

        private void ParseEnumStringConverter(XElement element)
        {
            string name = element.Attribute("Name").Value;
            EnumStringConverter converter = new EnumStringConverter(name);

            foreach (XElement elementEnumValue in element.Elements("EnumValue"))
            {
                int value = int.Parse(elementEnumValue.Attribute("Value").Value);
                string text = elementEnumValue.Attribute("Text").Value;

                converter.AddValue(value, text);
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
        private void ParseSection(XElement element, ObservableCollection<BinaryPart> partsList)
        {
            long id = long.Parse(element.Attribute("ID").Value);
            string name = element.Attribute("Name").Value;

            LoopType loopType = (LoopType)Enum.Parse(typeof(LoopType), element.Attribute("LoopType")?.Value ?? "NONE", true);
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
        private void ParseValue(XElement element, ObservableCollection<BinaryPart> partsList)
        {
            long id = long.Parse(element.Attribute("ID").Value);
            string name = element.Attribute("Name").Value;
            string type = element.Attribute("Type").Value.ToLower();
            string converterName = element.Attribute("Converter")?.Value ?? "";
            EnumStringConverter converter = !string.IsNullOrWhiteSpace(converterName) ? converters.Where(x => x.Name == converterName).FirstOrDefault() : null;

            BinaryValueType valueType = (BinaryValueType)Enum.Parse(typeof(BinaryValueType), type, true);
            BinaryValue binaryValue = new BinaryValue(id, name, valueType, converter);
            partsList.Add(binaryValue);
        }
    }
}