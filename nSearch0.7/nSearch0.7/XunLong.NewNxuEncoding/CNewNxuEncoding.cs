
/*
      '       迅龙中文分类搜索引擎 v0.7  nSearch版 
      '
      '        LGPL  许可发行
      '
      '       宁夏大学  张冬   zd4004@163.com   
      ' 
      '        官网 http://blog.163.com/zd4004/
 */


using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Collections;


namespace NewNxuEncoding
{
    public class CNewNxuEncoding
    {

        public Hashtable myHashtable =new  Hashtable();


       public CNewNxuEncoding()
       {
         myHashtable = NewBaseDataList();
    
       }

        /// <summary>
        /// 读取文件编码为BASE64
        /// </summary>
        /// <param name="FilePathX">需要转换的文件路径</param>
        public String EncodeWithString(String FilePathX)
        {
            System.IO.FileStream inFile;
            byte[] binaryData;

            try
            {
                inFile = new System.IO.FileStream(FilePathX,
                                                  System.IO.FileMode.Open,
                                                  System.IO.FileAccess.Read);

                binaryData = new Byte[inFile.Length];

                long bytesRead = inFile.Read(binaryData, 0,
                                            (int)inFile.Length);
                inFile.Close();
            }
            catch (System.Exception exp)
            {
                // Error creating stream or reading from it.
                 nSearch.DebugShow.ClassDebugShow.WriteLineF( exp.Message.ToString());
                return "Error";
            }

            // Convert the binary input into Base64 UUEncoded output.
            string base64String;
            try
            {
                base64String =
                   System.Convert.ToBase64String(binaryData,
                                                 0,
                                                 binaryData.Length);
                return base64String;
            }
            catch (System.ArgumentNullException)
            {
                 nSearch.DebugShow.ClassDebugShow.WriteLineF("Binary data array is null.");
                return "Error";
            }

        }

        /// <summary>
        /// 把编码转换为BIN写入文件
        /// </summary>
        /// <param name="FilePathX">文件路径</param>
        /// <param name="base64String">需要转换后写入的数据</param>
        public void DecodeWithString(String FilePathX, string base64String)
        {
            //System.IO.StreamReader inFile;

            byte[] binaryData;
            try
            {
                binaryData =
                    System.Convert.FromBase64String(base64String);
            }
            catch (System.ArgumentNullException)
            {
                 nSearch.DebugShow.ClassDebugShow.WriteLineF("Base 64 string is null.");
                return;
            }
            catch (System.FormatException)
            {
                nSearch.DebugShow.ClassDebugShow.WriteLineF("Base 64 string length is not " +
                    "4 or is not an even multiple of 4.");
                return;
            }

            // Write out the decoded data.
            System.IO.FileStream outFile;
            try
            {
                outFile = new System.IO.FileStream(FilePathX,
                                                   System.IO.FileMode.Create,
                                                   System.IO.FileAccess.Write);
                outFile.Write(binaryData, 0, binaryData.Length);
                outFile.Close();
            }
            catch (System.Exception exp)
            {
                // Error creating stream or writing to it.
                 nSearch.DebugShow.ClassDebugShow.WriteLineF(  exp.Message.ToString());
            }
        }

        /// <summary>
        /// 压缩字符串 返回编码好的字符
        /// </summary>
        /// <param name="uncompressedString">字符串</param>
        public string Compress(string uncompressedString)
        {
            byte[] byteData = System.Text.Encoding.UTF8.GetBytes(uncompressedString);
            MemoryStream ms = new MemoryStream();
            //Stream s = new GZipOutputStream(ms);

            //compressedStream = newGZipStream(targetStream, CompressionMode.Compress, true);

            Stream s = new GZipStream(ms, CompressionMode.Compress, true);

            s.Write(byteData, 0, byteData.Length);
            s.Close();
            byte[] compressData = (byte[])ms.ToArray();
            ms.Flush();
            ms.Close();
            return System.Convert.ToBase64String(compressData, 0, compressData.Length);
        }


