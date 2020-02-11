using System.Xml.Linq;

namespace BinaryDataReaderApp.Models
{
	/// <summary>
	/// Class for providing read/write access to an XML file. Implements interface <see cref="IXMLAccess"/>.
	/// </summary>
	public class XMLAccess : IXMLAccess
	{
		private string path;

		/// <summary>
		/// Creates a new instance of XMLAccess
		/// </summary>
		/// <param name="path">path of XML file</param>
		public XMLAccess(string path)
		{
			this.path = path;
		}

		/// <summary>
		/// Reads XML data
		/// </summary>
		public XElement ReadXMLData()
		{
			return XElement.Load(path);
		}

		/// <summary>
		/// Writes XML data
		/// </summary>
		/// <param name="data">XML data to write</param>
		public void WriteXMLData(XElement data)
		{
			data.Save(path);
		}
	}
}