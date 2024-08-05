using System;

namespace BeatForgeClient.Audio;

public class SineGenerator : ISampleProvider
{
    public short Read(float freq, int index, int size)
    {
        float samplesPerPeriod = 1 / freq;
        float phase = (2.0f * MathF.PI * index) / samplesPerPeriod;
        float value = MathF.Sin(phase);

        return (short)(32760 * value);
    }
}

public class SquareGenerator : ISampleProvider
{
    public short Read(float freq, int index, int size)
    {
        float samplesPerPeriod = 1 / freq;
        float halfSamplesPerPeriod = samplesPerPeriod / 2;

        return (index % samplesPerPeriod) < halfSamplesPerPeriod ? (short)32760 : (short)-32760;
    }
}

public class TriangleGenerator : ISampleProvider
{
    public short Read(float freq, int index, int size)
    {
        float samplesPerPeriod = 1 / freq; // Number of samples per period (1/frequency)
        float halfSamplesPerPeriod = samplesPerPeriod / 2; // Half of the samples per period

        float positionInPeriod = index % samplesPerPeriod;
        float normalizedPosition = positionInPeriod / samplesPerPeriod;

        float value;
        if (positionInPeriod < halfSamplesPerPeriod)
        {
            value = 2 * normalizedPosition - 1; // Rising edge of the triangle wave
        }
        else
        {
            value = 1 - 2 * (normalizedPosition - 0.5f); // Falling edge of the triangle wave
        }

        return (short)(32760 * value);
    }
}

public class PulseGenerator : ISampleProvider
{
    public short Read(float freq, int index, int size)
    {
        float samplesPerPeriod = 1 / freq; // Number of samples per period (1/frequency)
        float dutySamples = samplesPerPeriod / 8;

        return (index % samplesPerPeriod) < dutySamples ? (short)32760 : (short)-32760;
    }
}

public class SawtoothGenerator : ISampleProvider
{
    public short Read(float freq, int index, int size)
    {
        float samplesPerPeriod = 1 / freq;
        float normalizedPosition = (index % samplesPerPeriod) / samplesPerPeriod;
        float value = 2 * normalizedPosition - 1;

        return (short)(32760 * value);
    }
}

public class SynthKickGenerator : ISampleProvider
{
    public short Read(float freq, int index, int size)
    {
        // int phase = index / (20 + index / 100); // laser
        // int phase = index / (1 + index / 1000); // chirp
        float phase = (index - 50_000f) / index * (500f * freq);
        float @base = (index - 2000) / 150f;
        float mult = 32760 - Math.Min(@base * @base * @base, 32760);
        float value = MathF.Sin(phase) * mult;
        return (short)value;
    }
}

public class SnareGenerator : ISampleProvider
{
    private static readonly Random _random = new();
    private static readonly short[] _sample = new short[8096];

    public SnareGenerator()
    {
        for (var i = 0; i < _sample.Length; i++)
        {
            _sample[i] = (short) _random.Next();
        }
    }

    public short Read(float freq, int index, int size)
    {
        float mult = (size-index) / (float)size;
        float value = _sample[index%8096] * mult;
        return (short)value;
    }
}