using MediatR;
using FluentValidation;
using AutoMapper;
using Product.Core.Domain.Repository;

namespace Product.Core.Application.Products.Get;


public class GetProductQueryHandler : IRequestHandler<GetProductQuery, GetProductQueryResult>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public GetProductQueryHandler(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
        
    public async Task<GetProductQueryResult> Handle(GetProductQuery command, CancellationToken cancellationToken)
    {
        var validator = new GetProductQueryValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (validationResult != null && !validationResult.IsValid)                
            throw new ValidationException(validationResult.Errors);
            
        var Product = await _repository.GetByIdAsync(command.Id);
        if (Product == null) 
            throw new KeyNotFoundException($"Product with ID {command.Id} not found");
                        
        return _mapper.Map<GetProductQueryResult>(Product);         
    }
}

