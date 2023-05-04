using System;
using AutoMapper;
using BeatForgeClient.Infrastructure;

namespace BeatForgeClient.Dto;

public class DtoMappingProfile : Profile
{
    public DtoMappingProfile()
    {
        CreateMap<Note, NoteDto>();
        CreateMap<NoteDto, Note>()
            .BeforeMap((src, dst) =>
            {
                if (src.Start == null) throw new Exception("Start must be set");
                if (src.End == null) throw new Exception("End must be set");
                if (src.Pitch == null) throw new Exception("Pitch must be set");
                if (src.Duration == null) throw new Exception("Duration must be set");
                if (src.Channel == null) throw new Exception("Channel must be set");
            });
        
        CreateMap<Channel, ChannelDto>();
        CreateMap<ChannelDto, Channel>()
            .BeforeMap((src, dst) =>
            {
                src.Name ??= "New Channel";
                // if (src.Song == null) throw new Exception("Song must be set");
                if (src.Volume == null) throw new Exception("Volume must be set");
                if (src.Instrument == null) throw new Exception("Instrument must be set");
            });

        CreateMap<Instrument, InstrumentDto>();
        CreateMap<InstrumentDto, Instrument>()
            .BeforeMap((src, dst) =>
            {
                src.Name ??= "New Instrument";
                if (src.Channel == null) throw new Exception("Channel must be set");
            });

        CreateMap<Song, SongDto>();
        CreateMap<SongDto, Song>()
            .BeforeMap((src, dst) =>
            {
                src.Name ??= "New Song";
            });
        
        CreateMap<Preferences, PreferencesDto>();
        CreateMap<PreferencesDto, Preferences>()
            .BeforeMap((src, dst) =>
            {
                if (src.Song == null) throw new Exception("Song must be set");
                if (src.Volume == null) throw new Exception("Volume must be set");
            });
    }
}