        /// <summary>
        /// 压缩字符串 二进制
        /// </summary>
        /// <param name="uncompressedString">字符串</param>
        public byte[] dbsCompress(string uncompressedString)
        {
            byte[] byteData = System.Text.Encoding.UTF8.GetBytes(uncompressedString);
            MemoryStream ms = new MemoryStream();
            //Stream s = new GZipOutputStream(ms);

            //compressedStream = newGZipStream(targetStream, CompressionMode.Compress, true);

            Stream s = new GZipStream(ms, CompressionMode.Compress, true);

            s.Write(byteData, 0, byteData.Length);
            s.Close();
            byte[] compressData = (byte[])ms.ToArray();
            ms.Flush();
            ms.Close();
            return compressData;
        }

        /// <summary>
        /// 解压二进制 到字符串
        /// </summary>
        /// <param name="compressedString">字符串</param>
        public string dbsDeCompress(byte[] byteInput)
        {
            try
            {
                //string uncompressedString=string.Empty; 
                StringBuilder sb = new StringBuilder(409600);
                int totalLength = 0;
                //   byte[] byteInput = System.Convert.FromBase64String(compressedString);
                byte[] writeData = new byte[409600];
                //Stream s = new GZipInputStream(new MemoryStream(byteInput));
                //decompressedStream=newGZipStream(sourceStream,CompressionMode.Decompress,true);

                Stream s = new GZipStream(new MemoryStream(byteInput), CompressionMode.Decompress, true);

                while (true)
                {
                    int size = s.Read(writeData, 0, writeData.Length);
                    if (size > 0)
                    {
                        totalLength += size;
                        sb.Append(System.Text.Encoding.UTF8.GetString(writeData, 0, size));
                    }
                    else
                    {
                        break;
                    }
                }
                s.Flush();
                s.Close();
                return sb.ToString();
            }
            catch
            {
                int u = 0;
                return "";
            }
        }



        /// <summary>
        /// 解压字符串 
        /// </summary>
        /// <param name="compressedString">字符串</param>
        public string DeCompress(string compressedString)
        {
            //string uncompressedString=string.Empty; 
            StringBuilder sb = new StringBuilder(409600);
            int totalLength = 0;
            byte[] byteInput = System.Convert.FromBase64String(compressedString);
            byte[] writeData = new byte[4096];
            //Stream s = new GZipInputStream(new MemoryStream(byteInput));
            //decompressedStream=newGZipStream(sourceStream,CompressionMode.Decompress,true);

            Stream s = new GZipStream(new MemoryStream(byteInput), CompressionMode.Decompress, true);

            while (true)
            {
                int size = s.Read(writeData, 0, writeData.Length);
                if (size > 0)
                {
                    totalLength += size;
                    sb.Append(System.Text.Encoding.UTF8.GetString(writeData, 0, size));
                }
                else
                {
                    break;
                }
            }
            s.Flush();
            s.Close();
            return sb.ToString();
        }

        /// <summary>
        ///   中文到BASE64 再 压缩
        /// </summary>
        /// <param name="CNCHINESEDATA">字符串</param>
        public string CN2BASE64ZIP(String CNCHINESEDATA)
        {
            //中文到BASE64
            String TmpM1 = CN2CODE(CNCHINESEDATA);

            //BASE64到ZIP
            string TmpM2 = Compress(TmpM1);

            return TmpM2;

        }

        /// <summary>
        ///数据先解压 然后到中文
        /// </summary>
        /// <param name="SOURCEDATA">字符串</param>
        public string UNCN2BASE64ZIP(String SOURCEDATA)
        {
            string TmpN1 = DeCompress(SOURCEDATA);

            string TmpN2 = CODE2CN(TmpN1);

            return TmpN2;


        }

        /// <summary>
        ///中文编码到BASE64// 1 先编码为GB2312 // 2 转换为byte // 3 编码为BASE64	
        /// </summary>
        /// <param name="DataX">字符串</param>
        public String CN2CODE(String DataX)
        {

            Encoding gbx = System.Text.Encoding.GetEncoding("gb2312");


            Byte[] dataSV = gbx.GetBytes(DataX);

            string base64String;

            base64String =
                   System.Convert.ToBase64String(dataSV,
                                                 0,
                                                 dataSV.Length);

            return base64String;


        }

