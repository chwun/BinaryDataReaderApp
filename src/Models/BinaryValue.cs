namespace BinaryDataReaderApp.Models
{
	public abstract class BinaryValue : BinaryPart
	{
		/// <summary>
		/// Creates new instance of BinaryValue with ID and name
		/// </summary>
		/// <param name="id">ID of this value</param>
		/// <param name="name">Name of this value</param>
		public BinaryValue(long id, string name)
		: base(id, name)
		{
		}
	}
}