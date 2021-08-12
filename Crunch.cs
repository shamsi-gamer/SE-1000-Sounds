using System;


namespace SE_1000_Sounds
{
    partial class Program
    {
        static void CreatePulseSamples(string name, int firstNote, int lastNote, double seconds)
        {
            prevNote = firstNote * NoteScale - 1;

            var rndIndex = 0;

            for (int note = firstNote * NoteScale; note <= lastNote * NoteScale; note++)
            {
                var wav = new WaveFile(SampleRate, seconds);
                CreatePulseSample(note, ref rndIndex, wav);
                SaveSample(wav, name, note);
            }
        }


        static void CreatePulseSample(double note, ref int rndIndex, WaveFile wav)
        {
            double freqMult      = 16;
            double clickFreqMult =  2;

            double freq = note2freq(note) / freqMult;
            double L    = SampleRate/freq * 2;

            double clkFreq = freq * 64;
            double clkL    = SampleRate/clkFreq * 2;

            Console.Write("pulse " + note.ToString("0") + " ... ");

            var  f = 0.0;
            var df = 1.0/wav.Sample.Length;

            for (int i = 0, j = 0; i < wav.Sample.Length; i++, j++, f += df)
            {
                var c  = j % clkL;
                var ck = Math.Max(16, clkL);

                var s = Math.Sin(c / clkL * Tau / clickFreqMult);
                wav.Sample[i] = j < ck ? s * Volume : 0;

                if (j > i % L) 
                    j = 0;
            }

            Console.WriteLine("done");
        }
    }
}
