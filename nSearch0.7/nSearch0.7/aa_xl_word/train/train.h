//////////////////////////////////////////////////////////////////////
//������ΪICTCLAS���Ŀ���Դ��������ѵ������
//�����У���Ԫ�ķ�ѵ��������HMMѵ��������HMMѵ��������HMMѵ����������HMMѵ����
// ����ӵ��У���Ԫ�ķ�ѵ�������ںʹ���һ���֡�            
// 
//  
//����Ȩ��  Copyright?2002-2005������Ϣ��ѧ���� ��־�� 
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

#define MAX_WORDS 1000//����һ�����Ӳ�����1000����
#define WORD_MAXLENGTH 100//�ٶ�һ�������������������ֲ�����100���֣����ڵ����ڲ�
#define MAX_SENLENTH 20000//�ٶ�һ�����Ӳ�����20000����
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

	bool BigramhmmTrain(int WordNum);// ��Ժ�ķ��ʹ���hmmѵ��
	bool NrTrain(int WordNum);//����hmm�ͷ�����ѵ��
	bool NsTrain(CDictionary *m_dict,CContextStat *m_con,int WordNum,int TrainHandle);//������ѵ��
	bool TrTrain(CDictionary *m_dict,CContextStat *m_con,int WordNum,int TrainHandle);//��������ѵ��
	//����Ӧ��ʹ��ָ�����ã������޷��޸Ĵʵ�;���ֵ���õĴ���

	bool ReadLine(CString s,int *WordNum,int *RestLine);//���뵽array�С�
	bool GetRawLine(CString filename);
//	bool NeedNsTrain;//�Ƿ���Ҫnsѵ����ǡ�
//	bool NeedTrTrain;//�Ƿ���Ҫtrѵ����ǡ�
//	bool NeedNrTrain;//�Ƿ���Ҫnrѵ����ǡ�
//���ϱ��ȥ����ԭ���ǣ��ᵼ��ѵ�������ݲ�ƽ�⣡û����Щδ��½�ʵľ���Ҳ����Ҫѵ����δ��½��ģ���еġ���
	bool NsSplitTag(char *NsWord,int *NsTagNum);//Ns�ڲ���Ƿ���
	CDictionary m_dictCore,m_dictBigram,m_dictns,m_dicttr;
	CDictionary m_dictTrigram;//added in 2004-4-5
	CContextStat m_context,m_contextns,m_contexttr;


protected:
	bool m_bDisable;
	bool IsDataExists();
	PWORD_RESULT WordTag_Result,NsTag_Result;
	int NsPos[MAX_WORDS];//�洢ns��ǣ����ظı�WordTag_Result��
	char RawLine[MAX_SENLENTH];//recode the raw words line to get raw file.
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TRAINDDLG_H__5965BD82_A879_4158_95F3_6BCF5D2B8648__INCLUDED_)
