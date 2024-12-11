using AutoMapper;
using MoviesApi.Data.DTOS;
using MoviesApi.Models;

namespace MoviesApi.Profiles;

public class MovieProfile: Profile
{
    public MovieProfile()
    {
        CreateMap<CreateMovieDTO, Movie>();
        CreateMap<UpdateMovieDTO, Movie>();
        CreateMap<Movie, UpdateMovieDTO>();
        CreateMap<Movie, TimeToReadMovieDTO>();
    }
}
