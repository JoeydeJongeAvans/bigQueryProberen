using Google.Cloud.BigQuery.V2;
using Microsoft.AspNetCore.Mvc;

// ...
[ApiController]
[Route("api/[controller]")]
public class SearchTrendsController : ControllerBase
{
    private readonly BigQueryClient _bigQueryClient;
    private readonly ILogger<SearchTrendsController> _logger;

   public SearchTrendsController(BigQueryClient bigQueryClient, ILogger<SearchTrendsController> logger)
{
    _bigQueryClient = bigQueryClient;
    _logger = logger;
}

    [HttpGet]
   public async Task<ActionResult<IEnumerable<string>>> GetSearchQueries()
    {
        string query = @"
            SELECT
                track_name,
                artist_s__name,
                artist_count,
                released_year
            FROM
                `cmoefenen.spotify2023.spotifysongs`
            WHERE
                released_year = 2023";

        try
        {
            // Voer de query uit
            var queryJob = await _bigQueryClient.CreateQueryJobAsync(
                query,
                parameters: null
            );

            // Wacht tot de query is voltooid
            var queryResults = await queryJob.GetQueryResultsAsync();

            // Verwerk de resultaten
            var songs = new List<string>();
            foreach (var row in queryResults)
            {
                string songInfo = $"{row["track_name"]}, {row["artist_s__name"]}, {row["artist_count"]}, {row["released_year"]}";
                songs.Add(songInfo);
            }

            return Ok(songs);
        }
        catch (Exception ex)
        {
            // Behandel de uitzondering op de juiste manier (bijv. log de fout)
            _logger.LogError(ex, "Er is een fout opgetreden bij het verwerken van het verzoek.");
            return StatusCode(500, "Er is een interne serverfout opgetreden.");
        }
    }
}