        /// <summary>
        ///	BASE64到中文// 1 解码为byte // 2 转换为2312
        /// </summary>
        /// <param name="DataX">字符串</param>
        public String CODE2CN(String DataX)
        {
            try
            {
                //Encoding gb = System.Text.Encoding.GetEncoding("Ansi");

                byte[] binaryData;

                binaryData =
                            System.Convert.FromBase64String(DataX);

                Encoding gb = System.Text.Encoding.GetEncoding("gb2312");

                string base64String;

                base64String = gb.GetString(binaryData, 0, binaryData.Length);

                return base64String;
            }
            catch
            {
                return DataX;
            }

        }


        /// <summary>
        ///	New  中文 编码
        /// </summary>
        /// <param name="DataX">字符串</param>
        public String New_CN2CODE(String DataX)
        {

            Encoding gbx = System.Text.Encoding.GetEncoding("gb2312");


            Byte[] dataSV = gbx.GetBytes(DataX);

            string base64String;

            base64String =
                   System.Convert.ToBase64String(dataSV,
                                                 0,
                                                 dataSV.Length);

            return Base2NewBase(base64String);


        }

        /// <summary>
        ///	New 中文　解码
        /// </summary>
        /// <param name="DataX">字符串</param>
        public String New_CODE2CN(String DataX)
        {
            if (DataX.IndexOf("\\") != 0)  //不含有标志
            {
                return DataX;
            }

            string myDataX = NewBase2Base(DataX);


            try
            {
                //Encoding gb = System.Text.Encoding.GetEncoding("Ansi");

                byte[] binaryData;

                binaryData =
                            System.Convert.FromBase64String(myDataX);

                Encoding gb = System.Text.Encoding.GetEncoding("gb2312");

                string base64String;

                base64String = gb.GetString(binaryData, 0, binaryData.Length);

                return base64String;
            }
            catch
            {
                return DataX;
            }

        }

        
        /// <summary>
        ///	转义BASE64
        /// </summary>
        /// <param name="cdata">字符串</param> 
        public string Base2NewBase(string cdata)
        {
            string myTmpcdata = cdata;                             //存放原始变量
            Hashtable myHashtable = new Hashtable();
            myHashtable = NewBaseDataList();

            for (int i = 0; i < cdata.Length; i++)
            {
                foreach (DictionaryEntry deX in myHashtable) //ht为一个Hashtable实例
                {
                    if (myTmpcdata.IndexOf(deX.Key.ToString()) >= 0)
                    {
                        myTmpcdata = myTmpcdata.Replace(deX.Key.ToString(), deX.Value.ToString());
                    }

                }

            }

            return "\\" + myTmpcdata;  //前头加上标示
        }

        /// <summary>
        ///	还原转义
        /// </summary>
        /// <param name="cdata">字符串</param> 
        public string NewBase2Base(string cdata)
        {
            if (cdata.IndexOf("\\") != 0)  //不含有标志
            {
                return cdata;
            }

            string myTmpcdata = cdata.Substring(1, cdata.Length - 1);       //去掉前面的标示         //存放原始变量
      
            for (int i = 0; i < cdata.Length; i++)
            {
                foreach (DictionaryEntry deX in myHashtable) //ht为一个Hashtable实例
                {

                    if (myTmpcdata.IndexOf(deX.Value.ToString()) >= 0)
                    {
                        myTmpcdata = myTmpcdata.Replace(deX.Value.ToString(), deX.Key.ToString());
                    }
                }

            }
            return myTmpcdata;
        }

