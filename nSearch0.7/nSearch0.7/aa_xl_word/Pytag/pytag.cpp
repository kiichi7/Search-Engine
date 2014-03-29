#include "StdAfx.h"
#include "pytag.h"
#include <memory.h>
#include <string.h>
#include <math.h>
#include <stdlib.h>
CPytag::CPytag()
{	
}
CPytag::~CPytag()
{
}
bool CPytag::load()
{
	FILE *fp;
//	fp=fopen("d:\\mywork\\变调注音\\pydctinx.txt","rt");
	fp=fopen("PinyinData\\pydctinx.txt","rt");
	if(fp==NULL)
		return false;

    int h=0;
	for(  h=0;h<SCKNDNUM;h++)
	{
		fscanf(fp,"%s %d",sckndx[h].word,&sckndx[h].wordid);
		if(h>0)
			sckndx[h-1].rnum=sckndx[h].wordid-sckndx[h-1].wordid;		
	}
	sckndx[SCKNDNUM-1].rnum=SCKWDNUM-sckndx[h-1].wordid;

	fp=fopen("PinyinData\\pydct_bd.txt","rt");
	if(fp==NULL)
		return false;
	for(h=0;h<SCKWDNUM;h++)
	{
		fscanf(fp,"%s %s %d %s %s",ci[h],cixing[h],&prop[h],pinyin[h],bdpinyin[h]);
//		cilen[h]=strlen(ci[h]);
		cixing[h][1]='\0';
	}
	fp=fopen("PinyinData\\mwdpy_bd.txt","rt");
	if(fp==NULL)
		return false;
	char temp[9];
	int no;
	for(h=0;h<DUOYINNUM;h++)
	{
		fscanf(fp,"%d %s %s %d",&mwdpyid[h],temp,mwdpy[h],&no);
	}
	fp=fopen("PinyinData\\leftpy_bd.txt","rt");
	if(fp==NULL)
		return false;
	for(h=0;h<LEFTPY;h++)
	{
		fscanf(fp,"%s %s",leftzi[h],leftyin[h]);
	}
	fp=fopen("PinyinData\\namepy_bd.txt","rt");
	if(fp==NULL)
		return false;
	for(h=0;h<NMPY;h++)
	{
		fscanf(fp,"%s %s",fname[h],fnmpy[h]);
	}
	return true;
	fcloseall();
}
/*********************************************************************
 *
 *  Func Name  : BinarySearch
 *
 *  Description: Lookup the word in the mwdpy.txt for pinyin prop of 2;
                  which word+tag can't decide the pinyin.
 *  Parameters : leftwd: the word; 
 *
 *  Returns    : the index value
 *  Author     : Wang Zhifu
 *  History    : 
 *              1.create 2003-1-25
 *********************************************************************/	
bool CPytag::BinarySerch(char* zword,int* ziid)
{
	int left=0,mid,right=SCKNDNUM-1,cmp;
	char hp[3];
	char tempword[3];
	convert(tempword,zword,2);
	// 开始时将整个数组作为查找范围
	// left,right分别是查找范围内第一个和最后一个元素
	while(left<=right) { // 当前查找范围不为空
		mid = (left+right)/2; // 计算查找范围中间元素的序号
		convert(hp,sckndx[mid].word,2); // 取中间元素
		cmp=strncmp(hp,tempword,2); // 比较该元素的字跟欲查找的字
		if(cmp<0)  // 如果中间元素字较小，把右半段作为新的查找范围
			left=mid+1;
		else  // 如果中间元素字较大，把左半段作为新的查找范围
			if(cmp>0) 
				right=mid-1;
			else {  // 如果相等，查找成功
				*ziid=mid;
				return TRUE;
			}
	}
	*ziid=left;
	return FALSE;
}
/*********************************************************************
 *
 *  Func Name  : convert
 *
 *  Description: couse strncmp is sometime didn't works, didn't know why, 
                 so use this method to check and get the right result.

 *
 *  Parameters :  temppd :return word;
                 tempwd: source word;len :lenth
 *    
 *  Author     : Wang Zhifu
 *  History    : 
 *              create 2003-3-16
 *********************************************************************/	
