using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pos_System.API.Constants;
using Pos_System.API.Enums;
using Pos_System.API.Extensions;
using Pos_System.API.Payload.Response.Orders;
using Pos_System.API.Payload.Response.PaymentTypes;
using Pos_System.API.Payload.Response.Reports;
using Pos_System.API.Services.Interfaces;
using Pos_System.API.Utils;
using Pos_System.Domain.Models;
using Pos_System.Domain.Paginate;
using Pos_System.Repository.Interfaces;
using System.Linq;
using System.Linq.Expressions;

namespace Pos_System.API.Services.Implements
{
    public class ReportService : BaseService<ReportService>, IReportService
    {
        private readonly IMenuService _menuService;
        public ReportService(IUnitOfWork<PosSystemContext> unitOfWork, ILogger<ReportService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IMenuService menuService) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _menuService = menuService;
        }

        public async Task<StoreReportResponse> GetStoreReport(Guid storeId, DateTime? startDate, DateTime? endDate)
        {
            RoleEnum userRole = EnumUtil.ParseEnum<RoleEnum>(GetRoleFromJwt());
            if (storeId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Store.EmptyStoreIdMessage);
            Guid currentUserStoreId = Guid.Parse(GetStoreIdFromJwt());
            if (currentUserStoreId != storeId) throw new BadHttpRequestException(MessageConstant.Store.GetStoreOrdersUnAuthorized);
            Store store = await _unitOfWork.GetRepository<Store>().SingleOrDefaultAsync(predicate: x => x.Id.Equals(currentUserStoreId));
            if (store == null) throw new BadHttpRequestException(MessageConstant.Store.StoreNotFoundMessage);
            Guid brandId = store.BrandId;
            StoreReportResponse storeReportResponse = new StoreReportResponse(0.0, 0.0, 0.0, null, null, null, null, null, null, null);
            var orderList = await _unitOfWork.GetRepository<Order>().GetListAsync<Order>(
                   selector: x => new Order(),
                  predicate: BuildGetOrdersInStoreQuery(storeId, startDate, endDate)
                  );

            //Query by order status
            storeReportResponse.listOrderStatus.Add("Tất cả");
            storeReportResponse.listOrderStatus.Add("Đã thanh toán");
            storeReportResponse.listOrderStatus.Add("Đã hủy");
            storeReportResponse.listOrderStatus.Add("Đợi thanh toán");
            storeReportResponse.TotalOrderByStatus.Add(orderList.Count());
            storeReportResponse.TotalOrderByStatus.Add(orderList.Where(f => f.Status.Equals(OrderStatus.PAID.GetDescriptionFromEnum())).Count());
            storeReportResponse.TotalOrderByStatus.Add(orderList.Where(f => f.Status.Equals(OrderStatus.CANCELED.GetDescriptionFromEnum())).Count());
            storeReportResponse.TotalOrderByStatus.Add(orderList.Where(f => f.Status.Equals(OrderStatus.PENDING.GetDescriptionFromEnum())).Count());


            //Query revenue
            storeReportResponse.BasicRevenue = orderList.Where(f => f.Status.Equals(OrderStatus.PAID.GetDescriptionFromEnum())).Aggregate(0.0, (acc, x) => acc + x.TotalAmount);
            storeReportResponse.TotalDiscount = orderList.Where(f => f.Status.Equals(OrderStatus.PAID.GetDescriptionFromEnum())).Aggregate(0.0, (acc, x) => acc + x.Discount);
            storeReportResponse.TotalFinalRevenue = orderList.Where(f => f.Status.Equals(OrderStatus.PAID.GetDescriptionFromEnum())).Aggregate(0.0, (acc, x) => acc + x.FinalAmount);

            //Query by timeline
            for (int i = 6; i < 24; i++)
            {
                var count = 0;
                double countAmount = 0;
                foreach (var item in orderList)
                {
                    if (i == item.CheckOutDate.Hour && item.Status.Equals(OrderStatus.PAID.GetDescriptionFromEnum()))
                    {
                        count++;
                        countAmount += item.FinalAmount;

                    }
                }
                storeReportResponse.listTimeLine.Add(i.ToString() + "h");
                storeReportResponse.TotalOrderByTimeline.Add(count);
                storeReportResponse.TotalRevenueByTimeline.Add(countAmount);
            }

            //Query by orderType
            storeReportResponse.listOrderType.Add("Tại  Quán");
            storeReportResponse.listOrderType.Add("Mang đi");
            storeReportResponse.listOrderType.Add("Giao hàng");
            storeReportResponse.TotalOrderByType.Add(orderList.Where(f => f.OrderType.Equals(OrderType.EAT_IN.GetDescriptionFromEnum())).Count());
            storeReportResponse.TotalOrderByType.Add(orderList.Where(f => f.OrderType.Equals(OrderType.TAKE_AWAY.GetDescriptionFromEnum())).Count());
            storeReportResponse.TotalOrderByType.Add(orderList.Where(f => f.OrderType.Equals(OrderType.DELIVERY.GetDescriptionFromEnum())).Count());





            return storeReportResponse;
        }

        public async Task<SystemReport> GetSystemReport()
        {

            SystemReport? systemReport = new SystemReport(0, 0, 0, 0);
            var totalBrand = await _unitOfWork.GetRepository<Brand>().GetListAsync(
               predicate: x => x.Status.Equals(BrandStatus.Active.GetDescriptionFromEnum()));
            var totalStore = await _unitOfWork.GetRepository<Store>().GetListAsync(
        predicate: x => x.Status.Equals(StoreStatus.Active.GetDescriptionFromEnum()));
            var totalAccount = await _unitOfWork.GetRepository<Account>().GetListAsync(
        predicate: x => x.Status.Equals(AccountStatus.Active.GetDescriptionFromEnum()));

            var totalPayment = await _unitOfWork.GetRepository<PaymentType>().GetListAsync(
        );
            systemReport.TotalBrand = totalBrand.Count();
            systemReport.TotalStore = totalStore.Count();
            systemReport.TotalAccount = totalAccount.Count();
            systemReport.TotalPaymentMethod = totalPayment.Count();

            return systemReport;
        }



        private Expression<Func<Order, bool>> BuildGetOrdersInStoreQuery(Guid storeId, DateTime? startDate,
             DateTime? endDate)
        {
            Expression<Func<Order, bool>> filterQuery = p => p.Session.StoreId.Equals(storeId);
            if (startDate != null)
            {
                filterQuery = filterQuery.AndAlso(p => p.CheckInDate >= startDate);
            }

            if (endDate != null)
            {
                filterQuery = filterQuery.AndAlso(p => p.CheckInDate <= endDate);
            }

            return filterQuery;
        }
    }
}
