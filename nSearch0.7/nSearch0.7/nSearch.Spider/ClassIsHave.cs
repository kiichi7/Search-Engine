using System;
using System.Collections.Generic;
using System.Text;
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


namespace nSearch.Spider
{
    /// <summary>
    /// 是否已经存在于文件系统
    /// </summary>
    class ClassIsHave
    {

        /// <summary>
        /// 原始的URL
        /// </summary>
        Hashtable MainBataBase = Hashtable.Synchronized(new Hashtable());


        ArrayList a = ArrayList.Synchronized(new ArrayList());
        ArrayList b = ArrayList.Synchronized(new ArrayList());
        ArrayList c = ArrayList.Synchronized(new ArrayList());
        ArrayList d = ArrayList.Synchronized(new ArrayList());
        ArrayList e = ArrayList.Synchronized(new ArrayList());
        ArrayList f = ArrayList.Synchronized(new ArrayList());
        ArrayList g = ArrayList.Synchronized(new ArrayList());
        ArrayList h = ArrayList.Synchronized(new ArrayList());
        ArrayList i = ArrayList.Synchronized(new ArrayList());
        ArrayList j = ArrayList.Synchronized(new ArrayList());
        ArrayList k = ArrayList.Synchronized(new ArrayList());
        ArrayList l = ArrayList.Synchronized(new ArrayList());
        ArrayList m = ArrayList.Synchronized(new ArrayList());
        ArrayList n = ArrayList.Synchronized(new ArrayList());
        ArrayList o = ArrayList.Synchronized(new ArrayList());
        ArrayList p = ArrayList.Synchronized(new ArrayList());
        ArrayList q = ArrayList.Synchronized(new ArrayList());
        ArrayList r = ArrayList.Synchronized(new ArrayList());
        ArrayList s = ArrayList.Synchronized(new ArrayList());
        ArrayList t = ArrayList.Synchronized(new ArrayList());
        ArrayList u = ArrayList.Synchronized(new ArrayList());
        ArrayList v = ArrayList.Synchronized(new ArrayList());
        ArrayList w = ArrayList.Synchronized(new ArrayList());
        ArrayList x = ArrayList.Synchronized(new ArrayList());
        ArrayList y = ArrayList.Synchronized(new ArrayList());
        ArrayList z = ArrayList.Synchronized(new ArrayList());
        ArrayList n0 = ArrayList.Synchronized(new ArrayList());
        ArrayList n1 = ArrayList.Synchronized(new ArrayList());
        ArrayList n2 = ArrayList.Synchronized(new ArrayList());
        ArrayList n3 = ArrayList.Synchronized(new ArrayList());
        ArrayList n4 = ArrayList.Synchronized(new ArrayList());
        ArrayList n5 = ArrayList.Synchronized(new ArrayList());
        ArrayList n6 = ArrayList.Synchronized(new ArrayList());
        ArrayList n7 = ArrayList.Synchronized(new ArrayList());
        ArrayList n8 = ArrayList.Synchronized(new ArrayList());
        ArrayList n9 = ArrayList.Synchronized(new ArrayList());

        public  ClassIsHave()
        {
            MainBataBase.Clear();
            //加载
            MainBataBase.Add('a', a);
            MainBataBase.Add('b', b);
            MainBataBase.Add('c', c);
            MainBataBase.Add('d', d);
            MainBataBase.Add('e', e);
            MainBataBase.Add('f', f);
            MainBataBase.Add('g', g);
            MainBataBase.Add('h', h);
            MainBataBase.Add('i', i);
            MainBataBase.Add('j', j);
            MainBataBase.Add('k', k);
            MainBataBase.Add('l', l);
            MainBataBase.Add('m', m);
            MainBataBase.Add('n', n);
            MainBataBase.Add('o', o);
            MainBataBase.Add('p', p);
            MainBataBase.Add('q', q);
            MainBataBase.Add('r', r);
            MainBataBase.Add('s', s);
            MainBataBase.Add('t', t);
            MainBataBase.Add('u', u);
            MainBataBase.Add('v', v);
            MainBataBase.Add('w', w);
            MainBataBase.Add('x', x);
            MainBataBase.Add('y', y);
            MainBataBase.Add('z', z);
            MainBataBase.Add('0', n0);
            MainBataBase.Add('1', n1);
            MainBataBase.Add('2', n2);
            MainBataBase.Add('3', n3);
            MainBataBase.Add('4', n4);
            MainBataBase.Add('5', n5);
            MainBataBase.Add('6', n6);
            MainBataBase.Add('7', n7);
            MainBataBase.Add('8', n8);
            MainBataBase.Add('9', n9);
        
        }


        /// <summary>
        /// 大规模导入数据
        /// </summary>
        /// <param name="data"></param>
        public void InitData(ArrayList data)
        {
            foreach (string one in data)
            {

                string Md5X = getMD5name(one).ToLower();

                ArrayList newOne = (ArrayList)MainBataBase[Md5X[0]];

                newOne.Add(one);

                MainBataBase[Md5X[0]] = newOne;

                newOne = null;
                        
            }
        
        }

        /// <summary>
        /// 文件系统中是否已经具有
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool isHave(string url)
        {

            if (url == null)
            {
                return true;
            }

            if (url.Length ==0)
            {
                return true;
            }

            string Md5X = getMD5name(url).ToLower();
            ArrayList newOne = (ArrayList)MainBataBase[Md5X[0]];

            //如果不存在 则不管
            if (newOne.Contains(url) == false)
            {
                newOne = null;
                return false;
            }
            else
            {
                newOne = null;
                return true;
            }           
        
        }

        /// <summary>
        /// 压入一个已经存储到文件系统的url
        /// </summary>
        /// <param name="url"></param>
        public void SaveOneUrl(string url)
        {
            string Md5X = getMD5name(url).ToLower();
            ArrayList newOne = (ArrayList)MainBataBase[Md5X[0]];
            newOne.Add(url);
            MainBataBase[Md5X[0]] = newOne;
            newOne = null;
        }



        /// <summary>
        /// 得到URL的MD5名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string getMD5name(string url)
        {
            string strMd5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(url, "md5");
            return strMd5;
        }

    }
}
