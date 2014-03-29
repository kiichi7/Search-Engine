using System;
using System.Collections.Generic;
using System.Text;

namespace SpiderLib
{
    public class DocumentWorker
    {
    }
//    三、多线程

//我们用DocumentWorker类封装所有下载一个URL的操作。每当一个DocumentWorker的实例被创建，它就进入循环，等待下一个要处理的URL。下面是DocumentWorker的主循环： 
   
//  while(!m_spider.Quit ) 
//  { 
//  m_uri = m_spider.ObtainWork(); 
   
//  m_spider.SpiderDone.WorkerBegin(); 
//  string page = GetPage(); 
//  if(page!=null) 
//  ProcessPage(page); 
//  m_spider.SpiderDone.WorkerEnd(); 
//  } 
//    这个循环将一直运行，直至Quit标记被设置成了true（当用户点击"Cancel"按钮时，Quit标记就被设置成true）。在循环之内，我们调用ObtainWork获取一个URL。ObtainWork将一直等待，直到有一个URL可用--这要由其他线程解析文档并寻找链接才能获得。Done类利用WorkerBegin和WorkerEnd方法来确定何时整个下载操作已经完成。 
   
//    从图一可以看出，蜘蛛程序允许用户自己确定要使用的线程数量。在实践中，线程的最佳数量受许多因素影响。如果你的机器性能较高，或者有两个处理器，可以设置较多的线程数量；反之，如果网络带宽、机器性能有限，设置太多的线程数量其实不一定能够提高性能。 


}
