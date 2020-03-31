using System;
using BinaryDataReaderApp.Models;

namespace BinaryDataReaderApp.Events
{
	public class TemplatePartDetailsWindowEventArgs : EventArgs
	{
		//----------------
		// input
		// ---------------

		public BinaryPart Part { get; set; }

		//----------------
		// output
		// ---------------

		public bool DialogResult { get; set; }

		public string PartName { get; set; }

		public BinaryValueType ValueType { get; set; }

		public LoopSettings LoopSettings { get; set; }
	}
}