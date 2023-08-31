namespace BeatForgeClient.Audio;

public interface ISampleProvider
{
    short Read(float freq, int sample);
}
