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
    public class DownloadHtml
    {
        HttpWebResponse response; 
        Stream stream;
        public DownloadHtml( string m_uri)
        {
             HttpWebRequest request = (HttpWebRequest)WebRequest.Create(m_uri); 
             response =(HttpWebResponse)request.GetResponse(); 
             stream = response.GetResponseStream();
            
            //二进制文件读入
            if (!response.ContentType.ToLower().StartsWith("text/"))
            {
                SaveBinaryFile(response);
            }
            //文本文件
            else
            {           
                string buffer = "", line;
                StreamReader reader = new StreamReader(stream,System.Text.Encoding.Default); 
                while( (line = reader.ReadLine())!=null ) 
                { 
                buffer+=line+"\r\n"; 
                } 

            //装入整个文件之后，接着就要把它保存。
                SaveTextFile(buffer); 
            }


        }
        
        //二进制文件存储方法
        protected void SaveBinaryFile(HttpWebResponse response)
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
            string filename = convertFilename(response.ResponseUri);
            StreamWriter myWriter = new StreamWriter(filename, false, System.Text.Encoding.Default);
            myWriter.Write(buffer);
            myWriter.Close();   
        }

        //二进制文件处理方法（确定二进制文件保存到本地的路径和名称）
        protected string convertFilename(Uri ResponseUri)
        {
          string filename = HttpContext.Current.Request.MapPath("../") + "\\1.5\\" + System.IO.Path.GetFileName(ResponseUri.ToString());
      
            return filename;
        }
    }
  //  二、处理HTML页面 
  //  下面来看看如何处理HTML页面。首先要做的当然是下载HTML页面，这可以通过C#提供的HttpWebRequest类实现：    
  //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(m_uri); 
  //response = request.GetResponse(); 
  //stream = response.GetResponseStream();    
   
  //  接下来我们就从request创建一个stream流。在执行其他处理之前，我们要先确定该文件是二进制文件还是文本文件，不同的文件类型处理方式也不同。下面的代码确定该文件是否为二进制文件。  
  //if( !response.ContentType.ToLower().StartsWith("text/") ) 
  //{ 
  //SaveBinaryFile(response); 
  //return null; 
  //} 
  //string buffer = "",line; 
   
  //  如果该文件不是文本文件，我们将它作为二进制文件读入。如果是文本文件，首先从stream创建一个StreamReader，然后将文本文件的内容一行一行加入缓冲区。 
   
  //reader = new StreamReader(stream); 
  //while( (line = reader.ReadLine())!=null ) 
  //{ 
  //buffer+=line+"\r\n"; 
  //} 
   
  //  装入整个文件之后，接着就要把它保存为文本文件。 
   
  //SaveTextFile(buffer); 
   
  //  下面来看看这两类不同文件的存储方式。 
   
  //  二进制文件的内容类型声明不以"text/"开头，蜘蛛程序直接把二进制文件保存到磁盘，不必进行额外的处理，这是因为二进制文件不包含HTML，因此也不会再有需要蜘蛛程序处理的HTML链接。下面是写入二进制文件的步骤。 
   
  //  首先准备一个缓冲区临时地保存二进制文件的内容。 byte []buffer = new byte[1024]; 
  //  接下来要确定文件保存到本地的路径和名称。如果要把一个myhost.com网站的内容下载到本地的c:\test文件夹，二进制文件的网上路径和名称是http://myhost.com/images/logo.gif，则本地路径和名称应当是c:\test\images\logo.gif。与此同时，我们还要确保c:\test目录下已经创建了images子目录。这部分任务由convertFilename方法完成。 
 
  //string filename = convertFilename( response.ResponseUri );   
  //  convertFilename方法分离HTTP地址，创建相应的目录结构。确定了输出文件的名字和路径之后就可以打开读取Web页面的输入流、写入本地文件的输出流。 
   
  //Stream outStream = File.Create( filename ); 
  //Stream inStream = response.GetResponseStream(); 
   
  //  接下来就可以读取Web文件的内容并写入到本地文件，这可以通过一个循环方便地完成。 
   
  //int l; 
  //do 
  //{ 
  //l = inStream.Read(buffer,0, 
  //buffer.Length); 
  //if(l>0) 
  //outStream.Write(buffer,0,l); 
  //} while(l>0); 
}
