using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

// TODO: Implement fluent validators
// TODO: UpdateDiscount, check why it is handled as UPSERT. I may need to get first and check
// TODO: Add logs

public class DiscountService (DiscountContext dbContext, ILogger<DiscountService> logger): DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        logger.LogInformation($"Start {nameof(GetDiscount)} request {request}");
        
        var coupon = await dbContext
            .Coupons
            .FirstOrDefaultAsync(c => c.ProductName == request.ProductName) ??
                     new Coupon { ProductName = "No Coupon", Description = "No Discount", Amount = 0, Id = 0 };

        return coupon.Adapt<CouponModel>();
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        logger.LogInformation($"Start {nameof(CreateDiscount)} request {request}");

        if (request.Coupon is null) throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));
        
        var coupon = request.Coupon.Adapt<Coupon>();
        dbContext.Coupons.Add(coupon);
        await dbContext.SaveChangesAsync(context.CancellationToken);
        return request.Coupon;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        logger.LogInformation($"Start {nameof(UpdateDiscount)} request {request}");
        
        if (request.Coupon is null) throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));

        var coupon = request.Coupon.Adapt<Coupon>();
        
        dbContext.Coupons.Update(coupon);
        await dbContext.SaveChangesAsync(context.CancellationToken);
        return request.Coupon;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        logger.LogInformation($"Start {nameof(DeleteDiscount)} request {request}");
        
       if (request.ProductName is null) throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));
       
       var coupon = await dbContext
           .Coupons
           .FirstOrDefaultAsync(c => c.ProductName == request.ProductName);
       
       if (coupon is null) throw new RpcException(new Status(StatusCode.NotFound, $"Coupon not found for product name { request.ProductName }"));
       
       dbContext.Coupons.Remove(coupon);
       await dbContext.SaveChangesAsync(context.CancellationToken);
       
       return new DeleteDiscountResponse() { Success = true };
    }
}