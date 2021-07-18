using System;


namespace SE_1000_Sounds
{
    partial class Program
    {
        static void CreateToneSamples(string name, Func<double, double> generate, int firstNote, int lastNote, double seconds)
        {
            for (int note = firstNote * NoteScale; note <= lastNote * NoteScale; note++)
            {
                var wav = new WaveFile(SampleRate, seconds);
                CreateToneSample(name, generate, note, wav);
                SaveSample(wav, name, note);
            }
        }


        static void CreateToneSample(string name, Func<double, double> generate, double note, WaveFile wav)
        {
            double freq = note2freq(note);
            double L    = SampleRate/freq * 2;

            Console.Write(name + " note " + note.ToString("0") + " ... ");

            for (int i = 0; i < wav.Sample.Length; i++)
                wav.Sample[i] = generate((i % L) / L) * Volume;

            Console.WriteLine("done");
        }


        static Func<double, double> sine   = (double t) => Math.Sin(t * Tau);
        static Func<double, double> square = (double t) => t < 0.5 ? 1 : -1;
        static Func<double, double> tri    = (double t) => 
        { 
                 if (t <  0.25)             return t / 0.25;
            else if (t >= 0.25 && t < 0.75) return 1 - 4 * (t - 0.25);
            else                            return (t - 0.75) / 0.25 - 1; 
        };
        static Func<double, double> saw    = (double t) => t*2 - 1;
    }
}