void CPytag::convert(char* temppd,char* tempwd,int len)
{
	for(int order=0;order<len;order++)
		temppd[order]=(unsigned char)tempwd[order];
	temppd[len]='\0';
}
/*********************************************************************
 *
 *  Func Name  : GetDictWordPinyin
 *
 *  Description:  get a word(in the dictionary) pinyin tagged.
 *               considerate a biandiao pinyin.
 *
 *  Parameters : wd: the word to be tagged with pinyin
 *               tag:  the pinyin of the word
 *               id: the first chinese char 's id in index file
 *    
 *  Author     : Wang Zhifu
 *  History    : 
 *              create 2003-3-16
 *********************************************************************/
bool CPytag::GetDictWordPinyin(char* wd,char* tag,int id,char* npinyin,char* bdpy)
{
	bool findword=false;
	bool getpy=false;
	int wordno;//the word index for the word prob 2, when looking up mwdpy, as index.
	char temppy[WORD_MAXLENGTH*6],tempbdpy[WORD_MAXLENGTH*6];
	unsigned int lenth=strlen(wd);
	char words[WORD_MAXLENGTH],tempci[WORD_MAXLENGTH];
	convert(words,wd,lenth);
	memset(temppy,0,WORD_MAXLENGTH*6);
	memset(tempbdpy,0,WORD_MAXLENGTH*6);
	for(int j=sckndx[id].wordid;j<sckndx[id].rnum+sckndx[id].wordid;j++)
	{
		if(strlen(ci[j])==lenth)
		{
			convert(tempci,ci[j],lenth);
			if(strncmp(tempci,words,lenth)==0)
			{
				if(!findword)
				{
					findword=true;
					wordno=j;
				}
				if(prop[j]==0)//词能确定
				{
					getpy=true;
					strcpy(npinyin,pinyin[j]);
					strcpy(bdpy,bdpinyin[j]);
					break;
				}
				else if(prop[j]==1)//词+词性
				{
					if(strlen(tag)==2)
					{
						
						if(*(tag+1)=='g')//语素词.对ag,dg,ng,vg,tg先查找g的读音，不能找到查找相应另一个词性的。不能找到找其其他词性的都因
						{
							if(memcmp(cixing[j],"g",1)==0)
							{
								getpy=true;
								strcpy(npinyin,pinyin[j]);
								strcpy(bdpy,bdpinyin[j]);
								break;
							}
							else if(memcmp(tag,cixing[j],1)==0)//g前词性作为备选	
							{
								strcpy(temppy,pinyin[j]);
								strcpy(tempbdpy,bdpinyin[j]);
							}
						}
						else if(*tag=='n')
						{
							if((*(tag+1)=='r')&&(strlen(wd)<3))//人姓只能单独处理。
							{
								for(int sum=0;sum<NMPY;sum++)
								{
									if(lenth==strlen(fname[sum]))
									{
										if(strcmp(wd,fname[sum])==0){
											strcpy(npinyin,fnmpy[sum]);
											strcpy(bdpy,fnmpy[sum]);
											getpy=true;
											break;
										}
									}
								}
								if(getpy)
									break;
								else if(memcmp(tag,cixing[j],1)==0)//n作为备选	
								{
									strcpy(temppy,pinyin[j]);
									strcpy(tempbdpy,bdpinyin[j]);
								}
							}
							else//其他视为名词
							{
								if(memcmp(cixing[j],"n",1)==0)
								{
									getpy=true;
									strcpy(npinyin,pinyin[j]);
									strcpy(bdpy,bdpinyin[j]);
									break;
								}
								else if(tag[1]==cixing[j][0])//作为备选	
								{
									strcpy(temppy,pinyin[j]);
									strcpy(tempbdpy,bdpinyin[j]);
								}
								
							}
						}
						else//其他都取前一个
						{
							if(memcmp(tag,cixing[j],1)==0)																					
							{
								getpy=true;
								strcpy(npinyin,pinyin[j]);
								strcpy(bdpy,bdpinyin[j]);
								break;
							}
							else if(tag[1]==cixing[j][0])//作为备选	
							{
								strcpy(temppy,pinyin[j]);
								strcpy(tempbdpy,bdpinyin[j]);
							}
							
						}
					}
					else if(strcmp(tag,cixing[j])==0)//一级标注,且词本身不能确定读音无备选，最大概率
					{
						getpy=true;
						strcpy(npinyin,pinyin[j]);
						strcpy(bdpy,bdpinyin[j]);
						break;
					}
					
				}
				
			}
			else if(strncmp(tempci,words,lenth)>0)//如果后面大了，break;
			{
				break;
			}
			
		}
		else if(findword)
			break;
	}		
	if((findword)&&(!getpy))//词表中有,但是没有找到读音
	{
		if(strlen(temppy)!=0)//存在备选拼音
		{
			strcpy(npinyin,temppy);
			strcpy(bdpy,tempbdpy);
		}
		else//词表中有，根据词和词性（此时有两种情况属性2或者属性1但是多音词的词性在词典中不存在）都不能确定其读音，进行多音最大概率查找,二分查找
		{
			int left=0,mid,right=DUOYINNUM-1;
			// 开始时将整个数组作为查找范围
			// left,right分别是查找范围内第一个和最后一个元素
			while(left<=right) { // 当前查找范围不为空
				mid = (left+right)/2; // 计算查找范围中间元素的序号
				if(mwdpyid[mid]<wordno)  // 如果中间元素字较小，把右半段作为新的查找范围
					left=mid+1;
				else  // 如果中间元素字较大，把左半段作为新的查找范围
					if(mwdpyid[mid]>wordno) 
						right=mid-1;
					else {  // 如果相等，查找成功
						strcpy(npinyin,mwdpy[mid]);
						strcpy(bdpy,mwdpy[mid]);
						if(wordno==59554)//59554一晃 yi1huang3；变调。
						{
							strcpy(bdpy,"yi4_huang3_");
						}
						break;
					}
			}
			id=left;
		}
		
	}
	if(!findword)//词典种没有找到,有两种情况，一种是词典没有该字，但是索引中有（比如词典中有镢头，但是词典中没有镢）另一种就是词典中没有但是该字肯定有
		return false;
	else 
		return true;
}
/*********************************************************************
 *
 *  Func Name  : GetWordPinyin
 *
 *  Description:  get a word(in or out the dictionary) pinyin tagged.
 *               considerate a biandiao pinyin.
 *
 *  Parameters : wd: the word to be tagged with pinyin
 *               tag:  the pinyin of the word
 *               npinyin: pinyin
 *              bdpy: bian diao pinyin
 *    
 *  Author     : Wang Zhifu
 *  History    : 
 *              create 2003-3-16
 *********************************************************************/
