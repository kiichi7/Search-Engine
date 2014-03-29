//////////////////////////////////////////////////////////////////////
//������ΪICTCLAS���Ŀ���Դ��������ѵ������
//�����У���Ԫ�ķ�ѵ��������HMMѵ��������HMMѵ����������HMMѵ����
// ����ӵ��У���Ԫ�ķ�ѵ�������ںʹ���һ���֡�            
// 
//  
//����Ȩ��  Copyright?2003-2005������Ϣ��ѧ���� ��־�� 
//��ѭЭ�飺��Ȼ���Դ�������Դ���֤1.0
//Email: wangzf@cis.pku.edu.cn
//
/****************************************************************************
 *
 * Copyright (c) 2004 
 *     National Laboratory of Machine Perception
 *     PKU
 *     All rights reserved.
 *
 *
 * Filename: Train.cpp
 * Abstract:
 *           train a bigram model and all hmms(except nr hmm) for ICTCLAS
 * Author:   Wang Zhifu
 *          (wangzf@cis.pku.edu.cn)
 * Date:     2004-2-6
 *
 * Notes:     The Nr hmm is programmed by Wu Jinyi; thanks for that;
 *            The Test for the unknown words recognition, segged and tagged result
 *            is programmed by Lin Xiaojun; thanks for his help and kindness
 * 
 ****************************************************************************/

#include "stdafx.h"
#include "train.h"
#include  <io.h>
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////

CTrain::CTrain()
{
	WordTag_Result=new WORD_RESULT[MAX_WORDS];
	NsTag_Result=new WORD_RESULT[WORD_MAXLENGTH];
	
}
bool CTrain::Load()
{
	if(!IsDataExists())
		return false;
	//Load all the models, including bigram and all hmms
	m_context.Load("data\\lex.ctx");
	m_dictCore.Load("data\\dict.dat");
	m_dictBigram.Load("data\\bigram.dat");
	m_dictTrigram.Load("data\\trigram.dat");
//	m_dictnr.Load("data\\nr.dat");
	m_dictns.Load("data\\ns.dat");
	m_dicttr.Load("data\\tr.dat");
//	m_contextnr.Load("data\\nr.ctx");
	m_contextns.Load("data\\ns.ctx");
	m_contexttr.Load("data\\tr.ctx");
//	NeedNsTrain=false;//�Ƿ���Ҫnsѵ����ǡ�
//	NeedTrTrain=false;//�Ƿ���Ҫtrѵ����ǡ�
//	NeedNrTrain=false;//�Ƿ���Ҫnrѵ����ǡ�
	m_bDisable=true;
	return true;
}
CTrain::~CTrain()
{
	delete WordTag_Result;
	delete NsTag_Result;
}
bool CTrain::Save()
{
	//Save all the models, including bigram and all hmms
	if(m_context.Save("data\\lex.ctx")&&m_dictCore.Save("data\\dict.dat")&&m_dictTrigram.Save("data\\trigram.dat")
		&&m_dictBigram.Save("data\\bigram.dat")&&m_dictns.Save("data\\ns.dat")
		&&m_dicttr.Save("data\\tr.dat")&&m_contextns.Save("data\\ns.ctx")&&m_contexttr.Save("data\\tr.ctx"))
		return true;
	return false;
}

/*********************************************************************
 *
 *  Func Name  : Train
 *
 *  Description: train a paragraph to hmm,bigram,nshmm and trhmm models
 *
 *  Parameters : CString s:the paragraph
 *
 *  Returns    : fail or success
 *  Author     : Wang Zhifu
 *  History    : 1. created at 2004-2-12
 *********************************************************************/
bool CTrain::Train(CString s)
{
	if(s.IsEmpty())
		return true;//����
	int WordNum,RestLine=1;
	while(!s.IsEmpty())
	{
		if(ReadLine(s,&WordNum,&RestLine))
		{
			BigramhmmTrain(WordNum);// ��Ժ�ķ��ʹ���hmmѵ��
			//	if(NeedNrTrain)
			//		NrTrain(WordNum);//����hmmѵ��
//			if(NeedNsTrain)
			NsTrain(&m_dictns,&m_contextns,WordNum,28275);//����ѵ��
//			if(NeedTrTrain)
			TrTrain(&m_dicttr,&m_contexttr,WordNum,28274);//�����з�������ѵ��
			s=s.Right(RestLine);
		}
		else 
			return false;
	}
	return true;
}

/*********************************************************************
 *
 *  Func Name  : FileTrain
 *
 *  Description: train a file to into hmm,bigram,nshmm and trhmm models
 *
 *  Parameters : CString filename:the file
 *
 *  Returns    : fail or success
 *  Author     : Wang Zhifu
 *  History    : 1. created at 2004-2-11
 *********************************************************************/
