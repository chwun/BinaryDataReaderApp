using System;
using System.Collections.Generic;
using System.IO;

namespace BinaryDataReader.Lib
{
    /// <summary>
    /// Abstract base class for a part in a binary data template
    /// </summary>
    public abstract class BinaryPart
    {
        /// <summary>
        /// ID of this part
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Name of this part
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Loop settings for this part
        /// </summary>
        public LoopSettings LoopSettings { get; set; }

        /// <summary>
        /// Creates new instance of BinaryPart with ID and name 
        /// </summary>
        /// <param name="id">ID of this part</param>
        /// <param name="name">Name of this part</param>
        public BinaryPart(long id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
