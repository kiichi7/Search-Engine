using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.IO;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace SpiderLib
{
  public class DownloadFtp
    {
      FtpWebRequest ft;
      FtpWebResponse fr; 
      Stream stream;
      /// <summary>
      /// 连接FTP的方法
      /// </summary>
      /// <param name="ftpuri">ftp服务器地址，端口</param>
      /// <param name="ftpUserID">用户名</param>
      /// <param name="ftpPassword">密码</param>
      public DownloadFtp(string ftpuri, string ftpUserID, string ftpPassword) 
      {
          // 根据uri创建FtpWebRequest对象  
          ft = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpuri));
          // ftp用户名和密码  
          ft.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
          ft.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
         fr = (FtpWebResponse)ft.GetResponse();
          stream = fr.GetResponseStream();
          ////二进制文件读入
          //if (!fr.ContentType.ToLower().StartsWith("text/"))
          //{
          //    SaveBinaryFile(fr);
          //}
          ////文本文件
          //else
          //{
              string buffer = "", line;
              StreamReader reader = new StreamReader(stream);
              while ((line = reader.ReadLine()) != null)
              {
                  buffer += line + "\r\n";
              }

              //装入整个文件之后，接着就要把它保存为文本文件。
              SaveTextFile(buffer);
          //}
      }

      //二进制文件存储方法
      protected void SaveBinaryFile(FtpWebResponse response)
      {
          byte[] buffer = new byte[1024];
          string filename = convertFilename(response.ResponseUri);
          Stream outStream = File.Create(filename);
          Stream inStream = response.GetResponseStream();



          int l;
          do
          {
              l = inStream.Read(buffer, 0,
              buffer.Length);
              if (l > 0)
                  outStream.Write(buffer, 0, l);
          } while (l > 0);
          outStream.Close();
      }

      //文本文件存储方法
      protected void SaveTextFile(string buffer)
      {
          string filename = convertFilename(fr.ResponseUri);
          StreamWriter myWriter = new StreamWriter(filename, false, System.Text.Encoding.Default);
          myWriter.Write(buffer);
          myWriter.Close();
      }

      //二进制文件处理方法（确定二进制文件保存到本地的路径和名称）
      protected string convertFilename(Uri ResponseUri)
      {
          string filename = @"F:\\___毕业设计\\IM Spider\\IM Spider\\" + System.IO.Path.GetFileName(ResponseUri.ToString());
          return filename;
      }

    }
}