bool CTrain::FileTrain(CString filename)
{
	FILE *SourceFile;
	SourceFile=fopen(filename,"r");
	CStdioFile sSource(SourceFile);
	char line[MAX_SENLENTH];	
	m_fileLog.Open("log.txt",CFile::modeWrite|CFile::typeText|CFile::modeCreate|CFile::modeNoTruncate|CFile::shareDenyWrite);//Open log file to write
	m_fileLog.SeekToEnd();
	m_fileLog.WriteString((LPCTSTR)filename);//Record the filename
	m_fileLog.WriteString("\r\n");
	while(sSource.ReadString(line,MAX_SENLENTH))
	{
//		memset(RawLine,0,MAX_SENLENTH);//Initialize the raw line for write.

		CString m_line;
		CString s(line);
		s.TrimLeft();
		s.TrimRight();
		//����ȥ��ǰ����ĸ����ڣ������m_line�У������������֪���Ǹ��ط�����
		m_line=s.SpanExcluding(" ");
		m_fileLog.WriteString((LPCTSTR)m_line);//Record line
		m_fileLog.WriteString("\r\n");
		s=s.Mid(m_line.GetLength());
		s.TrimLeft();
		if(!Train(s))
		{
			m_fileLog.WriteString("This line for training is not succeed\r\n");
		}
//		fprintf(fpRaw,"%s\n",RawLine);
		memset(line,0,MAX_SENLENTH);
//		NeedNsTrain=false;//�Ƿ���Ҫnsѵ����ǡ�
//		NeedTrTrain=false;//�Ƿ���Ҫtrѵ����ǡ�
//		NeedNrTrain=false;//�Ƿ���Ҫnrѵ����ǡ�
	}
	sSource.Close();
	m_fileLog.Close();
//	fclose(fpRaw);
	return true;
}
/*********************************************************************
 *
 *  Func Name  : BigramhmmTrain
 *
 *  Description: train the sentence to hmm and bigram models
 *
 *  Parameters : WordNum:the word included in the sentence 
 *               Recorded in the WordTag_ResultArray.
 *
 *  Returns    : fail or success
 *  Author     : Wang Zhifu
 *  History    : 1. created at 2003-9-24
 *********************************************************************/