bool CPytag::GetWordPinyin(char* wd,char* tag,char* npinyin,char* bdpy)
{
	char zi[3];
	zi[0]=wd[0];
	zi[1]=wd[1];
	zi[2]='\0';
	int ziid=0;//return value for the dictionary index search
	int leftziid;//return value for left zi id

	//二分查找wd				
	if(BinarySerch(zi,&ziid))
	{
		if(GetDictWordPinyin(wd,tag,ziid,npinyin,bdpy))
			return true;
		else
		{
			if(!GetDictWordPinyin(zi,tag,ziid,npinyin,bdpy))
			{
				if(BiSearch(zi,&leftziid))
				{
					strcpy(npinyin,leftyin[leftziid]);
					strcpy(bdpy,leftyin[leftziid]);
				}
				else
				{
					strcpy(npinyin,"PPP_");
					strcpy(bdpy,"PPP_");
				}
			}
			if(strlen(wd)>2)
			{
				char temppy[9],tempbdpy[9];
				memset(temppy,0,9);
				memset(tempbdpy,0,9);
				for(unsigned int h=2;h<strlen(wd);h++)
				{
//					zi[0]=wd[h];
//					h++;
//					zi[1]=wd[h];
//					zi[2]='\0';
					zi[0]=wd[h];
					if(zi[0]>0)
						continue;
					else
					{
						h++;
						zi[1]=wd[h];
						zi[2]='\0';
					}
					unsigned char ch=(unsigned char)zi[0];
					if(ch<170)
					{
						
						if(memcmp(zi,"○",2)==0)
						{
							strcat(npinyin,"ling2_");
							strcat(bdpy,"ling2_");
						}
						else
							continue;
					}
					else if(BinarySerch(zi,&ziid))
					{
						if(GetDictWordPinyin(zi,tag,ziid,temppy,tempbdpy))
						{
							strcat(npinyin,temppy);
							strcat(bdpy,tempbdpy);
						}
						else
						{
							if(BiSearch(zi,&leftziid))
							{
								strcat(npinyin,leftyin[leftziid]);
								strcat(bdpy,leftyin[leftziid]);
							}
							else
							{
								strcat(npinyin,"PPP_");
								strcat(bdpy,"PPP_");
							}
						}
						
					}
					else//字典索引中没有的汉字
					{
						
						if(BiSearch(zi,&leftziid))
						{
							strcat(npinyin,leftyin[leftziid]);
							strcat(bdpy,leftyin[leftziid]);
						}
						else
						{
							strcat(npinyin,"PPP_");
							strcat(bdpy,"PPP_");
						}
					}
				}
				//说明是未登录词，需要考虑33变调！
				char TempPinyin[WORD_MAXLENGTH*9];
				strcpy(bdpy,WordRule33to32(wd,bdpy,TempPinyin,false));
			}

		}
	}
	else//字典索引中没有的汉字
	{
		unsigned char ch=(unsigned char)zi[0];
		if(ch<170)
		{
			if(memcmp(zi,"○",2)==0)//如果是其他字符，不注音;
			{
				strcpy(npinyin,"ling2_");
				strcpy(bdpy,"ling2_");
			}
		}
		else if(BiSearch(zi,&leftziid))
		{
			strcpy(npinyin,leftyin[leftziid]);
			strcpy(bdpy,leftyin[leftziid]);
		}
		else
		{
				strcpy(npinyin,"PPP_");
				strcpy(bdpy,"PPP_");
		}
		
		if(strlen(wd)>2)
		{
			char temppy[9],tempbdpy[9];
			memset(temppy,0,9);
			memset(tempbdpy,0,9);
			for(unsigned int h=2;h<strlen(wd);h++)
			{
				zi[0]=wd[h];
				h++;
				zi[1]=wd[h];
				zi[2]='\0';
				unsigned char ch=(unsigned char)zi[0];
				if(ch<170)
				{

					if(memcmp(zi,"○",2)==0)
					{
						strcat(npinyin,"ling2_");
						strcat(bdpy,"ling2_");
					}
					else
						continue;
				}
				else if(BinarySerch(zi,&ziid))
				{
					if(GetDictWordPinyin(zi,tag,ziid,temppy,tempbdpy))
					{
						strcat(npinyin,temppy);
						strcat(bdpy,tempbdpy);
					}
					else
					{
						if(BiSearch(zi,&leftziid))
						{
							strcat(npinyin,leftyin[leftziid]);
							strcat(bdpy,leftyin[leftziid]);
						}
						else
						{
							strcat(npinyin,"PPP_");
							strcat(bdpy,"PPP_");
						}
					}

				}
				else//字典索引中没有的汉字
				{
					
					if(BiSearch(zi,&leftziid))
					{
						strcat(npinyin,leftyin[leftziid]);
						strcat(bdpy,leftyin[leftziid]);
					}
					else
					{
						strcat(npinyin,"PPP_");
						strcat(bdpy,"PPP_");

					}
				}
			}
			//说明是未登录词，需要考虑33变调！
			char TempPinyin[WORD_MAXLENGTH*9];
			strcpy(bdpy,WordRule33to32(wd,bdpy,TempPinyin,false));
		}
	}
	return true;
}
/*********************************************************************
 *
 *  Func Name  : BiSearch
 *
 *  Description: Lookup the word int the leftpy.txt for pinyin
 *  Parameters : leftwd: the word
 *
 *  Returns    : the left zi id;
 *  Author     : Wang Zhifu
 *  History    : 
 *              1.create 2003-1-25
 *********************************************************************/				
