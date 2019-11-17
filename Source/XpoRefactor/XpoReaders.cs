using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;

namespace XpoRefactor
{
	/// <summary>
	/// A class to read the contents of an xpo file.
	/// It will remove header information and find dependencies
	/// </summary> 
	
	public class XpoReader
	{
        public Encoding fileEncoding;
		private UTF8Encoding utf8Encoding = new UTF8Encoding(true, true); // turn on BOM and error checking
		private string text;
		private char[] bufferText;
		/// <summary>
		/// The contents of the file with header information removed
		/// </summary>
		//public char[] Text
		//{
		//	get
		//	{
		//		return bufferText;
		//	}
		//}
        public char[] TextAsCharArray()
        {
            return bufferText;
        }
        public string Text()
        {
            return text;
        }

		/// <summary>
		/// The Application Object type in AOT. 
		/// Similar to UtilElementType, just as a 3 character representation.
		/// </summary>
		
		private int FindStartIndexForDocNode(int position)
		{
            int ret = text.IndexOf("#KERNDOC:", position, StringComparison.Ordinal); //DOK
			if (ret == -1)
                ret = text.IndexOf("#APPLDOC:", position, StringComparison.Ordinal); //DOA
			if (ret == -1)
                ret = text.IndexOf("#ACODDOC:", position, StringComparison.Ordinal); //DAC
			return ret;
		}
		/// <summary>
		/// Constructor of the Xpo reader class
		/// </summary>
		/// <param name="file">Xpo file to read</param>
		public XpoReader(string file)
		{
			//Open file and read contents
			FileStream fsr = null;
			BinaryReader breader = null;
						
			try
			{
				fsr = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);	
				
				breader = new BinaryReader(fsr);
				byte[] binaryContent = new byte[fsr.Length];					
				breader.Read(binaryContent, 0, (int) fsr.Length);
                // having read the entire contents of the buffer into memory all at once, it's easy to check for character set.
                try
                {
                    if (fsr.Length > 3 &&
                       binaryContent[0] == 0xEF &&
                       binaryContent[1] == 0xBB &&
                       binaryContent[2] == 0xBF)
                    {
                        fileEncoding = utf8Encoding;
                        text = fileEncoding.GetString(binaryContent, 3, binaryContent.Length - 3);
                    }
                    else
                    {
                        fileEncoding = Encoding.GetEncoding(1252); // all existing XPO files should be in 1252. Danish ones will be a problem.
                        text = fileEncoding.GetString(binaryContent, 0, binaryContent.Length);
                    }
                }
                catch (Exception)
                {
                    text = "";
                }
				bufferText = text.ToCharArray();
			
			}			
			finally
			{
				if (breader != null)
					breader.Close();		
			}
		}
        public string GetPropertyValue(string propertyName)
        {
            return this.GetPropertyValue(propertyName, text);
        }
        /// <summary>
        /// Extract a value from a property
        /// </summary>
        /// <param name="textToAnalyze">Text to analyze</param>
        /// <param name="propertyName">Name of property</param>
        /// <returns>Property value</returns>
        public string GetPropertyValue(string propertyName, string text)
        {
            if (propertyName == null) throw new ArgumentNullException(propertyName);

            int start = text.IndexOf(propertyName, StringComparison.Ordinal);

            if (start == -1)
                return "";

            int startValue = text.IndexOf("#", start, StringComparison.Ordinal) + 1;

            if (startValue == -1)
                return "";

            int endValue = text.IndexOf(Environment.NewLine, startValue, StringComparison.Ordinal);

            if (endValue == -1)
                endValue = text.Length;
            return text.Substring(startValue, endValue - startValue).Trim();
        }
	}
}