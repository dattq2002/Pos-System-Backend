using AutoMapper;
using Pos_System.API.Constants;
using Pos_System.API.Enums;
using Pos_System.API.Payload.Request.Orders;
using Pos_System.API.Services.Interfaces;
using Pos_System.API.Utils;
using Pos_System.Domain.Models;
using Pos_System.Repository.Interfaces;

namespace Pos_System.API.Services.Implements
{
    public class OrderService : BaseService<OrderService>, IOrderService
    {
        public OrderService(IUnitOfWork<PosSystemContext> unitOfWork, ILogger<OrderService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Guid> CreateNewOrder(Guid storeId, CreateNewOrderRequest createNewOrderRequest)
        {
            if (storeId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Store.EmptyStoreIdMessage);
            Store store = await _unitOfWork.GetRepository<Store>().SingleOrDefaultAsync(predicate: x => x.Id.Equals(storeId));
            if (store == null) throw new BadHttpRequestException(MessageConstant.Store.StoreNotFoundMessage);

            string currentUserName = GetUsernameFromJwt();
            DateTime currentTime = DateTime.Now;
            string currentTimeStamp = TimeUtils.GetTimestamp(currentTime);
            Account currentUser = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(predicate: x => x.Username.Equals(currentUserName));
            Session currentUserSession = await _unitOfWork.GetRepository<Session>().SingleOrDefaultAsync(predicate: x => 
                x.AccountId.Equals(currentUser.Id)
                && x.StoreId.Equals(storeId) 
                /*&& int.Parse(TimeUtils.GetTimestamp(x.StartDateTime)) < int.Parse(currentTimeStamp)
                && int.Parse(TimeUtils.GetTimestamp(x.EndDateTime)) > int.Parse(currentTimeStamp)*/);

            if (currentUserSession == null) throw new BadHttpRequestException(MessageConstant.Order.UserNotInSessionMessage);
            if (createNewOrderRequest.ProductsList.Count() < 1) throw new BadHttpRequestException(MessageConstant.Order.NoProductsInOrderMessage);

            string newInvoiceId = store.Code + currentTimeStamp;
            double SystemDiscountAmount = 0;
            int defaultGuest = 1;

            Order newOrder = new Order()
            {
                Id = Guid.NewGuid(),
                CheckInPerson = currentUser.Id,
                CheckInDate = currentTime,
                CheckOutDate = currentTime.AddMinutes(1),
                InvoiceId = newInvoiceId,
                TotalAmount = createNewOrderRequest.TotalAmount,
                Discount = createNewOrderRequest.DiscountAmount,
                FinalAmount = createNewOrderRequest.FinalAmount,
                Vat = createNewOrderRequest.VAT,
                Vatamount = createNewOrderRequest.VATAmount,
                OrderType = createNewOrderRequest.OrderType.GetDescriptionFromEnum(),
                NumberOfGuest = defaultGuest,
                Status = OrderStatus.PENDING.GetDescriptionFromEnum(),
                SessionId = currentUserSession.Id
            };

            List<OrderDetail> orderDetails = new List<OrderDetail>();
            createNewOrderRequest.ProductsList.ForEach(product =>
            {
                double totalProductAmount = product.SellingPrice * product.Quantity;
                double finalProductAmount = totalProductAmount - SystemDiscountAmount;
                orderDetails.Add(new OrderDetail()
                {
                    Id = Guid.NewGuid(),
                    MenuProductId = product.ProductInMenuId,
                    OrderId = newOrder.Id,
                    Quantity = product.Quantity,
                    SellingPrice = product.SellingPrice,
                    TotalAmount = totalProductAmount,
                    Discount = SystemDiscountAmount,
                    FinalAmount = finalProductAmount,
                    Notes = product.Note
                });
                if (product.Extras.Count > 0)
                {
                    product.Extras.ForEach(extra =>
                    {
                        double totalProductExtraAmount = extra.SellingPrice * extra.Quantity;
                        double finalProductExtraAmount = totalProductExtraAmount - SystemDiscountAmount;
                        orderDetails.Add(new OrderDetail()
                        {
                            Id = Guid.NewGuid(),
                            MenuProductId = extra.ProductInMenuId,
                            OrderId = newOrder.Id,
                            Quantity = extra.Quantity,
                            SellingPrice = extra.SellingPrice,
                            TotalAmount = totalProductExtraAmount,
                            Discount = SystemDiscountAmount,
                            FinalAmount = finalProductExtraAmount
                        });
                    });
                }
            });

            currentUserSession.NumberOfOrders++;
            currentUserSession.TotalAmount += newOrder.TotalAmount;
            currentUserSession.TotalFinalAmount += newOrder.FinalAmount;
            currentUserSession.TotalChangeCash -= newOrder.FinalAmount;
            currentUserSession.TotalDiscountAmount += newOrder.Discount;

            PaymentType paymentType = await _unitOfWork.GetRepository<PaymentType>().SingleOrDefaultAsync(predicate: x => 
                x.Name.Equals(createNewOrderRequest.Payment.GetDescriptionFromEnum()));

            if (paymentType == null) throw new BadHttpRequestException("Payment not found!");

            Payment newPaymentRequest = new Payment()
            {
                Id = Guid.NewGuid(),
                OrderId = newOrder.Id,
                Amount = newOrder.FinalAmount,
                PaymentTypeId = paymentType.Id
            };

            await _unitOfWork.GetRepository<Order>().InsertAsync(newOrder);
            await _unitOfWork.GetRepository<OrderDetail>().InsertRangeAsync(orderDetails);
            await _unitOfWork.GetRepository<Payment>().InsertAsync(newPaymentRequest);
            /*_unitOfWork.GetRepository<Session>().UpdateAsync(currentUserSession);*/
            await _unitOfWork.CommitAsync();

            return storeId;
        }
    }
}
