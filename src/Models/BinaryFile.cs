using System;
using System.Collections.Generic;
using System.IO;

namespace BinaryDataReaderApp.Models
{
	public class BinaryFile : ModelBase
	{
		private List<BinaryPart> parts;

		public List<BinaryPart> Parts
		{
			get
			{
				return parts;
			}
			set
			{
				parts = value;
				OnPropertyChanged();
			}
		}

		public BinaryFile(string file, BinaryDataTemplate template)
		{
			Read(file, template);
		}

		private void Read(string file, BinaryDataTemplate template)
		{
			try
			{
				using (BinaryReader reader = new BinaryReader(File.Open(file, FileMode.Open)))
				{
					Dictionary<long, object> globalValueCache = new Dictionary<long, object>();

					foreach (BinaryPart templatePart in template.Parts)
					{
						if (templatePart is BinarySection binarySectionTemplate)
						{
							ReadSection(binarySectionTemplate, Parts, globalValueCache, reader);
						}
						else if (templatePart is BinaryValue binaryValueTemplate)
						{
							ReadValue(binaryValueTemplate, Parts, globalValueCache, reader);
						}
					}
				}
			}
			catch
			{

			}
		}

		private void ReadSection(BinarySection binarySectionTemplate, List<BinaryPart> parts, Dictionary<long, object> globalValueCache, BinaryReader reader)
		{

		}

		private void ReadValue(BinaryValue binaryValueTemplate, List<BinaryPart> parts, Dictionary<long, object> globalValueCache, BinaryReader reader)
		{
			BinaryValue binaryValue = new BinaryValue(0, binaryValueTemplate.Name, binaryValueTemplate.ValueType);

			switch (binaryValueTemplate.ValueType)
			{
				case BinaryValueType.BYTE:
					binaryValue.Value = reader.ReadByte();
					break;
				case BinaryValueType.SHORT:
					binaryValue.Value = reader.ReadInt16();
					break;
				case BinaryValueType.USHORT:
					binaryValue.Value = reader.ReadUInt16();
					break;
				case BinaryValueType.INT:
					binaryValue.Value = reader.ReadInt32();
					break;
				case BinaryValueType.UINT:
					binaryValue.Value = reader.ReadUInt32();
					break;
				case BinaryValueType.LONG:
					binaryValue.Value = reader.ReadInt64();
					break;
				case BinaryValueType.ULONG:
					binaryValue.Value = reader.ReadUInt64();
					break;
				default:
					binaryValue = null;
					break;
			}

			if (binaryValue != null)
			{
				globalValueCache[binaryValueTemplate.ID] = binaryValue.Value;
				parts.Add(binaryValue);
			}
		}
	}
}