//////////////////////////////////////////////////////////////////////
//该类是为ICTCLAS做的开放源码所做的训练程序：
//功能有：二元文法训练，词性HMM训练，人名HMM训练，地名HMM训练，翻译名HMM训练，
// 可添加的有：三元文法训练，用于和词性一起打分。            
// 
//  
//著作权：  Copyright?2002-2005北大信息科学中心 王志富 
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
 * Filename: Train.h
 * Abstract:
 *           train a bigram model and all hmms for ICTCLAS
 * Author:   Wang Zhifu
 *          (wangzf@cis.pku.edu.cn)
 * Date:     2004-2-6
 *
 * Notes:     thanks for Wu Jinyi and Lin Xiaojun's help 
 *                
 * 
 ****************************************************************************/
//
#include "..\\Utility\\Utility.h"
#include "..\\Utility\\ContextStat.h"
#include "..\\Utility\\Dictionary.h"
#if !defined TRAIN_H
#define TRAIN_H

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#define MAX_WORDS 1000//假设一个句子不超过1000个词
#define WORD_MAXLENGTH 100//假定一个地名（翻译名）名字不超过100个字，用于地名内部
#define MAX_SENLENTH 20000//假定一个句子不超过20000个字
/////////////////////////////////////////////////////////////////////////////
// CTrain

class CTrain
{
// Construction
public:
	CTrain();
	virtual ~CTrain();
	bool Save();
	bool Load();
	CStdioFile m_fileLog;

	bool Train(CString s);
//	bool ParagraphTrain(char *sParagraph);
	bool FileTrain(CString filename);

	bool BigramhmmTrain(int WordNum);// 二院文法和词性hmm训练
	bool NrTrain(int WordNum);//人名hmm和翻译名训练
	bool NsTrain(CDictionary *m_dict,CContextStat *m_con,int WordNum,int TrainHandle);//地名的训练
	bool TrTrain(CDictionary *m_dict,CContextStat *m_con,int WordNum,int TrainHandle);//翻译名的训练
	//这里应该使用指针引用，否则无法修改词典和矩阵。值引用的错误

	bool ReadLine(CString s,int *WordNum,int *RestLine);//读入到array中。
	bool GetRawLine(CString filename);
//	bool NeedNsTrain;//是否需要ns训练标记。
//	bool NeedTrTrain;//是否需要tr训练标记。
//	bool NeedNrTrain;//是否需要nr训练标记。
//以上标记去掉的原因是，会导致训练的数据不平衡！没有这些未登陆词的句子也是需要训练到未登陆词模型中的·！
	bool NsSplitTag(char *NsWord,int *NsTagNum);//Ns内部标记分离
	CDictionary m_dictCore,m_dictBigram,m_dictns,m_dicttr;
	CDictionary m_dictTrigram;//added in 2004-4-5
	CContextStat m_context,m_contextns,m_contexttr;


protected:
	bool m_bDisable;
	bool IsDataExists();
	PWORD_RESULT WordTag_Result,NsTag_Result;
	int NsPos[MAX_WORDS];//存储ns标记，不必改变WordTag_Result。
	char RawLine[MAX_SENLENTH];//recode the raw words line to get raw file.
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TRAINDDLG_H__5965BD82_A879_4158_95F3_6BCF5D2B8648__INCLUDED_)
