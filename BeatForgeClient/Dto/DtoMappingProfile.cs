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
                if (src.Id is null) throw new Exception("Id must be set");
            });

        CreateMap<Channel, ChannelDto>();
        CreateMap<ChannelDto, Channel>()
            .BeforeMap((src, dst) =>
            {
                if (src.Id is null) throw new Exception("Id must be set");
                if (src.Name is null) throw new Exception("Name must be set");
            });

        CreateMap<Song, SongDto>();
        CreateMap<SongDto, Song>()
            .BeforeMap((src, dst) =>
            {
                if (src.Id is null) throw new Exception("Id must be set");
                if (src.Name is null) throw new Exception("Name must be set");
                if (src.Song is null) throw new Exception("Song must be set");
            });

        CreateMap<Preferences, PreferencesDto>();
        CreateMap<PreferencesDto, Preferences>()
            .BeforeMap((src, dst) =>
            {
                if (src.Id is null) throw new Exception("Id must be set");
                if (src.Song is null) throw new Exception("Song must be set");
            });
    }
}
