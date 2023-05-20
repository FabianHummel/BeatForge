using System;

namespace BeatForgeClient.Audio;

public class SineGenerator : ISampleProvider
{
    private int Size { get; }

    public SineGenerator(int size)
    {
        Size = size;
    }

    public short Read(float freq, int index) => (short)(32760 * Math.Sin((2 * Math.PI * freq) / Size * index));
}

public class SquareGenerator : ISampleProvider
{
    private int Size { get; }

    public SquareGenerator(int size)
    {
        Size = size;
    }

    public short Read(float freq, int index)
    {
        float samplesPerPeriod = Size / freq; // Number of samples per period (1/frequency)
        float halfSamplesPerPeriod = samplesPerPeriod / 2;  // Half of the samples per period

        return (short)((index % samplesPerPeriod) < halfSamplesPerPeriod ? 32760 : -32760);
    }
}

public class TriangleGenerator : ISampleProvider
{
    private int Size { get; }

    public TriangleGenerator(int size)
    {
        Size = size;
    }

    public short Read(float freq, int index)
    {
        float samplesPerPeriod = Size / freq; // Number of samples per period (1/frequency)
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
    private int Size { get; }
    private float DutyCycle { get; }

    public PulseGenerator(int size, float dutyCycle)
    {
        Size = size;
        DutyCycle = dutyCycle;
    }

    public short Read(float freq, int index)
    {
        float samplesPerPeriod = Size / freq; // Number of samples per period (1/frequency)
        float dutySamples = samplesPerPeriod * DutyCycle;

        return (short)((index % samplesPerPeriod) < dutySamples ? 32760 : -32760);
    }
}
