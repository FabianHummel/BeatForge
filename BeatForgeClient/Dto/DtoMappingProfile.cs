using System;
using AutoMapper;
using BeatForgeClient.Models;

namespace BeatForgeClient.Dto;

public class DtoMappingProfile : Profile
{
    public DtoMappingProfile()
    {
        CreateMap<Note, NoteDto>();
        CreateMap<NoteDto, Note>()
            .BeforeMap((src, dst) =>
            {
                if (src.Channel == null) throw new Exception("Channel must be set");
            });

        CreateMap<Channel, ChannelDto>();
        CreateMap<ChannelDto, Channel>()
            .BeforeMap((src, dst) =>
            {
                if (src.Name == null) throw new Exception("Name must be set");
                if (src.Volume == null) throw new Exception("Volume must be set");
                if (src.Notes == null) throw new Exception("Notes must be set");
                if (src.Instrument == null) throw new Exception("Instrument must be set");
                if (src.Song == null) throw new Exception("Song must be set");
            });

        CreateMap<Song, SongDto>();
        CreateMap<SongDto, Song>()
            .BeforeMap((src, dst) =>
            {
                if (src.Name == null) throw new Exception("Name must be set");
                if (src.Channels == null) throw new Exception("Channels must be set");
                if (src.Preferences == null) throw new Exception("Preferences must be set");
            });

        CreateMap<Preferences, PreferencesDto>();
        CreateMap<PreferencesDto, Preferences>()
            .BeforeMap((src, dst) =>
            {
                if (src.Volume == null) throw new Exception("Volume must be set");
                if (src.Song == null) throw new Exception("Song must be set");
            });
    }
}
