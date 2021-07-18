using System;
using System.IO;


namespace SE_1000_Sounds
{
    partial class Program
    {
        const double    Tau = Math.PI*2;
                        
        const int       NoteScale  = 2;
                        
        static uint     SampleRate = 44100;
        static double   MaxLength  = 10;

        static double   Volume     = 0.5;


        static Random   random = new Random(DateTime.Now.Millisecond);
        static double[] rnd    = new double[(int)Math.Round(MaxLength * SampleRate * 2)];


        static void Main(string[] args)
        {
            InitBuffers();

            //CreateToneSamples("Sine",     sine,     12, 150, 5);
            //CreateToneSamples("Square",   square,   12, 150, 5);
            //CreateToneSamples("Triangle", tri,      12, 150, 5);
            //CreateToneSamples("Saw",      saw,      12, 150, 5);

            //CreateLowNoiseSamples ("LowNoise",  12, 150, 5);
            //CreateHighNoiseSamples("HighNoise", 12, 150, 5);
            CreateBandNoiseSamples("BandNoise", 12, 150, 5);

            //CreateSweepDownSamples("SlowSweepDown", 12, 150, 48, 1);
            //CreateSweepUpSamples  ("SlowSweepUp",   12, 150, 48, 0.1);

            //CreateSweepDownSamples("FastSweepDown", 12, 150, 48, 0.1);
            //CreateSweepUpSamples  ("FastSweepUp",   12, 150, 48, 0.01);

            //CreateCrunchSamples   ("Crunch",    12, 150, 1);
        }


        static void InitBuffers()
        {
            int L = (int)Math.Round(MaxLength * SampleRate * 2);
  
            for (int i = 0; i < L; i++)
                rnd[i] = -1 + random.NextDouble()*2;
        }


        static public void ClearBuffer(double[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = 0;
        }


        static void SaveSample(WaveFile wav, string name, int note)
        {
            string userHome = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            string filePath = Path.GetFullPath(Path.Combine(userHome, "Space Engineers\\Samples"));

            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            filePath += "\\SE-909_" + name + "_" + note + ".wav";

            wav.Save(filePath);
        }


        static double note2freq(double note)
        { 
            return 440 * Math.Pow(2, (note/2 - 69) / 12.0);
        }
    }
}