bool CTrain::BigramhmmTrain(int WordNum)
{
	char LastWord[WORD_MAXLENGTH],TempWord[WORD_MAXLENGTH];//last���ڼ�¼��һ���ʣ�ʹδ��¼�ʵ�ʱ�򲻱ظı�WordTag_Result���൱��copy
	char LastBigram[2*WORD_MAXLENGTH];//������¼��2���Ĵʣ�����ͳ����Ԫ�ķ���
	char Trigram[3*WORD_MAXLENGTH];//��Ԫ��
	char Bigram[2*WORD_MAXLENGTH];
	strcpy(LastWord,SENTENCE_BEGIN);
	memset(LastBigram,0,2*WORD_MAXLENGTH);
	m_dictCore.AddItem(SENTENCE_BEGIN,1,1);//ÿ�μ�¼��ʼ����
//	m_dictCore.AddItem(SENTENCE_END,4,0);


//	bool NameTime=true;//��¼�����Ƿ��������֣�����������ִ���Ϊ����������Ϊ���µ�����
	for(int i=1;i<WordNum-1;i++)
	{
		if(m_dictCore.IsExist(WordTag_Result[i].sWord,-1))
		//�ʱ��д��ھ�ͳ�ƣ������δ��ھ�����ڣ����Բ�ͬ����ӣ� ��isexist��nhandleΪ-1ʱ����������
		{
			m_dictCore.AddItem(WordTag_Result[i].sWord,WordTag_Result[i].nHandle,1);
			m_context.Add(0,WordTag_Result[i-1].nHandle,WordTag_Result[i].nHandle,1);
			strcpy(Bigram,LastWord);
			strcat(Bigram,WORD_SEGMENTER);
			strcat(Bigram,WordTag_Result[i].sWord);
//			Bigram=LastWord+WORD_SEGMENTER+WordTag_Result[i].Word;
			m_dictBigram.AddItem(Bigram,3,1);
//			NameTime=true;
			strcpy(Trigram,LastBigram);
			strcat(Trigram,WORD_SEGMENTER);
			strcat(Trigram,WordTag_Result[i].sWord);
			if(strlen(LastBigram)>1)
				m_dictTrigram.AddItem(Trigram,30,1);
			strcpy(LastBigram,Bigram);
			strcpy(LastWord,WordTag_Result[i].sWord);
			
		}
		else//�ʱ��в����ڣ�˵����δ��¼�ʡ��ֱ����ʱ���δ��¼�ƶ����Ա��δ2
		{
			if(WordTag_Result[i].nHandle==30464)//���
			{
				strcpy(TempWord,WordTag_Result[i].sWord);
				m_dictCore.AddItem(WordTag_Result[i].sWord,WordTag_Result[i].nHandle,1);
				m_context.Add(0,WordTag_Result[i-1].nHandle,WordTag_Result[i].nHandle,1);//���ǰһ������ĸ������Լ����ָ���
				strcpy(Bigram,LastWord);
				strcat(Bigram,WORD_SEGMENTER);
				strcat(Bigram,WordTag_Result[i].sWord);
//				Bigram=LastWord+WORD_SEGMENTER+TempWord;
				m_dictBigram.AddItem(Bigram,3,1);
//				NameTime=true;
				strcpy(Trigram,LastBigram);
				strcat(Trigram,WORD_SEGMENTER);
				strcat(Trigram,WordTag_Result[i].sWord);
				if(strlen(LastBigram)>1)
					m_dictTrigram.AddItem(Trigram,30,1);
				strcpy(LastBigram,Bigram);
			}			
			else if(WordTag_Result[i].nHandle==28274)//����nr
			{
				strcpy(TempWord,"δ##��");
//				if(NameTime)
//				{
					m_dictCore.AddItem("δ##��",2,1);
					m_context.Add(0,WordTag_Result[i-1].nHandle,WordTag_Result[i].nHandle,1);//���ǰһ������ĸ������Լ����ָ���						
					strcpy(Bigram,LastWord);
					strcat(Bigram,WORD_SEGMENTER);
					strcat(Bigram,TempWord);
//					Bigram=LastWord+WORD_SEGMENTER+TempWord;
					m_dictBigram.AddItem(Bigram,3,1);
					strcpy(Trigram,LastBigram);
					strcat(Trigram,WORD_SEGMENTER);
					strcat(Trigram,TempWord);
					if(strlen(LastBigram)>1)
						m_dictTrigram.AddItem(Trigram,30,1);
					strcpy(LastBigram,Bigram);
//					NameTime=false;
//				}
//				else
//				{
//					NameTime=true;
//				}				
			}
			else if(WordTag_Result[i].nHandle==28275)////δ##��ns
			{
				m_dictCore.AddItem("δ##��",2,1);
				m_context.Add(0,WordTag_Result[i-1].nHandle,WordTag_Result[i].nHandle,1);//���ǰһ������ĸ������Լ����ָ���
//				NameTime=true;
				strcpy(TempWord,"δ##��");
				strcpy(Bigram,LastWord);
				strcat(Bigram,WORD_SEGMENTER);
				strcat(Bigram,TempWord);
//				Bigram=LastWord+WORD_SEGMENTER+TempWord;
				m_dictBigram.AddItem(Bigram,3,1);
				strcpy(Trigram,LastBigram);
				strcat(Trigram,WORD_SEGMENTER);
				strcat(Trigram,TempWord);
				if(strlen(LastBigram)>1)
					m_dictTrigram.AddItem(Trigram,30,1);
				strcpy(LastBigram,Bigram);
				
			}
			else if(WordTag_Result[i].nHandle==28276)//////δ##��nt
			{
				m_dictCore.AddItem("δ##��",2,1);
				m_context.Add(0,WordTag_Result[i-1].nHandle,WordTag_Result[i].nHandle,1);//���ǰһ������ĸ������Լ����ָ���
//				NameTime=true;
				strcpy(TempWord,"δ##��");
				strcpy(Bigram,LastWord);
				strcat(Bigram,WORD_SEGMENTER);
				strcat(Bigram,TempWord);
//				Bigram=LastWord+WORD_SEGMENTER+TempWord;
				m_dictBigram.AddItem(Bigram,3,1);
				strcpy(Trigram,LastBigram);
				strcat(Trigram,WORD_SEGMENTER);
				strcat(Trigram,TempWord);
				if(strlen(LastBigram)>1)
					m_dictTrigram.AddItem(Trigram,30,1);
				strcpy(LastBigram,Bigram);
			}
			else if(WordTag_Result[i].nHandle==28280)/////δ##�� nx
			{
				m_dictCore.AddItem("δ##��",2,1);
				m_context.Add(0,WordTag_Result[i-1].nHandle,WordTag_Result[i].nHandle,1);//���ǰһ������ĸ������Լ����ָ���
//				NameTime=true;
				strcpy(TempWord,"δ##��");
				strcpy(Bigram,LastWord);
				strcat(Bigram,WORD_SEGMENTER);
				strcat(Bigram,TempWord);
//				Bigram=LastWord+WORD_SEGMENTER+TempWord;
				m_dictBigram.AddItem(Bigram,3,1);
				strcpy(Trigram,LastBigram);
				strcat(Trigram,WORD_SEGMENTER);
				strcat(Trigram,TempWord);
				if(strlen(LastBigram)>1)
					m_dictTrigram.AddItem(Trigram,30,1);
				strcpy(LastBigram,Bigram);
			}
			else if(WordTag_Result[i].nHandle==28282)///δ##רnz
			{
				m_dictCore.AddItem("δ##ר",2,1);
				m_context.Add(0,WordTag_Result[i-1].nHandle,WordTag_Result[i].nHandle,1);//���ǰһ������ĸ������Լ����ָ���
//				NameTime=true;
				strcpy(TempWord,"δ##ר");
				strcpy(Bigram,LastWord);
				strcat(Bigram,WORD_SEGMENTER);
				strcat(Bigram,TempWord);
//				Bigram=LastWord+WORD_SEGMENTER+TempWord;
				m_dictBigram.AddItem(Bigram,3,1);
				strcpy(Trigram,LastBigram);
				strcat(Trigram,WORD_SEGMENTER);
				strcat(Trigram,TempWord);
				if(strlen(LastBigram)>1)
					m_dictTrigram.AddItem(Trigram,30,1);
				strcpy(LastBigram,Bigram);
			}
			else if(WordTag_Result[i].nHandle==29696)///δ##ʱ@��t 
			{
				m_dictCore.AddItem("δ##ʱ",2,1);
				m_context.Add(0,WordTag_Result[i-1].nHandle,WordTag_Result[i].nHandle,1);//���ǰһ������ĸ������Լ����ָ���
//				NameTime=true;
				strcpy(TempWord,"δ##ʱ");
				strcpy(Bigram,LastWord);
				strcat(Bigram,WORD_SEGMENTER);
				strcat(Bigram,TempWord);
//				Bigram=LastWord+WORD_SEGMENTER+TempWord;
				m_dictBigram.AddItem(Bigram,3,1);
				strcpy(Trigram,LastBigram);
				strcat(Trigram,WORD_SEGMENTER);
				strcat(Trigram,TempWord);
				if(strlen(LastBigram)>1)
					m_dictTrigram.AddItem(Trigram,30,1);
				strcpy(LastBigram,Bigram);
			}
			else if(WordTag_Result[i].nHandle==27904)///δ##�� m
			{
				m_dictCore.AddItem("δ##��",2,1);
				m_context.Add(0,WordTag_Result[i-1].nHandle,WordTag_Result[i].nHandle,1);//���ǰһ������ĸ������Լ����ָ���
//				NameTime=true;
				strcpy(TempWord,"δ##��");
				strcpy(Bigram,LastWord);
				strcat(Bigram,WORD_SEGMENTER);
				strcat(Bigram,TempWord);
//				Bigram=LastWord+WORD_SEGMENTER+TempWord;
				m_dictBigram.AddItem(Bigram,3,1);
				strcpy(Trigram,LastBigram);
				strcat(Trigram,WORD_SEGMENTER);
				strcat(Trigram,TempWord);
				if(strlen(LastBigram)>1)
					m_dictTrigram.AddItem(Trigram,30,1);
				strcpy(LastBigram,Bigram);
			}
			else///δ##��
			{
				m_dictCore.AddItem("δ##��",2,1);
				m_context.Add(0,WordTag_Result[i-1].nHandle,WordTag_Result[i].nHandle,1);//���ǰһ������ĸ������Լ����ָ���
//				NameTime=true;
				strcpy(TempWord,"δ##��");
				strcpy(Bigram,LastWord);
				strcat(Bigram,WORD_SEGMENTER);
				strcat(Bigram,TempWord);
//				Bigram=LastWord+WORD_SEGMENTER+TempWord;
				m_dictBigram.AddItem(Bigram,3,1);
				strcpy(Trigram,LastBigram);
				strcat(Trigram,WORD_SEGMENTER);
				strcat(Trigram,TempWord);
				if(strlen(LastBigram)>1)
					m_dictTrigram.AddItem(Trigram,30,1);
				strcpy(LastBigram,Bigram);
			}
			strcpy(LastWord,TempWord);
		}

	}
	//�����������������һ����ĩβ��ת�Ƹ��ʣ�֮���Բ�����ѭ���У���Ϊ�ʵ���û��SENTENCE_END "ĩ##ĩ"������ʱ������
	m_context.Add(0,WordTag_Result[WordNum-2].nHandle,WordTag_Result[WordNum-1].nHandle,1);
	m_dictCore.AddItem(SENTENCE_END,4,1);
	strcpy(TempWord,SENTENCE_END);
	strcpy(Bigram,LastWord);
	strcat(Bigram,WORD_SEGMENTER);
	strcat(Bigram,TempWord);
//	Bigram=LastWord+WORD_SEGMENTER+TempWord;
	m_dictBigram.AddItem(Bigram,3,1);
	strcpy(Trigram,LastBigram);
	strcat(Trigram,WORD_SEGMENTER);
	strcat(Trigram,TempWord);
	if(strlen(LastBigram)>1)
		m_dictTrigram.AddItem(Trigram,30,1);
	return true;	
}

