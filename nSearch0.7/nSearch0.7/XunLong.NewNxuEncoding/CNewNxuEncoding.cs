
/*
      '       Ѹ�����ķ����������� v0.7  nSearch�� 
      '
      '        LGPL  ��ɷ���
      '
      '       ���Ĵ�ѧ  �Ŷ�   zd4004@163.com   
      ' 
      '        ���� http://blog.163.com/zd4004/
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
        /// ��ȡ�ļ�����ΪBASE64
        /// </summary>
        /// <param name="FilePathX">��Ҫת�����ļ�·��</param>
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
        /// �ѱ���ת��ΪBINд���ļ�
        /// </summary>
        /// <param name="FilePathX">�ļ�·��</param>
        /// <param name="base64String">��Ҫת����д�������</param>
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
        /// ѹ���ַ��� ���ر���õ��ַ�
        /// </summary>
        /// <param name="uncompressedString">�ַ���</param>
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
        /// ѹ���ַ��� ������
        /// </summary>
        /// <param name="uncompressedString">�ַ���</param>
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
        /// ��ѹ������ ���ַ���
        /// </summary>
        /// <param name="compressedString">�ַ���</param>
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
        /// ��ѹ�ַ��� 
        /// </summary>
        /// <param name="compressedString">�ַ���</param>
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
        ///   ���ĵ�BASE64 �� ѹ��
        /// </summary>
        /// <param name="CNCHINESEDATA">�ַ���</param>
        public string CN2BASE64ZIP(String CNCHINESEDATA)
        {
            //���ĵ�BASE64
            String TmpM1 = CN2CODE(CNCHINESEDATA);

            //BASE64��ZIP
            string TmpM2 = Compress(TmpM1);

            return TmpM2;

        }

        /// <summary>
        ///�����Ƚ�ѹ Ȼ������
        /// </summary>
        /// <param name="SOURCEDATA">�ַ���</param>
        public string UNCN2BASE64ZIP(String SOURCEDATA)
        {
            string TmpN1 = DeCompress(SOURCEDATA);

            string TmpN2 = CODE2CN(TmpN1);

            return TmpN2;


        }

        /// <summary>
        ///���ı��뵽BASE64// 1 �ȱ���ΪGB2312 // 2 ת��Ϊbyte // 3 ����ΪBASE64	
        /// </summary>
        /// <param name="DataX">�ַ���</param>
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
        ///	BASE64������// 1 ����Ϊbyte // 2 ת��Ϊ2312
        /// </summary>
        /// <param name="DataX">�ַ���</param>
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
        ///	New  ���� ����
        /// </summary>
        /// <param name="DataX">�ַ���</param>
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
        ///	New ���ġ�����
        /// </summary>
        /// <param name="DataX">�ַ���</param>
        public String New_CODE2CN(String DataX)
        {
            if (DataX.IndexOf("\\") != 0)  //�����б�־
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
        ///	ת��BASE64
        /// </summary>
        /// <param name="cdata">�ַ���</param> 
        public string Base2NewBase(string cdata)
        {
            string myTmpcdata = cdata;                             //���ԭʼ����
            Hashtable myHashtable = new Hashtable();
            myHashtable = NewBaseDataList();

            for (int i = 0; i < cdata.Length; i++)
            {
                foreach (DictionaryEntry deX in myHashtable) //htΪһ��Hashtableʵ��
                {
                    if (myTmpcdata.IndexOf(deX.Key.ToString()) >= 0)
                    {
                        myTmpcdata = myTmpcdata.Replace(deX.Key.ToString(), deX.Value.ToString());
                    }

                }

            }

            return "\\" + myTmpcdata;  //ǰͷ���ϱ�ʾ
        }

        /// <summary>
        ///	��ԭת��
        /// </summary>
        /// <param name="cdata">�ַ���</param> 
        public string NewBase2Base(string cdata)
        {
            if (cdata.IndexOf("\\") != 0)  //�����б�־
            {
                return cdata;
            }

            string myTmpcdata = cdata.Substring(1, cdata.Length - 1);       //ȥ��ǰ��ı�ʾ         //���ԭʼ����
      
            for (int i = 0; i < cdata.Length; i++)
            {
                foreach (DictionaryEntry deX in myHashtable) //htΪһ��Hashtableʵ��
                {

                    if (myTmpcdata.IndexOf(deX.Value.ToString()) >= 0)
                    {
                        myTmpcdata = myTmpcdata.Replace(deX.Value.ToString(), deX.Key.ToString());
                    }
                }

            }
            return myTmpcdata;
        }

        //ת��  ��ԭ  ����  
        private Hashtable NewBaseDataList()   //��ʱΪ \\A - a   �� a - \\A
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
        ///	�жϾ������Ƿ�������
        /// </summary>
        /// <param name="words">�ַ���</param> 
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
        ///���ֺͺ������б���
        /// </summary>
        /// <param name="GData">�ַ���</param> 
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
        ///��ѹ�������ַ���   ����ǡ��򷵻ؽ�ѹ�������������ǡ��򷵻�ԭʼֵ
        /// </summary>
        /// <param name="data">�ַ���</param> 
        private string WordsISZIP(string data)
        {
            //ѹ������һ�������������/��ͷ���ҿ�����ȷ��ѹ
            if (data.IndexOf("//") != 0) { return data; }

            string KCBB = New_CODE2CN(data);

            if (KCBB == data)   //���û����ȷ��ѹ��ԭ������
            {
                return data;
            }

            return KCBB;   //���� ���ؽ�ѹ���

        }

  
        /// <summary>
        /// ȥ������Ļ���
        /// </summary>
        /// <param name="dataN">�ַ���</param>
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
        /// ����2���� ����ʱ ����==��
        /// </summary>
        /// <param name="DataX">�ַ���</param>
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
        /// �����ʽ���� ������ĺ�������ת��Ϊ NewBSAE64 �� ����
        /// </summary>
        /// <param name="myData">�ַ���</param>
        public string aInputDataC(string myData)
        {
            //�����ж���ʱֱ�ӷ��ر�����
            if (myData.IndexOf(' ') == -1) { return New_CN2CODE(myData); }

            //ȥ������ո�
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
        /// ������б任�ɺ��ִ�
        /// </summary>
        /// <param name="DataY">�ַ���</param> 
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
        ///  ������б任�ɺ��ִ����ɿ�ѹ��
        /// </summary>
        /// <param name="DataZ">�ַ���</param> 
        /// <param name="IsZip">�Ƿ���ѹ����־</param> 
        public string aOutputDataCrtL(string DataZ, string IsZip)
        {
            if (IsZip == "TT")
            {
                string myTmpDE = "";
                if (DataZ.IndexOf(" ") >= 0)
                { myTmpDE = aOutputDataC(DataZ); }   //����ִ�  ���пո� 
                else
                { myTmpDE = New_CODE2CN(DataZ); }    //����

                return myTmpDE;
            }
            else
            {
                return DataZ;
            }
        }

        /// <summary>
        /// ȥ��0�����ĵ�����
        /// </summary>
        /// <param name="data">��������</param>
        /// <returns>��������</returns>
        public string ssCN2CODE(string data)
        {
            string myAkcc = CN2CODE(data);

            myAkcc = myAkcc.Replace("+","!");
            myAkcc = myAkcc.Replace("=", "?");

            return myAkcc;
        }

        /// <summary>
        /// ȥ��0�ı��뵽����
        /// </summary>
        /// <param name="data">��������</param>
        /// <returns>��������</returns>
        public string ssCODE2CN(string data)
        {
            data = data.Replace( "!","+");
            data = data.Replace("?","=" );
            string myAkcc = CODE2CN(data);

            return myAkcc;

        }


        //"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/="
        /// <summary>
        /// Ŀ¼����  ��·������ת��ΪBASE64
        /// </summary>
        /// <param name="DataX">�ַ���</param>
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

            tmp99 = tmp99.Replace('/','~');   //ʹ֮����·���淶

            return tmp99;


        }

        /// <summary>
        ///	Ŀ¼���� �ѱ���ת��Ϊ·������ 
        /// </summary>
        /// <param name="DataX">�ַ���</param>
        public String DirCODE2CN(String DataX)
        {

            DataX = DataX.Replace('~','/');   //ʹ֮����·���淶

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
//MIME�ı��뷽����������֣�Base64ֻ��������õ�һ�֡�Base64���뷽����Ҫ���͵���Ϣת��Ϊ64��ASCII�ַ���'A'-'Z','a'-'z','0'-'9','+','/')��ɵ��ַ�����