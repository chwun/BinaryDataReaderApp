using BinaryDataReaderApp.Events;
using BinaryDataReaderApp.Localization;
using System.Collections.ObjectModel;
using System.IO;

namespace BinaryDataReaderApp.Models;

public class BinaryFile : ModelBase
{
	private readonly string binaryFile;
	private string errors;
	private string fileSizeText;
	private bool hasErrors;
	private List<HexDumpLine> hexDumpLines;
	private ObservableCollection<BinaryPart> parts;

	public BinaryFile(string binaryFile, BinaryDataTemplate template)
	{
		this.binaryFile = binaryFile;
		Template = template;
		Parts = new();
	}

	public List<HexDumpLine> HexDumpLines
	{
		get => hexDumpLines;
		private set
		{
			hexDumpLines = value;
			OnPropertyChanged();
		}
	}

	public ObservableCollection<BinaryPart> Parts
	{
		get => parts;
		private init
		{
			parts = value;
			OnPropertyChanged();
		}
	}

	public string Errors
	{
		get => errors;
		private set
		{
			errors = value;
			OnPropertyChanged();
		}
	}

	public bool HasErrors
	{
		get => hasErrors;
		private set
		{
			hasErrors = value;
			OnPropertyChanged();
		}
	}

	public string FileSizeText
	{
		get => fileSizeText;
		private set
		{
			fileSizeText = value;
			OnPropertyChanged();
		}
	}

	public BinaryDataTemplate Template { get; }

	public void Read()
	{
		ReadData();
		CreateByteList();
	}

	public void SetSelectionInHexDump(BinaryValue value)
	{
		// set selected state:
		var hexBytes = HexDumpLines.SelectMany(x => x.HexBytes);
		foreach (HexDumpByte hexByte in hexBytes)
		{
			if (hexByte.ByteOffset >= value.ByteOffset && hexByte.ByteOffset < value.ByteOffset + value.Length)
			{
				hexByte.IsSelected = true;
			}
			else
			{
				hexByte.IsSelected = false;
			}
		}

		// raise event:
		HexDumpLine selectedHexDumpLine = HexDumpLines.FirstOrDefault(x => x.HexBytes.Any(b => b.IsSelected));
		HexDumpSelectionChanged?.Invoke(
			this,
			new()
			{
				SelectedHexDumpLine = selectedHexDumpLine
			});
	}

	public BinaryValue FindValueByByteOffset(int byteOffset)
	{
		return FindPart(Parts, x => x is BinaryValue val && byteOffset >= val.ByteOffset && byteOffset < val.ByteOffset + val.Length) as BinaryValue;
	}

	private static BinaryPart FindPart(IEnumerable<BinaryPart> parts, Func<BinaryPart, bool> condition)
	{
		foreach (BinaryPart part in parts)
		{
			if (part is BinaryValue)
			{
				if (condition(part))
				{
					return part;
				}
			}
			else if (part is BinarySection section)
			{
				BinaryPart match = FindPart(section.Parts, condition);
				if (match != null)
				{
					return match;
				}
			}
		}

		return null;
	}

	private void ReadData()
	{
		var errorList = new List<string>();

		try
		{
			using BinaryReader reader = new(File.Open(binaryFile, FileMode.Open));
			var globalValueCache = new Dictionary<long, object>();
			int byteOffset = 0;

			foreach (BinaryPart templatePart in Template.Parts)
			{
				ReadPart(templatePart, Parts, globalValueCache, ref byteOffset, errorList, reader);
			}

			if (reader.BaseStream.Position < reader.BaseStream.Length)
			{
				errorList.Add(TranslationManager.GetResourceText("BinaryFile_FileTooLongError"));
			}
		}
		catch (EndOfStreamException)
		{
			errorList.Add(TranslationManager.GetResourceText("BinaryFile_EndOfStreamError"));
		}
		catch (Exception e)
		{
			errorList.Add(TranslationManager.GetResourceText("BinaryFile_Error") + $" (\"{e.Message}\")");
		}

		Errors = string.Join(Environment.NewLine, errorList);
		HasErrors = Errors.Length > 0;
	}

	private void CreateByteList()
	{
		try
		{
			byte[] bytes = File.ReadAllBytes(binaryFile);
			string hexString = BitConverter.ToString(bytes);
			string[] hexValues = hexString.Split('-');

			HexDumpLines = new();
			int byteOffset = 0;
			HexDumpLine currentHexLine = null;
			foreach (string hexByte in hexValues)
			{
				if (byteOffset % 16 == 0)
				{
					currentHexLine = new(byteOffset);
					HexDumpLines.Add(currentHexLine);
				}

				currentHexLine.HexBytes.Add(new(hexByte, byteOffset));

				byteOffset++;
			}

			FileSizeText = $"{bytes.Length} B";
		}
		catch
		{
		}
	}

