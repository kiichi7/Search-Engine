using System;
using System.IO;
using System.Text;
using Lucene.Net.Analysis;

namespace Lucene.Net.Analysis.XunLongX
{


    /*
          '       迅龙中文分类搜索引擎 v0.7  nSearch版 
          '
          '        LGPL  许可发行
          '
          '       宁夏大学  张冬   zd4004@163.com
          ' 
          '        官网 http://blog.163.com/zd4004/
     */

	public sealed class XunLongTokenizer : Tokenizer 
	{
		  
        /// <summary>
        /// 缓存分词结果
        /// </summary>
        private XunLongCNST[] Wordx0 = new XunLongCNST[1];



        private int WordxLen = 0;
        private int pWordx = 0;


		/** Max word length */
		private static int MAX_WORD_LEN = 255;

		/** buffer size: */
		private static int IO_BUFFER_SIZE = 256;

		//~ Instance fields --------------------------------------------------------

		/** word offset, used to imply which character(in ) is parsed */
		private int offset = 0;

		/** the index used only for ioBuffer */
		private int bufferIndex = 0;

		/** data length */
		private int dataLen = 0;

		/**
		 * character buffer, store the characters which are used to compose <br>
		 * the returned Token
		 */
		private char[] buffer = new char[MAX_WORD_LEN];

		/**
		 * I/O buffer, used to store the content of the input(one of the <br>
		 * members of Tokenizer)
		 */
		private char[] ioBuffer = new char[IO_BUFFER_SIZE];

		/** word type: single=>ASCII  double=>non-ASCII word=>default */
		private String tokenType = "word";

		/**
		 * tag: previous character is a cached double-byte character  "C1C2C3C4"
		 * ----(set the C1 isTokened) C1C2 "C2C3C4" ----(set the C2 isTokened)
		 * C1C2 C2C3 "C3C4" ----(set the C3 isTokened) "C1C2 C2C3 C3C4"
		 */
		private bool preIsTokened = false;

		//~ Constructors -----------------------------------------------------------

		/// <summary>
		/// Construct a token stream processing the given input.
		/// </summary>
		/// <param name="_in">I/O reader</param>
		public XunLongTokenizer(TextReader _in) 
		{
			input = _in;

            //中文分词               
            XunLongCNST[] cnx = ClassXunLongChinese.ChineseIntface(_in);

            if (cnx == null)
            {

            }
            else
            {

                WordxLen = cnx.Length;

                Wordx0 =  new XunLongCNST[WordxLen];

               // if (WordxLen > 180000)
                //{
               //     WordxLen = 180000;
               // }

                for (int i = 0; i < WordxLen; i++)
                {
                    Wordx0[i].cWord = cnx[i].cWord;
                    Wordx0[i].cType = cnx[i].cType;
                    Wordx0[i].cStart = cnx[i].cStart;
                    Wordx0[i].cLength = cnx[i].cLength;
                }
            }

		}

		//~ Methods ----------------------------------------------------------------

		/// <summary>
		///  Returns the next token in the stream, or null at EOS.
		/// </summary>
		/// <returns>Token</returns>
		public override Token Next()
		{
			/** how many character(s) has been stored in buffer */
			int length = 0;

			/** the position used to create Token */
			int start = offset;


        RETX:
            if (pWordx < WordxLen)
            {
                //System.out.println(new String(buffer, 0, length));
                // string iu = new String(buffer, 0, length);

                XunLongCNST iu = Wordx0[pWordx];

                pWordx++;

                //过滤掉 无效的消息  保留分词
                if ((iu.cWord!=null)&(ClassXunLongChinese.ChineseFilterIt(iu) == false))
                {
                    return new Token(iu.cWord, iu.cStart,iu.cStart+ iu.cLength, tokenType);
                }

                goto RETX;
            }
            else
            {
                return null;
            }

			// return new Token(new String(buffer, 0, length), start, start + length,tokenType);
		}
	}

}
