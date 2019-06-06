using System.Xml.Linq;

namespace BinaryDataReader.Lib
{
    /// <summary>
    /// Interface for providing binary data template as XML
    /// </summary>
    public interface IBinaryDataTemplateXMLProvider
    {
        /// <summary>
        /// Gets XML of binary data template
        /// </summary>
        XElement GetXMLData();
    }
}