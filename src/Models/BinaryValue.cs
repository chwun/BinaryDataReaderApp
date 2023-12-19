using BinaryDataReaderApp.Localization;

namespace BinaryDataReaderApp.Models;

public class BinaryValue : BinaryPart
{
	private int byteOffset;
	private object value;
	private BinaryValueType valueType;

	/// <summary>
	/// Creates new instance of BinaryValue with ID and name
	/// </summary>
	/// <param name="id">ID of this value</param>
	/// <param name="name">name of this value</param>
	/// <param name="valueType">type of this value</param>
	/// <param name="converter">converter (optional)</param>
	public BinaryValue(long id, string name, BinaryValueType valueType, int length, IntToStringConverter converter)
		: base(id, name)
	{
		ValueType = valueType;
		Length = length;
		HasError = false;
		Converter = converter;
	}

	public BinaryValueType ValueType
	{
		get => valueType;
		set
		{
			valueType = value;
			OnPropertyChanged();
		}
	}

	public object Value
	{
		get => value;
		set
		{
			this.value = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(ValueText));
		}
	}

	public string ValueText => this.GetValueText();

	public int ByteOffset
	{
		get => byteOffset;
		set
		{
			byteOffset = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(DescriptionText));
		}
	}

	public int Length { get; }

	public bool HasError
	{
		get;
		set;
		// OnPropertyChanged();
	}

	public string DescriptionText
	{
		get
		{
			string text = TranslationManager.GetResourceText("BinaryFile_ByteOffset");
			text += ": ";
			text += ByteOffset;
			return text;
		}
	}

	public IntToStringConverter Converter { get; }

	public static BinaryValue CreateErrorValue(string name, BinaryValueType valueType, int length) => new(0, name, valueType, length, null)
	{
		HasError = true
	};
}