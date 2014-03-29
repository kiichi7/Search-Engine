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
//	fp=fopen("d:\\mywork\\���ע��\\pydctinx.txt","rt");
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
	// ��ʼʱ������������Ϊ���ҷ�Χ
	// left,right�ֱ��ǲ��ҷ�Χ�ڵ�һ�������һ��Ԫ��
	while(left<=right) { // ��ǰ���ҷ�Χ��Ϊ��
		mid = (left+right)/2; // ������ҷ�Χ�м�Ԫ�ص����
		convert(hp,sckndx[mid].word,2); // ȡ�м�Ԫ��
		cmp=strncmp(hp,tempword,2); // �Ƚϸ�Ԫ�ص��ָ������ҵ���
		if(cmp<0)  // ����м�Ԫ���ֽ�С�����Ұ����Ϊ�µĲ��ҷ�Χ
			left=mid+1;
		else  // ����м�Ԫ���ֽϴ󣬰�������Ϊ�µĲ��ҷ�Χ
			if(cmp>0) 
				right=mid-1;
			else {  // �����ȣ����ҳɹ�
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
				if(prop[j]==0)//����ȷ��
				{
					getpy=true;
					strcpy(npinyin,pinyin[j]);
					strcpy(bdpy,bdpinyin[j]);
					break;
				}
				else if(prop[j]==1)//��+����
				{
					if(strlen(tag)==2)
					{
						
						if(*(tag+1)=='g')//���ش�.��ag,dg,ng,vg,tg�Ȳ���g�Ķ����������ҵ�������Ӧ��һ�����Եġ������ҵ������������ԵĶ���
						{
							if(memcmp(cixing[j],"g",1)==0)
							{
								getpy=true;
								strcpy(npinyin,pinyin[j]);
								strcpy(bdpy,bdpinyin[j]);
								break;
							}
							else if(memcmp(tag,cixing[j],1)==0)//gǰ������Ϊ��ѡ	
							{
								strcpy(temppy,pinyin[j]);
								strcpy(tempbdpy,bdpinyin[j]);
							}
						}
						else if(*tag=='n')
						{
							if((*(tag+1)=='r')&&(strlen(wd)<3))//����ֻ�ܵ�������
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
								else if(memcmp(tag,cixing[j],1)==0)//n��Ϊ��ѡ	
								{
									strcpy(temppy,pinyin[j]);
									strcpy(tempbdpy,bdpinyin[j]);
								}
							}
							else//������Ϊ����
							{
								if(memcmp(cixing[j],"n",1)==0)
								{
									getpy=true;
									strcpy(npinyin,pinyin[j]);
									strcpy(bdpy,bdpinyin[j]);
									break;
								}
								else if(tag[1]==cixing[j][0])//��Ϊ��ѡ	
								{
									strcpy(temppy,pinyin[j]);
									strcpy(tempbdpy,bdpinyin[j]);
								}
								
							}
						}
						else//������ȡǰһ��
						{
							if(memcmp(tag,cixing[j],1)==0)																					
							{
								getpy=true;
								strcpy(npinyin,pinyin[j]);
								strcpy(bdpy,bdpinyin[j]);
								break;
							}
							else if(tag[1]==cixing[j][0])//��Ϊ��ѡ	
							{
								strcpy(temppy,pinyin[j]);
								strcpy(tempbdpy,bdpinyin[j]);
							}
							
						}
					}
					else if(strcmp(tag,cixing[j])==0)//һ����ע,�Ҵʱ�����ȷ�������ޱ�ѡ��������
					{
						getpy=true;
						strcpy(npinyin,pinyin[j]);
						strcpy(bdpy,bdpinyin[j]);
						break;
					}
					
				}
				
			}
			else if(strncmp(tempci,words,lenth)>0)//���������ˣ�break;
			{
				break;
			}
			
		}
		else if(findword)
			break;
	}		
	if((findword)&&(!getpy))//�ʱ�����,����û���ҵ�����
	{
		if(strlen(temppy)!=0)//���ڱ�ѡƴ��
		{
			strcpy(npinyin,temppy);
			strcpy(bdpy,tempbdpy);
		}
		else//�ʱ����У����ݴʺʹ��ԣ���ʱ�������������2��������1���Ƕ����ʵĴ����ڴʵ��в����ڣ�������ȷ������������ж��������ʲ���,���ֲ���
		{
			int left=0,mid,right=DUOYINNUM-1;
			// ��ʼʱ������������Ϊ���ҷ�Χ
			// left,right�ֱ��ǲ��ҷ�Χ�ڵ�һ�������һ��Ԫ��
			while(left<=right) { // ��ǰ���ҷ�Χ��Ϊ��
				mid = (left+right)/2; // ������ҷ�Χ�м�Ԫ�ص����
				if(mwdpyid[mid]<wordno)  // ����м�Ԫ���ֽ�С�����Ұ����Ϊ�µĲ��ҷ�Χ
					left=mid+1;
				else  // ����м�Ԫ���ֽϴ󣬰�������Ϊ�µĲ��ҷ�Χ
					if(mwdpyid[mid]>wordno) 
						right=mid-1;
					else {  // �����ȣ����ҳɹ�
						strcpy(npinyin,mwdpy[mid]);
						strcpy(bdpy,mwdpy[mid]);
						if(wordno==59554)//59554һ�� yi1huang3�������
						{
							strcpy(bdpy,"yi4_huang3_");
						}
						break;
					}
			}
			id=left;
		}
		
	}
	if(!findword)//�ʵ���û���ҵ�,�����������һ���Ǵʵ�û�и��֣������������У�����ʵ�������ͷ�����Ǵʵ���û���㣩��һ�־��Ǵʵ���û�е��Ǹ��ֿ϶���
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

	//���ֲ���wd				
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
						
						if(memcmp(zi,"��",2)==0)
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
					else//�ֵ�������û�еĺ���
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
				//˵����δ��¼�ʣ���Ҫ����33�����
				char TempPinyin[WORD_MAXLENGTH*9];
				strcpy(bdpy,WordRule33to32(wd,bdpy,TempPinyin,false));
			}

		}
	}
	else//�ֵ�������û�еĺ���
	{
		unsigned char ch=(unsigned char)zi[0];
		if(ch<170)
		{
			if(memcmp(zi,"��",2)==0)//����������ַ�����ע��;
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

					if(memcmp(zi,"��",2)==0)
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
				else//�ֵ�������û�еĺ���
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
			//˵����δ��¼�ʣ���Ҫ����33�����
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
	// ��ʼʱ������������Ϊ���ҷ�Χ
	// left,right�ֱ��ǲ��ҷ�Χ�ڵ�һ�������һ��Ԫ��
	while(left<=right) { // ��ǰ���ҷ�Χ��Ϊ��
		mid = (left+right)/2; // ������ҷ�Χ�м�Ԫ�ص����
		convert(hp,leftzi[mid],3); // ȡ�м�Ԫ��
		cmp=strncmp(hp,tempword,2); // �Ƚϸ�Ԫ�ص��ָ������ҵ���
		if(cmp<0)  // ����м�Ԫ���ֽ�С�����Ұ����Ϊ�µĲ��ҷ�Χ
			left=mid+1;
		else  // ����м�Ԫ���ֽϴ󣬰�������Ϊ�µĲ��ҷ�Χ
			if(cmp>0) 
				right=mid-1;
			else {  // �����ȣ����ҳɹ�
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
	int PinyinNum=0;//һ���ʵ�ƴ������
	int i=0;
	while(!py.IsEmpty())
	{
		int pos=py.FindOneOf("12345");//position of the 12345 appeared in the pinyinstring.
		if(pos<0)//����ok�����Ȼ��ע��������û�������ģ�
			break;
		num[i]=py[pos];
		if(i>0)
			postion[i++]=pos+postion[i-1]+1;
		else
			postion[i++]=pos;
		py=py.Mid(pos+1);
		PinyinNum++;		
	}
	if(!ExistIndict)//δ��¼�ʣ���������ô���ӣ�33to23
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
		if(PinyinNum==4)//4�ִ�
		{
			
			if((num[0]=='3')&&(num[1]=='3')&&(num[2]=='3')&&(num[3]=='3'))
			{
				char First[5],Last[5],Middle[5];
				strncpy(First,sWord,4);//ȡǰ������,
				First[4]=0;
				strncpy(Last,sWord+4,4);//ȡ����������
				Last[4]=0;
				strncpy(Middle,sWord+2,4);//ȡ�м�������
				Middle[4]=0;

				if(m_dictCore.IsExist(First,-1)||m_dictCore.IsExist(Last,-1))//���ǰ���ߺ��������ֳɴ�
				{
					//change 3333 to 2323,e.g. �ҵ�С��
					rPinyin[postion[0]]='2';
					rPinyin[postion[2]]='2';
				}
				else if(m_dictCore.IsExist(Middle,-1))//�м�ɴʡ�
				{
					//change 3333 to 3223,e.g. ��С����
					rPinyin[postion[1]]='2';
					rPinyin[postion[2]]='2';
				}
				else
				{
					//change 3333 to 2223,e.g. ��С����
					rPinyin[postion[0]]='2';
					rPinyin[postion[1]]='2';
					rPinyin[postion[2]]='2';
				}
			}
		}
		else if(PinyinNum==3)//3�ִ�
		{
			if((num[0]=='3')&&(num[1]=='3')&&(num[2]=='3'))
			{
				char First[5],Last[5];
				strncpy(First,sWord,4);//ȡǰ������,
				First[4]=0;
				strncpy(Last,sWord+2,4);//ȡ����������
				Last[4]=0;
				if(m_dictCore.IsExist(First,-1))//���ǰ�������ֳɴ�
				{
					//change 333 to 223,e.g. ������
					rPinyin[postion[0]]='2';
					rPinyin[postion[1]]='2';
				}
				else if(m_dictCore.IsExist(Last,-1))
				{
					//change 333 to 323,e.g. ��С��
					rPinyin[postion[0]]='2';
					rPinyin[postion[1]]='2';
				}
			}
		}
		else if(PinyinNum==2)//2�ִ�
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
//	senten[l-1]='\0';//ȥ��ĩβ�Ļس���
	CString s(senten);
	s.TrimLeft();
	s.TrimRight();
	while(!s.IsEmpty()) {//���в�����
		npinyin[0]=0;
		bdpy[0]=0;
		CString w,t;
		w=s.SpanExcluding(" ");// �����Ĳ�����һ���ո񣬼ٶ��ִ��������Կո���Ϊ�ֽ��
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
		if(strcmp("w",t)==0)//���,����ע��
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
				if(ch<128)//�����ַ�ɾ��,��:[
					continue;
				else if(ch>169)
				{
					ChineseWord[k++]=w[j++];
					ChineseWord[k++]=w[j];
				}
				else//����ɾ�����磺�����м��Բ��,�����
				{
					zi[0]=w[j];
					zi[1]=w[j+1];
					zi[2]='\0';
					if(memcmp(zi,"��",2)==0)
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
				t=t.Left(i);//��]����С��λ��Ϊ���Ա��
			//����Ϊ�ʵ���������
			if(strlen(ChineseWord)!=0)//����/x���Ѿ�ɾ��������ע��
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
		if(HaveWordNo)//last word is "��", rule No for sentence;
		{
			int tone;
			CheckFirstTone(bdpy,&tone);
			if(tone=='4')//word "��" change tone 4 to 2 before 4
			{
				rBDPinyin[strlen(rBDPinyin)-2]='2';//change 3 to 2;
			}
		}
		if(bdpy[strlen(bdpy)-2]=='3')
			HaveTone3=true;
		else
			HaveTone3=false;
		if(strcmp(const_cast<char*>((LPCSTR)w),"��")==0)
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
				when fine "һ","��"in the word, only find once!!!!!!
				if the words contains 2 һ"or "��" in it,
				then the second can't be changed if need biandiao!~!!
				because һ is sometimes pronounce itself yi1,not biandiao
				so it checked by hand by Zhang Jingjing.
				so I checked all words have two "��"s.
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
//	fp=fopen("d:\\mywork\\ע��\\pydctnew.txt","rt");
	for(int h=0;h<SCKWDNUM;h++)
	{
		fscanf(fp,"%s %s %s %d",ci[h],cixing[h],pinyin[h],&prop[h]);
		cilen[h]=strlen(ci[h]);
		cixing[h][1]='\0';
	}
	for(h=0;h<SCKWDNUM;h++)
	{
//33���
		char rPinyin[WORD_MAXLENGTH*6];
		strcpy(bdpinyin[h],WordRule33to32(ci[h],pinyin[h],rPinyin,true));//use 33 rule;
//���ı��
		char* p=(CC_Find(ci[h],"��"));
		char* pCi;
		pCi=ci[h];
		if(p!=NULL)//�С�����
		{
			unsigned int Position=(p-pCi)/2;
			if(Position<cilen[h]/2-1)
			{
			CString py(bdpinyin[h]);
			int num[WORD_MAXLENGTH],postion[WORD_MAXLENGTH];
			int PinyinNum=0;//һ���ʵ�ƴ������
			i=0;
			while(!py.IsEmpty())
			{
				int pos=py.FindOneOf("12345");//position of the 12345 appeared in the pinyinstring.
				if(pos<0)//����ok�����Ȼ��ע��������û�������ģ�
					break;
				num[i]=py[pos];
				if(i>0)
					postion[i++]=pos+postion[i-1]+1;
				else
					postion[i++]=pos;
				py=py.Mid(pos+1);
				PinyinNum++;
			}
			if(num[Position+1]=='4')//��������ȥ��ǰ,change it to 2;
			{
				bdpinyin[h][postion[Position]]='2';
			}
			}
		}
////һ�ı��
		p=(CC_Find(ci[h],"һ"));
		if(p!=NULL)//�С�һ ��
		{
			unsigned int Position=(p-pCi)/2;
			if(Position<cilen[h]/2-1)
			{
			CString py(bdpinyin[h]);
			int num[WORD_MAXLENGTH],postion[WORD_MAXLENGTH];
			int PinyinNum=0;//һ���ʵ�ƴ������
			i=0;
			while(!py.IsEmpty())
			{
				int pos=py.FindOneOf("12345");//position of the 12345 appeared in the pinyinstring.
				if(pos<0)//����ok�����Ȼ��ע��������û�������ģ�
					break;
				num[i]=py[pos];
				if(i>0)
					postion[i++]=pos+postion[i-1]+1;
				else
					postion[i++]=pos;
				py=py.Mid(pos+1);
				PinyinNum++;
			}
			if(num[Position+1]=='4')//��һ����ȥ��ǰ,change it to 2;
			{
				bdpinyin[h][postion[Position]]='2';
			}
			else////��һ����ȥ��ǰ,change it to 4;
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
//	fp=fopen("d:\\mywork\\ע��\\pydctnew.txt","rt");
//	fp=fopen("d:\\mywork\\ע��\\namepy.txt","rt");
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
//	fp=fopen("d:\\mywork\\ע��\\pydctnew.txt","rt");
//	fp=fopen("d:\\mywork\\ע��\\namepy.txt","rt");
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
//	fp=fopen("d:\\mywork\\ע��\\pydctnew.txt","rt");
//	fp=fopen("d:\\mywork\\ע��\\namepy.txt","rt");
//	fp=fopen("d:\\mywork\\ע��\\mwdpy.txt","rt");
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