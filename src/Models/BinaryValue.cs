using BinaryDataReaderApp.Localization;

namespace BinaryDataReaderApp.Models
{
	public class BinaryValue : BinaryPart
	{
		private BinaryValueType valueType;
		private object value;
		private int byteOffset;
		private int length;
		private bool isSelected;

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

		public int ByteOffset
		{
			get
			{
				return byteOffset;
			}
			set
			{
				byteOffset = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(DescriptionText));
			}
		}

		public int Length
		{
			get
			{
				return length;
			}
			set
			{
				length = value;
				OnPropertyChanged();
			}
		}

		public bool IsSelected
		{
			get
			{
				return isSelected;
			}
			set
			{
				if (value != isSelected)
				{
					isSelected = value;
					OnPropertyChanged();
				}
			}
		}
		public string DescriptionText
		{
			get
			{
				string text = TranslationManager.Instance.GetResourceText("BinaryFile_ByteOffset");
				text += ": ";
				text += ByteOffset;
				return text;
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