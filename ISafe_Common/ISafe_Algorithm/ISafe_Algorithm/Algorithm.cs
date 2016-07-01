using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ISafe_Algorithm
{
    public class Algorithm
    {
        [DllImport("LeakProcess.DLL", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int InitAcousticProcessor(
        int processorNum,		//Number of acoustic processors
        int MEDIUM,				//MEDIUM
        int samplePerBlock,		//sample per block
        int dataBufferSize,		//input data buffer size
        int downFactor			//降采样系数
         );


        //Release Wave Processor
        [DllImport("LeakProcess.DLL", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReleaseAcousticProcessor();

        //Process one block of wave signal
        //Return value: 
        [DllImport("LeakProcess.DLL", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ProcessAcousticBlock(
                int processorIndex,		//Index of acoustic processor
                ref float pch_A0,
                ref float pch_A1,
                ref float pch_A2,
                ref float pch_B0,
                ref float pch_B1,
                ref float pch_B2,
                int sampleCount
                );

        //Detect leak for one block of wave signal
        [DllImport("LeakProcess.DLL", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int DetectAcousticBlock(
                int processorIndex,			 //Index of acoustic processor
                ref int pLeakStatus,		 //LEAK: ==1, leak detected; ==0, no leak.
                ref int pLeakTimeMs,		 //TMS
                ref int pLeakTimeS,			 //TS
                ref float pLeakDetectValue,	 //Leak detection value
                ref float pDynamicThreshold,	//dynamic threshold for acoustic processing
            	int blockTimeMs,			//Block TMS
		        int blockTimeS				//Block TS
                );

        //Update threshold settings
        [DllImport("LeakProcess.DLL", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int UpdateAcousticThresh(
                int processorIndex,			//Index of acoustic processor
                float threshMax,
                float threshMin
                );

        [DllImport("LeakProcess.DLL", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int UpdateAcousticDelay(
		        int processorIndex,			//Index of acoustic processor
		        int sensorDelay		    
		);

        //更新数据采集处理增益
        [DllImport("LeakProcess.DLL", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int UpdateAcousticGain(
		int processorIndex,			//Index of acoustic processor
		float gain1,			//第1级增益
		float gain2				//第2级增益
		);

        //Get one block of down-sampled signal   
        [DllImport("LeakProcess.DLL", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetDownSignalA(
               int processorIndex,			//Index of acoustic processor
               ref float pch_A,             //输入数组首地址
               int arraySize,             //输入数组长度
               ref int outCount           //实际输出数组长度
                );

        //Get one block of down-sampled signal   
        [DllImport("LeakProcess.DLL", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetDownSignalB(
               int processorIndex,			//Index of acoustic processor
               ref float pch_B,             //输出数组首地址
               int arraySize,             //输出数组长度
               ref int outCount                 //0-降采样后的原始数据 1-滤波 2-一级 3-二级
                );

        //Get one block of array processing signal 函数原型返回类型为float *
        [DllImport("LeakProcess.DLL", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetArraySignal(
                int processorIndex,			//Index of acoustic processor
                ref float pArray,               //数组输出 首地址
                int arraySize,              //输出数组长度
		        ref int outCount            //实际输出长度
                );

        //Get Mask signal                          函数原型返回类型为float *
        [DllImport("LeakProcess.DLL", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetMaskSignal(
               int processorIndex,			//Index of acoustic processor
               ref float pRlt,                 //数组输出 首地址
               int arraySize,              //输出数组长度
		       ref int outCount
               
                );

        //-----------------------------------------------------------------------------------------------
        //-------------Time processing API-----------------------------------------------------------
        //-----------------------------------------------------------------------------------------------

        //Init time processor
        [DllImport("LeakProcess.DLL", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int InitTimeProcessor(
                int sampleRate,			//sampling rate of PPS singal, Hz 
                int blockSize,			//Size of  time signal block
                int maxTimeStrDelay,	//maximum time string delay, in ms 300~400ms
                short threshPPS			//PPS signal threshold  5000
                );

        //Release  timer
        [DllImport("LeakProcess.DLL", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReleaseTimeProcessor();

        //Process one block of  timer samples
        [DllImport("LeakProcess.DLL", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ProcessTimeBlock(
                 ref float pTimeBuf,	//PPS data buffer 
                 int lenBuf		//buffer length
                );

        //Process  time string
        [DllImport("LeakProcess.DLL", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ProcessTimeString(
                 ref byte pTimeStr	//Timer string
                );

        //Get  time of latest block
        [DllImport("LeakProcess.DLL", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetBlockTime(
                ref int s,		//s: seconds since UTC time base: 1970/1/1, 0 hour, 0 min, 0 second
                ref int ms		//ms: ms
                );

        //Reset the  time
        [DllImport("LeakProcess.DLL", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ResetTimeProcessor();

        //-----------------------------------------------------------------------------------------------
        //-------------Pressure processing API-----------------------------------------------------------
        //-----------------------------------------------------------------------------------------------

        //Init Pressure processing engine for leak detection
        [DllImport("LeakProcess.DLL", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int InitPressureProcessor(
                int processorNum,		//presuuresensor index 编号从10开始 不能出现重复编号
                int MEDIUM,		        //MEDIUM
                int sampleRate,         //采样频率 10Hz 单位为Hz
                int samplePerBlock,		//sample per block  默认传输120 可在UI界面配置
                int dataBufferSize,     //K data Buffer size  默认传输120 可在UI界面配置
                float threshMax,
                float threshMin
            );

        //Release Pressure Processor
        [DllImport("LeakProcess.DLL", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReleasePressureProcessor();

        //Process one block of Pressure signal
        [DllImport("LeakProcess.DLL", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ProcessPressure(
                int processorIndex,		//Index of Pressure processor
                ref float pressureData,	//one point of Pressure data
                float s,               //数据首位时间秒部分
                float ms,              //数据首位时间毫秒部分
                int length,            
		        ref float pDetValue,   //Detection value MASK波形输出
                ref float pDetThresh   //检测门限值输出 作为门限值调整依据
                );

        //Detect leak for one block of pressure signal
        [DllImport("LeakProcess.DLL", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetPressureDetectRlt(
               int processorIndex,			//Index of Pressure processor
               ref int pLeakStatus,			//LEAK: ==1, leak detected; ==0, no leak.
               ref int pLeakTimeS,          //TMSec
               ref int pLeakTimeMs,			//TMS
               ref float pLeakDetectValue,		//Leak detection value  泄漏发生时的MASK值 
               ref float  pLeakDetectThresh     //泄漏发生时刻的门限值
                );

        //Update threshold settings
        [DllImport("LeakProcess.DLL", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int UpdatePressureThresh(
                int processorIndex,			//Index of acoustic processor
                float threshMax,                                                                                                                                                     
                float threshMin
                );

        //从注册表中读取注册信息
        [DllImport("LeakProcess.DLL", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadRegInf(string company,string machineCode,string regCode);

    }
}
