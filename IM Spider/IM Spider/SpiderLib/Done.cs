using System;
using System.Collections.Generic;
using System.Text;

namespace SpiderLib
{
    public class Done
    {
    }
  //  四、任务完成了吗？ 
   
  //  利用多个线程同时下载文件有效地提高了性能，但也带来了线程管理方面的问题。其中最复杂的一个问题是：蜘蛛程序何时才算完成了工作？在这里我们要借助一个专用的类Done来判断。 
   
  //  首先有必要说明一下"完成工作"的具体含义。只有当系统中不存在等待下载的URL，而且所有工作线程都已经结束其处理工作时，蜘蛛程序的工作才算完成。也就是说，完成工作意味着已经没有等待下载和正在下载的URL。 
   
  //  Done类提供了一个WaitDone方法，它的功能是一直等待，直到Done对象检测到蜘蛛程序已完成工作。下面是WaitDone方法的代码。 
   
  //public void WaitDone() 
  //{ 
  //Monitor.Enter(this); 
  //while ( m_activeThreads>0 ) 
  //{ 
  //Monitor.Wait(this); 
  //} 
  //Monitor.Exit(this); 
  //} 
   
   
   
  //  WaitDone方法将一直等待，直到不再有活动的线程。但必须注意的是，下载开始的最初阶段也没有任何活动的线程，所以很容易造成蜘蛛程序一开始就立即停止的现象。为解决这个问题，我们还需要另一个方法WaitBegin来等待蜘蛛程序进入"正式的"工作阶段。一般的调用次序是：先调用WaitBegin，再接着调用WaitDone，WaitDone将等待蜘蛛程序完成工作。下面是WaitBegin的代码： 
   
  //public void WaitBegin() 
  //{ 
  //Monitor.Enter(this); 
  //while ( !m_started ) 
  //{ 
  //Monitor.Wait(this); 
  //} 
  //Monitor.Exit(this); 
  //} 
   
   
   
  //  WaitBegin方法将一直等待，直到m_started标记被设置。m_started标记是由WorkerBegin方法设置的。工作线程在开始处理各个URL之时，会调用WorkerBegin；处理结束时调用WorkerEnd。WorkerBegin和WorkerEnd这两个方法帮助Done对象确定当前的工作状态。下面是WorkerBegin方法的代码： 
   
  //public void WorkerBegin() 
  //{ 
  //Monitor.Enter(this); 
  //m_activeThreads++; 
  //m_started = true; 
  //Monitor.Pulse(this); 
  //Monitor.Exit(this); 
  //} 
   
   
   
  //  WorkerBegin方法首先增加当前活动线程的数量，接着设置m_started标记，最后调用Pulse方法以通知（可能存在的）等待工作线程启动的线程。如前所述，可能等待Done对象的方法是WaitBegin方法。每处理完一个URL，WorkerEnd方法会被调用： 
   
  //public void WorkerEnd() 
  //{ 
  //Monitor.Enter(this); 
  //m_activeThreads--; 
  //Monitor.Pulse(this); 
  //Monitor.Exit(this); 
  //} 
   
  //  WorkerEnd方法减小m_activeThreads活动线程计数器，调用Pulse释放可能在等待Done对象的线程--如前所述，可能在等待Done对象的方法是WaitDone方法。 

}
