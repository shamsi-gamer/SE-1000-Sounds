using System;
using System.IO;


namespace SE_909_Sounds
{
    partial class Program
    {
        const double    Tau = Math.PI*2;
                        
                        
        static uint     sampleRate = 44100;
        static double   seconds    = 10;

        static double   volume     = 0.5;


        static double[] rnd        = new double[(int)Math.Round(seconds * sampleRate * 2)];
        static double[] click      = new double[(int)Math.Round(seconds * sampleRate * 2)];

        static double[] altBuffer  = new double[(int)Math.Round(seconds * sampleRate)];

        static Random   random     = new Random(DateTime.Now.Millisecond);


        static WaveFile wav        = new WaveFile(sampleRate, seconds);


        static void Main(string[] args)
        {
            InitBuffers();

            CreateToneSamples("Sine",     sine,     12, 150);
            CreateToneSamples("Square",   square,   12, 150);
            CreateToneSamples("Triangle", tri,      12, 150);
            CreateToneSamples("Saw",      saw,      12, 150);

            CreateLowNoiseSamples ("LowNoise",  12, 150);
            CreateHighNoiseSamples("HighNoise", 12, 150);
            //CreateBandNoiseSamples("BandNoise", 12, 150);
        }


        //double sign(double d) { return d / Math.abs(d); }

        static void InitBuffers()
        {
            double freq = 1500;
            int    L    = (int)Math.Round(seconds * sampleRate * 2);
            double len  = 0.005;
            int    Len  = (int)(len * L);
  
            for (int i = 0; i < L; i++)
            {
                rnd[i] = -1 + random.NextDouble()*2;

                double f = Math.Min(Math.Max(0, 1 - (double)i/Len), 1);
                click[i] = sine(f * freq / i, 0, 0, 0) * Math.Pow(f, 16);
            }
        }


        static public void ClearBuffer(double[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = 0;
        }


        static void SaveSample(string name, int note)
        {
            string userHome = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            string filePath = Path.GetFullPath(Path.Combine(userHome, "Space Engineers\\Samples"));

            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            filePath += "\\SE-909_" + name + "_" + note + ".wav";

            wav.Save(filePath);
        }
    }
}