bool CPytag::BiSearch(char* leftwd,int* leftziid)
{
	*leftziid=0;
	int left=0,mid,right=LEFTPY-1,cmp;
	char hp[3];
	char tempword[3];
	convert(tempword,leftwd,3);
	// 开始时将整个数组作为查找范围
	// left,right分别是查找范围内第一个和最后一个元素
	while(left<=right) { // 当前查找范围不为空
		mid = (left+right)/2; // 计算查找范围中间元素的序号
		convert(hp,leftzi[mid],3); // 取中间元素
		cmp=strncmp(hp,tempword,2); // 比较该元素的字跟欲查找的字
		if(cmp<0)  // 如果中间元素字较小，把右半段作为新的查找范围
			left=mid+1;
		else  // 如果中间元素字较大，把左半段作为新的查找范围
			if(cmp>0) 
				right=mid-1;
			else {  // 如果相等，查找成功
				*leftziid=mid;
				return TRUE;
			}
	}
	*leftziid=left;
	return FALSE;
}

/*********************************************************************
 *
 *  Func Name  : WordRule33to32
 *
 *  Description:  Use the 33 to 23 rule on  the wordpinyin .
 *               every pinyin is segged with '_' .
 *               everyword have a biandiao pinyin.
 *
 *  Parameters : filename: the file name to change the dict.
 *    
 *  Returns    : bool
 *  Author     : Wang Zhifu
 *  History    : 
 *              create 2004-3-16
 *********************************************************************/

