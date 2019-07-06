using System.Collections.Generic;

namespace BinaryDataReaderApp.Models
{
    /// <summary>
	/// Data class representing a section (e.g. struct) in a binary data template
	/// </summary>
	public class BinarySection : BinaryPart
	{
		/// <summary>
		/// Loop settings for this section
		/// </summary>
		public LoopSettings LoopSettings { get; set; }

		public List<BinaryPart> Parts { get; set; }

		/// <summary>
		/// Creates new instance of BinarySection with ID and name
		/// </summary>
		/// <param name="id">ID of this value</param>
		/// <param name="name">Name of this value</param>
		public BinarySection(long id, string name)
		 : base(id, name)
		{
			Parts = new List<BinaryPart>();
		}
	}
}