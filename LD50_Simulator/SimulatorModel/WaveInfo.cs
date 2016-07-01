using System.IO;
namespace SimulatorModel
{
    public class Wav
    {
        public byte[] bInfo;
        public WavInfo GetWavInfo(FileStream fs)
        {
            WavInfo wavInfo = new WavInfo();
            if (fs.Length >= 44)
            {
                bInfo = new byte[100];
                fs.Read(bInfo, 0, 100);
                int Length = GetHeadLen(bInfo);
                System.Text.Encoding.Default.GetString(bInfo, 0, 4);
                wavInfo.groupid = System.Text.Encoding.Default.GetString(bInfo, 0, 4);
                wavInfo.filesize = System.BitConverter.ToInt32(bInfo, 4);
                wavInfo.rifftype = System.Text.Encoding.Default.GetString(bInfo, 8, 4);
                wavInfo.chunkid = System.Text.Encoding.Default.GetString(bInfo, 12, 4);
                wavInfo.chunksize = System.BitConverter.ToInt32(bInfo, 16);
                wavInfo.wformattag = System.BitConverter.ToInt16(bInfo, 20);
                wavInfo.wchannels = System.BitConverter.ToUInt16(bInfo, 22);
                wavInfo.dwsamplespersec = System.BitConverter.ToUInt32(bInfo, 24);
                wavInfo.dwavgbytespersec = System.BitConverter.ToUInt32(bInfo, 28);
                wavInfo.wblockalign = System.BitConverter.ToUInt16(bInfo, 32);
                wavInfo.wbitspersample = System.BitConverter.ToUInt16(bInfo, 34);
                wavInfo.datachunkid = "data";// System.Text.Encoding.Default.GetString(bInfo, 36, 4);
                wavInfo.datasize = GetWavLen(bInfo);// System.BitConverter.ToInt32(bInfo, 40);
                wavInfo.HeadSize = GetHeadLen(bInfo);
            }
            return wavInfo;
        }
        public int GetWavLen(byte[] fs)
        {
            //"data"后面的4个字节就是声音数据长度了 
            byte[] bLen = new byte[4];
            int ilen = GetDataLen(fs);
            bLen[0] = fs[ilen + 0];
            bLen[1] = fs[ilen + 1];
            bLen[2] = fs[ilen + 2];
            bLen[3] = fs[ilen + 3];
            //fs.Read(bInfo,GetDataLen(fs), 4);
            return System.BitConverter.ToInt32(bLen, 0);
        }

        public int GetHeadLen(byte[] fs)
        {
            //头文件长度
            return GetDataLen(fs) + 4;
        }

        /// <summary>
        /// data标识出现的位置  从32位开始向后找 返回位置
        /// </summary>
        /// <param name="fs"></param>
        /// <returns></returns>
        private int GetDataLen(byte[] fs)
        {
            if (fs.Length >= 90)
            {
                for (int i = 32; i <= 100; i++)
                {
                    //bInfo = new byte[1];
                    //fs.Read(bInfo, 0, 1);
                    byte[] tmpfs = new byte[1];
                    tmpfs[0] = fs[i - 4];
                    string bit = System.Text.Encoding.ASCII.GetString(tmpfs);//bInfo);
                    if (bit == "d")
                    {
                        //bInfo = new byte[1];
                        //fs.Read(bInfo, 0, 1);
                        tmpfs = new byte[1];
                        tmpfs[0] = fs[i - 3];
                        bit = System.Text.Encoding.ASCII.GetString(tmpfs);//bInfo);
                        if (bit == "a")
                        {
                            //bInfo = new byte[1];
                            //fs.Read(bInfo, 0, 1);
                            tmpfs = new byte[1];
                            tmpfs[0] = fs[i - 2];
                            bit = System.Text.Encoding.ASCII.GetString(tmpfs);//bInfo);
                            if (bit == "t")
                            {
                                //bInfo = new byte[1];
                                //fs.Read(bInfo, 0, 1);
                                tmpfs = new byte[1];
                                tmpfs[0] = fs[i - 1];
                                bit = System.Text.Encoding.ASCII.GetString(tmpfs);//bInfo);
                                if (bit == "a")
                                {
                                    return i;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                return 0;
            }
            else
            {
                return 0;
            }
        }
    }

    //wav 头文件结构
    public struct WavInfo
    {
        public string groupid;
        public string rifftype;
        public long filesize;
        public string chunkid;
        public long chunksize;
        public short wformattag; //记录着此声音的格式代号，例如WAVE_FORMAT_PCM，WAVE_F0RAM_ADPCM等等。 
        public ushort wchannels; //记录声音的频道数。 
        public ulong dwsamplespersec;//记录每秒取样数。 
        public ulong dwavgbytespersec;//记录每秒的数据量。 
        public ushort wblockalign;//记录区块的对齐单位。 
        public ushort wbitspersample;//记录每个取样所需的位元数。 
        public string datachunkid;
        public long datasize;
        public long HeadSize; //文件头长度
    }

}