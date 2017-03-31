using System;
using DiffWebApi.Core.Models;
using DiffWebApi.Core.Services.Abstract;
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
                return NotFound();
            }
        }

        [HttpPost]
        [Route("{id:int}/{position:required}")]
        public IActionResult AddPositionData(int id, PositionType position, [FromBody] Base64Dto data)
        {
            try
            {
                if (position == PositionType.Invalid)
                {
                    return BadRequest();
                }

                _comparisonService.AddOrUpdateData(id, position, data.Data);

                return CreatedAtRoute(new { Id = id }, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }
    }
}