        //转义  还原  定义  
        private Hashtable NewBaseDataList()   //真时为 \\A - a   假 a - \\A
        {
            Hashtable NewBase64Predicates = new Hashtable();

            NewBase64Predicates.Add("a", "\\A");

            NewBase64Predicates.Add("b", "\\B");

            NewBase64Predicates.Add("c", "\\C");

            NewBase64Predicates.Add("d", "\\D");

            NewBase64Predicates.Add("e", "\\E");

            NewBase64Predicates.Add("f", "\\F");

            NewBase64Predicates.Add("g", "\\G");

            NewBase64Predicates.Add("h", "\\H");

            NewBase64Predicates.Add("i", "\\I");

            NewBase64Predicates.Add("j", "\\J");

            NewBase64Predicates.Add("k", "\\K");

            NewBase64Predicates.Add("l", "\\L");

            NewBase64Predicates.Add("m", "\\M");

            NewBase64Predicates.Add("n", "\\N");

            NewBase64Predicates.Add("o", "\\O");
            NewBase64Predicates.Add("p", "\\P");

            NewBase64Predicates.Add("q", "\\Q");

            NewBase64Predicates.Add("r", "\\R");

            NewBase64Predicates.Add("s", "\\S");

            NewBase64Predicates.Add("t", "\\T");

            NewBase64Predicates.Add("u", "\\U");

            NewBase64Predicates.Add("v", "\\V");

            NewBase64Predicates.Add("w", "\\W");

            NewBase64Predicates.Add("x", "\\X");

            NewBase64Predicates.Add("y", "\\Y");

            NewBase64Predicates.Add("z", "\\Z");

            return NewBase64Predicates;

        }

