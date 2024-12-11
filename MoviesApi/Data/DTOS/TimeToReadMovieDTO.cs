using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Data.DTOS;

public class TimeToReadMovieDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public int Duration { get; set; }

    public DateTime RequestTime { get; set; } = DateTime.Now;
}
