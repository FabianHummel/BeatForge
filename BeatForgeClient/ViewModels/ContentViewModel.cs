using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Timers;
using BeatForgeClient.Extensions;
using BeatForgeClient.Audio;
using BeatForgeClient.Models;
using ReactiveUI;
using SharpAudio;
using Timer = System.Timers.Timer;

namespace BeatForgeClient.ViewModels;

public class ContentViewModel : ViewModelBase
{
    private int _playback = 0;
    private bool _playing = false;
    private readonly Timer _timer = new();
    private readonly AudioEngine _engine = AudioEngine.CreateDefault();
    private readonly List<(AudioSource, AudioBuffer)> _disposables = new();
    private readonly AudioFormat _format = new AudioFormat
    {
        BitsPerSample = 16,
        Channels = 1,
        SampleRate = 44100
    };

    public MainWindowViewModel MainVm { get; }
    public ObservableCollection<ChannelDto> Channels => MainVm.ChannelsViewModel.SongChannels;

    public ContentViewModel(MainWindowViewModel mainVm)
    {
        MainVm = mainVm;

        MainVm.ChannelsViewModel.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(MainVm.ChannelsViewModel.SelectedChannel))
            {
                LoadChannelNotes();
            }
        };

        _timer.Elapsed += TimerOnElapsed;
        _timer.Elapsed += (sender, e) => ClearDisposables();
    }

    public ObservableCollection<NoteDto> ChannelNotes { get; } = new();

    public int Playback
    {
        get => _playback;
        set
        {
            this.RaiseAndSetIfChanged(ref _playback, value);
            if (value >= MainVm.SettingsViewModel.SongLength * 4)
            {
                _playback = 0;
            }
        }
    }

    public bool Playing
    {
        get => _playing;
        set
        {
            this.RaiseAndSetIfChanged(ref _playing, value);
            if (value is false)
            {
                Console.WriteLine("Stopping playback...");
                // StopPlaying();
                _timer.Stop();
                Playback = 0;
            }
            else
            {
                if (MainVm.Song is null) return;
                Console.WriteLine("Starting playback...");
                // StartPlaying();
                _timer.Interval = 1.0f / MainVm.Song.Bpm * 60.0f * 1000;
                SaveChannelNotes();
                PlayNotesAtHead();
                _timer.Start();
            }
        }
    }

    private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        Playback += 1;
        PlayNotesAtHead();
    }

    private void ClearDisposables()
    {
        for (var i = 0; i < _disposables.Count; i++) {
            var kvp = _disposables[i];
            var (source, buffer) = kvp;
            if (!source.IsPlaying())
            {
                try {
                    var bufs = source.BuffersQueued; // Need this line to update the buffers
                    _disposables.Remove(kvp);
                    buffer.Dispose();
                    source.Dispose();
                } catch (Exception ex) {
                    Console.WriteLine($"exception: {ex.ToString()}");
                }
            }    
        }
    }

    private static float PitchToFrequency(int pitch)
    {
        float baseFrequency = 261.63f * 4; // Frequency of C4
        return baseFrequency * MathF.Pow(2, pitch / 12.0f);
    }

    private static int BeatsToSamples(int beats, int bpm, int sampleRate)
    {
        return (int)(beats * 60.0f / bpm * sampleRate);
    }

    private static short[] GenerateSamples(ISampleProvider generator, float freq, int start, int size, int bpm)
    {
        var buffer = new short[size];
        for (var i = 0; i < size; i++)
        {
            buffer[i] = generator.Read(freq / size / bpm * 60.0f, start + i);
        }

        return buffer;
    }

    private void PlayNotesAtHead()
    {
        if (MainVm.Song is null) return;
        foreach (var channel in Channels)
        {
            PlayNotesInChannel(channel);
        }
    }

    private ISampleProvider CreateGenerator(Instrument instrument)
    {
        return instrument switch
        {
            Instrument.Sine => new SineGenerator(),
            Instrument.Square => new SquareGenerator(),
            Instrument.Triangle => new TriangleGenerator(),
            Instrument.Pulse => new PulseGenerator(),
            Instrument.Sawtooth => new SawtoothGenerator(),
            _ => throw new ArgumentOutOfRangeException(nameof(instrument))
        };
    }

    private void PlayNoteImmediate(ChannelDto channel, NoteDto note, ISampleProvider generator, float volume = 1.0f) 
    {
        var frequency = PitchToFrequency(
            pitch: -note.Pitch);

        var start = BeatsToSamples(
            beats: Playback,
            bpm: MainVm.Song.Bpm,
            _format.SampleRate);

        var size = BeatsToSamples(
            beats: note.Duration,
            bpm: MainVm.Song.Bpm,
            _format.SampleRate);

        var samples = GenerateSamples(
            generator: generator,
            freq: frequency,
            start: start,
            size: size,
            bpm: MainVm.Song.Bpm);

        var buffer = _engine.CreateBuffer();
        buffer.BufferData(samples, _format);

        var source = _engine.CreateSource();
        source.Volume = volume;
        source.QueueBuffer(buffer);
        source.Play();

        _disposables.Add((source, buffer));
    }

    private void PlayNotesInChannel(ChannelDto channel)
    {
        ISampleProvider generator = CreateGenerator(channel.Instrument);
        var notes = channel.Notes.Where(note => note.Start == Playback);

        foreach (var note in notes)
        {
            PlayNoteImmediate(channel, note, generator, channel.Volume);
        }
    }

    /* private static short MergeSamples(params short[] arr)
    {
        var total = arr.Aggregate<short, long>(0, (current, sample) => current + sample);
        return (short)(total / arr.Length);
    }

    private static short[] MergeNotes(IEnumerable<(int start, short[] samples)> notes, long totalSize)
    {
        var mergedSamples = new short[totalSize];
        var sampleCounts = new int[totalSize];

        foreach (var note in notes)
        {
            var startIndex = note.start;
            var endIndex = startIndex + note.samples.Length;

            for (var i = startIndex; i < endIndex; i++)
            {
                mergedSamples[i] += note.samples[i - startIndex];
                sampleCounts[i]++;
            }
        }

        for (var i = 0; i < mergedSamples.Length; i++)
        {
            if (sampleCounts[i] == 0) continue;
            mergedSamples[i] /= (short)sampleCounts[i];
        }

        return mergedSamples;
    } */

    /* private List<short[]>? Compile(AudioFormat format)
    {
        if (MainVm.Song is null) return null;
        var totalSampleSize = BeatsToSamples(
            MainVm.Song.Preferences.Length * 4,
            MainVm.Song.Preferences.Bpm,
            format.SampleRate);
        var channelSamples = new List<short[]>(Channels.Count);

        for (var i = 0; i < Channels.Count; i++)
        {
            var channel = Channels[i];
            var instrument = channel.Instrument;
            ISampleProvider generator = instrument switch
            {
                Instrument.Sine => new SineGenerator(totalSampleSize),
                Instrument.Square => new SquareGenerator(totalSampleSize),
                Instrument.Triangle => new TriangleGenerator(totalSampleSize),
                Instrument.Pulse => new PulseGenerator(totalSampleSize, 12.0f),
                _ => throw new ArgumentOutOfRangeException(nameof(instrument)),
            };

            var notes = from note in channel.Notes
                let start = BeatsToSamples(
                    note.Start,
                    MainVm.Song.Preferences.Bpm,
                    format.SampleRate)
                let size = BeatsToSamples(
                    note.Duration,
                    MainVm.Song.Preferences.Bpm,
                    format.SampleRate)
                where start + size <= totalSampleSize
                let frequency = PitchToFrequency(
                    note.Pitch)
                let samples = GenerateSamples(
                    generator: generator,
                    freq: frequency,
                    sample: start,
                    length: size)
                select (start, samples);

            channelSamples.Add(MergeNotes(notes, totalSampleSize));
        }
        
        return channelSamples;
    }

    private void StartPlaying()
    {
        var channelSamples = Compile(_format);
        if (channelSamples is null) return;
        
        var sources = new List<AudioSource>();
        for (var index = 0; index < channelSamples.Count; index++)
        {
            var samples = channelSamples[index];
            var channel = Channels[index];
            var buffer = _engine.CreateBuffer();
            buffer.BufferData(samples, _format);
        
            var source = _engine.CreateSource();
            source.Volume = (float) channel.Volume / 100.0f;
            source.QueueBuffer(buffer);
            sources.Add(source);
        }
        
        foreach (var source in sources)
        {
            source.Play();
        }
        
        // while (sources.Any(source => source.IsPlaying()))
        // {
        //     Thread.Sleep(100);
        // }
        //
        // foreach (var source in sources)
        // {
        //     source.Dispose();
        // }
    }

    private void StopPlaying()
    {
        
    } */

    public void LoadChannelNotes()
    {
        if (MainVm.ChannelsViewModel.SelectedChannel == null) return;
        Logger.Task("Loading notes... ");
        ChannelNotes.ReplaceAll(MainVm.ChannelsViewModel.SelectedChannel.Notes);
        Logger.Complete($"({ChannelNotes.Count} notes loaded).");
    }

    public void ToggleNoteAt(int start, int pitch)
    {
        if (MainVm.ChannelsViewModel.SelectedChannel == null) return;
        var channel = MainVm.ChannelsViewModel.SelectedChannel;
        
        // Logger.Task($"Toggling note at {start} {pitch}... ");
        var note = ChannelNotes.FirstOrDefault(n =>
            n.Channel == channel &&
            n.Start == start &&
            n.Pitch == pitch);
        if (note is not null)
        {
            ChannelNotes.Remove(note);
        }
        else
        {
            note = new()
            {
                Start = start,
                Pitch = pitch,
                Channel = MainVm.ChannelsViewModel.SelectedChannel,
                Duration = 1
            };
            ChannelNotes.Add(note);

            ISampleProvider generator = CreateGenerator(channel.Instrument);
            PlayNoteImmediate(channel, note, generator, channel.Volume);
        }

        this.RaisePropertyChanged(nameof(ChannelNotes));
        var added = note is null ? "added" : "removed";
        // Logger.Complete($"Note {added}.");
    }

    public void SaveChannelNotes()
    {
        if (MainVm.ChannelsViewModel.SelectedChannel == null) return;
        Logger.Task("Saving notes... ");
        MainVm.ChannelsViewModel.SelectedChannel.Notes.ReplaceAll(ChannelNotes);
        Logger.Complete("Notes saved.");
    }
}
