

/* this ALWAYS GENERATED file contains the IIDs and CLSIDs */

/* link this file in with the server and any clients */


 /* File created by MIDL compiler version 6.00.0366 */
/* at Sat Apr 07 16:29:39 2007
 */
/* Compiler settings for .\aa_xl_word.idl:
    Oicf, W1, Zp8, env=Win32 (32b run)
    protocol : dce , ms_ext, c_ext
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
//@@MIDL_FILE_HEADING(  )

#pragma warning( disable: 4049 )  /* more than 64k source lines */


#ifdef __cplusplus
extern "C"{
#endif 


#include <rpc.h>
#include <rpcndr.h>

#ifdef _MIDL_USE_GUIDDEF_

#ifndef INITGUID
#define INITGUID
#include <guiddef.h>
#undef INITGUID
#else
#include <guiddef.h>
#endif

#define MIDL_DEFINE_GUID(type,name,l,w1,w2,b1,b2,b3,b4,b5,b6,b7,b8) \
        DEFINE_GUID(name,l,w1,w2,b1,b2,b3,b4,b5,b6,b7,b8)

#else // !_MIDL_USE_GUIDDEF_

#ifndef __IID_DEFINED__
#define __IID_DEFINED__

typedef struct _IID
{
    unsigned long x;
    unsigned short s1;
    unsigned short s2;
    unsigned char  c[8];
} IID;

#endif // __IID_DEFINED__

#ifndef CLSID_DEFINED
#define CLSID_DEFINED
typedef IID CLSID;
#endif // CLSID_DEFINED

#define MIDL_DEFINE_GUID(type,name,l,w1,w2,b1,b2,b3,b4,b5,b6,b7,b8) \
        const type name = {l,w1,w2,{b1,b2,b3,b4,b5,b6,b7,b8}}

#endif !_MIDL_USE_GUIDDEF_

MIDL_DEFINE_GUID(IID, LIBID_aa_xl_wordLib,0x5DFAF7B7,0xE60E,0x4A7E,0xAB,0xB1,0x13,0x81,0xAD,0x00,0xEE,0x2A);


MIDL_DEFINE_GUID(IID, DIID__Daa_xl_word,0x6EEC6F7E,0xCB2E,0x45CB,0xAC,0x0D,0x6F,0xB3,0x7C,0x79,0x25,0xD4);


MIDL_DEFINE_GUID(IID, DIID__Daa_xl_wordEvents,0xF9498B14,0xEB18,0x4E58,0xA0,0x30,0x49,0x52,0x51,0xC8,0xAD,0xE0);


MIDL_DEFINE_GUID(CLSID, CLSID_aa_xl_word,0x39655597,0xBED9,0x4EE9,0x99,0x70,0x65,0xF8,0x11,0xE8,0x14,0x4A);

#undef MIDL_DEFINE_GUID

#ifdef __cplusplus
}
#endif



