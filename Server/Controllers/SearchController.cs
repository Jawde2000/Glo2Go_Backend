using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly SearchService _searchService;

    public SearchController(SearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpGet("search")]
    public IActionResult Search([FromQuery] string term)
    {
        var result = _searchService.CombinedSearch(term);
        return Ok(result);
    }
}
