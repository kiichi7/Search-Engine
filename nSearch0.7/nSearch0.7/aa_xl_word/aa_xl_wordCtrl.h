#pragma once

// aa_xl_wordCtrl.h : Caa_xl_wordCtrl ActiveX �ؼ����������


// Caa_xl_wordCtrl : �й�ʵ�ֵ���Ϣ������� aa_xl_wordCtrl.cpp��

class Caa_xl_wordCtrl : public COleControl
{
	DECLARE_DYNCREATE(Caa_xl_wordCtrl)

// ���캯��
public:
	Caa_xl_wordCtrl();

// ��д
public:
	virtual void OnDraw(CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid);
	virtual void DoPropExchange(CPropExchange* pPX);
	virtual void OnResetState();
	virtual DWORD GetControlFlags();

    virtual void   Replace(char   *strIn,   char   *   f_c,   char   *   r_c);

// ʵ��
protected:
	~Caa_xl_wordCtrl();

	DECLARE_OLECREATE_EX(Caa_xl_wordCtrl)    // �๤���� guid
	DECLARE_OLETYPELIB(Caa_xl_wordCtrl)      // GetTypeInfo
	DECLARE_PROPPAGEIDS(Caa_xl_wordCtrl)     // ����ҳ ID
	DECLARE_OLECTLTYPE(Caa_xl_wordCtrl)		// �������ƺ�����״̬

// ��Ϣӳ��
	DECLARE_MESSAGE_MAP()


afx_msg BSTR aa_X_word_ocx(BSTR a) ;
// ����ӳ��
	DECLARE_DISPATCH_MAP()



	afx_msg void AboutBox();

// �¼�ӳ��
	DECLARE_EVENT_MAP()

// ���Ⱥ��¼� ID
public:
	enum {
	};
};