/*********************************************************************
 *
 *  Func Name  : NsTrTrain
 *
 *  Description: train the sentence to nshmm and trhmm models
 *
 *  Parameters : WordNum:the word included in the sentence 
 *               Recorded in the WordTag_ResultArray.
 *	             m_dict: the dict need trained( tr or ns dictionary)
 *               m_con:  the context need trained( tr or ns context)
 *               TrainHandle: marked by ns or tr; only one of these 2 types. 
 *
 *  Returns    : fail or success
 *  Author     : Wang Zhifu
 *  History    : 1. created at 2004-2-10
 *********************************************************************/
bool CTrain::NsTrain(CDictionary *m_dict,CContextStat *m_con,int WordNum,int TrainHandle)//�ݲ�����[����/ns ��/a Ͽ��/n]ns������ĵ�����ʶ��
{

	//����ns�ⲿ���,��nspos�������WordTag_Result.nhandle���������ظı�WordTag_Result
	for(int i=1;i<WordNum-1;i++)
	{
		if(WordTag_Result[i].nHandle!=TrainHandle)//28275ns,28274tr.
		{
			if(NsPos[i-1]==TrainHandle)
			{
				NsPos[i]=12;
			}
			else
				NsPos[i]=0;//�޹�ns��ǣ�Ĭ��
		}
		else
		{
			if(m_dictCore.IsExist(WordTag_Result[i].sWord,-1))
			{
				if(NsPos[i-1]==TrainHandle)
				{
					NsPos[i]=12;
				}
				else
					NsPos[i]=0;//�޹�ns��ǣ�Ĭ��
			}
			else
			{
				NsPos[i]=TrainHandle;
				if(NsPos[i-1]==12)///�����һ��������Ϊ��һ�����������ģ�����Ϊ���������м�ɷ�
					NsPos[i-1]=13;
				else if(NsPos[i-1]!=TrainHandle)
					NsPos[i-1]=11;//�����һ��������Ϊ��һ�����������ģ�����Ϊ�õ���������
/*				if((WordTag_Result[i+1].nHandle==TrainHandle)&&(!m_dictCore.IsExist(WordTag_Result[i+1].sWord,-1)))
//				//�����һ���ǵ����Ҹõ������ڴʵ��У���Ҫѵ����������Ϊһ��ʻ�
///				{
//					NsPos[i+1]=TrainHandle;
//					i++;
//				}
//				else
//				{
//					NsPos[i+1]=12;//����һ�����Ϊ��������
//					i++;
//				}*/
			}
		}
	}
	//������ʼĩ����Ϊ���ȼ��ߣ������ǰ����Ϊ���������ģ��ɸ���������
	NsPos[0]=100;//��nsѵ���п�ʼΪ100
	NsPos[WordNum-1]=101;//��nsѵ���н���Ϊ101
	//�ⲿ�����ϡ���ʼ�ڲ����ͬʱѵ����

	int LastHandle=100;
	m_dict->AddItem(WordTag_Result[0].sWord,NsPos[0],1);
	for(int i=1;i<WordNum;i++)
	{
		if(NsPos[i]!=TrainHandle)
		{
			m_con->Add(0,LastHandle,NsPos[i],1);
			if(NsPos[i]!=0)//���Ϊ0�Ĳ�ͳ�Ƶ������ʵ���ȥ			
				m_dict->AddItem(WordTag_Result[i].sWord,NsPos[i],1);
			LastHandle=NsPos[i];
		}
		else
		{
			int NsNum;
			NsSplitTag(WordTag_Result[i].sWord,&NsNum);
			for(int j=0;j<NsNum;j++)
			{
				m_con->Add(0,LastHandle,NsTag_Result[j].nHandle,1);
				m_dict->AddItem(NsTag_Result[j].sWord,NsTag_Result[j].nHandle,1);
				LastHandle=NsTag_Result[j].nHandle;
			}
		}
	}
	return true;
}