char* CPytag::WordRule33to32(char* sWord,char* sPinyin,char* rPinyin,bool ExistIndict)
{
	rPinyin[0]=0;
	strcpy(rPinyin,sPinyin);
	CString py(sPinyin);
	int num[WORD_MAXLENGTH],postion[WORD_MAXLENGTH];
	int PinyinNum=0;//一个词的拼音个数
	int i=0;
	while(!py.IsEmpty())
	{
		int pos=py.FindOneOf("12345");//position of the 12345 appeared in the pinyinstring.
		if(pos<0)//卡拉ok最后虽然有注音，但是没有声调的！
			break;
		num[i]=py[pos];
		if(i>0)
			postion[i++]=pos+postion[i-1]+1;
		else
			postion[i++]=pos;
		py=py.Mid(pos+1);
		PinyinNum++;		
	}
	if(!ExistIndict)//未登录词，不考虑那么复杂，33to23
	{
		for(i=1;i<PinyinNum;i++)
		{
			if((num[i]=='3')&&(num[i-1]=='3'))
				rPinyin[postion[i-1]]='2';
		}
	}
	else 
	{
		//pinyintagging, needn't any more
		/*
		if(PinyinNum==4)//4字词
		{
			
			if((num[0]=='3')&&(num[1]=='3')&&(num[2]=='3')&&(num[3]=='3'))
			{
				char First[5],Last[5],Middle[5];
				strncpy(First,sWord,4);//取前两个字,
				First[4]=0;
				strncpy(Last,sWord+4,4);//取后面两个字
				Last[4]=0;
				strncpy(Middle,sWord+2,4);//取中间两个字
				Middle[4]=0;

				if(m_dictCore.IsExist(First,-1)||m_dictCore.IsExist(Last,-1))//如果前或者后面两个字成词
				{
					//change 3333 to 2323,e.g. 我党小组
					rPinyin[postion[0]]='2';
					rPinyin[postion[2]]='2';
				}
				else if(m_dictCore.IsExist(Middle,-1))//中间成词。
				{
					//change 3333 to 3223,e.g. 党小组里
					rPinyin[postion[1]]='2';
					rPinyin[postion[2]]='2';
				}
				else
				{
					//change 3333 to 2223,e.g. 党小组里
					rPinyin[postion[0]]='2';
					rPinyin[postion[1]]='2';
					rPinyin[postion[2]]='2';
				}
			}
		}
		else if(PinyinNum==3)//3字词
		{
			if((num[0]=='3')&&(num[1]=='3')&&(num[2]=='3'))
			{
				char First[5],Last[5];
				strncpy(First,sWord,4);//取前两个字,
				First[4]=0;
				strncpy(Last,sWord+2,4);//取后面两个字
				Last[4]=0;
				if(m_dictCore.IsExist(First,-1))//如果前面两个字成词
				{
					//change 333 to 223,e.g. 管理组
					rPinyin[postion[0]]='2';
					rPinyin[postion[1]]='2';
				}
				else if(m_dictCore.IsExist(Last,-1))
				{
					//change 333 to 323,e.g. 党小组
					rPinyin[postion[0]]='2';
					rPinyin[postion[1]]='2';
				}
			}
		}
		else if(PinyinNum==2)//2字词
		{	
			if((num[0]=='3')&&(num[1]=='3')&&(num[2]=='3'))
				rPinyin[postion[0]]='2';//33 to 23
		}
		for(i=1;i<PinyinNum;i++)//to ensure that there other conditions to convert 33 to23
		{
			if((num[i]=='3')&&(num[i-1]=='3'))
				rPinyin[postion[i-1]]='2';
		}
		*/
	}

	return rPinyin;
}
/*********************************************************************
 *
 *  Func Name  : SentenceBianDiaotag
 *
 *  Description:  get sentence pinyin tagged.
 *               Considerate a biandiao pinyin.
 *
 *  Parameters : filename: the file name to change the dict.
 *    
 *  Returns    : bool
 *  Author     : Wang Zhifu
 *  History    : 
 *              create 2004-3-18
 *********************************************************************/
