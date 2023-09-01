using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Timers;
using BeatForgeClient.Audio;
using BeatForgeClient.Extensions;
using BeatForgeClient.Models;
using ReactiveUI;
using SharpAudio;
using Timer = System.Timers.Timer;

namespace BeatForgeClient.ViewModels;

public class ContentViewModel : ViewModelBase
{
    private int _playback = 0;
    private bool _playing = false;
    public readonly Timer _timer = new();
    private readonly AudioEngine _engine = AudioEngine.CreateDefault();
    private readonly List<(AudioSource, AudioBuffer)> _disposables = new();
    private readonly AudioFormat _format = new AudioFormat
    {
        BitsPerSample = 16,
        Channels = 1,
        SampleRate = 44100
    };

    public MainWindowViewModel MainVm { get; }
    
    public ContentViewModel(MainWindowViewModel mainVm)
    {
        MainVm = mainVm;

        _timer.Elapsed += TimerOnElapsed;
        _timer.Elapsed += (_, _) => ClearDisposables();
        
        MainVm.ChannelsViewModel.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(MainVm.ChannelsViewModel.SelectedChannel))
            {
                this.RaisePropertyChanged(nameof(ChannelNotes));
                OtherNotes.ReplaceAll(MainVm.ChannelsViewModel.SongChannels
                    .Where(channel => channel.Id != MainVm.ChannelsViewModel.SelectedChannel?.Id)
                    .SelectMany(channel => channel.Notes));
            }
        };
    }

    public ObservableCollection<NoteDto>? ChannelNotes => MainVm.ChannelsViewModel.SelectedChannel?.Notes;
    public ObservableCollection<NoteDto> OtherNotes { get; } = new();

    public int Playback
    {
        get => _playback;
        set
        {
            this.RaiseAndSetIfChanged(ref _playback, value);
            if (value >= MainVm.TitlebarViewModel.SelectedSong!.Preferences.Length * 4)
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
                if (MainVm.TitlebarViewModel.SelectedSong is null) return;
                Console.WriteLine("Starting playback...");
                // StartPlaying();
                _timer.Interval = 1.0f / MainVm.TitlebarViewModel.SelectedSong.Preferences.Bpm * 60.0f * 1000;
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
                    var unused = source.BuffersQueued; // Need this line to update the buffers
                    _disposables.Remove(kvp);
                    buffer.Dispose();
                    source.Dispose();
                } catch (Exception ex) {
                    Console.WriteLine($"exception: {ex}");
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
        if (MainVm.TitlebarViewModel.SelectedSong is null) return;
        foreach (var channel in MainVm.TitlebarViewModel.SelectedSong.Channels)
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
            Instrument.Cringe => new CringeGenerator(),
            _ => throw new ArgumentOutOfRangeException(nameof(instrument))
        };
    }

    private void PlayNoteImmediate(NoteDto note, ISampleProvider generator, float volume = 1.0f) 
    {
        var frequency = PitchToFrequency(
            pitch: -note.Pitch);

        var start = BeatsToSamples(
            beats: Playback,
            bpm: MainVm.TitlebarViewModel.SelectedSong!.Preferences.Bpm,
            _format.SampleRate);

        var size = BeatsToSamples(
            beats: note.Duration,
            bpm: MainVm.TitlebarViewModel.SelectedSong!.Preferences.Bpm,
            _format.SampleRate);

        var samples = GenerateSamples(
            generator: generator,
            freq: frequency,
            start: start,
            size: size,
            bpm: MainVm.TitlebarViewModel.SelectedSong!.Preferences.Bpm);

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
        var generator = CreateGenerator(channel.Instrument);
        var notes = channel.Notes.Where(note => note.Start == Playback);

        foreach (var note in notes)
        {
            PlayNoteImmediate(note, generator, channel.ProcessedVolume);
        }
    }

    public void ToggleNoteAt(int start, int pitch)
    {
        if (MainVm.ChannelsViewModel.SelectedChannel is null) return;
        var channel = MainVm.ChannelsViewModel.SelectedChannel;
        
        var note = ChannelNotes!.FirstOrDefault(n =>
            n.Channel == channel &&
            n.Start == start &&
            n.Pitch == pitch);
        if (note is not null)
        {
            ChannelNotes!.Remove(note);
        }
        else
        {
            note = new NoteDto
            {
                Start = start,
                Pitch = pitch,
                Channel = MainVm.ChannelsViewModel.SelectedChannel,
                Duration = 1
            };
            ChannelNotes!.Add(note);

            var generator = CreateGenerator(channel.Instrument);
            PlayNoteImmediate(note, generator, channel.Volume);
        }
    }
}
