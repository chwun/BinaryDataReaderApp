namespace BinaryDataReaderApp.Models
{
	public class BinaryValue : BinaryPart
	{
		private BinaryValueType valueType;
		private object value;

		public BinaryValueType ValueType
		{
			get
			{
				return valueType;
			}
			set
			{
				valueType = value;
				OnPropertyChanged();
			}
		}

		public object Value
		{
			get
			{
				return value;
			}
			set
			{
				this.value = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Creates new instance of BinaryValue with ID and name
		/// </summary>
		/// <param name="id">ID of this value</param>
		/// <param name="name">Name of this value</param>
		/// <param name="valueType">Type of this value</param>
		public BinaryValue(long id, string name, BinaryValueType valueType)
		: base(id, name)
		{
			ValueType = valueType;
		}
	}
}