bool CPytag::SentenceBianDiaotag(char* senten,char* sPinyin, char* rBDPinyin)
{
	char zi[3];
	int l=strlen(senten);
	char ChineseWord[WORD_MAXLENGTH];
//	memset(py,0,MAX_PYSENTEN);
	sPinyin[0]=0;
	rBDPinyin[0]=0;
	char npinyin[WORD_MAXLENGTH*9];
	char bdpy[WORD_MAXLENGTH*9];
	bool HaveWordNo=false;
	bool HaveTone3=false;
//	senten[l-1]='\0';//去掉末尾的回车富
	CString s(senten);
	s.TrimLeft();
	s.TrimRight();
	while(!s.IsEmpty()) {//空行不处理
		npinyin[0]=0;
		bdpy[0]=0;
		CString w,t;
		w=s.SpanExcluding(" ");// 函数的参数是一个空格，假定分词语料是以空格作为分界符
		s=s.Mid(w.GetLength());
		s.TrimLeft();
		int i=w.Find('/');
		if(i<0) {
//			continue;
//			return false;//
			t.Format("nx");//
		}
		else//
		{//
		t=w.Mid(i+1);
		w=w.Left(i);
		}//
		if(strcmp("w",t)==0)//标点,不许注音
		{
			strcat(sPinyin,const_cast<char*>((LPCSTR)w));
			strcat(sPinyin," ");
			strcat(rBDPinyin,const_cast<char*>((LPCSTR)w));
			strcat(rBDPinyin," ");
			HaveWordNo=false;
			HaveTone3=false;
			continue;
		}
		else
		{
			int k=0;
			for(int j=0;j<w.GetLength();j++)
			{
				
				unsigned char ch=(unsigned char) w[j];
				if(ch<128)//西文字符删除,如:[
					continue;
				else if(ch>169)
				{
					ChineseWord[k++]=w[j++];
					ChineseWord[k++]=w[j];
				}
				else//中文删除，如：任命中间的圆点,○除外
				{
					zi[0]=w[j];
					zi[1]=w[j+1];
					zi[2]='\0';
					if(memcmp(zi,"○",2)==0)
					{
						ChineseWord[k++]=w[j++];
						ChineseWord[k++]=w[j];
					}
					else
						j++;
				}
			}
			ChineseWord[k]=0;
			i=t.Find(']');
			if(i>0)
				t=t.Left(i);//有]采用小单位作为词性标记
			//以下为词典搜索过程
			if(strlen(ChineseWord)!=0)//可能/x的已经删除。不许注音
			{
				GetWordPinyin(ChineseWord,(char*)((LPCSTR)t),npinyin,bdpy);
			}
		}
		if(HaveTone3)//last word' end is tone 3; rule 33 for sentence;
		{
			int tone;
			CheckFirstTone(bdpy,&tone);
			if(tone=='3')//33 to 23;
			{
				rBDPinyin[strlen(rBDPinyin)-2]='2';//change 3 to 2;
			}
		}
		if(HaveWordNo)//last word is "不", rule No for sentence;
		{
			int tone;
			CheckFirstTone(bdpy,&tone);
			if(tone=='4')//word "不" change tone 4 to 2 before 4
			{
				rBDPinyin[strlen(rBDPinyin)-2]='2';//change 3 to 2;
			}
		}
		if(bdpy[strlen(bdpy)-2]=='3')
			HaveTone3=true;
		else
			HaveTone3=false;
		if(strcmp(const_cast<char*>((LPCSTR)w),"不")==0)
			HaveWordNo=true;
		else
			HaveWordNo=false;

		//add new word's pinyin to pinyin string;
		strcat(sPinyin,npinyin);
		strcat(rBDPinyin,bdpy);
	}
	CString py(sPinyin);
	CString sbdpy(rBDPinyin);
	py.Replace("_"," ");
	sbdpy.Replace("_"," ");
	strcpy(sPinyin,const_cast<char*>((LPCSTR)py));
	strcpy(rBDPinyin,const_cast<char*>((LPCSTR)sbdpy));
	return true;
}
/*********************************************************************
 *
 *  Func Name  : CheckFirstTone
 *
 *  Description:  get first tone of a pinyin string.
 *
 *  Parameters : pinyin : the string; tone the tone;
 *    
 *  Returns    : bool
 *  Author     : Wang Zhifu
 *  History    : 
 *              create 2004-3-18
 *********************************************************************/
