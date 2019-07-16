namespace BinaryDataReaderApp.Models
{
    /// <summary>
    /// Data class for loop settings of binary parts
    /// </summary>
    public class LoopSettings : ModelBase
    {
        private LoopType type;
        private int loopCountFixed;
        private long loopCountReference;

        /// <summary>
        /// Loop type
        /// </summary>
        public LoopType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Fixed loop count
        /// </summary>
        public int LoopCountFixed
        {
            get
            {
                return loopCountFixed;
            }
            set
            {
                loopCountFixed = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// ID of binary part containing loop count
        /// </summary>
        public long LoopCountReference
        {
            get
            {
                return loopCountReference;
            }
            set
            {
                loopCountReference = value;
                OnPropertyChanged();
            }
        }
    }
}