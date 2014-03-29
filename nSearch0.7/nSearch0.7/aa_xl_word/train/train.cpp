//////////////////////////////////////////////////////////////////////
//该类是为ICTCLAS做的开放源码所做的训练程序：
//功能有：二元文法训练，词性HMM训练，地名HMM训练，翻译名HMM训练，
// 可添加的有：三元文法训练，用于和词性一起打分。            
// 
//  
//著作权：  Copyright?2003-2005北大信息科学中心 王志富 
//遵循协议：自然语言处理开放资源许可证1.0
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
//	NeedNsTrain=false;//是否需要ns训练标记。
//	NeedTrTrain=false;//是否需要tr训练标记。
//	NeedNrTrain=false;//是否需要nr训练标记。
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
		return true;//空行
	int WordNum,RestLine=1;
	while(!s.IsEmpty())
	{
		if(ReadLine(s,&WordNum,&RestLine))
		{
			BigramhmmTrain(WordNum);// 二院文法和词性hmm训练
			//	if(NeedNrTrain)
			//		NrTrain(WordNum);//人名hmm训练
//			if(NeedNsTrain)
			NsTrain(&m_dictns,&m_contextns,WordNum,28275);//地名训练
//			if(NeedTrTrain)
			TrTrain(&m_dicttr,&m_contexttr,WordNum,28274);//人名中翻译名的训练
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
		//首先去掉前面的哪个日期，存放在m_line中，如果出错，可以知道那个地方出错
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
//		NeedNsTrain=false;//是否需要ns训练标记。
//		NeedTrTrain=false;//是否需要tr训练标记。
//		NeedNrTrain=false;//是否需要nr训练标记。
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
	char LastWord[WORD_MAXLENGTH],TempWord[WORD_MAXLENGTH];//last用于纪录上一个词，使未登录词的时候不必改变WordTag_Result，相当于copy
	char LastBigram[2*WORD_MAXLENGTH];//用来记录上2个的词，用来统计三元文法。
	char Trigram[3*WORD_MAXLENGTH];//三元词
	char Bigram[2*WORD_MAXLENGTH];
	strcpy(LastWord,SENTENCE_BEGIN);
	memset(LastBigram,0,2*WORD_MAXLENGTH);
	m_dictCore.AddItem(SENTENCE_BEGIN,1,1);//每次纪录开始个数
//	m_dictCore.AddItem(SENTENCE_END,4,0);


//	bool NameTime=true;//记录人名是否连续出现，如果连续出现词书为奇数，则认为是新的人名
	for(int i=1;i<WordNum-1;i++)
	{
		if(m_dictCore.IsExist(WordTag_Result[i].sWord,-1))
		//词表中存在就统计！仅词形存在就算存在，词性不同就添加！ 当isexist的nhandle为-1时，仅检查词形
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
		else//词表中不存在，说明是未登录词。分别处理，词表中未登录制定词性标记未2
		{
			if(WordTag_Result[i].nHandle==30464)//标点
			{
				strcpy(TempWord,WordTag_Result[i].sWord);
				m_dictCore.AddItem(WordTag_Result[i].sWord,WordTag_Result[i].nHandle,1);
				m_context.Add(0,WordTag_Result[i-1].nHandle,WordTag_Result[i].nHandle,1);//添加前一个词类的个数；以及邻现个数
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
			else if(WordTag_Result[i].nHandle==28274)//人名nr
			{
				strcpy(TempWord,"未##人");
//				if(NameTime)
//				{
					m_dictCore.AddItem("未##人",2,1);
					m_context.Add(0,WordTag_Result[i-1].nHandle,WordTag_Result[i].nHandle,1);//添加前一个词类的个数；以及邻现个数						
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
			else if(WordTag_Result[i].nHandle==28275)////未##地ns
			{
				m_dictCore.AddItem("未##地",2,1);
				m_context.Add(0,WordTag_Result[i-1].nHandle,WordTag_Result[i].nHandle,1);//添加前一个词类的个数；以及邻现个数
//				NameTime=true;
				strcpy(TempWord,"未##地");
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
			else if(WordTag_Result[i].nHandle==28276)//////未##团nt
			{
				m_dictCore.AddItem("未##团",2,1);
				m_context.Add(0,WordTag_Result[i-1].nHandle,WordTag_Result[i].nHandle,1);//添加前一个词类的个数；以及邻现个数
//				NameTime=true;
				strcpy(TempWord,"未##团");
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
			else if(WordTag_Result[i].nHandle==28280)/////未##串 nx
			{
				m_dictCore.AddItem("未##串",2,1);
				m_context.Add(0,WordTag_Result[i-1].nHandle,WordTag_Result[i].nHandle,1);//添加前一个词类的个数；以及邻现个数
//				NameTime=true;
				strcpy(TempWord,"未##串");
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
			else if(WordTag_Result[i].nHandle==28282)///未##专nz
			{
				m_dictCore.AddItem("未##专",2,1);
				m_context.Add(0,WordTag_Result[i-1].nHandle,WordTag_Result[i].nHandle,1);//添加前一个词类的个数；以及邻现个数
//				NameTime=true;
				strcpy(TempWord,"未##专");
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
			else if(WordTag_Result[i].nHandle==29696)///未##时@按t 
			{
				m_dictCore.AddItem("未##时",2,1);
				m_context.Add(0,WordTag_Result[i-1].nHandle,WordTag_Result[i].nHandle,1);//添加前一个词类的个数；以及邻现个数
//				NameTime=true;
				strcpy(TempWord,"未##时");
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
			else if(WordTag_Result[i].nHandle==27904)///未##数 m
			{
				m_dictCore.AddItem("未##数",2,1);
				m_context.Add(0,WordTag_Result[i-1].nHandle,WordTag_Result[i].nHandle,1);//添加前一个词类的个数；以及邻现个数
//				NameTime=true;
				strcpy(TempWord,"未##数");
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
			else///未##它
			{
				m_dictCore.AddItem("未##它",2,1);
				m_context.Add(0,WordTag_Result[i-1].nHandle,WordTag_Result[i].nHandle,1);//添加前一个词类的个数；以及邻现个数
//				NameTime=true;
				strcpy(TempWord,"未##它");
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
	//整句结束后，须添加最后一个到末尾的转移概率，之所以不放在循环中，因为词典中没有SENTENCE_END "末##末"，这个词必须加入
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
bool CTrain::NsTrain(CDictionary *m_dict,CContextStat *m_con,int WordNum,int TrainHandle)//暂不考虑[京东/ns 大/a 峡谷/n]ns这样大的地名的识别
{

	//更改ns外部标记,用nspos数组代替WordTag_Result.nhandle，这样不必改变WordTag_Result
	for(int i=1;i<WordNum-1;i++)
	{
		if(WordTag_Result[i].nHandle!=TrainHandle)//28275ns,28274tr.
		{
			if(NsPos[i-1]==TrainHandle)
			{
				NsPos[i]=12;
			}
			else
				NsPos[i]=0;//无关ns标记，默认
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
					NsPos[i]=0;//无关ns标记，默认
			}
			else
			{
				NsPos[i]=TrainHandle;
				if(NsPos[i-1]==12)///如果上一个不是作为上一个地名的下文，则作为两个地名中间成分
					NsPos[i-1]=13;
				else if(NsPos[i-1]!=TrainHandle)
					NsPos[i-1]=11;//如果上一个不是作为上一个地名的下文，则作为该地名的上文
/*				if((WordTag_Result[i+1].nHandle==TrainHandle)&&(!m_dictCore.IsExist(WordTag_Result[i+1].sWord,-1)))
//				//如果下一个是地名且该地名不在词典中，需要训练，不能视为一般词汇
///				{
//					NsPos[i+1]=TrainHandle;
//					i++;
//				}
//				else
//				{
//					NsPos[i+1]=12;//改下一个标记为地名下文
//					i++;
//				}*/
			}
		}
	}
	//最后更改始末，因为优先级高，如果以前被改为地名上下文，可更正回来。
	NsPos[0]=100;//在ns训练中开始为100
	NsPos[WordNum-1]=101;//在ns训练中结束为101
	//外部标记完毕。开始内部标记同时训练。

	int LastHandle=100;
	m_dict->AddItem(WordTag_Result[0].sWord,NsPos[0],1);
	for(int i=1;i<WordNum;i++)
	{
		if(NsPos[i]!=TrainHandle)
		{
			m_con->Add(0,LastHandle,NsPos[i],1);
			if(NsPos[i]!=0)//标记为0的不统计到地名词典中去			
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
bool CTrain::TrTrain(CDictionary *m_dict,CContextStat *m_con,int WordNum,int TrainHandle)//暂不考虑[京东/ns 大/a 峡谷/n]ns这样大的地名的识别
{

	//更改ns外部标记,用nspos数组代替WordTag_Result.nhandle，这样不必改变WordTag_Result
	//、增加一个标记20作为非翻译人名。
	//注意翻译人名的上下文如果是20，或者是TrainHandle；不能改动，否则只能是12.11.13
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
				NsPos[i]=0;//无关tr标记，默认
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
					NsPos[i]=0;//无关tr标记，默认
			}
			else
			{
				if(!IsAllForeign(WordTag_Result[i].sWord))
				{
					NsPos[i]=20;//如果不是翻译人名，标记为非翻译人名。
					HaveChineseNr=true;
					continue;
				}
				NsPos[i]=TrainHandle;
				if(NsPos[i-1]==12)//如果上一个是作为上一个翻译人名的下文，则作为两个翻译人名中间成分
					NsPos[i-1]=13;
				else if((NsPos[i-1]!=20)&&(NsPos[i-1]!=TrainHandle))
					NsPos[i-1]=11;//如果上一个不是作为上一个翻译人名的下文，而且不是非翻译人名，则作为该翻译人名的上文
//				if((WordTag_Result[i+1].nHandle==TrainHandle)&&(!m_dictCore.IsExist(WordTag_Result[i+1].sWord,-1)))
//				//如果下一个是人名且该人名不在词典中，可能需要训练，不能视为一般词汇
//				{
//					if(!IsAllForeign(WordTag_Result[i+1].sWord))//不是翻译人名，是普通人名
//					{
//						NsPos[i+1]=20;//改下一个标记为标记为非翻译人名。
//						HaveChineseNr=true;
//						i++;
//					}
//					else//是翻译人名需要训练，标记为翻译人名
//					{
//						NsPos[i+1]=TrainHandle;
//						i++;
//					}
//				}
//				else
//				{
//					NsPos[i+1]=12;//改下一个标记为翻译人名下文
//					i++;
//				}
			}
		}
	}
	int j;
	if(HaveChineseNr)
	{
		for(int i=1;i<WordNum-1;i++)//重新check，对非翻译人名。还做了上下文，情况视为不存在。
		{
			if(NsPos[i]==20)
			{
				if(NsPos[i-1]==TrainHandle)
				{//如果作为了翻译人名的下文，则下找，找到下文。			{
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
				if(NsPos[i+1]==TrainHandle)//如果作为了翻译人名的上文，则上找，找到上文。
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

	//注意此时还有20存在。因为上面的循环不能更改20标记，否则可能出现前面改为0，后面又重新作为上下文的情况

	//最后更改始末，因为优先级高，如果以前被改为翻译人名上下文，可更正回来。
	NsPos[0]=100;//在ns训练中开始为100
	NsPos[WordNum-1]=101;//在ns训练中结束为101
	//外部标记完毕。开始内部标记同时训练。

	char LastWord[WORD_MAXLENGTH];
	strcpy(LastWord,WordTag_Result[0].sWord);//初始为第一个词
	int LastHandle=100;
	m_dict->AddItem(WordTag_Result[0].sWord,NsPos[0],1);
	for(int i=1;i<WordNum;i++)
	{
		if(NsPos[i]!=TrainHandle)
		{
			if(NsPos[i]==20)//非翻译人名跳过
				continue;			
			m_con->Add(0,LastHandle,NsPos[i],1);
			if(NsPos[i]!=0)//标记为0的不统计到翻译人名词典中去			
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
				sNsWord=sNsWord.Mid(1);//地名内部有核心词典以外字，比如字母等。不训练！
		}
	}
	*NsTagNum=i;
	NsTag_Result[i-1].nHandle=3;//末词为3
	NsTag_Result[0].nHandle=1;//首词为1	
	for(i=1;i<*NsTagNum-1;i++)
	{
		NsTag_Result[i].nHandle=2;//中间为2
	}
	if(i>WORD_MAXLENGTH)
		return false;//溢出
	return true;
}

/*********************************************************************
 *
 *  Func Name  : ReadLine
 *
 *  Description: to get a sentence(ended by 。！？：；)form the paragraph
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
	//改动：把人名合并训练，方便hmm和ns的训练。bool NameTime=true;//记录人名是否连续出现，如果连续出现词书为奇数，则认为是新的人名
	bool NameTime=true;
	int j=1;
	s.TrimLeft();
	s.TrimRight();
	CString word,tag;
	char TempWord[WORD_MAXLENGTH];
	memset(TempWord,0,WORD_MAXLENGTH);
	strcpy(WordTag_Result[0].sWord,SENTENCE_BEGIN);
	WordTag_Result[0].nHandle=1;//在bigram和词性hmm训练中开始为1
	while(!s.IsEmpty())
	{
		word=s.SpanExcluding(" ");
		s=s.Mid(word.GetLength());
		s.TrimLeft();
		int i=word.Find('/');
		if(i<0) 
			return false;//语料错误！
		tag=word.Mid(i+1);
		word=word.Left(i);
		i=word.Find('[');//注意，这种情况属于未登陆团体词或成语，只能训练到词性标注的hmm中，没有自己的hmm，对这个括号外标注只好去掉
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
		int nHandle=m_dictCore.GetPOSValue(const_cast<char*>((LPCSTR)tag));
		strcpy(TempWord,const_cast<char*>((LPCSTR)word));
		strcpy(WordTag_Result[j].sWord,TempWord);
//		strcat(RawLine,TempWord);
		WordTag_Result[j].nHandle=nHandle;
//		if(nHandle==28275)//未##地
//		{
//			if(!m_dictCore.IsExist(TempWord,-1))//再核心词典中不存在
//				NeedNsTrain=true;
//			NameTime=true;
//		}
/*		else*/ if(nHandle==28274)//人名
		{
			if(NameTime)
			{
				NameTime=false;
			}
			else
			{
				j=j-1;//退回一个值。
				strcat(WordTag_Result[j].sWord,TempWord);//把上一个人名粘在一起
//				if(!m_dictCore.IsExist(WordTag_Result[j].sWord,-1))//再核心词典中不存在
//				{
//					if(IsAllForeign(WordTag_Result[j].sWord))
//						NeedTrTrain=true;
//					else
//						NeedNrTrain=true;
//				}
				
				NameTime=true;
			}		
		}
		else if((strcmp(TempWord,"。")==0)||(strcmp(TempWord,"！")==0)||(strcmp(TempWord,"？")==0)||(strcmp(TempWord,"；")==0)||(strcmp(TempWord,"：")==0))
		//end of sentence, … is not included,because, it can be in the sentence. 
		{
			if(!s.IsEmpty())
			{
				j+=1;
				word=s.SpanExcluding(" ");
				strcpy(TempWord,const_cast<char*>((LPCSTR)word));
				if(strcmp(TempWord,"”/w")==0)//如果是在引言中，避免引号成为下一句的开头,多读一个词。
				{
					s=s.Mid(word.GetLength());
					s.TrimLeft();
					int i=word.Find('/');
					tag=word.Mid(i+1);
					word=word.Left(i);
					WordTag_Result[j].nHandle=m_dictCore.GetPOSValue(const_cast<char*>((LPCSTR)tag));
					strcpy(WordTag_Result[j++].sWord,"”");
//					strcat(RawLine,TempWord);
				}
				strcpy(WordTag_Result[j].sWord,SENTENCE_END);
				WordTag_Result[j].nHandle=4;//在bigram和词性hmm训练中结束为4
				*WordNum=j+1;
				if(*WordNum>MAX_WORDS)//溢出
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
	WordTag_Result[j].nHandle=4;//在bigram和词性hmm训练中结束为4
	*WordNum=j+1;
	if(*WordNum>MAX_WORDS)//溢出
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
		//首先去掉前面的哪个日期，存放在m_line中，如果出错，可以知道那个地方出错
		m_line=s.SpanExcluding(" ");
		s=s.Mid(m_line.GetLength());
		if(m_line.GetLength()>0)
		{
			i=m_line.Find('/');
			if(i<0) 
				return false;//语料错误！
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
				return false;//语料错误！
			tag=word.Mid(i+1);
			word=word.Left(i);
			i=word.Find('[');//注意，这种情况属于未登陆团体词或成语，只能训练到词性标注的hmm中，没有自己的hmm，对这个括号外标注只好去掉
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