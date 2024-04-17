namespace Catalog.API.Products.Commands.CreateProduct
{
    public record CreateProductCommand (string Name, List<string> Category, string Description, string ImageFile, decimal Price) 
        : ICommand<CreateProductResult>;
    public record CreateProductResult (Guid Id);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage($"{nameof(CreateProductCommand.Name)} is required")
                .Length(2, 150)
                .WithMessage($"{nameof(CreateProductCommand.Name)} must be between 2 and 150 characters");
            
            RuleFor(x => x.Category)
                .NotEmpty()
                .WithMessage($"{nameof(CreateProductCommand.Category)} is required");
            
            RuleFor(x => x.ImageFile)
                .NotEmpty()
                .WithMessage($"{nameof(CreateProductCommand.ImageFile)} is required");
            
            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage($"{nameof(CreateProductCommand.Price)} must be greater than 0");
        }
    }

    internal class CreateProductCommandHandler (IDocumentSession session) 
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // Create product entity from command object
            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price
            };

            // save to database
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            // return CreateProductResult
            return new CreateProductResult(product.Id);
        }
    }
}
