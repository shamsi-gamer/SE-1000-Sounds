using System;
using System.IO;


namespace SE_909_Sounds
{
    public class WaveFile
    {
        WaveHeader      m_header;
        WaveFormatChunk m_format;
        WaveDataChunk   m_data;

        public double[] Sample;


        public WaveFile(uint sampleRate, double seconds)
        {
            m_header = new WaveHeader();
            m_format = new WaveFormatChunk(sampleRate);
            m_data   = new WaveDataChunk();

            Sample   = new double[(int)Math.Round(sampleRate * seconds)];
        }


        void PrepareSampleData()
        {
            m_data.shortArray = new short[Sample.Length * 2]; // 2 channels

            for (int i = 0; i < Sample.Length; i++)
            { 
                m_data.shortArray[i*2  ] = 
                m_data.shortArray[i*2+1] = (short)Math.Min(Math.Max(-0x7ff8, Math.Round(0x7ff8 * Sample[i])), 0x7ff8); // 0x7ff8 is the max amplitude for 16-bit autio
            }

            m_data.dwChunkSize = (uint)(m_data.shortArray.Length * (m_format.wBitsPerSample / 8));
        }


        public void Save(string filePath)
        {
            PrepareSampleData();


            var fs = new FileStream(filePath, FileMode.Create);
            var w  = new BinaryWriter(fs);


            w.Write(m_header.sGroupID.ToCharArray());
            w.Write(m_header.dwFileLength);
            w.Write(m_header.sRiffType.ToCharArray());


            w.Write(m_format.sChunkID.ToCharArray());
            w.Write(m_format.dwChunkSize);
            w.Write(m_format.wFormatTag);
            w.Write(m_format.wChannels);
            w.Write(m_format.dwSamplesPerSec);
            w.Write(m_format.dwAvgBytesPerSec);
            w.Write(m_format.wBlockAlign);
            w.Write(m_format.wBitsPerSample);


            w.Write(m_data.sChunkID.ToCharArray());
            w.Write(m_data.dwChunkSize);

            foreach (var val in m_data.shortArray)
                w.Write(val);


            w.Seek(4, SeekOrigin.Begin);
            w.Write((uint)w.BaseStream.Length - 8);


            w.Close();
            fs.Close();
        }


        public void Clear()
        {
            for (int i = 0; i < Sample.Length; i++)
                Sample[i] = 0;
        }
    }


    public class WaveHeader
    {
        public string sGroupID;     // RIFF
        public uint   dwFileLength; // total file length minus 8 (used by RIFF)
        public string sRiffType;    // always WAVE

        public WaveHeader()
        {
            sGroupID     = "RIFF";
            dwFileLength =  0;
            sRiffType    = "WAVE";
        }
    }


    public class WaveFormatChunk
    {
        public string sChunkID;
        public uint   dwChunkSize;      // length of header in bytes
        public ushort wFormatTag;       // 1 (MS PCM)
        public ushort wChannels;        // # of channels
        public uint   dwSamplesPerSec;  // audio frequency in Hz
        public uint   dwAvgBytesPerSec; // for estimating RAM allocation
        public ushort wBlockAlign;      // sample frame size in bytes
        public ushort wBitsPerSample;

        public WaveFormatChunk(uint sampleRate)
        {
            sChunkID         = "fmt ";
            dwChunkSize      = 16;
            wFormatTag       = 1;
            wChannels        = 2;
            dwSamplesPerSec  = sampleRate;
            wBitsPerSample   = 16;
            wBlockAlign      = (ushort)(wChannels * (wBitsPerSample / 8));
            dwAvgBytesPerSec = dwSamplesPerSec * wBlockAlign;
        }
    }


    public class WaveDataChunk
    {
        public string  sChunkID;
        public uint    dwChunkSize; // length of header in bytes
        public short[] shortArray;

        public WaveDataChunk()
        {
            sChunkID    = "data";
            dwChunkSize = 0;
            shortArray  = new short[0];
        }
    }
}
