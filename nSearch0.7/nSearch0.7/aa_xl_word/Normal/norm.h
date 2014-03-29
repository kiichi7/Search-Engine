#include "..\\Utility\\Utility.h"

#define WORD_MAXLENGTH 100
#define CHANGETIME 100

#define  T_SINGLE  20//SINGLE byte
#define  T_DELIMITER T_SINGLE+1//delimiter
#define  T_CHINESE   T_SINGLE+2//Chinese Char
#define  T_NUM       T_SINGLE+3//����
#define  T_OTHER     T_SINGLE+4//Other

class CNorm
{
public:
	CNorm();
	virtual ~CNorm();
	 char NumOut[10][3];
	 int ctype[CHANGETIME];
	 bool IsAllAlabNum(char *sWord);//�ж��Ƿ�ȫ�ǰ���������
	 char sSameTypeString[CHANGETIME][WORD_MAXLENGTH];//�洢һ���ʵ��Ӷ�
	 bool SentenceNorm(char* senten, char* ResultSen);//�Ծ��ӹ淶��
	 bool Processing(char* NumWord,int* time);//�����ʽ��зֶδ���
	 bool ReadWord(CString s,char* word,char* tag,int* lenth);//�Ӿ����ֶ�ȡһ����
	 bool ChangeWord(char* sWord,char* sTag,char* sResult,bool type);//��һ�����ʽ��������ֵ������
	 bool ConvertNumber(char* sWord,char* Result,bool type);//��һ�����ֶν����������֣��������ֵ������
	 int GetCharType(unsigned char *sChar);//�õ�һ���ַ���������
	 bool GetSameType(char* sChar,int* lenth,int* type);//�õ���ͬ���͵��ַ������ȡ�

};