using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Basket.API.Basket.CheckoutBasket;

public record BasketCheckoutCommand(BasketCheckoutDto BasketCheckoutDto) : ICommand<BasketCheckoutResult>;
public record BasketCheckoutResult(bool IsSuccess);
public class CheckoutBasketCommandValidator
    : AbstractValidator<BasketCheckoutCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BasketCheckoutDto).NotNull().WithMessage("BasketCheckoutDto can't be null");
        RuleFor(x => x.BasketCheckoutDto.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public class CheckoutBasketHandler
    (IBasketRepository repository, IPublishEndpoint publishEndpoint) : 
    ICommandHandler<BasketCheckoutCommand, BasketCheckoutResult>
{
    public async Task<BasketCheckoutResult> Handle(BasketCheckoutCommand command, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasket(command.BasketCheckoutDto.UserName, cancellationToken);
        if (basket == null)
        {
            return new BasketCheckoutResult(false);
        }

        var eventMessage = command.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();
        eventMessage.TotalPrice = basket.TotalPrice;

        await publishEndpoint.Publish(eventMessage, cancellationToken);

        await repository.DeleteBasket(command.BasketCheckoutDto.UserName, cancellationToken);

        return new BasketCheckoutResult(true);
    }
}
