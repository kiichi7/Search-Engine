//#include "..\\Utility\\Dictionary.h"
//#include "..\\Utility\\Utility.h"
#define WORD_MAXLENGTH 100
#define SOFTRETURN  22
#define HARDRETURN  '\n'
#define ENDOFFILE   '\x1a'

//**********SCKBASE.H
#define SCKWDNUM 73893  
#define SCKNDNUM 6680 
#define DUOYINNUM 854
#define LEFTPY 86//84 2312 character not in cluded in the dict, add 镕、珮
#define NMPY 116
//#define MAX_PYSENTEN 10000
//#define MAX_ZI 2000
class CPytag
{
public:
	CPytag();
	virtual ~CPytag();
	bool load();
	bool BinarySerch(char* zword,int* ziid);
	bool BiSearch(char* leftwd,int* leftziid);
	bool GetDictWordPinyin(char* wd,char* tag,int id,char* npinyin,char* bdpy);//get dictionary word pinyin
	bool GetWordPinyin(char* wd,char* tag,char* npinyin,char* bdpy);//get all word pinyin, include "out-of-vocabulary"
	bool CheckFirstTone(char* pinyin,int* tone);//to get first tone of pinyin string
	char* WordRule33to32(char* sWord,char* sPinyin,char* rPinyin,bool ExistIndict);//use 33 to 23 rule of biandiao.
	bool SentenceBianDiaotag(char* senten,char* sPinyin,char* rBDPinyin);//sentence pinyin tagging; include biandiao;
	void convert(char* temppd,char* tempwd,int len);


// dictinary index. for pydctinx.txt
	struct inx 
	{char word[3];
	int wordid;
	short rnum;
	} sckndx[SCKNDNUM];

// for dictionary of pydct
	
	char ci[SCKWDNUM][9],cixing[SCKWDNUM][2],pinyin[SCKWDNUM][29],bdpinyin[SCKWDNUM][29];
	short prop[SCKWDNUM];
//	unsigned int cilen[SCKWDNUM];
//  name pinyin	for namepy.txt
	char fname[NMPY][9];
	char fnmpy[NMPY][15];//15 is enough for name pinyin.

//left zi pinyin for leftzi.txt
	char leftzi[LEFTPY][3],leftyin[LEFTPY][9];//chuang3_

// for the multi-pinyin word list file : mwdpy.txt
	int mwdpyid[DUOYINNUM];
	char mwdpy[DUOYINNUM][29];//词典中最长的注音为24+4*_+28；

	
//////////////////////////////////
// for initialize the dict ,when pinyin tagging, needn't.
//	void ChangeDict(char* filename);
//	void ChangeNameDict(char* filename);
//	void ChangeLeftDict(char* filename);
//	void ChangeMpDict(char* filename);
//	CDictionary m_dictCore;



};