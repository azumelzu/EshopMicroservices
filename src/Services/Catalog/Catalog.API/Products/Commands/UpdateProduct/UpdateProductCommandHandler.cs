namespace Catalog.API.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price) : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage($"{nameof(UpdateProductCommand.Id)} is required");
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage($"{nameof(UpdateProductCommand.Name)} is required")
            .Length(2, 150)
            .WithMessage($"{nameof(UpdateProductCommand.Name)} must be between 2 and 150 characters");
            
        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage($"{nameof(UpdateProductCommand.Category)} is required");
            
        RuleFor(x => x.ImageFile)
            .NotEmpty()
            .WithMessage($"{nameof(UpdateProductCommand.ImageFile)} is required");
            
        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage($"{nameof(UpdateProductCommand.Price)} must be greater than 0");
    }
}

internal class UpdateProductCommandHandler (IDocumentSession session) 
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
        
        if (product is null) throw new ProductNotFoundException(command.Id);
        
        product.Name = command.Name;
        product.Category = command.Category;
        product.Description = command.Description;
        product.ImageFile = command.ImageFile;
        product.Price = command.Price;
        
        session.Update(product);
        
        await session.SaveChangesAsync(cancellationToken);
        
        return new UpdateProductResult(true);
    }
}