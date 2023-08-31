using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Timers;
using Avalonia.Media.Imaging;
using BeatForgeClient.Extensions;
using BeatForgeClient.Audio;
using BeatForgeClient.Models;
using ReactiveUI;
using SharpAudio;
using SkiaSharp;
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

    private void RepaintCanvas()
    {
        
    }

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
