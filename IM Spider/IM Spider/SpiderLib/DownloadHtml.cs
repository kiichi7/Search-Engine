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
            
            //�������ļ�����
            if (!response.ContentType.ToLower().StartsWith("text/"))
            {
                SaveBinaryFile(response);
            }
            //�ı��ļ�
            else
            {           
                string buffer = "", line;
                StreamReader reader = new StreamReader(stream,System.Text.Encoding.Default); 
                while( (line = reader.ReadLine())!=null ) 
                { 
                buffer+=line+"\r\n"; 
                } 

            //װ�������ļ�֮�󣬽��ž�Ҫ�������档
                SaveTextFile(buffer); 
            }


        }
        
        //�������ļ��洢����
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
        
        //�ı��ļ��洢����
        protected void SaveTextFile(string buffer)
        {
            string filename = convertFilename(response.ResponseUri);
            StreamWriter myWriter = new StreamWriter(filename, false, System.Text.Encoding.Default);
            myWriter.Write(buffer);
            myWriter.Close();   
        }

        //�������ļ���������ȷ���������ļ����浽���ص�·�������ƣ�
        protected string convertFilename(Uri ResponseUri)
        {
          string filename = HttpContext.Current.Request.MapPath("../") + "\\1.5\\" + System.IO.Path.GetFileName(ResponseUri.ToString());
      
            return filename;
        }
    }
  //  ��������HTMLҳ�� 
  //  ������������δ���HTMLҳ�档����Ҫ���ĵ�Ȼ������HTMLҳ�棬�����ͨ��C#�ṩ��HttpWebRequest��ʵ�֣�    
  //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(m_uri); 
  //response = request.GetResponse(); 
  //stream = response.GetResponseStream();    
   
  //  ���������Ǿʹ�request����һ��stream������ִ����������֮ǰ������Ҫ��ȷ�����ļ��Ƕ������ļ������ı��ļ�����ͬ���ļ����ʹ���ʽҲ��ͬ������Ĵ���ȷ�����ļ��Ƿ�Ϊ�������ļ���  
  //if( !response.ContentType.ToLower().StartsWith("text/") ) 
  //{ 
  //SaveBinaryFile(response); 
  //return null; 
  //} 
  //string buffer = "",line; 
   
  //  ������ļ������ı��ļ������ǽ�����Ϊ�������ļ����롣������ı��ļ������ȴ�stream����һ��StreamReader��Ȼ���ı��ļ�������һ��һ�м��뻺������ 
   
  //reader = new StreamReader(stream); 
  //while( (line = reader.ReadLine())!=null ) 
  //{ 
  //buffer+=line+"\r\n"; 
  //} 
   
  //  װ�������ļ�֮�󣬽��ž�Ҫ��������Ϊ�ı��ļ��� 
   
  //SaveTextFile(buffer); 
   
  //  ���������������಻ͬ�ļ��Ĵ洢��ʽ�� 
   
  //  �������ļ�������������������"text/"��ͷ��֩�����ֱ�ӰѶ������ļ����浽���̣����ؽ��ж���Ĵ���������Ϊ�������ļ�������HTML�����Ҳ����������Ҫ֩��������HTML���ӡ�������д��������ļ��Ĳ��衣 
   
  //  ����׼��һ����������ʱ�ر���������ļ������ݡ� byte []buffer = new byte[1024]; 
  //  ������Ҫȷ���ļ����浽���ص�·�������ơ����Ҫ��һ��myhost.com��վ���������ص����ص�c:\test�ļ��У��������ļ�������·����������http://myhost.com/images/logo.gif���򱾵�·��������Ӧ����c:\test\images\logo.gif�����ͬʱ�����ǻ�Ҫȷ��c:\testĿ¼���Ѿ�������images��Ŀ¼���ⲿ��������convertFilename������ɡ� 
 
  //string filename = convertFilename( response.ResponseUri );   
  //  convertFilename��������HTTP��ַ��������Ӧ��Ŀ¼�ṹ��ȷ��������ļ������ֺ�·��֮��Ϳ��Դ򿪶�ȡWebҳ�����������д�뱾���ļ���������� 
   
  //Stream outStream = File.Create( filename ); 
  //Stream inStream = response.GetResponseStream(); 
   
  //  �������Ϳ��Զ�ȡWeb�ļ������ݲ�д�뵽�����ļ��������ͨ��һ��ѭ���������ɡ� 
   
  //int l; 
  //do 
  //{ 
  //l = inStream.Read(buffer,0, 
  //buffer.Length); 
  //if(l>0) 
  //outStream.Write(buffer,0,l); 
  //} while(l>0); 
}
