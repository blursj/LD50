using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACUServer
{
    class WaveControl
    {
        #region 基础值
        private byte[] Riff = new byte[] { 0x52, 0x49, 0x46, 0x46 };//riff,或者char[]={'R','I','F','F'}为了方便使用byte[]
        private byte[] wave = new byte[] { 0x57, 0x41, 0x56, 0x45 };//wave
        private byte[] Fmt = new byte[] { 0x66, 0x6d, 0x74, 0x20 };//fmf
        private byte[] pcm = new byte[] { 0x01, 0x00 };//默认pcm无压缩
        private byte[] data = new byte[] { 0x64, 0x61, 0x74, 0x61 };//"data"标志
        #endregion

        #region 默认值
        private int subChunckSize = 18;//每次采样得到的样本位数，默认16位代表一个采集数据
        private Int16 numberChanel = 1;//默认单通道        
        private int bitsPerSample = 16;//样本数据位数，默认16，一个量化样本占2byte。
        #endregion

        #region 设定值，初始化值
        private int catchRate = 0;//采集频率,直接使用bitconvert.getbytes()
        private int byteRate = 0;//=采样频率*音频通道数*每次采样得到的样本位数/8
        private Int16 blockAlign = 2;//默认为2=====通道数*每次采样得到的样本位数/8，0002H，也就是2=1*16/8
        #endregion

        #region 随文件改变的值
        private int audioSize = 0;//Wav文件实际音频数据所占的大小
        private int chunkSize = 38;//从下一个地址开始到文件尾的总字节数。高位字节在后面,实际=文件总大小-8
        #endregion

        private string _waveFilePath = "";
        private System.IO.FileStream fstream = null;

        private long _FileLength = 0L;//文件大小

        /// <summary>
        /// wave文件的路径，以及采集频率，如果文件存在，则采集频率设置将失效
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="rate"></param>
        public WaveControl(string filePath, Int32 rate = 100)
        {
            _waveFilePath = filePath;
            if (!System.IO.File.Exists(_waveFilePath))
            {
                catchRate = rate;
                fstream = System.IO.File.Create(_waveFilePath);
                fstream.Write(Riff, 0, 4);
                fstream.Write(BitConverter.GetBytes(chunkSize), 0, 4);
                fstream.Write(wave, 0, 4);
                fstream.Write(Fmt, 0, 4);
                fstream.Write(BitConverter.GetBytes(subChunckSize), 0, 4);
                fstream.Write(pcm, 0, 2);
                fstream.Write(BitConverter.GetBytes(numberChanel), 0, 2);
                var bytes = BitConverter.GetBytes(catchRate);
                fstream.Write(bytes, 0, bytes.Length);

                byteRate = 200;//catchRate * numberChanel * subChunckSize / 8;
                fstream.Write(BitConverter.GetBytes(byteRate), 0, 4);

                blockAlign = (Int16)(numberChanel * subChunckSize / 8);
                fstream.Write(BitConverter.GetBytes(blockAlign), 0, 2);
                fstream.Write(BitConverter.GetBytes(bitsPerSample), 0, 4);

                fstream.Write(data, 0, 4);
                fstream.Write(BitConverter.GetBytes(audioSize), 0, 4);//44字节
                //fstream.WriteByte(0);
                //fstream.WriteByte(0);//46字节
            }
            else
            {
                fstream = new System.IO.FileStream(_waveFilePath, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite);
                byte[] bytes = new byte[4];
                fstream.Seek(25, System.IO.SeekOrigin.Begin);
                fstream.Read(bytes, 0, 4);
                catchRate = BitConverter.ToInt32(bytes, 0);
                System.IO.FileInfo info = new System.IO.FileInfo(_waveFilePath);
                fstream.Seek(info.Length, System.IO.SeekOrigin.Begin);//重置流位置
            }
            _FileLength = fstream.Length;
        }

        public void Stop()
        {
            if (fstream != null)
            {
                fstream.Close();
            }
        }

        #region 写文件
        /// <summary>
        /// 文件流设置
        /// </summary>
        /// <param name="datas"></param>
        public void Write(Int16[] datas)
        {
            fstream.Seek(_FileLength, System.IO.SeekOrigin.Begin);

            if (datas.Length == 0)
            {
                return;
            }

            byte[] bytes = new byte[datas.Length * 2];

            for (int i = 0; i < datas.Length; i++)
            {
                var res = BitConverter.GetBytes(datas[i]);
                bytes[i] = res[0];
                bytes[i + 1] = res[1];
                i++;
            }
            fstream.Write(bytes, 0, bytes.Length);

            _FileLength += bytes.Length;

            #region 改变riff的size值
            chunkSize = chunkSize + datas.Length * 2;
            SetChunkSize(chunkSize);
            #endregion

            #region
            audioSize = audioSize + datas.Length * 2;
            SetAudioSize(audioSize);
            #endregion
        }
        #endregion

        private void SetChunkSize(int size)
        {
            fstream.Seek(4, System.IO.SeekOrigin.Begin);
            fstream.Write(BitConverter.GetBytes(size), 0, 4);
        }

        private void SetAudioSize(int size)
        {
            fstream.Seek(42, System.IO.SeekOrigin.Begin);
            fstream.Write(BitConverter.GetBytes(size), 0, 4);
        }
    }
}
