using System;


namespace SE_909_Sounds
{
    partial class Program
    {
        static double prevNote;


        static void CreateLowNoiseSamples(string name, int firstNote, int lastNote)
        {
            ClearBuffer(altBuffer);

            prevNote = firstNote*NoteScale - 1;

            var rndIndex = 0;

            for (int note = firstNote * NoteScale; note <= lastNote * NoteScale; note++)
            {
                CreateLowNoiseSample(note, ref rndIndex);
                SaveSample(name, note);

                var temp   = wav.Sample;
                wav.Sample = altBuffer;
                altBuffer  = temp;
            }
        }


        static void CreateLowNoiseSample(double note, ref int rndIndex)
        {
            wav.Clear();

            var step = 0.1;

            for (double n = prevNote; n < note; n++)
            {
                for (double f = step; f <= 1.0; f += step)
                {
                    Console.Write("low noise " + note.ToString("0") + " (" + f.ToString("0.000") + ") ... ");

                    double freq = note2freq(n + f);
                    double L    = sampleRate / freq * 2;

                    for (int i = 0; i < wav.Sample.Length; i++)
                        wav.Sample[i] += Math.Cos(((i % L) / L + rnd[rndIndex]) * Tau);

                    rndIndex++;
    
                    Console.WriteLine("done");
                }
            }

            for (int i = 0; i < wav.Sample.Length; i++)
            {
                wav.Sample[i] *= step / 22;
                wav.Sample[i] += altBuffer[i];
            }

            prevNote = note;
        }


        static void CreateHighNoiseSamples(string name, int firstNote, int lastNote)
        {
            ClearBuffer(altBuffer);

            prevNote = lastNote*NoteScale;

            var rndIndex = 0;

            for (int note = lastNote * NoteScale; note >= firstNote * NoteScale; note--)
            {
                CreateHighNoiseSample(note, ref rndIndex);
                SaveSample(name, note);

                var temp   = wav.Sample;
                wav.Sample = altBuffer;
                altBuffer  = temp;
            }
        }


        static void CreateHighNoiseSample(double note, ref int rndIndex)
        {
            wav.Clear();

            var step = 0.1;

            for (double n = prevNote; n > note; n--)
            {
                for (double f = 1.0; f > 0; f -= step)
                {
                    Console.Write("high noise " + note.ToString("0") + " (" + f.ToString("0.000") + ") ... ");

                    double freq = note2freq(n + f);
                    double L    = sampleRate / freq * 2;

                    for (int i = 0; i < wav.Sample.Length; i++)
                        wav.Sample[i] += Math.Cos(((i % L) / L + rnd[rndIndex]) * Tau);

                    rndIndex++;
    
                    Console.WriteLine("done");
                }
            }

            for (int i = 0; i < wav.Sample.Length; i++)
            {
                wav.Sample[i] *= step / 22;
                wav.Sample[i] += altBuffer[i];
            }

            prevNote = note;
        }


        static void CreateBandNoiseSamples(string name, int firstNote, int lastNote)
        {
            prevNote = firstNote * NoteScale - 1;

            var rndIndex = 0;

            for (int note = firstNote * NoteScale; note <= lastNote * NoteScale; note++)
            {
                CreateBandNoiseSample(note, ref rndIndex);
                SaveSample(name, note);
            }
        }


        static void CreateBandNoiseSample(double note, ref int rndIndex)
        {
            wav.Clear();

            var step   = 0.01;
            var spread = 4;

            for (double n = prevNote - spread+1; n < note + spread+1; n++)
            {
                for (double f = step; f <= 1.0; f += step)
                {
                    Console.Write("band noise " + note.ToString("0") + " (" + n.ToString("0") + ", " + f.ToString("0.000") + ") ... ");

                    double freq = note2freq(n + f);
                    double L    = sampleRate / freq * 2;

                    for (int i = 0; i < wav.Sample.Length; i++)
                        wav.Sample[i] += Math.Cos(((i % L) / L + rnd[rndIndex]) * Tau);

                    rndIndex++;
    
                    Console.WriteLine("done");
                }

            }

            for (int i = 0; i < wav.Sample.Length; i++)
                wav.Sample[i] *= step;

            prevNote = note;
        }
    }
}