bool CPytag::CheckFirstTone(char* pinyin,int* tone)
{
	CString py(pinyin);
	int pos=py.FindOneOf("12345");//position of the 12345 appeared in the pinyinstring.
	if(pos<0)
		return false;
	*tone=py[pos];
	return true;
}
/*********************************************************************
 *
 *  Func Name  : ChangeDict
 *
 *  Description:  Change the pydctnew to biandiao pinyin file.
 *               In contrast, every pinyin is segged with '_' .
 *               Also everyword have a biandiao pinyin.
 *
 *  Parameters : filename: the file name to change the dict.
 *    
 *  Returns    : bool
 *  Author     : Wang Zhifu
 *  History    : 
 *              create 2004-3-16
                bug finded at 2004-3-19
				when fine "一","不"in the word, only find once!!!!!!
				if the words contains 2 一"or "不" in it,
				then the second can't be changed if need biandiao!~!!
				because 一 is sometimes pronounce itself yi1,not biandiao
				so it checked by hand by Zhang Jingjing.
				so I checked all words have two "不"s.
				so I didn't fix the bug after I have got the biandiao dict.
				although it can be easily fixed.
 *********************************************************************/
/*
void CPytag::ChangeDict(char* filename)
{
	m_dictCore.Load("data\\dictint.dat");

//	char Character[4][3];
//	int num[4],postion[4];
	int i=0;
	char* cp=NULL;
	char pyResult[25];
	memset(pyResult,0,25);
	char rfilename[100];
	strcpy(rfilename,filename);
	FILE *fp=fopen(filename,"rt");
	rfilename[strlen(filename)-4]=0;
	strcat(rfilename,"_bd.txt");
	FILE *fpbd=fopen(rfilename,"w+");

//	FILE *fp=fopen(filename,"rt");
//	filename[strlen(filename)-4]=0;
//	strcat(filename,"_bd.txt");
//	FILE *fpbd=fopen(filename,"w+");
//	fp=fopen("d:\\mywork\\注音\\pydctnew.txt","rt");
	for(int h=0;h<SCKWDNUM;h++)
	{
		fscanf(fp,"%s %s %s %d",ci[h],cixing[h],pinyin[h],&prop[h]);
		cilen[h]=strlen(ci[h]);
		cixing[h][1]='\0';
	}
	for(h=0;h<SCKWDNUM;h++)
	{
//33变调
		char rPinyin[WORD_MAXLENGTH*6];
		strcpy(bdpinyin[h],WordRule33to32(ci[h],pinyin[h],rPinyin,true));//use 33 rule;
//不的变调
		char* p=(CC_Find(ci[h],"不"));
		char* pCi;
		pCi=ci[h];
		if(p!=NULL)//有“不”
		{
			unsigned int Position=(p-pCi)/2;
			if(Position<cilen[h]/2-1)
			{
			CString py(bdpinyin[h]);
			int num[WORD_MAXLENGTH],postion[WORD_MAXLENGTH];
			int PinyinNum=0;//一个词的拼音个数
			i=0;
			while(!py.IsEmpty())
			{
				int pos=py.FindOneOf("12345");//position of the 12345 appeared in the pinyinstring.
				if(pos<0)//卡拉ok最后虽然有注音，但是没有声调的！
					break;
				num[i]=py[pos];
				if(i>0)
					postion[i++]=pos+postion[i-1]+1;
				else
					postion[i++]=pos;
				py=py.Mid(pos+1);
				PinyinNum++;
			}
			if(num[Position+1]=='4')//“不“在去声前,change it to 2;
			{
				bdpinyin[h][postion[Position]]='2';
			}
			}
		}
////一的变调
		p=(CC_Find(ci[h],"一"));
		if(p!=NULL)//有“一 ”
		{
			unsigned int Position=(p-pCi)/2;
			if(Position<cilen[h]/2-1)
			{
			CString py(bdpinyin[h]);
			int num[WORD_MAXLENGTH],postion[WORD_MAXLENGTH];
			int PinyinNum=0;//一个词的拼音个数
			i=0;
			while(!py.IsEmpty())
			{
				int pos=py.FindOneOf("12345");//position of the 12345 appeared in the pinyinstring.
				if(pos<0)//卡拉ok最后虽然有注音，但是没有声调的！
					break;
				num[i]=py[pos];
				if(i>0)
					postion[i++]=pos+postion[i-1]+1;
				else
					postion[i++]=pos;
				py=py.Mid(pos+1);
				PinyinNum++;
			}
			if(num[Position+1]=='4')//“一“在去声前,change it to 2;
			{
				bdpinyin[h][postion[Position]]='2';
			}
			else////“一“在去声前,change it to 4;
			{
				bdpinyin[h][postion[Position]]='4';
			}
			}
		}
		CString py(pinyin[h]);
		py.Replace("1","1_");
		py.Replace("2","2_");
		py.Replace("3","3_");
		py.Replace("4","4_");
		py.Replace("5","5_");
		CString pybd(bdpinyin[h]);
		pybd.Replace("1","1_");
		pybd.Replace("2","2_");
		pybd.Replace("3","3_");
		pybd.Replace("4","4_");
		pybd.Replace("5","5_");
		fprintf(fpbd,"%s %s %d %s %s\n",ci[h],cixing[h],prop[h],const_cast<char*>((LPCSTR)py),const_cast<char*>((LPCSTR)pybd));

	}
	fcloseall();

}
void CPytag::ChangeNameDict(char* filename)
{
	char rfilename[100];
	strcpy(rfilename,filename);
	FILE *fp=fopen(filename,"rt");
	rfilename[strlen(filename)-4]=0;
	strcat(rfilename,"_bd.txt");
	FILE *fpbd=fopen(rfilename,"w+");
//	fp=fopen("d:\\mywork\\注音\\pydctnew.txt","rt");
//	fp=fopen("d:\\mywork\\注音\\namepy.txt","rt");
	for(int h=0;h<NMPY;h++)
	{
		fscanf(fp,"%s %s",fname[h],fnmpy[h]);
		CString py(fnmpy[h]);
		py.Replace("1","1_");
		py.Replace("2","2_");
		py.Replace("3","3_");
		py.Replace("4","4_");
		py.Replace("5","5_");
		fprintf(fpbd,"%s %s\n",fname[h],const_cast<char*>((LPCSTR)py));

	}
	fcloseall();
}
void CPytag::ChangeLeftDict(char* filename)
{
	char rfilename[100];
	strcpy(rfilename,filename);
	FILE *fp=fopen(filename,"rt");
	rfilename[strlen(filename)-4]=0;
	strcat(rfilename,"_bd.txt");
	FILE *fpbd=fopen(rfilename,"w+");
//	fp=fopen("d:\\mywork\\注音\\pydctnew.txt","rt");
//	fp=fopen("d:\\mywork\\注音\\namepy.txt","rt");
	for(int h=0;h<LEFTPY;h++)
	{
		fscanf(fp,"%s %s",leftzi[h],leftyin[h]);
		CString py(leftyin[h]);
		py.Replace("1","1_");
		py.Replace("2","2_");
		py.Replace("3","3_");
		py.Replace("4","4_");
		py.Replace("5","5_");
		fprintf(fpbd,"%s %s\n",leftzi[h],const_cast<char*>((LPCSTR)py));

	}
	fcloseall();
}
void CPytag::ChangeMpDict(char* filename)
{
	char rfilename[100];
	strcpy(rfilename,filename);
	FILE *fp=fopen(filename,"rt");
	rfilename[strlen(filename)-4]=0;
	strcat(rfilename,"_bd.txt");
	FILE *fpbd=fopen(rfilename,"w+");
//	fp=fopen("d:\\mywork\\注音\\pydctnew.txt","rt");
//	fp=fopen("d:\\mywork\\注音\\namepy.txt","rt");
//	fp=fopen("d:\\mywork\\注音\\mwdpy.txt","rt");
	char temp[9];
	int no;
	for(int h=0;h<DUOYINNUM;h++)
	{
		fscanf(fp,"%d %s %s %d",&mwdpyid[h],temp,mwdpy[h],&no);
		CString py(mwdpy[h]);
		py.Replace("1","1_");
		py.Replace("2","2_");
		py.Replace("3","3_");
		py.Replace("4","4_");
		py.Replace("5","5_");
		fprintf(fpbd,"%d %s %s %d\n",mwdpyid[h],temp,const_cast<char*>((LPCSTR)py),no);
	}
	fcloseall();
}
*/