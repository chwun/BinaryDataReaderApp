using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace BinaryDataReader.Lib
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
        /// <param name="templateXMLProvider">Object providing binary data template from XML</param>
        public BinaryDataTemplate(string name, IBinaryDataTemplateXMLProvider templateXMLProvider)
        {
            Name = name;
            Parts = new List<BinaryPart>();

            ReadFromXML(templateXMLProvider);
        }

        /// <summary>
        /// Reads template from given XML provider
        /// </summary>
        /// <param name="templateXMLProvider">Object providing binary data template from XML</param>
        private void ReadFromXML(IBinaryDataTemplateXMLProvider templateXMLProvider)
        {
            XElement xmlData = templateXMLProvider.GetXMLData();

            Name = xmlData.Attribute("Name").Value;

            foreach (XElement element in xmlData.Elements())
            {
                ParsePart(element);
            }
        }

        /// <summary>
        /// Parses template data for binary part from given XML
        /// </summary>
        /// <param name="element">XElement containing template data for binary part</param>
        private void ParsePart(XElement element)
        {
            if (element.Name == "Section")
            {
                ParseSection(element);
            }
            else if (element.Name == "Value")
            {
                ParseValue(element);
            }
        }

        /// <summary>
        /// Parses template data for binary section from given XML
        /// </summary>
        /// <param name="element">XElement containing template data for binary section</param>
        private void ParseSection(XElement element)
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

            Parts.Add(binarySection);

            foreach (XElement childElement in element.Elements())
            {
                ParsePart(childElement);
            }
        }
        /// <summary>
        /// Parses template data for binary value from given XML
        /// </summary>
        /// <param name="element">XElement containing template data for binary value</param>
        private void ParseValue(XElement element)
        {
            long id = long.Parse(element.Attribute("ID").Value);
            string name = element.Attribute("Name").Value;
            string type = element.Attribute("Type").Value;

            switch (type)
            {
                case "byte":
                    BinaryValue<byte> binaryValue_byte = new BinaryValue<byte>(id, name);
                    Parts.Add(binaryValue_byte);
                    break;

                case "short":
                    BinaryValue<short> binaryValue_short = new BinaryValue<short>(id, name);
                    Parts.Add(binaryValue_short);
                    break;

                case "ushort":
                    BinaryValue<ushort> binaryValue_ushort = new BinaryValue<ushort>(id, name);
                    Parts.Add(binaryValue_ushort);
                    break;

                case "int":
                    BinaryValue<int> binaryValue_int = new BinaryValue<int>(id, name);
                    Parts.Add(binaryValue_int);
                    break;

                case "uint":
                    BinaryValue<uint> binaryValue_uint = new BinaryValue<uint>(id, name);
                    Parts.Add(binaryValue_uint);
                    break;

                default:
                    break;
            }
        }
    }
}