#include "StdAfx.h"
#include "norm.h"
#include <memory.h>
#include <string.h>
#include <math.h>
#include <stdlib.h>
CNorm::CNorm()
{
	char out[]="零一二三四五六七八九";
	for( int i=0; i<10; i++)
	{
		NumOut[i][0]=out[2*i];
		NumOut[i][1]=out[2*i+1];
		NumOut[i][2]=0;
	}
}
CNorm::~CNorm()
{
}

bool CNorm::IsAllAlabNum(char *sWord)
{
	int nLen = strlen(sWord);
	unsigned char nchar[3];
	if(nLen<=0)
		return false;
	for(int i=0;i<nLen;++i){
		if(sWord[i]>0){
			if(sWord[i]>'9'||sWord[i]<'0')
				return false;
		}
		else{
			nchar[0] = sWord[i];
			nchar[1] = sWord[i+1];
			nchar[2] = 0;
			i++;
			if(!(nchar[0]==163&&(nchar[1]>=176&&nchar[1]<=185)))
				return false;
		}
	}
	return true;
}

bool CNorm::SentenceNorm(char* senten,char* ResultSen)
{

	int l=strlen(senten);
//	senten[l-1]='\0';//去掉末尾的回车富
	CString s(senten);
	CString Result("");
	s.TrimLeft();
	s.TrimRight();
	char sWord[WORD_MAXLENGTH],LastWord[WORD_MAXLENGTH],LastTwoWord[WORD_MAXLENGTH];
	char NextWord[WORD_MAXLENGTH],NextTwoWord[WORD_MAXLENGTH],TempWord[WORD_MAXLENGTH];
	char NextThreeWord[WORD_MAXLENGTH];
	char sResult[WORD_MAXLENGTH];
	sResult[0]=0;
	sWord[0]=0;
	char sTag[3],NextTag[3],NextTwoTag[3],NextThreeTag[3];
	sTag[0]=0;
	int lenth=0;
	bool HaveChanged=false;
	while(!s.IsEmpty()) {

		//GET THE WORD AND TAG FROM THE SENTENCE;
		if(!ReadWord(s,sWord,sTag,&lenth))
			return false;
		s=s.Mid(lenth);
		s.TrimLeft();
		HaveChanged=false;
		//NOW ANALYST THE WORD / TAG , RECORD POSSIBLE WORD NEED TO BE CONVERTED OR AS INFORMATION
		int WordType=GetCharType((unsigned char *)sWord);

		if(strcmp("q",sTag)==0)
		{
			// RULE NO1: 克／听/q  -->克每听/q 每195 - 191
			char* p=CC_Find(sWord,"／");
			if(strcmp(sWord,"℃")==0)
				strcpy(sResult,"摄氏度");
			else
				strcpy(sResult,sWord);
			if((WordType==T_CHINESE)&&(p!=NULL))
			{
				sResult[p-sWord]=(unsigned char)195;
				sResult[p-sWord+1]=(unsigned char)191;
			}
			strcat(sResult,"/");
			strcat(sResult,sTag);
		}
		//（可能包含数字的词性有：m,nx,t,nt,nz)
		//码值读法;单独处理，不需要用到上下文信息
		//数字串里可能含有的字符：（除调数字词）
		//几数第上成±—＋∶·．／+-./○百千万亿佰仟％‰%
		// RULE NO2:  nz,nx,nt码读
		else if((strcmp("nz",sTag)==0)||(strcmp("nx",sTag)==0)||(strcmp("nt",sTag)==0)||(strlen(sWord)>15&&IsAllAlabNum(sWord)))
		{
				ChangeWord(sWord,sTag,sResult,true);//true码读
		}

		//数字和时间的复杂情况；需要上下文信息；３/m  ＋/m  Ｘ/nx 
		//数字串里可能含有的字符：（除调数字词）
		//几数第上成±—＋∶·．／+-./○百千万亿佰仟％‰%
        else if(strcmp("m",sTag)==0)
		{
			//考虑上文
			// RULE NO3:  上文是：电话，邮编，传真，专利号： 码读法，对所有的m
			if((((strcmp(LastTwoWord,"电话")==0)||(strcmp(LastTwoWord,"传真")==0)||(strcmp(LastTwoWord,"邮编")==0)||(strcmp(LastTwoWord,"专利号")==0))
				&&((strcmp(LastWord,":")==0)||(strcmp(LastWord,"：")==0)||(strcmp(LastWord,"是")==0)))
				||((strcmp(LastWord,"电话")==0)||(strcmp(LastWord,"传真")==0)||(strcmp(LastWord,"邮编")==0)||(strcmp(LastWord,"专利号")==0)))
			{
				//所有数字码读，忽略中间符号，比如010-62753835，中的-；专利中的.
				ChangeWord(sWord,sTag,sResult,true);//true码读
				HaveChanged=true;				
			}
			//考虑下文
			else if(ReadWord(s,NextWord,NextTag,&lenth))
			{
				// RULE NO4:  下文是：国道，航班，信箱，次列车，路汽车；码读法
				//路：待考虑，２１/m 路/q  车/n ；２１/m  路/q  公共/b  汽车/n，可读值，所以不加入
				//词列车：也存在上个问题，故略去

				if((strcmp(NextWord,"国道")==0)||(strcmp(NextWord,"航班")==0)||(strcmp(NextWord,"信箱")==0))
				{
					s=s.Mid(lenth);
					s.TrimLeft();
					ChangeWord(sWord,sTag,sResult,true);
					HaveChanged=true;
					strcat(sResult,"  ");
					strcat(sResult,NextWord);
					strcat(sResult,"/");
					strcat(sResult,NextTag);
				}
				
				//RULE NO5: 循环数字处理：下文是 “和，或，到，,～~,—,- ", 并列数字串处理，要依赖下文.				
				else if((strcmp(NextWord,"和")==0)||(strcmp(NextWord,"或")==0)||(strcmp(NextWord,"到")==0)
					||(strcmp(NextWord,"—")==0)||(strcmp(NextWord,"～")==0)||(strcmp(NextWord,"~")==0)||(strcmp(NextWord,"-")==0))
				{
					s=s.Mid(lenth);
					s.TrimLeft();
					if(ReadWord(s,NextTwoWord,NextTwoTag,&lenth))
					{

						if(strcmp("m",NextTwoTag)==0)
						{
							s=s.Mid(lenth);
							s.TrimLeft();
							if(HaveChanged)//已经确定了上一个的读法，沿用，而且肯定是码读法
							{
								if((strcmp(NextWord,"—")==0)||(strcmp(NextWord,"～")==0)||(strcmp(NextWord,"~")==0)||(strcmp(NextWord,"-")==0))
								{
									strcpy(NextWord,"到");
									strcpy(NextTag,"p");//连词
								}
								strcat(sResult,"  ");
								strcat(sResult,NextWord);
								strcat(sResult,"/");
								strcat(sResult,NextTag);
								strcat(sResult,"  ");
								ChangeWord(NextTwoWord,NextTwoTag,TempWord,true);
								HaveChanged=true;
								strcat(sResult,TempWord);
							}
							else if(ReadWord(s,NextThreeWord,NextThreeTag,&lenth))
							{
								if((strcmp(NextThreeWord,"国道")==0)||(strcmp(NextThreeWord,"航班")==0)||(strcmp(NextThreeWord,"信箱")==0))
								{
									ChangeWord(sWord,sTag,sResult,true);
									strcat(sResult,"  ");
									if((strcmp(NextWord,"—")==0)||(strcmp(NextWord,"～")==0)||(strcmp(NextWord,"~")==0)||(strcmp(NextWord,"-")==0))
									{
										strcpy(NextWord,"到");
										strcpy(NextTag,"p");//连词
									}
									strcat(sResult,NextWord);
									strcat(sResult,"/");
									strcat(sResult,NextTag);
									strcat(sResult,"  ");
									ChangeWord(NextTwoWord,NextTwoTag,TempWord,true);
									strcat(sResult,TempWord);
									HaveChanged=true;
								}
								//默认值读
								else
								{
									ChangeWord(sWord,sTag,sResult,false);
									strcat(sResult,"  ");
									if((strcmp(NextWord,"—")==0)||(strcmp(NextWord,"～")==0)||(strcmp(NextWord,"~")==0)||(strcmp(NextWord,"-")==0))
									{
										strcpy(NextWord,"到");
										strcpy(NextTag,"p");//连词
									}
									strcat(sResult,NextWord);
									strcat(sResult,"/");
									strcat(sResult,NextTag);
									strcat(sResult,"  ");
									ChangeWord(NextTwoWord,NextTwoTag,TempWord,false);
									strcat(sResult,TempWord);
									HaveChanged=true;

								}

							}
							// s已经为空，默认值读
							else
							{
									ChangeWord(sWord,sTag,sResult,false);
									strcat(sResult,"  ");
									if((strcmp(NextWord,"—")==0)||(strcmp(NextWord,"～")==0)||(strcmp(NextWord,"~")==0)||(strcmp(NextWord,"-")==0))
									{
										strcpy(NextWord,"到");
										strcpy(NextTag,"p");//连词
									}
									strcat(sResult,NextWord);
									strcat(sResult,"/");
									strcat(sResult,NextTag);
									strcat(sResult,"  ");
									ChangeWord(NextTwoWord,NextTwoTag,TempWord,false);
									strcat(sResult,TempWord);
									HaveChanged=true;
							}

						}
						
						//RULE NO6:年的读法：码读。
						//１９７４/m  —/w  １９７７年/t  
						else if(strcmp("t",NextTwoTag)==0)
						{
							s=s.Mid(lenth);
							s.TrimLeft();
							if(CC_Find(NextTwoWord,"年")!=NULL)
							{
									ChangeWord(sWord,sTag,sResult,true);
									strcat(sResult,"  ");
									if((strcmp(NextWord,"—")==0)||(strcmp(NextWord,"～")==0)||(strcmp(NextWord,"~")==0)||(strcmp(NextWord,"-")==0))
									{
										strcpy(NextWord,"到");
										strcpy(NextTag,"p");//连词
									}
									strcat(sResult,NextWord);
									strcat(sResult,"/");
									strcat(sResult,NextTag);
									strcat(sResult,"  ");
									ChangeWord(NextTwoWord,NextTwoTag,TempWord,true);
									strcat(sResult,TempWord);
									HaveChanged=true;
							}
							//值读
							else
							{
									ChangeWord(sWord,sTag,sResult,false);
									strcat(sResult,"  ");
									if((strcmp(NextWord,"—")==0)||(strcmp(NextWord,"～")==0)||(strcmp(NextWord,"~")==0)||(strcmp(NextWord,"-")==0))
									{
										strcpy(NextWord,"到");
										strcpy(NextTag,"p");//连词
									}
									strcat(sResult,NextWord);
									strcat(sResult,"/");
									strcat(sResult,NextTag);
									strcat(sResult,"  ");
									ChangeWord(NextTwoWord,NextTwoTag,TempWord,false);
									strcat(sResult,TempWord);
									HaveChanged=true;
							}

						}
						else//不是时间，也不是数词，默认值读.
						{
							        ChangeWord(sWord,sTag,sResult,false);
									strcat(sResult,"  ");
									strcat(sResult,NextWord);
									strcat(sResult,"/");
									strcat(sResult,NextTag);
									strcat(sResult,"  ");
									HaveChanged=true;
						}

					}
					// s已经为空，默认值读
					else
					{
						ChangeWord(sWord,sTag,sResult,false);
						strcat(sResult,"  ");
						strcat(sResult,NextWord);
						strcat(sResult,"/");
						strcat(sResult,NextTag);
						strcat(sResult,"  ");
						HaveChanged=true;
					}

				}
				
				//RULE NO7:×仅仅在数词后面才读为乘
				//２/m  ×/w  ７．５/m  米/q 
				//４/m  ×/w  ２００/m  米/q  自由泳/n
				//打/v  满/a  “/w  ××/nx  ”/w  的/u  答卷/n
				//永发/nr  ５８３/m  ×/w  ４０６/m  ×/w  ４０１/m  ｍｍＹＢＪ—５８０Ｂ/nx  
				else if(strcmp(NextWord,"×")==0)
				{
					s=s.Mid(lenth);
					s.TrimLeft();
					ChangeWord(sWord,sTag,sResult,false);
					strcat(sResult,"  乘/v");//?

				}
				//下文正常处理RULE NO8:  默认都是值读法
				else
				{
					ChangeWord(sWord,sTag,sResult,false);
				}
				
			}
			// s已经为空
			else
			{
				ChangeWord(sWord,sTag,sResult,false);//true码读
//				HaveChange=true;	
			}
		}
		
		else if(strcmp("t",sTag)==0)//时间
		{
			//RULE NO6:年的读法：码读。注意
			//80/m  年代/n  初/f  。/w  
			//有/v  ２０００年/t  历史/n 
			//这/r  是/v  １９９７年/t  历史/n  题材/n 
			//所以没有加入，后面是历史的时候值读的情况
			if(CC_Find(sWord,"年")!=NULL)
			{
				ChangeWord(sWord,sTag,sResult,true);
				HaveChanged=true;
			}
			else
			{
				ChangeWord(sWord,sTag,sResult,false);
				HaveChanged=true;
			}

		}
		else if(strcmp("w",sTag)==0)//标点，此时注意：有一些符号转换
		{
			//RULE NO9:  ℃不涉及位置替换问题，所以不判断数字
			///＄/w  ２·０７/m
			//４/m  —/w  ６/m  ℃/w  。/w 
			//４/m  —/w  ６/m  ℃/q  。/w  
			//今天/t  气温/n  ：/w  ９/m  —/w  １９/m  ℃/w  。/w  
			if(strcmp(sWord,"℃")==0)
				strcpy(sResult,"摄氏度/q");
			//RULE NO10:  ＋可放心替换，习语，名词中也如公式一样读加
			//３/m  ＋/m  Ｘ/nx
			//“/w  ＋＋＋/w  ”/w  ，/w  心脏病/n 
			else if(strcmp(sWord,"＋")==0)
				strcpy(sResult,"加/v");
			//RULE NO11:  ＝可放心替换，习语，名词中也如公式一样读等于
			else if(strcmp(sWord,"＝")==0)
				strcpy(sResult,"等于/v");
			//RULE NO12: ＄->美元 :注意调换位置
			else if(strcmp(sWord,"＄")==0)
			{
				//如果标注有问题，没有找到/不继续
				if(ReadWord(s,NextWord,NextTag,&lenth))
				{
					if(strcmp(NextTag,"m")==0)
					{
						s=s.Mid(lenth);
						s.TrimLeft();
						ChangeWord(NextWord,NextTag,sResult,false);
						strcat(sResult,"  美元/q");						
					}
				}
				else
				{
					strcpy(sResult,sWord);
					strcat(sResult,"/");
					strcat(sResult,sTag);
				}

			}
			// RULE NO13: ￥->人民币 :不用调换位置
			else if(strcmp(sWord,"￥")==0)
			{
				strcpy(sResult,"人民币/n");
			}
			//RULE NO1: 克／听/q  -->克每听/q 每195 - 191
			//１１．０９/m  元/q  ／/w  平方米/q  (其实是标注错误，但是这里也永上这个规则
			//４５０/m  克/q  ／/w  听/v
			//４０万/m  吨/q  ／日/t 
			//６０/m  微/ag  克/vg  ／/w  立方米/q  
			//９．２９/m  吨/q  ／/w  （/w  平方公里/q  ·月/t 
			//羽毛球/n  拍/v  ／/w  ＢＳ５０２/nx 
			// 泽琳/nr  ／/w  郑/nr  丽梦/nr 
			// 八百百万/m  大/a  卡/n  ／/w  小时/n 
			//如果上下文有一个为q，读为“每”
//			else if(strcmp(sWord,"／")==0)
//			{
//			}
			else
			{
				//其他标点情况
				strcpy(sResult,sWord);
				strcat(sResult,"/");
				strcat(sResult,sTag);

			}



			
		}
		//非以上所有情况，原样返回。
		else {
			strcpy(sResult,sWord);
			strcat(sResult,"/");
			strcat(sResult,sTag);

		}

		strcpy(LastTwoWord,LastWord);
		strcpy(LastWord,sWord);
		strcat(sResult,"  ");		
		Result+=sResult;
    }
	strcpy(ResultSen,const_cast<char*>((LPCSTR)Result));
	return true;
}
//dont' forget change single char to CC char;
bool CNorm::Processing(char* NumWord,int* time)
{
	int samelenth=0,i=0;
	//char Number_chn[]= "０１２３４５６７８９";
    //char Number_eng[]="0123456789";
	//因为0123456789的内码都小于64，可以放心替换。
	//将所有的单字节数字替换为双字节
	CString s(NumWord);
	s.Replace("0","０");
	s.Replace("1","１");
	s.Replace("2","２");
	s.Replace("3","３");
	s.Replace("4","４");
	s.Replace("5","５");
	s.Replace("6","６");
	s.Replace("7","７");
	s.Replace("8","８");
	s.Replace("9","９");
	while((i<CHANGETIME)&&(!s.IsEmpty()))
	{
		int type;
		CString same;
		GetSameType((char*)((LPCSTR)s),&samelenth,&type);
		same=s.Left(samelenth);
		strcpy(sSameTypeString[i],const_cast<char*>((LPCSTR)same));
		ctype[i]=type;
		s=s.Mid(samelenth);
		i++;
	}
	*time=i;
	return true;
}
bool CNorm::ReadWord(CString s,char* sWord,char* sTag,int* lenth)
{
	sTag[0]=0;
	sWord[0]=0;
	if(s.IsEmpty())
		return false;
	CString word,tag;
	word=s.SpanExcluding(" ");// 函数的参数是一个空格，假定分词语料是以空格作为分界符
	*lenth=word.GetLength();
//	s=s.Mid(word.GetLength());
//	s.TrimLeft();
	int i=word.Find('/');


//为了修改(1945/46年/t)而添加   04_06_23
	int j = word.ReverseFind('/');
	if(i>=0){
		if(j>i&&word[j-1]==']'){
			word = word.Left(j-1);
			j = word.ReverseFind('/');
		}
		if(j-i>2){
			int x,y;
			x = word.Find("年");
			if(x>0){
				y = word.Left(x).ReverseFind('/');
				if(y>0){
					word = word.Left(y) + word.Mid(y+1);
				}
			}
			i = word.ReverseFind('/');
		}
	}

	if(i<0) {
		//如果原来标注结果有问题，没有标注词性，那么返回词性nx
		strcpy(sWord,const_cast<char*>((LPCSTR)word));
		strcpy(sTag,"nx");
		return false;
	}
	tag=word.Mid(i+1);
	word=word.Left(i);
	i=word.Find('[');//注意，这种情况属于未登陆团体词或成语
	if(i>=0)
	{
		word=word.Mid(i+1);
	}
	i=tag.Find(']');
	if(i>0)
	{
		tag=tag.Left(i);
	}
	//需要的，如/l/%表示常用语
	i=tag.Find('/');
	if(i>0)
	{
		tag=tag.Left(i);
	}
	tag.MakeLower();
	strcpy(sWord,const_cast<char*>((LPCSTR)word));
	strcpy(sTag,const_cast<char*>((LPCSTR)tag));
	return true;
}
bool CNorm::ChangeWord(char* sWord,char* sTag,char* sResult,bool type)
{
	int time=0;
	char TempNumResult[WORD_MAXLENGTH];
	memset(TempNumResult,0,WORD_MAXLENGTH);
	bool CurrentConvert=false;
	//分段处理
	Processing(sWord,&time);
	sResult[0]=0;

	//特殊情况；
	///$１９７４/nx;$1974/nx;
	//其实是标注错误，但是放在这里正确处理。
	if((time==2)&&(ctype[0]==T_SINGLE)&&(strcmp(sSameTypeString[0],"$")==0)&&(ctype[1]==T_NUM))
	{
		if(ConvertNumber(sSameTypeString[1],TempNumResult,false))//值读
		{
			strcpy(sResult,TempNumResult);
			strcat(sResult,"/m  美元/q");
			return true;
		}
		else
		{
			ConvertNumber(sSameTypeString[1],TempNumResult,true);
			strcpy(sResult,TempNumResult);
			strcat(sResult,"/m");
			return false;
		}
	}
	if(type)//码读
	{
		for(int j=0;j<time;j++)//find the number and change it, for others didn't care.
		{
			////马自达３２３/nz;ＳＤＨ２．５Ｇｂ/nx 	ＨＷ９０００/nx 
			if(ctype[j]==T_NUM)
			{
				ConvertNumber(sSameTypeString[j],TempNumResult,true);
				strcat(sResult,TempNumResult);
			}
			else if((ctype[j]==T_DELIMITER)&&strcmp(sSameTypeString[j],"—")==0)
			{
				//// RULE NO3: —->杠 , only in nz, nt ,nx;
				//ｍｍＹＢＪ—５８０Ｂ/nx  ;//ＹＢＪ８５—９５
				strcat(sResult,"杠");
			}
			//ＳＤＨ２．５Ｇｂ/nx  
			else if(strcmp(sSameTypeString[j],"．")==0||strcmp(sSameTypeString[j],".")==0)
				strcat(sResult,"点");
			// RULE NO3: - ->杠 , only in nz, nt ,nx;
			//b-１９７４/nx;
			else if((ctype[j]==T_SINGLE)&&strcmp(sSameTypeString[j],"-")==0)
				strcat(sResult,"杠");
			else
				strcat(sResult,sSameTypeString[j]);
		}
		strcat(sResult,"/");
		strcat(sResult,sTag);		
	}
	else//值读
		//数字串里可能含有的字符：（除调数字词）
		//几数第上成±—＋∶·．／+-./○百千万亿佰仟％‰%
		//2、.·．    小数点后按位读。小数点的判断（前后都是数字）
		//3、/／       分数的读法：１／４读时需要把数字次序换一下，（前后都是数字）
		//4、%‰％      数字后置，读为“百分之”，同理还有‰（注意，54.3%这种情况要把2，4结合起来）
		//5、:∶：
		//
	{
		for(int j=0;j<time;j++)//find the number and change it, for others didn't care.
		{
			CurrentConvert=false;
			if(ctype[j]==T_NUM)
			{
				if((ctype[j+2]==T_NUM)&&(j+2<time))
				{
					if(strcmp(sSameTypeString[j+1],"．")==0||strcmp(sSameTypeString[j+1],"·")==0||strcmp(sSameTypeString[j+1],".")==0)
					{
						//54.3%/m 
						if((j+3<time)&&(strcmp(sSameTypeString[j+3],"％")==0||strcmp(sSameTypeString[j+3],"%")==0))
						{
							strcat(sResult,"百分之");
							ConvertNumber(sSameTypeString[j],TempNumResult,false);//小数点前值读
							strcat(sResult,TempNumResult);
							strcat(sResult,"点");
							ConvertNumber(sSameTypeString[j+2],TempNumResult,true);//小数点后码读
							strcat(sResult,TempNumResult);
							j=j+3;
							CurrentConvert=true;
						}
						//０．７‰
						else if((j+3<time)&&strcmp(sSameTypeString[j+3],"‰")==0)
						{
							strcat(sResult,"千分之");
							ConvertNumber(sSameTypeString[j],TempNumResult,false);//小数点前值读
							strcat(sResult,TempNumResult);
							strcat(sResult,"点");
							ConvertNumber(sSameTypeString[j+2],TempNumResult,true);//小数点后码读
							strcat(sResult,TempNumResult);
							j=j+3;
							CurrentConvert=true;
						}
						//１．４∶２．４/m 
						//只转换到:，后面进入循环即可。
						else if((j+3<time)&&(strcmp(sSameTypeString[j+3],":")==0||strcmp(sSameTypeString[j+3],"∶")==0||strcmp(sSameTypeString[j+3],"：")==0))
						{
							ConvertNumber(sSameTypeString[j],TempNumResult,false);//小数点前值读
							strcat(sResult,TempNumResult);
							strcat(sResult,"点");
							ConvertNumber(sSameTypeString[j+2],TempNumResult,true);//小数点后码读
							strcat(sResult,TempNumResult);
							strcat(sResult,"比");
							j=j+3;
							CurrentConvert=true;
						}
						//１１·２９/m
						else
						{
							ConvertNumber(sSameTypeString[j],TempNumResult,false);//小数点前值读
							strcat(sResult,TempNumResult);
							strcat(sResult,"点");
							ConvertNumber(sSameTypeString[j+2],TempNumResult,true);//小数点后码读
							strcat(sResult,TempNumResult);
							j=j+2;
							CurrentConvert=true;
						}
					}
					else if(strcmp(sSameTypeString[j+1],"/")==0||strcmp(sSameTypeString[j+1],"／")==0)
					{
						//９／１０/m
						ConvertNumber(sSameTypeString[j+2],TempNumResult,false);//次序要换一下
						strcat(sResult,TempNumResult);
						strcat(sResult,"分之");
						ConvertNumber(sSameTypeString[j],TempNumResult,false);
						strcat(sResult,TempNumResult);
						j=j+2;
						CurrentConvert=true;
					}
					else if(strcmp(sSameTypeString[j+1],":")==0||strcmp(sSameTypeString[j+1],"∶")==0||strcmp(sSameTypeString[j+1],"：")==0)
					{
						//１∶２．４/m 
						//只转换到:，后面进入循环即可。
						ConvertNumber(sSameTypeString[j],TempNumResult,false);//次序要换一下
						strcat(sResult,TempNumResult);
						strcat(sResult,"比");
						j=j+1;
						CurrentConvert=true;
					}
				}
				//７％
				else if((j+1<time)&&(strcmp(sSameTypeString[j+1],"％")==0||strcmp(sSameTypeString[j+1],"%")==0))
				{
					strcat(sResult,"百分之");
					ConvertNumber(sSameTypeString[j],TempNumResult,false);
					strcat(sResult,TempNumResult);
					j=j+1;
					CurrentConvert=true;
				}
				////７‰
				else if((j+1<time)&&strcmp(sSameTypeString[j+1],"‰")==0)
				{
					strcat(sResult,"千分之");
					ConvertNumber(sSameTypeString[j],TempNumResult,false);
					strcat(sResult,TempNumResult);
					j=j+1;
					CurrentConvert=true;
				}
				//不满足上述情况
				if(CurrentConvert==false)
				{
					ConvertNumber(sSameTypeString[j],TempNumResult,false);
					strcat(sResult,TempNumResult);
				}
			}
			//几数第上成±—＋∶·．／+-./○百千 万亿佰仟％‰%
			else if(strcmp(sSameTypeString[j],"±")==0)				
				strcat(sResult,"加减");
			else if(strcmp(sSameTypeString[j],"．")==0||strcmp(sSameTypeString[j+1],"·")==0||strcmp(sSameTypeString[j+1],".")==0)
			{
				strcat(sResult,"点");
				if((j+1<time)&&(ctype[j+1]==T_NUM))
				{
					ConvertNumber(sSameTypeString[j+1],TempNumResult,true);
					strcat(sResult,TempNumResult);
					j++;
				}
			}
			else if((strcmp(sSameTypeString[j],"＋")==0)||(strcmp(sSameTypeString[j],"+")==0))
				strcat(sResult,"加");
			//汉字比如：○百千万亿佰仟几数第上成
			else
				strcat(sResult,sSameTypeString[j]);
		}
		strcat(sResult,"/");
		strcat(sResult,sTag);
	}
	return true;
}
bool CNorm::ConvertNumber(char* sWord,char* Result,bool type)
{
	//type纪录那种转换形势读法，不同，莫认为数字量,忽略最先的0，true为1990 年，和.056小数点后数字读法,要读0
	Result[0]=0;
//	memset(sResult,0,WORD_MAXLENGTH);

	char subout[WORD_MAXLENGTH][3];
	int lenth,tmplen;
	int nullend;//末尾全0的位置
	int nullstart;//起始全0的位置
	
	char Number[3];
	memset(Number,0,3);

//	char tmp[3];
//	memset(tmp,0,3);
//	char Number_chn[]= "０１２３４５６７８９";
//	char out[]="零一二三四五六七八九";
//	char danwei[]="万千百十亿千百十万千百十";//最多转换13位！	
//０	163 - 176
//１	163 - 177
//２	163 - 178
//３	163 - 179
//４	163 - 180
//５	163 - 181
//６	163 - 182
//７	163 - 183
//８	163 - 184
//９	163 - 185

	//得到个数lenth个，输出字串
	lenth=strlen(sWord)/2;

	for(int j=0;j<lenth;j++)
	{
//		tmp[0]=snum[2*j];
//		tmp[1]=snum[2*j+1];
//		tmp[3]=0;
//		i=0;
//		char* p;
//		p=CC_Find(Number_chn,temp);
//		if(p==NULL)
//			return false;
//		else
//			strcpy(subout,NumOut[(p-Number_chn)/2]);
		int order=(unsigned char)sWord[2*j+1]-176;
		if(order<0||order>9)
			return false;
		else
			strcpy(subout[j],NumOut[order]);

	}

	if(type)
	{
		for(int j=0;j<lenth;j++)
			strcat(Result,subout[j]);
//		resultlen=2*lenth;
	}
	else//情况有：1020222;200;(后全零）;04(表月份）;15(不读一十五,读十五）
	{
		if(lenth>13)
		{
			for(int j=0;j<lenth;j++)
				strcat(Result,subout[j]);
//			resultlen=2*lenth;
//			return false;
		}
		nullend=lenth-1;
		nullstart=0;
		if(memcmp(subout[lenth-1],"零",2)==0)//检查末尾0的个数
		{
			for(int j=lenth-2;j>=0;j--)
			{
				if(memcmp(subout[j],"零",2)!=0)
				{
					nullend=j;
					break;
				}
			}
		}
		if(memcmp(subout[0],"零",2)==0)//检查起始0的个数
		{
			for(int j=1;j<nullend;j++)
			{
				if(memcmp(subout[j],"零",2)!=0)
				{
					nullstart=j;
					break;
				}
			}
		}
		tmplen=lenth-nullstart;
		for(int j=nullstart;j<=nullend;j++)//从起始不是0的位置读到末尾全零前, 如果是0部读单位
		{
			strcat(Result,subout[j]);
//			resultlen+=2;
			if(memcmp(subout[j],"零",2)!=0)
			{
				if(tmplen==13)
					strcat(Result,"万");
				else if(tmplen==12)
					strcat(Result,"千");
				else if(tmplen==11)
					strcat(Result,"百");
				else if(tmplen==10)
					strcat(Result,"十");
				else if(tmplen==9)
					strcat(Result,"亿");
				else if(tmplen==8)
					strcat(Result,"千");
				else if(tmplen==7)
					strcat(Result,"百");
				else if(tmplen==6)
					strcat(Result,"十");
				else if(tmplen==5)
					strcat(Result,"万");
				else if(tmplen==4)
					strcat(Result,"千");
				else if(tmplen==3)
					strcat(Result,"百");
				else if(tmplen==2)
					strcat(Result,"十");
	//			resultlen+=2;
			}
			tmplen--;
		}
		if((lenth==2)&&(memcmp(subout[0],"一",2)==0))
		{
			strcpy(Result,"十");
//			resultlen+=2;
			if(memcmp(subout[1],"零",2)!=0)
			{
//				resultlen+=2;
				strcat(Result,subout[1]);
			}
		}
	}
//	int templen=strlen(Result);
	Result[strlen(Result)]=0;
	return true;
}
int CNorm::GetCharType(unsigned char* sChar)
{
	if(*sChar<128)
	{
		return T_SINGLE;
	}
	else if(*sChar==163&&*(sChar+1)>175&&*(sChar+1)<186)
		return T_NUM;
	else if(*sChar==161||*sChar==163)
		return T_DELIMITER;
	else if(*sChar>=170)
		return T_CHINESE;
	else
		return T_OTHER;
}
bool CNorm::GetSameType(char* sChar,int* lenth,int* type)
{
	int charlenth;
	char* p;
	p=sChar;
	int firsttype=GetCharType((unsigned char*)p);
	*type=firsttype;
	if(firsttype==T_SINGLE)
		charlenth=1;
	else
		charlenth=2;
	while(*p!=0)
	{
		int type=GetCharType((unsigned char*)p);
		if(firsttype==type)
			p+=charlenth;
		else
			break;
		
	}
	*lenth=p-sChar;
	return true;
}

