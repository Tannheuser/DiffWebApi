using System;
using System.Linq;
using DiffWebApi.Core.Models;
using DiffWebApi.Core.Services.Abstract;
using DiffWebApi.Web.Data;
using DiffWebApi.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DiffWebApi.Web.Controllers
{
    [Route("v1/diff")]
    public class CompareController : Controller
    {
        private readonly IComparisonService _comparisonService;
        private readonly ILogger _logger;

        public CompareController(IComparisonService comparisonService, ILogger<CompareController> logger)
        {
            _comparisonService = comparisonService;
            _logger = logger;
        }

        [Route("{id:int}")]
        public IActionResult ComparePositionsData(int id)
        {
            try
            {
                var comparisonResult = _comparisonService.Compare(id);
                var diffResultType = Enum.GetName(typeof(EqualityType), comparisonResult.Equality);

                return new ObjectResult(new ComparisonResultDto { DiffResultType = diffResultType, Diffs = comparisonResult.Diff });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Route("{id:int}/{positionString:required}")]
        public IActionResult AddPositionData(int id, string positionString, [FromBody] Base64Dto data)
        {
            try
            {
                if (!TryParsePositionString(positionString, out PositionType position))
                {
                    return BadRequest("Specified position was not found");
                }

                _comparisonService.AddOrUpdateData(id, position, data.Data);

                return CreatedAtRoute(new { Id = id }, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            
        }

        private bool TryParsePositionString(string positionString, out PositionType position)
        {
            return Enum.TryParse(positionString, true, out position);
        }
    }
}