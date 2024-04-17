using Basket.API.Data;

namespace Basket.API.Basket.Commands.DeleteBasket;

public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;
public record DeleteBasketResult(bool IsSuccess);

public class DeleteCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("UserName is required");
    }
}

public class DeleteBasketCommandHandler(IBasketRepository repository) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        var ok = await repository.DeleteBasketAsync(command.UserName, cancellationToken);
        return new DeleteBasketResult(ok);
    }
}