	private void ReadPart(BinaryPart templatePart, ObservableCollection<BinaryPart> partsList, Dictionary<long, object> globalValueCache,
		ref int byteOffset, List<string> errorList, BinaryReader reader)
	{
		if (templatePart is BinarySection templateSection)
		{
			ReadSection(templateSection, partsList, globalValueCache, ref byteOffset, errorList, reader);
		}
		else if (templatePart is BinaryValue templateValue)
		{
			ReadValue(templateValue, partsList, globalValueCache, ref byteOffset, errorList, reader);
		}
	}

	private void ReadSection(BinarySection templateSection, ObservableCollection<BinaryPart> partsList, Dictionary<long, object> globalValueCache,
		ref int byteOffset, List<string> errorList, BinaryReader reader)
	{
		long numberOfLoops = 1;
		LoopSettings loopSettings = templateSection.LoopSettings;
		if (loopSettings != null)
		{
			if (loopSettings.Type == LoopType.FIXED)
			{
				numberOfLoops = loopSettings.LoopCountFixed;
			}
			else if (loopSettings.Type == LoopType.REFERENCE)
			{
				if (globalValueCache.TryGetValue(loopSettings.LoopCountReference, out object loopCountReference))
				{
					numberOfLoops = Convert.ToInt64(loopCountReference);
				}
			}
		}

		for (int i = 0; i < numberOfLoops; i++)
		{
			string name = numberOfLoops > 1 ? $"{templateSection.Name} [{i}]" : templateSection.Name;
			BinarySection binarySection = new(0, name);

			foreach (BinaryPart templatePart in templateSection.Parts)
			{
				ReadPart(templatePart, binarySection.Parts, globalValueCache, ref byteOffset, errorList, reader);
			}

			if (binarySection.Parts.Any())
			{
				partsList.Add(binarySection);
			}
		}
	}

	private static void ReadValue(BinaryValue templateValue, ObservableCollection<BinaryPart> partsList, Dictionary<long, object> globalValueCache,
		ref int byteOffset, List<string> errorList, BinaryReader reader)
	{
		BinaryValue binaryValue = new(0, templateValue.Name, templateValue.ValueType, templateValue.Converter);
		binaryValue.ByteOffset = byteOffset;

		try
		{
			switch (templateValue.ValueType)
			{
				case BinaryValueType.BYTE:
					binaryValue.Value = reader.ReadByte();
					byteOffset += 1;
					break;

				case BinaryValueType.SHORT:
					binaryValue.Value = reader.ReadInt16();
					byteOffset += 2;
					break;

				case BinaryValueType.USHORT:
					binaryValue.Value = reader.ReadUInt16();
					byteOffset += 2;
					break;

				case BinaryValueType.INT:
					binaryValue.Value = reader.ReadInt32();
					byteOffset += 4;
					break;

				case BinaryValueType.UINT:
					binaryValue.Value = reader.ReadUInt32();
					byteOffset += 4;
					break;

				case BinaryValueType.LONG:
					binaryValue.Value = reader.ReadInt64();
					byteOffset += 8;
					break;

				case BinaryValueType.ULONG:
					binaryValue.Value = reader.ReadUInt64();
					byteOffset += 8;
					break;

				case BinaryValueType.BOOL:
					binaryValue.Value = reader.ReadBoolean();
					byteOffset += 1;
					break;

				case BinaryValueType.FLOAT:
					binaryValue.Value = reader.ReadSingle();
					byteOffset += 4;
					break;

				case BinaryValueType.DOUBLE:
					binaryValue.Value = reader.ReadDouble();
					byteOffset += 8;
					break;

				default:
					binaryValue = BinaryValue.CreateErrorValue(templateValue.Name, templateValue.ValueType);
					break;
			}
		}
		catch (Exception e)
		{
			binaryValue = BinaryValue.CreateErrorValue(templateValue.Name, templateValue.ValueType);
			errorList.Add(TranslationManager.GetResourceText("BinaryFile_ValueError") + $" \"{e.Message}\"s");
		}

		binaryValue.Length = BinaryValueTypeHelper.GetLength(binaryValue.ValueType);
		globalValueCache[templateValue.ID] = binaryValue.Value;
		partsList.Add(binaryValue);
	}

	#region events

	public delegate void HexDumpSelectionChangedEventHandler(object sender, HexDumpSelectionChangedEventArgs e);

	public event HexDumpSelectionChangedEventHandler HexDumpSelectionChanged;

	#endregion events
}