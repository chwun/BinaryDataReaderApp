using System;
using BinaryDataReaderApp.Localization;

namespace BinaryDataReaderApp.Models
{
	public class BinaryValue : BinaryPart
	{
		private BinaryValueType valueType;
		private object value;
		private int byteOffset;
		private int length;
		private bool hasError;
		private IntToStringConverter converter;

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
				OnPropertyChanged(nameof(ValueText));
			}
		}

		public string ValueText
		{
			get
			{
				if (Converter != null)
				{
					try
					{
						string convertedText = Converter.GetText(Convert.ToInt32(Value));
						return $"{convertedText} ({Value})";
					}
					catch
					{
						return Value.ToString();
					}
				}
				else
				{
					return Value.ToString();
				}
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

		public bool HasError
		{
			get
			{
				return hasError;
			}
			set
			{
				hasError = value;
				// OnPropertyChanged();
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

		public IntToStringConverter Converter
		{
			get
			{
				return converter;
			}
			private set
			{
				converter = value;
			}
		}

		// <summary>
		/// Creates new instance of BinaryValue with ID and name
		/// </summary>
		/// <param name="id">ID of this value</param>
		/// <param name="name">name of this value</param>
		/// <param name="valueType">type of this value</param>
		/// <param name="converter">converter (optional)</param>/ 	
		public BinaryValue(long id, string name, BinaryValueType valueType, IntToStringConverter converter)
		: base(id, name)
		{
			ValueType = valueType;
			HasError = false;
			Converter = converter;
		}

		public static BinaryValue CreateErrorValue(string name, BinaryValueType valueType)
		{
			return new BinaryValue(0, name, valueType, null)
			{
				HasError = true
			};
		}
	}
}