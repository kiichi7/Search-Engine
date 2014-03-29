using System;
using System.Collections.Generic;
using System.Text;

namespace SpiderLib
{
    public class ParseHTML
    {
    }
//    一、HTML解析
//HTML解析器由ParseHTML类实现，使用非常方便：首先创建该类的一个实例，然后将它的Source属性设置为要解析的HTML文档： 
   
//  ParseHTML parse = new ParseHTML(); 
//  parse.Source = "<p>Hello World</p>"; 
//接下来就可以利用循环来检查HTML文档包含的所有文本和标记。通常，检查过程可以从一个测试Eof方法的while循环开始： 
   
//  while(!parse.Eof()) 
//  { 
//  char ch = parse.Parse();  
//    Parse方法将返回HTML文档包含的字符--它返回的内容只包含那些非HTML标记的字符，如果遇到了HTML标记，Parse方法将返回0值，表示现在遇到了一个HTML标记。遇到一个标记之后，我们可以用GetTag()方法来处理它。 
   
//  if(ch==0) 
//  { 
//  HTMLTag tag = parse.GetTag(); 
//  }    
//    一般地，蜘蛛程序最重要的任务之一就是找出各个HREF属性，这可以借助C#的索引功能完成。例如，下面的代码将提取出HREF属性的值（如果存在的话）。 

//Attribute href = tag["HREF"]; 
//string link = href.Value; 
//    获得Attribute对象之后，通过Attribute.Value可以得到该属性的值。 


}
