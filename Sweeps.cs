using System;


namespace SE_909_Sounds
{
    partial class Program
    {
        static void CreateSweepDownSamples(string name, int firstNote, int lastNote, int noteSpread, double len)
        {
            prevNote = firstNote * NoteScale - 1;

            for (int note = firstNote * NoteScale; note <= lastNote * NoteScale; note++)
            {
                var wav = new WaveFile(
                    SampleRate, 
                    CreateSweepDownSample(name, note, noteSpread, len));

                SaveSample(wav, name, note);
            }
        }


        static double[] CreateSweepDownSample(string name, double note, int noteSpread, double len)
        {
            Console.Write(name + " " + note.ToString("0") + " ... ");

            var fStart = note2freq(note + noteSpread*NoteScale/2);
            var fEnd   = note2freq(note - noteSpread*NoteScale/2);

            var smpLen = (int)(len * 440/fStart * SampleRate);
            var sample = new double[smpLen];


            var f  =  fStart;
            var df = (fEnd-fStart)/smpLen/2;

            for (int i = 0; i < smpLen; i++)
            { 
                double L = SampleRate/f;
                sample[i] += Math.Sin(i/L * Tau) * Volume * (1-i/(double)smpLen);
                f += df;
            }
    
            Console.WriteLine("done");

            return sample;
        }


        static void CreateSweepUpSamples(string name, int firstNote, int lastNote, int noteSpread, double len)
        {
            prevNote = firstNote * NoteScale - 1;

            for (int note = firstNote * NoteScale; note <= lastNote * NoteScale; note++)
            {
                var wav = new WaveFile(
                    SampleRate, 
                    CreateSweepUpSample(name, note, noteSpread, len));

                SaveSample(wav, name, note);
            }
        }


        static double[] CreateSweepUpSample(string name, double note, int noteSpread, double len)
        {
            Console.Write(name + " " + note.ToString("0") + " ... ");

            var fStart = note2freq(note - noteSpread*NoteScale/2);
            var fEnd   = note2freq(note + noteSpread*NoteScale/2);

            var smpLen = (int)(len * 440/fStart * SampleRate);
            var sample = new double[smpLen];

            for (int i = 0; i < smpLen; i++)
            { 
                var f = fStart + (fEnd-fStart)*Math.Pow(i/(double)smpLen/2, 2);
                double L = SampleRate/f;
                sample[i] += Math.Sin(i/L * Tau) * Volume * (1-i/(double)smpLen);
            }
    
            Console.WriteLine("done");

            return sample;
        }
    }
}
