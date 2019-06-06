using System;

namespace BinaryDataReader.Lib
{
    /// <summary>
    /// Data class for loop settings of binary parts
    /// </summary>
    public class LoopSettings
    {
        /// <summary>
        /// Loop type
        /// </summary>
        public LoopType Type { get; set; }

        /// <summary>
        /// Fixed loop count
        /// </summary>
        public int LoopCountFixed { get; set; }

        /// <summary>
        /// ID of binary part containing loop count
        /// </summary>
        public long LoopCountReference { get; set; }
    }
}