using System;

namespace BeatForgeClient.Audio;

public class SineGenerator : ISampleProvider
{
    public short Read(float freq, int index)
    {
        float samplesPerPeriod = 1 / freq;
        float phase = (2.0f * MathF.PI * index) / samplesPerPeriod;
        float value = (float)Math.Sin(phase);

        return (short)(32760 * value);
    }
}

public class SquareGenerator : ISampleProvider
{
    public short Read(float freq, int index)
    {
        float samplesPerPeriod = 1 / freq;
        float halfSamplesPerPeriod = samplesPerPeriod / 2;

        return (index % samplesPerPeriod) < halfSamplesPerPeriod ? (short)32760 : (short)-32760;
    }
}

public class TriangleGenerator : ISampleProvider
{
    public short Read(float freq, int index)
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
    public short Read(float freq, int index)
    {
        float samplesPerPeriod = 1 / freq; // Number of samples per period (1/frequency)
        float dutySamples = samplesPerPeriod / 8;

        return (index % samplesPerPeriod) < dutySamples ? (short)32760 : (short)-32760;
    }
}

public class SawtoothGenerator : ISampleProvider
{
    public short Read(float freq, int index)
    {
        float samplesPerPeriod = 1 / freq;
        float normalizedPosition = (index % samplesPerPeriod) / samplesPerPeriod;
        float value = 2 * normalizedPosition - 1;

        return (short)(32760 * value);
    }
}

public class CringeGenerator : ISampleProvider
{
    private readonly Random _random = new();
    public short Read(float freq, int index)
    {
        float samplesPerPeriod = 1 / freq;
        float normalizedPosition = (index % samplesPerPeriod) / samplesPerPeriod;
        float value = 2 * normalizedPosition - 1;

        return (short) (32760 * value + _random.NextDouble() * 1000*_random.NextDouble());
    }
}