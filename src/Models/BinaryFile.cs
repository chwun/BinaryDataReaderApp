using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace BinaryDataReaderApp.Models
{
    public class BinaryFile : ModelBase
    {
        private string binaryFile;
        private BinaryDataTemplate template;

        private ObservableCollection<BinaryPart> parts;

        public ObservableCollection<BinaryPart> Parts
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

        public BinaryFile(string binaryFile, BinaryDataTemplate template)
        {
            this.binaryFile = binaryFile;
            this.template = template;
            Parts = new ObservableCollection<BinaryPart>();
        }

        public void Read()
        {
            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open(binaryFile, FileMode.Open)))
                {
                    Dictionary<long, object> globalValueCache = new Dictionary<long, object>();

                    foreach (BinaryPart templatePart in template.Parts)
                    {
                        ReadPart(templatePart, Parts, globalValueCache, reader);
                    }
                }
            }
            catch
            {

            }
        }

        private void ReadPart(BinaryPart templatePart, ObservableCollection<BinaryPart> partsList, Dictionary<long, object> globalValueCache, BinaryReader reader)
        {
            if (templatePart is BinarySection templateSection)
            {
                ReadSection(templateSection, partsList, globalValueCache, reader);
            }
            else if (templatePart is BinaryValue templateValue)
            {
                ReadValue(templateValue, partsList, globalValueCache, reader);
            }
        }

        private void ReadSection(BinarySection templateSection, ObservableCollection<BinaryPart> partsList, Dictionary<long, object> globalValueCache, BinaryReader reader)
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
                    if (globalValueCache.ContainsKey(loopSettings.LoopCountReference))
                    {
                        numberOfLoops = Convert.ToInt64(globalValueCache[loopSettings.LoopCountReference]);
                    }
                }
            }

            for (int i = 0; i < numberOfLoops; i++)
            {
				string name = (numberOfLoops > 1) ? $"{templateSection.Name}[{i}]" : templateSection.Name;
                BinarySection binarySection = new BinarySection(0, name);

                foreach (BinaryPart templatePart in templateSection.Parts)
                {
                    ReadPart(templatePart, binarySection.Parts, globalValueCache, reader);
                }

                if (binarySection.Parts.Any())
                {
                    partsList.Add(binarySection);
                }
            }
        }

        private void ReadValue(BinaryValue templateValue, ObservableCollection<BinaryPart> partsList, Dictionary<long, object> globalValueCache, BinaryReader reader)
        {
            BinaryValue binaryValue = new BinaryValue(0, templateValue.Name, templateValue.ValueType);

            switch (templateValue.ValueType)
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
                case BinaryValueType.BOOL:
                    binaryValue.Value = reader.ReadBoolean();
                    break;
                default:
                    binaryValue = null;
                    break;
            }

            if (binaryValue != null)
            {
                globalValueCache[templateValue.ID] = binaryValue.Value;
                partsList.Add(binaryValue);
            }
        }
    }
}