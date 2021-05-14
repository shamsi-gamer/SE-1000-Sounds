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
            double freq = note2freq(note);
            double L    = SampleRate/freq * 2;

            double clkFreq = freq * 64;
            double clkL    = SampleRate/clkFreq * 2;

            Console.Write("crunch " + note.ToString("0") + " ... ");

            var  f = 0.0;
            var df = 1.0/wav.Sample.Length;

            for (int i = 0, j = 0; i < wav.Sample.Length; i++, j++, f += df)
            {
                var c  = j % clkL;
                var ck = Math.Max(16, clkL);

                var s = Math.Sin(c / clkL * Tau);
                wav.Sample[i] = j < ck ? s * Volume : 0;

                if (j > i % L) 
                    j = 0;
            }

            Console.WriteLine("done");
        }
    }
}
