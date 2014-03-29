#include "StdAfx.h"
#include "norm.h"
#include <memory.h>
#include <string.h>
#include <math.h>
#include <stdlib.h>
CNorm::CNorm()
{
	char out[]="��һ�����������߰˾�";
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
//	senten[l-1]='\0';//ȥ��ĩβ�Ļس���
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
			// RULE NO1: �ˣ���/q  -->��ÿ��/q ÿ195 - 191
			char* p=CC_Find(sWord,"��");
			if(strcmp(sWord,"��")==0)
				strcpy(sResult,"���϶�");
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
		//�����ܰ������ֵĴ����У�m,nx,t,nt,nz)
		//��ֵ����;������������Ҫ�õ���������Ϣ
		//���ִ�����ܺ��е��ַ������������ִʣ�
		//�������ϳɡ������á�����+-./���ǧ���ڰ�Ǫ����%
		// RULE NO2:  nz,nx,nt���
		else if((strcmp("nz",sTag)==0)||(strcmp("nx",sTag)==0)||(strcmp("nt",sTag)==0)||(strlen(sWord)>15&&IsAllAlabNum(sWord)))
		{
				ChangeWord(sWord,sTag,sResult,true);//true���
		}

		//���ֺ�ʱ��ĸ����������Ҫ��������Ϣ����/m  ��/m  ��/nx 
		//���ִ�����ܺ��е��ַ������������ִʣ�
		//�������ϳɡ������á�����+-./���ǧ���ڰ�Ǫ����%
        else if(strcmp("m",sTag)==0)
		{
			//��������
			// RULE NO3:  �����ǣ��绰���ʱ࣬���棬ר���ţ� ������������е�m
			if((((strcmp(LastTwoWord,"�绰")==0)||(strcmp(LastTwoWord,"����")==0)||(strcmp(LastTwoWord,"�ʱ�")==0)||(strcmp(LastTwoWord,"ר����")==0))
				&&((strcmp(LastWord,":")==0)||(strcmp(LastWord,"��")==0)||(strcmp(LastWord,"��")==0)))
				||((strcmp(LastWord,"�绰")==0)||(strcmp(LastWord,"����")==0)||(strcmp(LastWord,"�ʱ�")==0)||(strcmp(LastWord,"ר����")==0)))
			{
				//������������������м���ţ�����010-62753835���е�-��ר���е�.
				ChangeWord(sWord,sTag,sResult,true);//true���
				HaveChanged=true;				
			}
			//��������
			else if(ReadWord(s,NextWord,NextTag,&lenth))
			{
				// RULE NO4:  �����ǣ����������࣬���䣬���г���·�����������
				//·�������ǣ�����/m ·/q  ��/n ������/m  ·/q  ����/b  ����/n���ɶ�ֵ�����Բ�����
				//���г���Ҳ�����ϸ����⣬����ȥ

				if((strcmp(NextWord,"����")==0)||(strcmp(NextWord,"����")==0)||(strcmp(NextWord,"����")==0))
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
				
				//RULE NO5: ѭ�����ִ��������� ���ͣ��򣬵���,��~,��,- ", �������ִ�����Ҫ��������.				
				else if((strcmp(NextWord,"��")==0)||(strcmp(NextWord,"��")==0)||(strcmp(NextWord,"��")==0)
					||(strcmp(NextWord,"��")==0)||(strcmp(NextWord,"��")==0)||(strcmp(NextWord,"~")==0)||(strcmp(NextWord,"-")==0))
				{
					s=s.Mid(lenth);
					s.TrimLeft();
					if(ReadWord(s,NextTwoWord,NextTwoTag,&lenth))
					{

						if(strcmp("m",NextTwoTag)==0)
						{
							s=s.Mid(lenth);
							s.TrimLeft();
							if(HaveChanged)//�Ѿ�ȷ������һ���Ķ��������ã����ҿ϶��������
							{
								if((strcmp(NextWord,"��")==0)||(strcmp(NextWord,"��")==0)||(strcmp(NextWord,"~")==0)||(strcmp(NextWord,"-")==0))
								{
									strcpy(NextWord,"��");
									strcpy(NextTag,"p");//����
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
								if((strcmp(NextThreeWord,"����")==0)||(strcmp(NextThreeWord,"����")==0)||(strcmp(NextThreeWord,"����")==0))
								{
									ChangeWord(sWord,sTag,sResult,true);
									strcat(sResult,"  ");
									if((strcmp(NextWord,"��")==0)||(strcmp(NextWord,"��")==0)||(strcmp(NextWord,"~")==0)||(strcmp(NextWord,"-")==0))
									{
										strcpy(NextWord,"��");
										strcpy(NextTag,"p");//����
									}
									strcat(sResult,NextWord);
									strcat(sResult,"/");
									strcat(sResult,NextTag);
									strcat(sResult,"  ");
									ChangeWord(NextTwoWord,NextTwoTag,TempWord,true);
									strcat(sResult,TempWord);
									HaveChanged=true;
								}
								//Ĭ��ֵ��
								else
								{
									ChangeWord(sWord,sTag,sResult,false);
									strcat(sResult,"  ");
									if((strcmp(NextWord,"��")==0)||(strcmp(NextWord,"��")==0)||(strcmp(NextWord,"~")==0)||(strcmp(NextWord,"-")==0))
									{
										strcpy(NextWord,"��");
										strcpy(NextTag,"p");//����
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
							// s�Ѿ�Ϊ�գ�Ĭ��ֵ��
							else
							{
									ChangeWord(sWord,sTag,sResult,false);
									strcat(sResult,"  ");
									if((strcmp(NextWord,"��")==0)||(strcmp(NextWord,"��")==0)||(strcmp(NextWord,"~")==0)||(strcmp(NextWord,"-")==0))
									{
										strcpy(NextWord,"��");
										strcpy(NextTag,"p");//����
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
						
						//RULE NO6:��Ķ����������
						//��������/m  ��/w  ����������/t  
						else if(strcmp("t",NextTwoTag)==0)
						{
							s=s.Mid(lenth);
							s.TrimLeft();
							if(CC_Find(NextTwoWord,"��")!=NULL)
							{
									ChangeWord(sWord,sTag,sResult,true);
									strcat(sResult,"  ");
									if((strcmp(NextWord,"��")==0)||(strcmp(NextWord,"��")==0)||(strcmp(NextWord,"~")==0)||(strcmp(NextWord,"-")==0))
									{
										strcpy(NextWord,"��");
										strcpy(NextTag,"p");//����
									}
									strcat(sResult,NextWord);
									strcat(sResult,"/");
									strcat(sResult,NextTag);
									strcat(sResult,"  ");
									ChangeWord(NextTwoWord,NextTwoTag,TempWord,true);
									strcat(sResult,TempWord);
									HaveChanged=true;
							}
							//ֵ��
							else
							{
									ChangeWord(sWord,sTag,sResult,false);
									strcat(sResult,"  ");
									if((strcmp(NextWord,"��")==0)||(strcmp(NextWord,"��")==0)||(strcmp(NextWord,"~")==0)||(strcmp(NextWord,"-")==0))
									{
										strcpy(NextWord,"��");
										strcpy(NextTag,"p");//����
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
						else//����ʱ�䣬Ҳ�������ʣ�Ĭ��ֵ��.
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
					// s�Ѿ�Ϊ�գ�Ĭ��ֵ��
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
				
				//RULE NO7:�����������ʺ���Ŷ�Ϊ��
				//��/m  ��/w  ������/m  ��/q 
				//��/m  ��/w  ������/m  ��/q  ����Ӿ/n
				//��/v  ��/a  ��/w  ����/nx  ��/w  ��/u  ���/n
				//����/nr  ������/m  ��/w  ������/m  ��/w  ������/m  ���٣£ʡ���������/nx  
				else if(strcmp(NextWord,"��")==0)
				{
					s=s.Mid(lenth);
					s.TrimLeft();
					ChangeWord(sWord,sTag,sResult,false);
					strcat(sResult,"  ��/v");//?

				}
				//������������RULE NO8:  Ĭ�϶���ֵ����
				else
				{
					ChangeWord(sWord,sTag,sResult,false);
				}
				
			}
			// s�Ѿ�Ϊ��
			else
			{
				ChangeWord(sWord,sTag,sResult,false);//true���
//				HaveChange=true;	
			}
		}
		
		else if(strcmp("t",sTag)==0)//ʱ��
		{
			//RULE NO6:��Ķ����������ע��
			//80/m  ���/n  ��/f  ��/w  
			//��/v  ����������/t  ��ʷ/n 
			//��/r  ��/v  ����������/t  ��ʷ/n  ���/n 
			//����û�м��룬��������ʷ��ʱ��ֵ�������
			if(CC_Find(sWord,"��")!=NULL)
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
		else if(strcmp("w",sTag)==0)//��㣬��ʱע�⣺��һЩ����ת��
		{
			//RULE NO9:  �治�漰λ���滻���⣬���Բ��ж�����
			///��/w  ��������/m
			//��/m  ��/w  ��/m  ��/w  ��/w 
			//��/m  ��/w  ��/m  ��/q  ��/w  
			//����/t  ����/n  ��/w  ��/m  ��/w  ����/m  ��/w  ��/w  
			if(strcmp(sWord,"��")==0)
				strcpy(sResult,"���϶�/q");
			//RULE NO10:  ���ɷ����滻��ϰ�������Ҳ�繫ʽһ������
			//��/m  ��/m  ��/nx
			//��/w  ������/w  ��/w  ��/w  ���ಡ/n 
			else if(strcmp(sWord,"��")==0)
				strcpy(sResult,"��/v");
			//RULE NO11:  ���ɷ����滻��ϰ�������Ҳ�繫ʽһ��������
			else if(strcmp(sWord,"��")==0)
				strcpy(sResult,"����/v");
			//RULE NO12: ��->��Ԫ :ע�����λ��
			else if(strcmp(sWord,"��")==0)
			{
				//�����ע�����⣬û���ҵ�/������
				if(ReadWord(s,NextWord,NextTag,&lenth))
				{
					if(strcmp(NextTag,"m")==0)
					{
						s=s.Mid(lenth);
						s.TrimLeft();
						ChangeWord(NextWord,NextTag,sResult,false);
						strcat(sResult,"  ��Ԫ/q");						
					}
				}
				else
				{
					strcpy(sResult,sWord);
					strcat(sResult,"/");
					strcat(sResult,sTag);
				}

			}
			// RULE NO13: ��->����� :���õ���λ��
			else if(strcmp(sWord,"��")==0)
			{
				strcpy(sResult,"�����/n");
			}
			//RULE NO1: �ˣ���/q  -->��ÿ��/q ÿ195 - 191
			//����������/m  Ԫ/q  ��/w  ƽ����/q  (��ʵ�Ǳ�ע���󣬵�������Ҳ�����������
			//������/m  ��/q  ��/w  ��/v
			//������/m  ��/q  ����/t 
			//����/m  ΢/ag  ��/vg  ��/w  ������/q  
			//��������/m  ��/q  ��/w  ��/w  ƽ������/q  ����/t 
			//��ë��/n  ��/v  ��/w  �£ӣ�����/nx 
			// ����/nr  ��/w  ֣/nr  ����/nr 
			// �˰ٰ���/m  ��/a  ��/n  ��/w  Сʱ/n 
			//�����������һ��Ϊq����Ϊ��ÿ��
//			else if(strcmp(sWord,"��")==0)
//			{
//			}
			else
			{
				//����������
				strcpy(sResult,sWord);
				strcat(sResult,"/");
				strcat(sResult,sTag);

			}



			
		}
		//���������������ԭ�����ء�
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
	//char Number_chn[]= "��������������������";
    //char Number_eng[]="0123456789";
	//��Ϊ0123456789�����붼С��64�����Է����滻��
	//�����еĵ��ֽ������滻Ϊ˫�ֽ�
	CString s(NumWord);
	s.Replace("0","��");
	s.Replace("1","��");
	s.Replace("2","��");
	s.Replace("3","��");
	s.Replace("4","��");
	s.Replace("5","��");
	s.Replace("6","��");
	s.Replace("7","��");
	s.Replace("8","��");
	s.Replace("9","��");
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
	word=s.SpanExcluding(" ");// �����Ĳ�����һ���ո񣬼ٶ��ִ��������Կո���Ϊ�ֽ��
	*lenth=word.GetLength();
//	s=s.Mid(word.GetLength());
//	s.TrimLeft();
	int i=word.Find('/');


//Ϊ���޸�(1945/46��/t)�����   04_06_23
	int j = word.ReverseFind('/');
	if(i>=0){
		if(j>i&&word[j-1]==']'){
			word = word.Left(j-1);
			j = word.ReverseFind('/');
		}
		if(j-i>2){
			int x,y;
			x = word.Find("��");
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
		//���ԭ����ע��������⣬û�б�ע���ԣ���ô���ش���nx
		strcpy(sWord,const_cast<char*>((LPCSTR)word));
		strcpy(sTag,"nx");
		return false;
	}
	tag=word.Mid(i+1);
	word=word.Left(i);
	i=word.Find('[');//ע�⣬�����������δ��½����ʻ����
	if(i>=0)
	{
		word=word.Mid(i+1);
	}
	i=tag.Find(']');
	if(i>0)
	{
		tag=tag.Left(i);
	}
	//��Ҫ�ģ���/l/%��ʾ������
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
	//�ֶδ���
	Processing(sWord,&time);
	sResult[0]=0;

	//���������
	///$��������/nx;$1974/nx;
	//��ʵ�Ǳ�ע���󣬵��Ƿ���������ȷ����
	if((time==2)&&(ctype[0]==T_SINGLE)&&(strcmp(sSameTypeString[0],"$")==0)&&(ctype[1]==T_NUM))
	{
		if(ConvertNumber(sSameTypeString[1],TempNumResult,false))//ֵ��
		{
			strcpy(sResult,TempNumResult);
			strcat(sResult,"/m  ��Ԫ/q");
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
	if(type)//���
	{
		for(int j=0;j<time;j++)//find the number and change it, for others didn't care.
		{
			////���Դ����/nz;�ӣģȣ������ǣ�/nx 	�ȣף�������/nx 
			if(ctype[j]==T_NUM)
			{
				ConvertNumber(sSameTypeString[j],TempNumResult,true);
				strcat(sResult,TempNumResult);
			}
			else if((ctype[j]==T_DELIMITER)&&strcmp(sSameTypeString[j],"��")==0)
			{
				//// RULE NO3: ��->�� , only in nz, nt ,nx;
				//���٣£ʡ���������/nx  ;//�٣£ʣ���������
				strcat(sResult,"��");
			}
			//�ӣģȣ������ǣ�/nx  
			else if(strcmp(sSameTypeString[j],"��")==0||strcmp(sSameTypeString[j],".")==0)
				strcat(sResult,"��");
			// RULE NO3: - ->�� , only in nz, nt ,nx;
			//b-��������/nx;
			else if((ctype[j]==T_SINGLE)&&strcmp(sSameTypeString[j],"-")==0)
				strcat(sResult,"��");
			else
				strcat(sResult,sSameTypeString[j]);
		}
		strcat(sResult,"/");
		strcat(sResult,sTag);		
	}
	else//ֵ��
		//���ִ�����ܺ��е��ַ������������ִʣ�
		//�������ϳɡ������á�����+-./���ǧ���ڰ�Ǫ����%
		//2��.����    С�����λ����С������жϣ�ǰ�������֣�
		//3��/��       �����Ķ�������������ʱ��Ҫ�����ִ���һ�£���ǰ�������֣�
		//4��%�룥      ���ֺ��ã���Ϊ���ٷ�֮����ͬ���С루ע�⣬54.3%�������Ҫ��2��4���������
		//5��:�ã�
		//
	{
		for(int j=0;j<time;j++)//find the number and change it, for others didn't care.
		{
			CurrentConvert=false;
			if(ctype[j]==T_NUM)
			{
				if((ctype[j+2]==T_NUM)&&(j+2<time))
				{
					if(strcmp(sSameTypeString[j+1],"��")==0||strcmp(sSameTypeString[j+1],"��")==0||strcmp(sSameTypeString[j+1],".")==0)
					{
						//54.3%/m 
						if((j+3<time)&&(strcmp(sSameTypeString[j+3],"��")==0||strcmp(sSameTypeString[j+3],"%")==0))
						{
							strcat(sResult,"�ٷ�֮");
							ConvertNumber(sSameTypeString[j],TempNumResult,false);//С����ǰֵ��
							strcat(sResult,TempNumResult);
							strcat(sResult,"��");
							ConvertNumber(sSameTypeString[j+2],TempNumResult,true);//С��������
							strcat(sResult,TempNumResult);
							j=j+3;
							CurrentConvert=true;
						}
						//��������
						else if((j+3<time)&&strcmp(sSameTypeString[j+3],"��")==0)
						{
							strcat(sResult,"ǧ��֮");
							ConvertNumber(sSameTypeString[j],TempNumResult,false);//С����ǰֵ��
							strcat(sResult,TempNumResult);
							strcat(sResult,"��");
							ConvertNumber(sSameTypeString[j+2],TempNumResult,true);//С��������
							strcat(sResult,TempNumResult);
							j=j+3;
							CurrentConvert=true;
						}
						//�������ã�����/m 
						//ֻת����:���������ѭ�����ɡ�
						else if((j+3<time)&&(strcmp(sSameTypeString[j+3],":")==0||strcmp(sSameTypeString[j+3],"��")==0||strcmp(sSameTypeString[j+3],"��")==0))
						{
							ConvertNumber(sSameTypeString[j],TempNumResult,false);//С����ǰֵ��
							strcat(sResult,TempNumResult);
							strcat(sResult,"��");
							ConvertNumber(sSameTypeString[j+2],TempNumResult,true);//С��������
							strcat(sResult,TempNumResult);
							strcat(sResult,"��");
							j=j+3;
							CurrentConvert=true;
						}
						//����������/m
						else
						{
							ConvertNumber(sSameTypeString[j],TempNumResult,false);//С����ǰֵ��
							strcat(sResult,TempNumResult);
							strcat(sResult,"��");
							ConvertNumber(sSameTypeString[j+2],TempNumResult,true);//С��������
							strcat(sResult,TempNumResult);
							j=j+2;
							CurrentConvert=true;
						}
					}
					else if(strcmp(sSameTypeString[j+1],"/")==0||strcmp(sSameTypeString[j+1],"��")==0)
					{
						//��������/m
						ConvertNumber(sSameTypeString[j+2],TempNumResult,false);//����Ҫ��һ��
						strcat(sResult,TempNumResult);
						strcat(sResult,"��֮");
						ConvertNumber(sSameTypeString[j],TempNumResult,false);
						strcat(sResult,TempNumResult);
						j=j+2;
						CurrentConvert=true;
					}
					else if(strcmp(sSameTypeString[j+1],":")==0||strcmp(sSameTypeString[j+1],"��")==0||strcmp(sSameTypeString[j+1],"��")==0)
					{
						//���ã�����/m 
						//ֻת����:���������ѭ�����ɡ�
						ConvertNumber(sSameTypeString[j],TempNumResult,false);//����Ҫ��һ��
						strcat(sResult,TempNumResult);
						strcat(sResult,"��");
						j=j+1;
						CurrentConvert=true;
					}
				}
				//����
				else if((j+1<time)&&(strcmp(sSameTypeString[j+1],"��")==0||strcmp(sSameTypeString[j+1],"%")==0))
				{
					strcat(sResult,"�ٷ�֮");
					ConvertNumber(sSameTypeString[j],TempNumResult,false);
					strcat(sResult,TempNumResult);
					j=j+1;
					CurrentConvert=true;
				}
				////����
				else if((j+1<time)&&strcmp(sSameTypeString[j+1],"��")==0)
				{
					strcat(sResult,"ǧ��֮");
					ConvertNumber(sSameTypeString[j],TempNumResult,false);
					strcat(sResult,TempNumResult);
					j=j+1;
					CurrentConvert=true;
				}
				//�������������
				if(CurrentConvert==false)
				{
					ConvertNumber(sSameTypeString[j],TempNumResult,false);
					strcat(sResult,TempNumResult);
				}
			}
			//�������ϳɡ������á�����+-./���ǧ ���ڰ�Ǫ����%
			else if(strcmp(sSameTypeString[j],"��")==0)				
				strcat(sResult,"�Ӽ�");
			else if(strcmp(sSameTypeString[j],"��")==0||strcmp(sSameTypeString[j+1],"��")==0||strcmp(sSameTypeString[j+1],".")==0)
			{
				strcat(sResult,"��");
				if((j+1<time)&&(ctype[j+1]==T_NUM))
				{
					ConvertNumber(sSameTypeString[j+1],TempNumResult,true);
					strcat(sResult,TempNumResult);
					j++;
				}
			}
			else if((strcmp(sSameTypeString[j],"��")==0)||(strcmp(sSameTypeString[j],"+")==0))
				strcat(sResult,"��");
			//���ֱ��磺���ǧ���ڰ�Ǫ�������ϳ�
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
	//type��¼����ת�����ƶ�������ͬ��Ī��Ϊ������,�������ȵ�0��trueΪ1990 �꣬��.056С��������ֶ���,Ҫ��0
	Result[0]=0;
//	memset(sResult,0,WORD_MAXLENGTH);

	char subout[WORD_MAXLENGTH][3];
	int lenth,tmplen;
	int nullend;//ĩβȫ0��λ��
	int nullstart;//��ʼȫ0��λ��
	
	char Number[3];
	memset(Number,0,3);

//	char tmp[3];
//	memset(tmp,0,3);
//	char Number_chn[]= "��������������������";
//	char out[]="��һ�����������߰˾�";
//	char danwei[]="��ǧ��ʮ��ǧ��ʮ��ǧ��ʮ";//���ת��13λ��	
//��	163 - 176
//��	163 - 177
//��	163 - 178
//��	163 - 179
//��	163 - 180
//��	163 - 181
//��	163 - 182
//��	163 - 183
//��	163 - 184
//��	163 - 185

	//�õ�����lenth��������ִ�
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
	else//����У�1020222;200;(��ȫ�㣩;04(���·ݣ�;15(����һʮ��,��ʮ�壩
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
		if(memcmp(subout[lenth-1],"��",2)==0)//���ĩβ0�ĸ���
		{
			for(int j=lenth-2;j>=0;j--)
			{
				if(memcmp(subout[j],"��",2)!=0)
				{
					nullend=j;
					break;
				}
			}
		}
		if(memcmp(subout[0],"��",2)==0)//�����ʼ0�ĸ���
		{
			for(int j=1;j<nullend;j++)
			{
				if(memcmp(subout[j],"��",2)!=0)
				{
					nullstart=j;
					break;
				}
			}
		}
		tmplen=lenth-nullstart;
		for(int j=nullstart;j<=nullend;j++)//����ʼ����0��λ�ö���ĩβȫ��ǰ, �����0������λ
		{
			strcat(Result,subout[j]);
//			resultlen+=2;
			if(memcmp(subout[j],"��",2)!=0)
			{
				if(tmplen==13)
					strcat(Result,"��");
				else if(tmplen==12)
					strcat(Result,"ǧ");
				else if(tmplen==11)
					strcat(Result,"��");
				else if(tmplen==10)
					strcat(Result,"ʮ");
				else if(tmplen==9)
					strcat(Result,"��");
				else if(tmplen==8)
					strcat(Result,"ǧ");
				else if(tmplen==7)
					strcat(Result,"��");
				else if(tmplen==6)
					strcat(Result,"ʮ");
				else if(tmplen==5)
					strcat(Result,"��");
				else if(tmplen==4)
					strcat(Result,"ǧ");
				else if(tmplen==3)
					strcat(Result,"��");
				else if(tmplen==2)
					strcat(Result,"ʮ");
	//			resultlen+=2;
			}
			tmplen--;
		}
		if((lenth==2)&&(memcmp(subout[0],"һ",2)==0))
		{
			strcpy(Result,"ʮ");
//			resultlen+=2;
			if(memcmp(subout[1],"��",2)!=0)
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

