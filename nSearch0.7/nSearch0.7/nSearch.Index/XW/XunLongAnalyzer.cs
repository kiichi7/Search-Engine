using System;
using System.IO;
using System.Collections;
using Lucene.Net.Analysis;
/*
      '       迅龙中文分类搜索引擎 v0.7  nSearch版 
      '
      '        LGPL  许可发行
      '
      '       宁夏大学  张冬   zd4004@163.com
      ' 
      '        官网 http://blog.163.com/zd4004/
 */

namespace Lucene.Net.Analysis.XunLongX
{

    /*
          '       迅龙中文分类搜索引擎  v0.6
          '
          '        LGPL  许可发行
          '
          '       宁夏大学  张冬   zd4004@163.com
          ' 
          '        官网 http://blog.163.com/zd4004/
     */


	public class XunLongAnalyzer : Analyzer 
	{
		//~ Static fields/initializers ---------------------------------------------

		/**
		  * An array containing some common English words that are not usually
		  * useful for searching. and some double-byte interpunctions.....
		  */
		private static String[] stopWords = 
		{
			"a", "and", "are", "as", "at", "be",
			"but", "by", "for", "if", "in",
			"into", "is", "it", "no", "not",
			"of", "on", "or", "s", "such", "t",
			"that", "the", "their", "then",
			"there", "these", "they", "this",
			"to", "was", "will", "with", "",
			"www"
		};

		//~ Instance fields --------------------------------------------------------

		/** stop word list */
		private Hashtable stopTable;

		//~ Constructors -----------------------------------------------------------

		/// <summary>
		/// Builds an analyzer which removes words in STOP_WORDS.
		/// </summary>
		public XunLongAnalyzer() 
		{
			stopTable = StopFilter.MakeStopSet(stopWords);

            ClassXunLongChinese.SetRun(111);
		}

		/// <summary>
		/// Builds an analyzer which removes words in the provided array.
		/// </summary>
		/// <param name="stopWords">stop word array</param>
		public XunLongAnalyzer(String[] stopWords) 
		{
			stopTable = StopFilter.MakeStopSet(stopWords);
		}

		//~ Methods ----------------------------------------------------------------

		/// <summary>
		/// get token stream from input
		/// </summary>
		/// <param name="fieldName">lucene field name</param>
		/// <param name="reader">input reader</param>
		/// <returns>Token Stream</returns>
		public override sealed TokenStream TokenStream(String fieldName, TextReader reader) 
		{
			return new StopFilter(new XunLongTokenizer(reader), stopTable);
		}
	}
}