        /// <summary>
        ///	判断句子中是否含有中文
        /// </summary>
        /// <param name="words">字符串</param> 
        public bool WordsIScn(string words)
        {
            string TmmP;

            for (int i = 0; i < words.Length; i++)
            {
                TmmP = words.Substring(i, 1);

                byte[] sarr = System.Text.Encoding.GetEncoding("gb2312").GetBytes(TmmP);

                if (sarr.Length == 2)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///汉字和汉字序列编码
        /// </summary>
        /// <param name="GData">字符串</param> 
        public string enSelectCode2(string GData)
        {
            if (GData.IndexOf(' ') < 0)
            {
                return New_CN2CODE(GData);
            }

            string TTT = GData;
            string[] TTT2 = TTT.Split(' ');

            for (int i = 0; i < TTT2.Length; i++)
            {
                if (WordsIScn(TTT2[i]))
                {
                    TTT2[i] = New_CN2CODE(TTT2[i]);
                }
            }
            string TTT3 = "";
            for (int i = 0; i < TTT2.Length; i++)
            {
                TTT3 = TTT3 + TTT2[i] + " ";

            }
            return TTT3.Trim();
        }


        /// <summary>
        ///是压缩过的字符串   如果是　则返回解压结果　　如果不是　则返回原始值
        /// </summary>
        /// <param name="data">字符串</param> 
        private string WordsISZIP(string data)
        {
            //压缩队列一般具有特征　以/打头　且可以正确解压
            if (data.IndexOf("//") != 0) { return data; }

            string KCBB = New_CODE2CN(data);

            if (KCBB == data)   //如果没有正确解压　原样返回
            {
                return data;
            }

            return KCBB;   //否则 返回解压结果

        }

  
        /// <summary>
        /// 去掉多余的换行
        /// </summary>
        /// <param name="dataN">字符串</param>
        private string clearNN(string dataN)
        {
            string X = dataN;
            while (X.IndexOf("\r\n\r\n\r\n") > 0)
            {
                X = X.Replace("\r\n\r\n\r\n", "\r\n");
            }

            while (X.IndexOf("\r\n\r\n") > 0)
            {
                X = X.Replace("\r\n\r\n", "\r\n");
            }

            while (X.IndexOf("\n\n\n") > 0)
            {
                X = X.Replace("\n\n\n", "\n");
            }

            while (X.IndexOf("\n\n") > 0)
            {
                X = X.Replace("\n\n", "\n");
            }

            return X;

        }

        /// <summary>
        /// 编码2中文 错误时 补充==　
        /// </summary>
        /// <param name="DataX">字符串</param>
        public string TTCODE2CN(String DataX)
        {
            int n = 0;
            string mySource = DataX;
            string myDt="";
        TYR:
            n = n + 1;

               myDt = New_CODE2CN(DataX);
               if (myDt!=DataX )
               {
                 return myDt ;
               }

                if (n == 1)
                { DataX = DataX + "="; goto TYR; }

                if (n == 2)
                { DataX = DataX + "="; goto TYR; }

                return mySource;
          }
   

        /// <summary>
        /// 输入格式化： 把输入的汉字序列转变为 NewBSAE64 的 序列
        /// </summary>
        /// <param name="myData">字符串</param>
        public string aInputDataC(string myData)
        {
            //不含有队列时直接返回编码结果
            if (myData.IndexOf(' ') == -1) { return New_CN2CODE(myData); }

            //去除多余空格
            long L2 = 0;

            L2 = myData.Length;

            string[] myTmpData1 = myData.Split(' ');
            string myTmpData3 = "";

            for (int i = 0; i < myTmpData1.Length; i++)
            {
                if (myTmpData1[i].Length > 0)
                {
                    myTmpData1[i] = New_CN2CODE(myTmpData1[i]);
                    myTmpData3 = myTmpData3 + " " + myTmpData1[i];
                }
            }

            return myTmpData3.Trim();

        }

        /// <summary>
        /// 编码队列变换成汉字串
        /// </summary>
        /// <param name="DataY">字符串</param> 
        public string aOutputDataC(string DataY)
        {
            if (DataY.IndexOf(" ") == -1)
            {
                return New_CODE2CN(DataY);
            }
            string[] DataRE = DataY.Split(' ');
            string DATAVBAK = "";
            string TmpWEp = "";
            for (int i = 0; i < DataRE.Length; i++)
            {
                TmpWEp = New_CODE2CN(DataRE[i]);
                DATAVBAK = DATAVBAK + TmpWEp;
            }
            return DATAVBAK;
        }

        /// <summary>
        ///  编码队列变换成汉字串　可控压缩
        /// </summary>
        /// <param name="DataZ">字符串</param> 
        /// <param name="IsZip">是否是压缩标志</param> 
        public string aOutputDataCrtL(string DataZ, string IsZip)
        {
            if (IsZip == "TT")
            {
                string myTmpDE = "";
                if (DataZ.IndexOf(" ") >= 0)
                { myTmpDE = aOutputDataC(DataZ); }   //错误分词  含有空格 
                else
                { myTmpDE = New_CODE2CN(DataZ); }    //中文

                return myTmpDE;
            }
            else
            {
                return DataZ;
            }
        }

        /// <summary>
        /// 去掉0的中文到编码
        /// </summary>
        /// <param name="data">中文数据</param>
        /// <returns>编码数据</returns>
        public string ssCN2CODE(string data)
        {
            string myAkcc = CN2CODE(data);

            myAkcc = myAkcc.Replace("+","!");
            myAkcc = myAkcc.Replace("=", "?");

            return myAkcc;
        }

        /// <summary>
        /// 去掉0的编码到中文
        /// </summary>
        /// <param name="data">编码数据</param>
        /// <returns>中文数据</returns>
        public string ssCODE2CN(string data)
        {
            data = data.Replace( "!","+");
            data = data.Replace("?","=" );
            string myAkcc = CODE2CN(data);

            return myAkcc;

        }


        //"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/="
        /// <summary>
        /// 目录编码  把路径名称转换为BASE64
        /// </summary>
        /// <param name="DataX">字符串</param>
        public String DirCN2CODE(String DataX)
        {

            Encoding gbx = System.Text.Encoding.GetEncoding("gb2312");


            Byte[] dataSV = gbx.GetBytes(DataX);

            string base64String;

            base64String =
                   System.Convert.ToBase64String(dataSV,
                                                 0,
                                                 dataSV.Length);

            string tmp99 = base64String;

            tmp99 = tmp99.Replace('/','~');   //使之符合路径规范

            return tmp99;


        }

        /// <summary>
        ///	目录解码 把编码转换为路径名称 
        /// </summary>
        /// <param name="DataX">字符串</param>
        public String DirCODE2CN(String DataX)
        {

            DataX = DataX.Replace('~','/');   //使之符合路径规范

            try
            {
                //Encoding gb = System.Text.Encoding.GetEncoding("Ansi");

                byte[] binaryData;

                binaryData =
                            System.Convert.FromBase64String(DataX);

                Encoding gb = System.Text.Encoding.GetEncoding("gb2312");

                string base64String;

                base64String = gb.GetString(binaryData, 0, binaryData.Length);

                return base64String;
            }
            catch
            {
                return DataX;
            }

        }






    }




}
//MIME的编码方案于有许多种，Base64只是其中最常用的一种。Base64编码方案将要传送的信息转化为64个ASCII字符（'A'-'Z','a'-'z','0'-'9','+','/')组成的字符串。