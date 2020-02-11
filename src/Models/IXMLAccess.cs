using System.Xml.Linq;

namespace BinaryDataReaderApp.Models
{
	/// <summary>
	/// Interface for providing access to an XML file
	/// </summary>
	public interface IXMLAccess
	{
		/// <summary>
		/// Reads XML data
		/// </summary>
		XElement ReadXMLData();

		/// <summary>
		/// Writes XML data
		/// </summary>
		/// <param name="data">XML data to write</param>
		void WriteXMLData(XElement data);
	}
}