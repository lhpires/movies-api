using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MoviesApi.Data;
using MoviesApi.Data.DTOS;
using MoviesApi.Models;

namespace MoviesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController : ControllerBase
{

    //private static List<Movie> movies = new List<Movie>();
    private MovieContext _context;
    private IMapper _mapper;

    private static int id = 0;

    public MoviesController(MovieContext context,IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="CreateMovieDTO">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult addMovie([FromBody] CreateMovieDTO input)
    {
        // Não vamos precisar instanciar a classe Movie, porque vamos utilizar uma lib mapper para fazer implicitamente essa instanciação
        //Movie movie = new Movie
        //{

        //};

        Movie movie = _mapper.Map<Movie>(input);
        _context.Movies.Add(movie);  // Adiciona o filme ao conjunto "Movies"
        _context.SaveChanges();      // Salva as alterações no banco de dados
        return CreatedAtAction(nameof(getMovieById), new { id = movie.Id },movie);
    }

    [HttpGet]
    public IEnumerable<TimeToReadMovieDTO> getMovies([FromQuery] int page = 1,int offset = 10)
    {
        // Paginando uma lista enumerada
        int skip = (page > 0) ? (page -= 1) : 0;

        return _mapper.Map<List<TimeToReadMovieDTO>>(_context.Movies.Skip(skip).Take(offset));
    }

    [HttpGet("{id}")]
    public IActionResult getMovieById(int id)
    {
        var movie = _context.Movies.FirstOrDefault(movie => movie.Id == id);

        if (movie == null) return NotFound();

        var movieOutput = _mapper.Map<TimeToReadMovieDTO>(movie);
        return Ok(movieOutput);
    }

    [HttpPut("{id}")]
    public IActionResult updateMovie(int id, [FromBody] UpdateMovieDTO input)
    {
        var movie = _context.Movies.FirstOrDefault(movie => movie.Id == id);
        if (movie == null) return NotFound();
        _mapper.Map(input,movie);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult patchMovie(int id,JsonPatchDocument<UpdateMovieDTO> input)
    {
        // Busca o filme pelo ID no banco de dados usando o Entity Framework.
        // Se nenhum filme for encontrado, retorna uma resposta 404 Not Found.
        var movie = _context.Movies.FirstOrDefault(movie => movie.Id == id);
        if (movie == null) return NotFound();

        // Cria uma nova instância de UpdateMovieDTO mapeando os dados do filme encontrado.
        // Isso é necessário para aplicar o patch sem modificar diretamente o objeto original.
        var movieToUpdate = _mapper.Map<UpdateMovieDTO>(movie);

        // Aplica as alterações contidas no documento JSON Patch ao objeto movieToUpdate.
        // ModelState é usado para capturar quaisquer erros que possam ocorrer durante a aplicação do patch.
        input.ApplyTo(movieToUpdate, ModelState);

        // Valida o objeto movieToUpdate após as alterações do JSON Patch.
        // Se o modelo não for válido, retorna uma resposta 400 Bad Request com os detalhes dos erros.
        if (!TryValidateModel(movieToUpdate))
        {
            return ValidationProblem(ModelState);
        }

        // Atualiza o objeto original (movie) com os valores do objeto atualizado (movieToUpdate).
        // Essa etapa é necessária porque movie será salvo no banco de dados.
        _mapper.Map(movieToUpdate, movie);

        // Salva as alterações no banco de dados.
        _context.SaveChanges();

        // Retorna uma resposta 204 No Content para indicar que a operação foi bem-sucedida.
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteMovie(int id)
    {
        var movie = _context.Movies.FirstOrDefault(movie => movie.Id == id);
        if (movie == null) return NotFound();

        _context.Remove(movie);
        _context.SaveChanges();

        return NoContent();
    }
}