/*********************************************************************
 *
 *  Func Name  : NsTrTrain
 *
 *  Description: train the sentence to nshmm and trhmm models
 *
 *  Parameters : WordNum:the word included in the sentence 
 *               Recorded in the WordTag_ResultArray.
 *	             m_dict: the dict need trained( tr or ns dictionary)
 *               m_con:  the context need trained( tr or ns context)
 *               TrainHandle: marked by ns or tr; only one of these 2 types. 
 *
 *  Returns    : fail or success
 *  Author     : Wang Zhifu
 *  History    : 1. created at 2004-2-10
 *********************************************************************/
bool CTrain::TrTrain(CDictionary *m_dict,CContextStat *m_con,int WordNum,int TrainHandle)//�ݲ�����[����/ns ��/a Ͽ��/n]ns������ĵ�����ʶ��
{

	//����ns�ⲿ���,��nspos�������WordTag_Result.nhandle���������ظı�WordTag_Result
	//������һ�����20��Ϊ�Ƿ���������
	//ע�ⷭ�������������������20��������TrainHandle�����ܸĶ�������ֻ����12.11.13
	bool HaveChineseNr=false;
	for(int i=1;i<WordNum-1;i++)
	{
		if(WordTag_Result[i].nHandle!=TrainHandle)//28275ns,28274tr.
		{
			if(NsPos[i-1]==TrainHandle)
			{
				NsPos[i]=12;
			}
			else
				NsPos[i]=0;//�޹�tr��ǣ�Ĭ��
		}
		else
		{
			if(m_dictCore.IsExist(WordTag_Result[i].sWord,-1))
			{
				if(NsPos[i-1]==TrainHandle)
				{
					NsPos[i]=12;
				}
				else
					NsPos[i]=0;//�޹�tr��ǣ�Ĭ��
			}
			else
			{
				if(!IsAllForeign(WordTag_Result[i].sWord))
				{
					NsPos[i]=20;//������Ƿ������������Ϊ�Ƿ���������
					HaveChineseNr=true;
					continue;
				}
				NsPos[i]=TrainHandle;
				if(NsPos[i-1]==12)//�����һ������Ϊ��һ���������������ģ�����Ϊ�������������м�ɷ�
					NsPos[i-1]=13;
				else if((NsPos[i-1]!=20)&&(NsPos[i-1]!=TrainHandle))
					NsPos[i-1]=11;//�����һ��������Ϊ��һ���������������ģ����Ҳ��ǷǷ�������������Ϊ�÷�������������
//				if((WordTag_Result[i+1].nHandle==TrainHandle)&&(!m_dictCore.IsExist(WordTag_Result[i+1].sWord,-1)))
//				//�����һ���������Ҹ��������ڴʵ��У�������Ҫѵ����������Ϊһ��ʻ�
//				{
//					if(!IsAllForeign(WordTag_Result[i+1].sWord))//���Ƿ�������������ͨ����
//					{
//						NsPos[i+1]=20;//����һ�����Ϊ���Ϊ�Ƿ���������
//						HaveChineseNr=true;
//						i++;
//					}
//					else//�Ƿ���������Ҫѵ�������Ϊ��������
//					{
//						NsPos[i+1]=TrainHandle;
//						i++;
//					}
//				}
//				else
//				{
//					NsPos[i+1]=12;//����һ�����Ϊ������������
//					i++;
//				}
			}
		}
	}
	int j;
	if(HaveChineseNr)
	{
		for(int i=1;i<WordNum-1;i++)//����check���ԷǷ��������������������ģ������Ϊ�����ڡ�
		{
			if(NsPos[i]==20)
			{
				if(NsPos[i-1]==TrainHandle)
				{//�����Ϊ�˷������������ģ������ң��ҵ����ġ�			{
					for(j=i+1;j<WordNum-1;j++)
					{
						if((NsPos[j]!=20)&&(NsPos[j]!=TrainHandle))
						{
							if(NsPos[j]==11)
								NsPos[j]=13;
							else
								NsPos[j]=12;
							break;
						}
					}
				}
				if(NsPos[i+1]==TrainHandle)//�����Ϊ�˷������������ģ������ң��ҵ����ġ�
				{
					for(j=i-1;j>0;j--)
					{
						if((NsPos[j]!=20)&&(NsPos[j]!=TrainHandle))
						{
							if(NsPos[j]==12)
								NsPos[j]=13;
							else
								NsPos[j]=11;
							break;
						}
					}
				}
			}	
		}
	}

	//ע���ʱ����20���ڡ���Ϊ�����ѭ�����ܸ���20��ǣ�������ܳ���ǰ���Ϊ0��������������Ϊ�����ĵ����

	//������ʼĩ����Ϊ���ȼ��ߣ������ǰ����Ϊ�������������ģ��ɸ���������
	NsPos[0]=100;//��nsѵ���п�ʼΪ100
	NsPos[WordNum-1]=101;//��nsѵ���н���Ϊ101
	//�ⲿ�����ϡ���ʼ�ڲ����ͬʱѵ����

	char LastWord[WORD_MAXLENGTH];
	strcpy(LastWord,WordTag_Result[0].sWord);//��ʼΪ��һ����
	int LastHandle=100;
	m_dict->AddItem(WordTag_Result[0].sWord,NsPos[0],1);
	for(int i=1;i<WordNum;i++)
	{
		if(NsPos[i]!=TrainHandle)
		{
			if(NsPos[i]==20)//�Ƿ�����������
				continue;			
			m_con->Add(0,LastHandle,NsPos[i],1);
			if(NsPos[i]!=0)//���Ϊ0�Ĳ�ͳ�Ƶ����������ʵ���ȥ			
				m_dict->AddItem(WordTag_Result[i].sWord,NsPos[i],1);
			LastHandle=NsPos[i];
		}
		else
		{
			int NsNum;
			NsSplitTag(WordTag_Result[i].sWord,&NsNum);
			for(int j=0;j<NsNum;j++)
			{
				m_con->Add(0,LastHandle,NsTag_Result[j].nHandle,1);
				m_dict->AddItem(NsTag_Result[j].sWord,NsTag_Result[j].nHandle,1);
				LastHandle=NsTag_Result[j].nHandle;
			}
		}
	}
	return true;
}
/*********************************************************************
 *
 *  Func Name  : NsSplitTag
 *
 *  Description: split the ns/tr words into role tagged words
 *               the first word is tagged 1;the last 3
 *               2 for all the rest of middle.
 *
 *  Parameters : NsWord:the word need to split into role tagged words
 *	             NsTagNum: return the splitted words' number
 *
 *  Returns    : fail or success
 *  Author     : Wang Zhifu
 *  History    : 1. created at 2004-2-10
 *********************************************************************/

