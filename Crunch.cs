using System;


namespace SE_909_Sounds
{
    partial class Program
    {
        static void CreateCrunchSamples(string name, int firstNote, int lastNote, double seconds)
        {
            prevNote = firstNote * NoteScale - 1;

            var rndIndex = 0;

            for (int note = firstNote * NoteScale; note <= lastNote * NoteScale; note++)
            {
                var wav = new WaveFile(SampleRate, seconds);
                CreateCrunchSample(note, ref rndIndex, wav);
                SaveSample(wav, name, note);
            }
        }


        static void CreateCrunchSample(double note, ref int rndIndex, WaveFile wav)
        {
            var step   = 0.02;
            var spread = 8;

            for (double n = prevNote - spread+1; n < note + spread+1; n++)
            {
                for (double f = step; f <= 1.0; f += step)
                {
                    Console.Write("crunch " + note.ToString("0") + " (" + n.ToString("0") + ", " + f.ToString("0.000") + ") ... ");

                    double freq = note2freq(n + f);
                    double L    = SampleRate / freq * 2;

                    for (int i = 0; i < wav.Sample.Length; i++)
                        wav.Sample[i] += Math.Cos(((i % L) / L + rnd[rndIndex]) * Tau);

                    rndIndex++;
    
                    Console.WriteLine("done");
                }

            }

            for (int i = 0; i < wav.Sample.Length; i++)
            {
                var pos  = i/(double)wav.Sample.Length * SampleRate;
                var rate = SampleRate / 30;
                var f    = (pos%rate) / (rate);

                wav.Sample[i] *= step * Math.Min(Math.Pow(1-f+0.05, 30), 1) * Volume;
            }

            prevNote = note;
        }
    }
}
