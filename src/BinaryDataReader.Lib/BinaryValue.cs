namespace BinaryDataReader.Lib
{
    /// <summary>
    /// Data class representing a value of type T in a binary data template 
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    public class BinaryValue<T> : BinaryPart where T : struct
    {
        /// <summary>
        /// Creates new instance of BinaryValue with ID and name
        /// </summary>
        /// <param name="id">ID of this value</param>
        /// <param name="name">Name of this value</param>
        public BinaryValue(long id, string name)
            : base(id, name)
        {
        }
    }
}