﻿using System;


namespace SE_1000_Sounds
{
    partial class Program
    {
        static double   prevNote;
        static double[] altBuffer;



        static void CreateLowNoiseSamples(string name, int firstNote, int lastNote, double seconds)
        {
            altBuffer = new double[(int)Math.Round(seconds * SampleRate)];

            prevNote = firstNote*NoteScale - 1;

            var rndIndex = 0;

            for (int note = firstNote * NoteScale; note <= lastNote * NoteScale; note++)
            {
                var wav = new WaveFile(SampleRate, seconds);
                CreateLowNoiseSample(note, ref rndIndex, wav);
                SaveSample(wav, name, note);

                var temp   = wav.Sample;
                wav.Sample = altBuffer;
                altBuffer  = temp;
            }
        }



        static void CreateLowNoiseSample(double note, ref int rndIndex, WaveFile wav)
        {
            var step = 0.1;

            for (double n = prevNote; n < note; n++)
            {
                for (double f = step; f <= 1.0; f += step)
                {
                    Console.Write("low noise " + note.ToString("0") + " (" + f.ToString("0.000") + ") ... ");

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
                wav.Sample[i] *= step / 22;
                wav.Sample[i] += altBuffer[i];
            }

            prevNote = note;
        }



        static void CreateHighNoiseSamples(string name, int firstNote, int lastNote, double seconds)
        {
            altBuffer = new double[(int)Math.Round(seconds * SampleRate)];

            prevNote = lastNote*NoteScale;

            var rndIndex = 0;

            for (int note = lastNote * NoteScale; note >= firstNote * NoteScale; note--)
            {
                var wav = new WaveFile(SampleRate, seconds);
                CreateHighNoiseSample(note, ref rndIndex, wav);
                SaveSample(wav, name, note);

                var temp   = wav.Sample;
                wav.Sample = altBuffer;
                altBuffer  = temp;
            }
        }



        static void CreateHighNoiseSample(double note, ref int rndIndex, WaveFile wav)
        {
            var step = 0.1;

            for (double n = prevNote; n > note; n--)
            {
                for (double f = 1.0; f > 0; f -= step)
                {
                    Console.Write("high noise " + note.ToString("0") + " (" + f.ToString("0.000") + ") ... ");

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
                wav.Sample[i] *= step / 22;
                wav.Sample[i] += altBuffer[i];
            }

            prevNote = note;
        }



        static void CreateBandNoiseSamples(string name, int spread, int firstNote, int lastNote, double seconds)
        {
            prevNote = firstNote * NoteScale - 1;

            var rndIndex = 0;

            for (int note = firstNote * NoteScale; note <= lastNote * NoteScale; note++)
            {
                var wav = new WaveFile(SampleRate, seconds);
                CreateBandNoiseSample(name, note, spread, ref rndIndex, wav);
                SaveSample(wav, name, note);
            }
        }



        static void CreateBandNoiseSample(string name, double note, int spread, ref int rndIndex, WaveFile wav)
        {
            var step   = 0.01;
            var offset = 0.5f;

            for (double n = prevNote - spread+offset; n < note + spread+offset; n++)
            {
                for (double f = step; f <= 1.0; f += step)
                {
                    Console.Write(name + " " + note.ToString("0") + " (" + n.ToString("0") + ", " + f.ToString("0.000") + ") ... ");

                    double freq = note2freq(n + f);
                    double L    = SampleRate / freq * 2;

                    for (int i = 0; i < wav.Sample.Length; i++)
                        wav.Sample[i] += Math.Cos(((i % L) / L + rnd[rndIndex]) * Tau);

                    rndIndex++;
    
                    Console.WriteLine("done");
                }

            }

            for (int i = 0; i < wav.Sample.Length; i++)
                wav.Sample[i] *= 0.01;

            prevNote = note;
        }
    }
}
