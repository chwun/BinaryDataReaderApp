using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using BinaryDataReaderApp.Events;
using BinaryDataReaderApp.Localization;

namespace BinaryDataReaderApp.Models
{
    public class BinaryFile : ModelBase
    {
        private string binaryFile;
        private BinaryDataTemplate template;
        private List<HexDumpLine> hexDumpLines;
        private ObservableCollection<BinaryPart> parts;
        private string errors;
        private bool hasErrors;
        private string fileSizeText;

        public List<HexDumpLine> HexDumpLines
        {
            get
            {
                return hexDumpLines;
            }
            private set
            {
                hexDumpLines = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<BinaryPart> Parts
        {
            get
            {
                return parts;
            }
            private set
            {
                parts = value;
                OnPropertyChanged();
            }
        }

        public string Errors
        {
            get
            {
                return errors;
            }
            private set
            {
                errors = value;
                OnPropertyChanged();
            }
        }

        public bool HasErrors
        {
            get
            {
                return hasErrors;
            }
            private set
            {
                hasErrors = value;
                OnPropertyChanged();
            }
        }

        public string FileSizeText
        {
            get
            {
                return fileSizeText;
            }
            private set
            {
                fileSizeText = value;
                OnPropertyChanged();
            }
        }

        public BinaryDataTemplate Template { get => template; private set => template = value; }

        #region events

        public delegate void HexDumpSelectionChangedEventHandler(object sender, HexDumpSelectionChangedEventArgs e);

        public event HexDumpSelectionChangedEventHandler HexDumpSelectionChanged;

        #endregion

        public BinaryFile(string binaryFile, BinaryDataTemplate template)
        {
            this.binaryFile = binaryFile;
            this.Template = template;
            Parts = new ObservableCollection<BinaryPart>();
        }

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
                if ((hexByte.ByteOffset >= value.ByteOffset) && (hexByte.ByteOffset < value.ByteOffset + value.Length))
                {
                    hexByte.IsSelected = true;
                }
                else
                {
                    hexByte.IsSelected = false;
                }
            }

            // raise event:
            HexDumpLine selectedHexDumpLine = HexDumpLines.Where(x => x.HexBytes.Any(b => b.IsSelected)).FirstOrDefault();
            HexDumpSelectionChanged?.Invoke(
                this,
                new HexDumpSelectionChangedEventArgs()
                {
                    SelectedHexDumpLine = selectedHexDumpLine
                });
        }

        public BinaryValue FindValueByByteOffset(int byteOffset)
        {
            return (FindPart(Parts, x => (x is BinaryValue val) && (byteOffset >= val.ByteOffset) && (byteOffset < val.ByteOffset + val.Length)) as BinaryValue);
        }

        private BinaryPart FindPart(IEnumerable<BinaryPart> parts, Func<BinaryPart, bool> condition)
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
                    var match = FindPart(section.Parts, condition);
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
            List<string> errorList = new List<string>();

            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open(binaryFile, FileMode.Open)))
                {
                    Dictionary<long, object> globalValueCache = new Dictionary<long, object>();
                    int byteOffset = 0;

                    foreach (BinaryPart templatePart in Template.Parts)
                    {
                        ReadPart(templatePart, Parts, globalValueCache, ref byteOffset, errorList, reader);
                    }

                    if (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        errorList.Add(TranslationManager.Instance.GetResourceText("BinaryFile_FileTooLongError"));
                    }
                }
            }
            catch (EndOfStreamException)
            {
                errorList.Add(TranslationManager.Instance.GetResourceText("BinaryFile_EndOfStreamError"));
            }
            catch (Exception e)
            {
                errorList.Add(TranslationManager.Instance.GetResourceText("BinaryFile_Error") + $" (\"{e.Message}\")");
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

                HexDumpLines = new List<HexDumpLine>();
                int byteOffset = 0;
                HexDumpLine currentHexLine = null;
                foreach (string hexByte in hexValues)
                {
                    if (byteOffset % 16 == 0)
                    {
                        currentHexLine = new HexDumpLine(byteOffset);
                        HexDumpLines.Add(currentHexLine);
                    }

                    currentHexLine.HexBytes.Add(new HexDumpByte(hexByte, byteOffset));

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
                    if (globalValueCache.ContainsKey(loopSettings.LoopCountReference))
                    {
                        numberOfLoops = Convert.ToInt64(globalValueCache[loopSettings.LoopCountReference]);
                    }
                }
            }

            for (int i = 0; i < numberOfLoops; i++)
            {
                string name = (numberOfLoops > 1) ? $"{templateSection.Name} [{i}]" : templateSection.Name;
                BinarySection binarySection = new BinarySection(0, name);

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

        private void ReadValue(BinaryValue templateValue, ObservableCollection<BinaryPart> partsList, Dictionary<long, object> globalValueCache,
            ref int byteOffset, List<string> errorList, BinaryReader reader)
        {
            BinaryValue binaryValue = new BinaryValue(0, templateValue.Name, templateValue.ValueType, templateValue.Converter);
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
                errorList.Add(TranslationManager.Instance.GetResourceText("BinaryFile_ValueError") + $" \"{e.Message}\"s");
            }

            binaryValue.Length = BinaryValueTypeHelper.GetLength(binaryValue.ValueType);
            globalValueCache[templateValue.ID] = binaryValue.Value;
            partsList.Add(binaryValue);
        }
    }
}