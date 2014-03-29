

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


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


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 440
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __aa_xl_wordidl_h__
#define __aa_xl_wordidl_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef ___Daa_xl_word_FWD_DEFINED__
#define ___Daa_xl_word_FWD_DEFINED__
typedef interface _Daa_xl_word _Daa_xl_word;
#endif 	/* ___Daa_xl_word_FWD_DEFINED__ */


#ifndef ___Daa_xl_wordEvents_FWD_DEFINED__
#define ___Daa_xl_wordEvents_FWD_DEFINED__
typedef interface _Daa_xl_wordEvents _Daa_xl_wordEvents;
#endif 	/* ___Daa_xl_wordEvents_FWD_DEFINED__ */


#ifndef __aa_xl_word_FWD_DEFINED__
#define __aa_xl_word_FWD_DEFINED__

#ifdef __cplusplus
typedef class aa_xl_word aa_xl_word;
#else
typedef struct aa_xl_word aa_xl_word;
#endif /* __cplusplus */

#endif 	/* __aa_xl_word_FWD_DEFINED__ */


#ifdef __cplusplus
extern "C"{
#endif 

void * __RPC_USER MIDL_user_allocate(size_t);
void __RPC_USER MIDL_user_free( void * ); 


#ifndef __aa_xl_wordLib_LIBRARY_DEFINED__
#define __aa_xl_wordLib_LIBRARY_DEFINED__

/* library aa_xl_wordLib */
/* [control][helpstring][helpfile][version][uuid] */ 


EXTERN_C const IID LIBID_aa_xl_wordLib;

#ifndef ___Daa_xl_word_DISPINTERFACE_DEFINED__
#define ___Daa_xl_word_DISPINTERFACE_DEFINED__

/* dispinterface _Daa_xl_word */
/* [helpstring][uuid] */ 


EXTERN_C const IID DIID__Daa_xl_word;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("6EEC6F7E-CB2E-45CB-AC0D-6FB37C7925D4")
    _Daa_xl_word : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _Daa_xl_wordVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _Daa_xl_word * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _Daa_xl_word * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _Daa_xl_word * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _Daa_xl_word * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _Daa_xl_word * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _Daa_xl_word * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _Daa_xl_word * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        END_INTERFACE
    } _Daa_xl_wordVtbl;

    interface _Daa_xl_word
    {
        CONST_VTBL struct _Daa_xl_wordVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _Daa_xl_word_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define _Daa_xl_word_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define _Daa_xl_word_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define _Daa_xl_word_GetTypeInfoCount(This,pctinfo)	\
    (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo)

#define _Daa_xl_word_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo)

#define _Daa_xl_word_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)

#define _Daa_xl_word_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___Daa_xl_word_DISPINTERFACE_DEFINED__ */


#ifndef ___Daa_xl_wordEvents_DISPINTERFACE_DEFINED__
#define ___Daa_xl_wordEvents_DISPINTERFACE_DEFINED__

/* dispinterface _Daa_xl_wordEvents */
/* [helpstring][uuid] */ 


EXTERN_C const IID DIID__Daa_xl_wordEvents;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("F9498B14-EB18-4E58-A030-495251C8ADE0")
    _Daa_xl_wordEvents : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _Daa_xl_wordEventsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _Daa_xl_wordEvents * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _Daa_xl_wordEvents * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _Daa_xl_wordEvents * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _Daa_xl_wordEvents * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _Daa_xl_wordEvents * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _Daa_xl_wordEvents * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _Daa_xl_wordEvents * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        END_INTERFACE
    } _Daa_xl_wordEventsVtbl;

    interface _Daa_xl_wordEvents
    {
        CONST_VTBL struct _Daa_xl_wordEventsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _Daa_xl_wordEvents_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define _Daa_xl_wordEvents_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define _Daa_xl_wordEvents_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define _Daa_xl_wordEvents_GetTypeInfoCount(This,pctinfo)	\
    (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo)

#define _Daa_xl_wordEvents_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo)

#define _Daa_xl_wordEvents_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)

#define _Daa_xl_wordEvents_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___Daa_xl_wordEvents_DISPINTERFACE_DEFINED__ */


EXTERN_C const CLSID CLSID_aa_xl_word;

#ifdef __cplusplus

class DECLSPEC_UUID("39655597-BED9-4EE9-9970-65F811E8144A")
aa_xl_word;
#endif
#endif /* __aa_xl_wordLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