bool CTrain::NsSplitTag(char *NsWord,int *NsTagNum)
{
	int i=0;
	CString sNsWord(NsWord);
	char sWordMatch[WORD_MAXLENGTH];
	memset(sWordMatch,0,WORD_MAXLENGTH);
	while(!sNsWord.IsEmpty())
	{
		if(m_dictCore.GetMMWord(const_cast<char*>((LPCSTR)sNsWord),sWordMatch))
		{
			sNsWord=sNsWord.Mid(strlen(sWordMatch));
			strcpy(NsTag_Result[i].sWord,sWordMatch);
			i++;
		}
		else
		{
			if(sNsWord[0]<0)
			{
				strcpy(NsTag_Result[i].sWord,const_cast<char*>((LPCSTR)sNsWord.Left(2)));
				sNsWord=sNsWord.Mid(2);
				i++;
			}
			else
				sNsWord=sNsWord.Mid(1);//�����ڲ��к��Ĵʵ������֣�������ĸ�ȡ���ѵ����
		}
	}
	*NsTagNum=i;
	NsTag_Result[i-1].nHandle=3;//ĩ��Ϊ3
	NsTag_Result[0].nHandle=1;//�״�Ϊ1	
	for(i=1;i<*NsTagNum-1;i++)
	{
		NsTag_Result[i].nHandle=2;//�м�Ϊ2
	}
	if(i>WORD_MAXLENGTH)
		return false;//���
	return true;
}

