using AutoMapper;
using Base.Core.Domain.Common;
using Base.WebApi.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product.Core.Application.Products.Create;
using Product.Core.Application.Products.Delete;
using Product.Core.Application.Products.Modify;
using Product.Core.Domain.Repository;
using System.Threading;

namespace ApiStock.Controller
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductController(IMapper mapper, IMediator mediator, IProductRepository repository)
        {
            this._mediator = mediator;
            this._repository = repository;
            this._mapper = mapper;
        }

        /// <summary>
        /// TODO return with ApiResponseWithData
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _repository.GetAllAsync());
        }

        /// <summary>
        /// TODO return with ApiResponseWithData
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _repository.GetByIdAsync(id));
        }

        /// <summary>
        /// TODO return with ApiResponseWithData
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("getbyname/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            return Ok(await _repository.GetByName(name));
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateProductCommand command)
        {
            try
            {
                var response = await _mediator.Send(command);

                return Created(string.Empty, new ApiResponseWithData<CreateProductResult>
                {
                    Success = true,
                    Message = "Product created successfully",
                    Data = _mapper.Map<CreateProductResult>(response)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseWithData<CreateProductResult>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(ModifyProductCommand command)
        {
            try
            {
                var response = await _mediator.Send(command);

                return Created(string.Empty, new ApiResponseWithData<ModifyProductResult>
                {
                    Success = true,
                    Message = "Product modified successfully",
                    Data = _mapper.Map<ModifyProductResult>(response)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseWithData<ModifyProductResult>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        /// <summary>
        /// TODO return with ApiResponseWithData
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var obj = new DeleteProductCommand { Id = id };
            var result = await _mediator.Send(obj);
            return Ok(result);
        }
    }
}
