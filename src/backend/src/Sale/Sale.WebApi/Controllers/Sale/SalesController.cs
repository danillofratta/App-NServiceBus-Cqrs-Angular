using AutoMapper;
using Base.WebApi;
using Base.WebApi.Controller;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sale.Core.Application.Sale.Query.GetSalesList;
using Sale.Core.Application.Sales.Create;
using Sale.Core.Domain.Repository;
using Sale.WebApi.Controllers.Sale.Create;
using Sale.WebApi.Controllers.Sale.GetList;

namespace Sale.WebApi.Controllers.Sale;

/// <summary>
/// Sale EndPoint
/// TODO: create versioning 
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class SalesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ISaleRepository _repository;

    public SalesController(IMediator mediator, IMapper mapper, ISaleRepository repository)
    {
        _mediator = mediator;
        _mapper = mapper;
        _repository = repository;
    }

    /// <summary>
    /// creates the sale and generates the item discounts and calculates the total sale
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validator = new CreateSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<CreateSaleCommand>(request);
            var response = await _mediator.Send(command, cancellationToken);

            return Created(string.Empty, new ApiResponseWithData<CreateSaleResponse>
            {
                Success = true,
                Message = "Sale created successfully",
                Data = _mapper.Map<CreateSaleResponse>(response)
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponseWithData<CreateSaleResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpGet("GetList")]
    public async Task<IActionResult> GetListSale(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? orderBy = null,
        [FromQuery] bool isDescending = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Invalid pagination parameters. Page number must be >= 1 and page size must be between 1 and 100."
                });
            }

            var query = new GetListSaleQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                OrderBy = orderBy,
                IsDescending = isDescending
            };

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ApiResponseWithData<PagedResult<GetListSaleResponse>>
            {
                Success = true,
                Message = "Sales list retrieved successfully",
                Data = new PagedResult<GetListSaleResponse>
                {
                    Items = _mapper.Map<List<GetListSaleResponse>>(result.Items),
                    TotalCount = result.TotalCount,
                    PageNumber = result.PageNumber,
                    TotalPages = result.TotalPages
                }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = ex.Message
            });
        }
    }
}




