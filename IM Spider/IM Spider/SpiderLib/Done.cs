using System;
using System.Collections.Generic;
using System.Text;

namespace SpiderLib
{
    public class Done
    {
    }
  //  �ġ������������ 
   
  //  ���ö���߳�ͬʱ�����ļ���Ч����������ܣ���Ҳ�������̹߳���������⡣������ӵ�һ�������ǣ�֩������ʱ��������˹���������������Ҫ����һ��ר�õ���Done���жϡ� 
   
  //  �����б�Ҫ˵��һ��"��ɹ���"�ľ��庬�塣ֻ�е�ϵͳ�в����ڵȴ����ص�URL���������й����̶߳��Ѿ������䴦����ʱ��֩�����Ĺ���������ɡ�Ҳ����˵����ɹ�����ζ���Ѿ�û�еȴ����غ��������ص�URL�� 
   
  //  Done���ṩ��һ��WaitDone���������Ĺ�����һֱ�ȴ���ֱ��Done�����⵽֩���������ɹ�����������WaitDone�����Ĵ��롣 
   
  //public void WaitDone() 
  //{ 
  //Monitor.Enter(this); 
  //while ( m_activeThreads>0 ) 
  //{ 
  //Monitor.Wait(this); 
  //} 
  //Monitor.Exit(this); 
  //} 
   
   
   
  //  WaitDone������һֱ�ȴ���ֱ�������л���̡߳�������ע����ǣ����ؿ�ʼ������׶�Ҳû���κλ���̣߳����Ժ��������֩�����һ��ʼ������ֹͣ������Ϊ���������⣬���ǻ���Ҫ��һ������WaitBegin���ȴ�֩��������"��ʽ��"�����׶Ρ�һ��ĵ��ô����ǣ��ȵ���WaitBegin���ٽ��ŵ���WaitDone��WaitDone���ȴ�֩�������ɹ�����������WaitBegin�Ĵ��룺 
   
  //public void WaitBegin() 
  //{ 
  //Monitor.Enter(this); 
  //while ( !m_started ) 
  //{ 
  //Monitor.Wait(this); 
  //} 
  //Monitor.Exit(this); 
  //} 
   
   
   
  //  WaitBegin������һֱ�ȴ���ֱ��m_started��Ǳ����á�m_started�������WorkerBegin�������õġ������߳��ڿ�ʼ�������URL֮ʱ�������WorkerBegin���������ʱ����WorkerEnd��WorkerBegin��WorkerEnd��������������Done����ȷ����ǰ�Ĺ���״̬��������WorkerBegin�����Ĵ��룺 
   
  //public void WorkerBegin() 
  //{ 
  //Monitor.Enter(this); 
  //m_activeThreads++; 
  //m_started = true; 
  //Monitor.Pulse(this); 
  //Monitor.Exit(this); 
  //} 
   
   
   
  //  WorkerBegin�����������ӵ�ǰ��̵߳���������������m_started��ǣ�������Pulse������֪ͨ�����ܴ��ڵģ��ȴ������߳��������̡߳���ǰ���������ܵȴ�Done����ķ�����WaitBegin������ÿ������һ��URL��WorkerEnd�����ᱻ���ã� 
   
  //public void WorkerEnd() 
  //{ 
  //Monitor.Enter(this); 
  //m_activeThreads--; 
  //Monitor.Pulse(this); 
  //Monitor.Exit(this); 
  //} 
   
  //  WorkerEnd������Сm_activeThreads��̼߳�����������Pulse�ͷſ����ڵȴ�Done������߳�--��ǰ�����������ڵȴ�Done����ķ�����WaitDone������ 

}
