using AutoMapper;
using Base.WebApi;
using Base.WebApi.Controller;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payment.Core.Domain.Application.Payment.Query.GetSalesList;
using Payment.Core.Domain.Repository;
using Payment.WebApi.Controllers.Payment.GetList;

namespace Payment.WebApi.Controllers.Payment;

/// <summary>
/// Sale EndPoint
/// TODO: create versioning 
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class PaymentController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IPaymentRepository _repository;

    public PaymentController(IMediator mediator, IMapper mapper, IPaymentRepository repository)
    {
        _mediator = mediator;
        _mapper = mapper;
        _repository = repository;
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

            var query = new GetListPaymentQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                OrderBy = orderBy,
                IsDescending = isDescending
            };

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ApiResponseWithData<PagedResult<GetListPaymentResponse>>
            {
                Success = true,
                Message = "Sales list retrieved successfully",
                Data = new PagedResult<GetListPaymentResponse>
                {
                    Items = _mapper.Map<List<GetListPaymentResponse>>(result.Items),
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




