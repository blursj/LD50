using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CTAPIHelper
{

    struct CTOVERLAPPED
    {
        uint dwStatus; /* completion status		*/
        uint dwLength; /* length of result buffer	*/
        byte[] pData;        /* result buffer		*/
        uint OffsetHigh;   /* not used (as per Win32)	*/
        IntPtr hEvent;      /* event handle to signal	*/
    }

    struct CT_TAGVALUE_ITEMS
    {
        uint dwLength;                         /* size, in bytes, of this structure	*/
        UInt64 nTimestamp;                           /*	timestamp							*/
        UInt64 nValueTimestamp;                 /*	value timestamp						*/
        UInt64 nQualityTimestamp;                   /*	quality timestamp					*/
        byte bQualityGeneral;                   /*	quality general						*/
        byte bQualitySubstatus;                 /*	quality substatus					*/
        byte bQualityLimit;                     /*	quality limit						*/
        byte bQualityExtendedSubstatus;         /*	quality extended substatus			*/
        uint nQualityDatasourceErrorCode;       /*	quality datasource error			*/
        bool bOverride;                          /*	quality override flag				*/
        bool bControlMode;                      /*	quality control mode flag			*/
    }

    class CTAPI
    {
       

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern IntPtr ctOpen(string sComputerName, string sUsername, string sPassword, uint nMode);

        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool ctTagRead(IntPtr hCTAPI, string tag,
                               [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder value, int length);/* read from tag			*/



        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool ctOpenEx(string arg1, string arg2, string arg3, uint arg4, IntPtr arg5);
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern IntPtr ctClientCreate();
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool ctClientDestroy(IntPtr arg1);
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool ctClose(IntPtr arg1);                        /* Close CTAPI interface	*/
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool ctCloseEx(IntPtr arg1, bool arg2);
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool ctCancelIO(IntPtr arg1, ref CTOVERLAPPED ctv);              /* cancel pending I/O		*/
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern uint ctCicode(IntPtr arg1, string arg2, uint arg3, uint arg4, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder arg5, uint arg6, ref CTOVERLAPPED ctv);   /* execute cicode		*/
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool ctPointWrite(IntPtr arg1, IntPtr arg2, ref IntPtr call, uint arg3, ref CTOVERLAPPED ctv);      /* write to point IntPtr	*/
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool ctPointRead(IntPtr arg1, IntPtr arg2, ref IntPtr call, uint arg3, ref CTOVERLAPPED ctv);       /* read from point IntPtr	*/
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern IntPtr ctTagToPoint(IntPtr arg1, string arg2, uint arg3, ref CTOVERLAPPED ctv);       /* convert tag into point IntPtr*/
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool ctPointClose(IntPtr arg1, IntPtr arg2);                   /* free a point IntPtr		*/
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern IntPtr ctPointCopy(IntPtr arg1);                      /* copy a point IntPtr		*/
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool ctPointGetProperty(IntPtr arg1, string arg5, ref IntPtr call, uint arg2, ref uint arg3, uint arg4);   /* get point property		*/
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern uint ctPointDataSize(IntPtr arg1);                   /* size of point data buffer	*/
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern uint ctPointBitShift(IntPtr arg1);                   /* calculate bit shift offset	*/
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool ctPointToStr(IntPtr arg1, ref byte[] arg2, uint arg3, ref byte[] arg4, uint arg5, uint arg6);        /* format point data to string	*/
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool ctStrToPoint(IntPtr arg1, string arg2, uint arg3, ref byte[] arg6, uint arg4, uint arg5);       /* format string data into point*/
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool ctTagWrite(IntPtr arg1, string arg2, string arg3);                 /* write to tag				*/
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool ctTagWriteEx(IntPtr arg1, string arg2, string arg3, ref CTOVERLAPPED ctv);    /* write to tag	overlaped	*/

        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool ctTagReadEx(IntPtr arg1, string arg2, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder arg3, uint arg4, ref CT_TAGVALUE_ITEMS ctt);      /* read extended data from tag			*/
        /*[DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool    ctEngToRaw(double*, double, CTSCALE*, uint);   
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool    ctRawToEng(double*, double, CTSCALE*, uint);   
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool    ctGetOverlappedResult(IntPtr, CTOVERLAPPED*, uint*, bool); 
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool    ctEngToRaw(double*, double, CTSCALE*, uint);           
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool    ctRawToEng(double*, double, CTSCALE*, uint);           
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern IntPtr    ctFindFirst(IntPtr, LPCTSTR, LPCTSTR, IntPtr*, uint);
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern IntPtr    ctFindFirstEx(IntPtr, LPCTSTR, LPCTSTR, LPCTSTR, IntPtr*, uint); */
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool ctFindNext(IntPtr arg1, ref IntPtr arg2);
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool ctFindPrev(IntPtr arg1, ref IntPtr arg2);
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern uint ctFindScroll(IntPtr arg1, uint arg2, long arg3, ref IntPtr arg4);
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool ctFindClose(IntPtr arg1);
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern long ctFindNumRecords(IntPtr arg1);

        //[DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        //public static extern bool    ctGetProperty(IntPtr arg1, string arg2, void* arg3, uint arg4, ref uint arg5, uint arg6);

        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern IntPtr ctListNew(IntPtr arg1, uint arg2);                 /* create poll list		*/

        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool ctListFree(IntPtr arg1);                     /* free poll list		*/
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern IntPtr ctListAdd(IntPtr arg1, string arg2);                        /* add tag to poll list		*/

        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern IntPtr ctListAddEx(IntPtr hList, string sTag, bool bRaw, int nPollPeriodMS, double dDeadband); // add tag to poll list

        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool ctListDelete(IntPtr arg1);                                                       // delete tag from poll list

        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool ctListRead(IntPtr arg1, ref CTOVERLAPPED ctv);                                          // read poll list
        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool ctListWrite(IntPtr arg1, string arg2, ref CTOVERLAPPED ctv);                                 // write poll list item

        //[DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        //public static extern bool    ctListData(IntPtr, void*, uint, uint);                                        // get list data

        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern IntPtr ctListEvent(IntPtr arg1, uint arg2);                                                   // get list event

        //[DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        //public static extern bool    ctListItem(IntPtr, uint, void*, uint, uint);                                 // get tag element extended data

        [DllImport("ctapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool ctTagGetProperty(IntPtr hCTAPI, string szTagName, string szProperty, ref IntPtr pData, uint dwBufferLength, uint dwType);			// get a tag property

    }

}
