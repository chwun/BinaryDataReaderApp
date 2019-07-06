using System.Xml.Linq;

namespace BinaryDataReaderApp.Models
{
	/// <summary>
	/// Class for providing binary data template from XML file.see Implements interface <see cref="IBinaryDataTemplateXMLProvider"/>.
	/// </summary>
	public class BinaryDataTemplateXMLProvider : IBinaryDataTemplateXMLProvider
	{
		private string path;

		/// <summary>
		/// Creates new instance of BinaryDataTemplateXMLProvider
		/// </summary>
		/// <param name="path">Path of XLM file</param>
		public BinaryDataTemplateXMLProvider(string path)
		{
			this.path = path;
		}

		/// <summary>
		/// Gets XML of binary data template
		/// </summary>
		public XElement GetXMLData()
		{
			return XElement.Load(path);
		}
	}
}