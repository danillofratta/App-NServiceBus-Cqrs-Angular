using AutoMapper;
using Base.WebApi;
using Base.WebApi.Controller;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stock.Core.Application.Stock.AddProductStock;
using Stock.Core.Application.Stock.Query.GetList;
using Stock.Core.Domain.Repository;
using Stock.WebApi.Controllers.Command.AddProductStock;
using Stock.WebApi.Controllers.Query.GetList;

namespace ApiStock.Controller
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class StockController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IStockRepository _repository;

        public StockController(IMediator mediator, IMapper mapper, IStockRepository repository)
        {
            _mediator = mediator;
            _mapper = mapper;
            _repository = repository;
        }

        [HttpPost()]
        public async Task<ActionResult> AddProductIntoStock([FromBody] AddProductStockRequest request, CancellationToken cancellationToken)
        {
            try
            {
                //var validator = new CreateSaleRequestValidator();
                //var validationResult = await validator.ValidateAsync(request, cancellationToken);

                //if (!validationResult.IsValid)
                //return BadRequest(validationResult.Errors);

                var command = _mapper.Map<AddProductStockCommand>(request);
                var response = await _mediator.Send(command, cancellationToken);

                return Created(string.Empty, new ApiResponseWithData<AddProductStockResponse>
                {
                    Success = true,
                    Message = "Add Product in Stock successfully",
                    Data = _mapper.Map<AddProductStockResponse>(response)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseWithData<AddProductStockResponse>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetListStock(
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

                var query = new GetListStockQuery
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    IsDescending = isDescending
                };

                var result = await _mediator.Send(query, cancellationToken);

                return Ok(new ApiResponseWithData<PagedResult<GetListStockResponse>>
                {
                    Success = true,
                    Message = "Stock list retrieved successfully",
                    Data = new PagedResult<GetListStockResponse>
                    {
                        Items = _mapper.Map<List<GetListStockResponse>>(result.Items),
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

        //[HttpGet]
        //public async Task<ActionResult<List<StockDto>>> GetAll()
        //{
        //    var list = (from a in _context.Stock
        //                join b in _context.Product on a.Idproduct equals b.Id
        //                select new SharedDatabase.Dto.StockDto
        //                {
        //                    id = a.Id,
        //                    idproduct = a.Idproduct,
        //                    amount = a.Amount,
        //                    nameproduct = b.Name
        //                }).ToList<StockDto>();

        //    await _hubContext.Clients.All.SendAsync("GetListStock", list);

        //    return Ok(list);
        //}
    }
}

