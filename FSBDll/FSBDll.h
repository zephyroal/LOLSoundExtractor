// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the FSBDLL_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// FSBDLL_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef FSBDLL_EXPORTS
#define FSBDLL_API extern "C" __declspec(dllexport)
#else
#define FSBDLL_API extern "C"  __declspec(dllimport)
#endif

FSBDLL_API  int GetTotalNum();
FSBDLL_API int GetNowNum();
FSBDLL_API void AbortFSBMoudle();
FSBDLL_API int   ExtractFDBFile(int argc, char strFrom[],char strTo[]);
FSBDLL_API const char* GetOutBuffer();
FSBDLL_API void SetBuffer(char *ViewBuffer);