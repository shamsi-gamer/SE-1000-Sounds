using System;


namespace SE_909_Sounds
{
    partial class Program
    {
        static Func<double, double, int, int, double> sine   = (double t, double note, int i, int L) => Math.Sin(t * Tau);
        static Func<double, double, int, int, double> square = (double t, double note, int i, int L) => t < 0.5 ? 1 : -1;
        static Func<double, double, int, int, double> tri    = (double t, double note, int i, int L) => 
        { 
                 if (t <  0.25)             return t / 0.25;
            else if (t >= 0.25 && t < 0.75) return 1 - 4 * (t - 0.25);
            else                            return (t - 0.75) / 0.25 - 1; 
        };
        static Func<double, double, int, int, double> saw   = (double t, double note, int i, int L) => t*2 - 1;


        static void CreateToneSamples(string name, Func<double, double, int, int, double> generate, int firstNote, int lastNote)
        {
            // *2 for quater-tones
            for (int note = firstNote * 2; note <= lastNote * 2; note++)
            {
                CreateToneSample(name, generate, note);
                SaveSample(name, note);
            }
        }


        static void CreateToneSample(string name, Func<double, double, int, int, double> generate, double note)
        {
            double freq = note2freq(note);//440 * Math.Pow(2, (note/2 - 69) / 12.0);
            double L    = sampleRate/freq * 2;

            Console.Write(name + " note " + note.ToString("0") + " ... ");

            for (int i = 0; i < wav.Sample.Length; i++)
                wav.Sample[i] = generate((i % L) / L, note, i, (int)L) * volume;

            Console.WriteLine("done");
        }


        static double note2freq(double note)
        { 
            return 440 * Math.Pow(2, (note/2 - 69) / 12.0);
        }
    }
}
