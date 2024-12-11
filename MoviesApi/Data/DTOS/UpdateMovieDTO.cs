using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Data.DTOS;

public class UpdateMovieDTO
{
    [Required(ErrorMessage = "Title is Required")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Genre is required")]
    [MaxLength(ErrorMessage = "Length greather than 50 caharacteres")]
    [StringLength(50, ErrorMessage = "Genre cannot be longer than 50 characters")] // StringLength sem alocação de me´mória em banco
    public string Genre { get; set; }

    [Required(ErrorMessage = "Duration is a integer required")]
    [Range(70, 600, ErrorMessage = "Duration is not valid")]
    public int Duration { get; set; }
}
