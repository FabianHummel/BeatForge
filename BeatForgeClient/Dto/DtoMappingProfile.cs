using System;
using AutoMapper;
using BeatForgeClient.Models;

namespace BeatForgeClient.Dto;

public class DtoMappingProfile : Profile
{
    public DtoMappingProfile()
    {
        CreateMap<Note, NoteDto>();
        CreateMap<NoteDto, Note>();

        CreateMap<Channel, ChannelDto>();
        CreateMap<ChannelDto, Channel>();

        CreateMap<Song, SongDto>();
        CreateMap<SongDto, Song>();

        CreateMap<Preferences, PreferencesDto>();
        CreateMap<PreferencesDto, Preferences>();
    }
}
