using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Collections;
/*
      '       迅龙中文分类搜索引擎 v0.7  nSearch版 
      '
      '        LGPL  许可发行
      '
      '       宁夏大学  张冬   zd4004@163.com
      ' 
      '        官网 http://blog.163.com/zd4004/
 */

namespace nSearch.FS
{
    public class CNewNxuEncoding
    {


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


    }

    
}