/*********************************************************************
 *
 *  Func Name  : ReadLine
 *
 *  Description: to get a sentence(ended by ����������)form the paragraph
 *               and got nr splitted tag combined.

 *  Parameters : CString s:the paragraph needed to be segged into sentences
 *	             WordNum: return the sentences got words' number
 *               RestLine: return the lenth of paragraph rest of the sentence
 *
 *  Returns    : fail or success
 *  Author     : Wang Zhifu
 *  History    : 1. created at 2004-2-12
                 2. refined at 2004-2-15 for NameTime;
 *********************************************************************/

bool CTrain::ReadLine(CString s,int *WordNum,int *RestLine)
{
	//�Ķ����������ϲ�ѵ��������hmm��ns��ѵ����bool NameTime=true;//��¼�����Ƿ��������֣�����������ִ���Ϊ����������Ϊ���µ�����
	bool NameTime=true;
	int j=1;
	s.TrimLeft();
	s.TrimRight();
	CString word,tag;
	char TempWord[WORD_MAXLENGTH];
	memset(TempWord,0,WORD_MAXLENGTH);
	strcpy(WordTag_Result[0].sWord,SENTENCE_BEGIN);
	WordTag_Result[0].nHandle=1;//��bigram�ʹ���hmmѵ���п�ʼΪ1
	while(!s.IsEmpty())
	{
		word=s.SpanExcluding(" ");
		s=s.Mid(word.GetLength());
		s.TrimLeft();
		int i=word.Find('/');
		if(i<0) 
			return false;//���ϴ���
		tag=word.Mid(i+1);
		word=word.Left(i);
		i=word.Find('[');//ע�⣬�����������δ��½����ʻ���ֻ��ѵ�������Ա�ע��hmm�У�û���Լ���hmm��������������עֻ��ȥ��
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
		int nHandle=m_dictCore.GetPOSValue(const_cast<char*>((LPCSTR)tag));
		strcpy(TempWord,const_cast<char*>((LPCSTR)word));
		strcpy(WordTag_Result[j].sWord,TempWord);
//		strcat(RawLine,TempWord);
		WordTag_Result[j].nHandle=nHandle;
//		if(nHandle==28275)//δ##��
//		{
//			if(!m_dictCore.IsExist(TempWord,-1))//�ٺ��Ĵʵ��в�����
//				NeedNsTrain=true;
//			NameTime=true;
//		}
/*		else*/ if(nHandle==28274)//����
		{
			if(NameTime)
			{
				NameTime=false;
			}
			else
			{
				j=j-1;//�˻�һ��ֵ��
				strcat(WordTag_Result[j].sWord,TempWord);//����һ������ճ��һ��
//				if(!m_dictCore.IsExist(WordTag_Result[j].sWord,-1))//�ٺ��Ĵʵ��в�����
//				{
//					if(IsAllForeign(WordTag_Result[j].sWord))
//						NeedTrTrain=true;
//					else
//						NeedNrTrain=true;
//				}
				
				NameTime=true;
			}		
		}
		else if((strcmp(TempWord,"��")==0)||(strcmp(TempWord,"��")==0)||(strcmp(TempWord,"��")==0)||(strcmp(TempWord,"��")==0)||(strcmp(TempWord,"��")==0))
		//end of sentence, �� is not included,because, it can be in the sentence. 
		{
			if(!s.IsEmpty())
			{
				j+=1;
				word=s.SpanExcluding(" ");
				strcpy(TempWord,const_cast<char*>((LPCSTR)word));
				if(strcmp(TempWord,"��/w")==0)//������������У��������ų�Ϊ��һ��Ŀ�ͷ,���һ���ʡ�
				{
					s=s.Mid(word.GetLength());
					s.TrimLeft();
					int i=word.Find('/');
					tag=word.Mid(i+1);
					word=word.Left(i);
					WordTag_Result[j].nHandle=m_dictCore.GetPOSValue(const_cast<char*>((LPCSTR)tag));
					strcpy(WordTag_Result[j++].sWord,"��");
//					strcat(RawLine,TempWord);
				}
				strcpy(WordTag_Result[j].sWord,SENTENCE_END);
				WordTag_Result[j].nHandle=4;//��bigram�ʹ���hmmѵ���н���Ϊ4
				*WordNum=j+1;
				if(*WordNum>MAX_WORDS)//���
					return false;
				*RestLine=s.GetLength();
				break;
			}
			NameTime=true;
		}
		else
			NameTime=true;
		j++;
	}
	*RestLine=s.GetLength();
	strcpy(WordTag_Result[j].sWord,SENTENCE_END);
	WordTag_Result[j].nHandle=4;//��bigram�ʹ���hmmѵ���н���Ϊ4
	*WordNum=j+1;
	if(*WordNum>MAX_WORDS)//���
		return false;
	return true;
}
bool CTrain::GetRawLine(CString filename)
{	
	//get the raw file, named with _raw.txt.
	FILE *fpRaw;
	char RawFile[_MAX_PATH];
	strcpy(RawFile,const_cast<char*>((LPCSTR)filename));
	RawFile[strlen(RawFile)-4]=0;
	strcat(RawFile,"_raw.txt");
	if((fpRaw=fopen(RawFile,"wt"))==NULL) 
		return false;//Cannot open the raw  file to write

	FILE *SourceFile;
	SourceFile=fopen(filename,"r");
	CStdioFile sSource(SourceFile);
	char line[MAX_SENLENTH];
	while(sSource.ReadString(line,MAX_SENLENTH))
	{
		int i=0;
		memset(RawLine,0,MAX_SENLENTH);//Initialize the raw line for write.
		CString s(line);
		s.TrimLeft();
		s.TrimRight();
//		if(s.IsEmpty())
//		{
//			fprintf(fpRaw,"\n");
//			continue;
//		}
		CString word,tag;
		char TempWord[WORD_MAXLENGTH];
		memset(TempWord,0,WORD_MAXLENGTH);
		CString m_line;
		//����ȥ��ǰ����ĸ����ڣ������m_line�У������������֪���Ǹ��ط�����
		m_line=s.SpanExcluding(" ");
		s=s.Mid(m_line.GetLength());
		if(m_line.GetLength()>0)
		{
			i=m_line.Find('/');
			if(i<0) 
				return false;//���ϴ���
			m_line=m_line.Left(i);
			fprintf(fpRaw,"%s  ",const_cast<char*>((LPCSTR)m_line));
		}
		s.TrimLeft();
		while(!s.IsEmpty())
		{
			word=s.SpanExcluding(" ");
			s=s.Mid(word.GetLength());
			s.TrimLeft();
			i=word.Find('/');
			if(i<0) 
				return false;//���ϴ���
			tag=word.Mid(i+1);
			word=word.Left(i);
			i=word.Find('[');//ע�⣬�����������δ��½����ʻ���ֻ��ѵ�������Ա�ע��hmm�У�û���Լ���hmm��������������עֻ��ȥ��
			if(i>=0)		
			{
				word=word.Mid(i+1);
			}
			strcpy(TempWord,const_cast<char*>((LPCSTR)word));
			strcat(RawLine,TempWord);
		}
		memset(line,0,MAX_SENLENTH);
		fprintf(fpRaw,"%s\n",RawLine);
	}
	sSource.Close();
	fclose(fpRaw);
	return true;
}


bool CTrain::NrTrain(int WordNum)
{
	return true;
}


bool CTrain::IsDataExists()
{
	//changed for test the bigram and dict and lex
	char sDataFiles[][40]={"data\\bigram.dat",//data\\BigramDict.dct",data\\Bigramtest.dat
		                   "data\\dict.dat",//"data\\coreDict.dct",data\\dicttest.dat
						   "data\\lex.ctx",//"data\\lexical.ctx",data\\lextest.ctx
	//	                   "data\\nr.dct",
	//					   "data\\nr.ctx",
		                   "data\\ns.dat",
						   "data\\ns.ctx",
		                   "data\\tr.dat",
						   "data\\tr.ctx",
						   ""

	};
	int i=0;
	while(sDataFiles[i][0]!=0)
	{
		if((_access( sDataFiles[i], 0 ))==-1)
			return false;
		i++;
	}
	m_bDisable=false;
	return true;
}