using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pos_System.API.Constants;
using Pos_System.API.Enums;
using Pos_System.API.Payload.Response.Report;
using Pos_System.API.Payload.Response.Stores;
using Pos_System.API.Services.Interfaces;
using Pos_System.API.Utils;
using Pos_System.Domain.Models;
using Pos_System.Domain.Paginate;
using Pos_System.Repository.Interfaces;

namespace Pos_System.API.Services.Implements
{
    public class ReportService : BaseService<ReportService>, IReportService
    {
        private readonly IMenuService _menuService;
        public readonly IOrderService _orderService;
        public ReportService(IUnitOfWork<PosSystemContext> unitOfWork, ILogger<ReportService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IMenuService menuService, IOrderService orderService) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _menuService = menuService;
            _orderService = orderService;
        }

        public async Task<GetStoreEndDayReport> GetStoreEndDayReport(Guid storeId, DateTime? startDate, DateTime? endDate)
        {
            RoleEnum userRole = EnumUtil.ParseEnum<RoleEnum>(GetRoleFromJwt());
            if (storeId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Store.EmptyStoreIdMessage);
            Guid currentUserStoreId = Guid.Parse(GetStoreIdFromJwt());
            if (currentUserStoreId != storeId) throw new BadHttpRequestException(MessageConstant.Store.GetStoreOrdersUnAuthorized);

            Guid userBrandId = await _unitOfWork.GetRepository<Store>()
    .SingleOrDefaultAsync(selector: x => x.BrandId, predicate: x => x.Id.Equals(currentUserStoreId));

            GetStoreEndDayReport report = new GetStoreEndDayReport();

            List<Category> categories = (List<Category>)await _unitOfWork.GetRepository<Category>().GetListAsync(
             predicate: x => x.BrandId.Equals(userBrandId)
         );
            List<Order> orders = (List<Order>)await _unitOfWork.GetRepository<Order>().GetListAsync(
                include: x => x.Include(order => order.OrderDetails).ThenInclude(x => x.MenuProduct).ThenInclude(x => x.Product).Include(x => x.PromotionOrderMappings).Include(order => order.Session),
                predicate: p => p.Session.StoreId.Equals(storeId) && p.CheckInDate >= startDate && p.CheckInDate <= endDate.Value.AddDays(1) && p.Status.Equals(OrderStatus.PAID.GetDescriptionFromEnum()),
                orderBy: x => x.OrderByDescending(x => x.CheckInDate)
                );
            report.StoreId = currentUserStoreId;
            report.TotalProductDiscount = 0;
            report.TotalProduct = 0;
            report.TotalAmountSizeL = 0;
            report.TotalAmountSizeM = 0;
            report.TotalAmountSizeS = 0;
            report.TotalSizeL = 0;
            report.TotalSizeM = 0;
            report.TotalSizeS = 0;
            report.TotalPromotionUsed = 0;
            for (int i = 6; i < 24; i++)
            {
                report.TimeLine.Add(i);
                report.TotalAmountTimeLine.Add(0);
                report.TotalOrderTimeLine.Add(0);

            }
            foreach (var category in categories)
            {
                report.CategoryReports.Add(new CategoryReport(category.Id, category.Name, 0, 0, 0, 0, new List<ProductReport>()));
            }
            foreach (var item in orders)
            {
                report.TotalAmount += item.TotalAmount;
                report.TotalDiscount += item.Discount;
                report.VatAmount += item.Vatamount;
                report.FinalAmount += item.FinalAmount;
                report.TotalOrder++;

                if (item.PromotionOrderMappings.Count() > 0)
                {
                    report.TotalPromotionUsed++;
                }
                if (item.OrderType == OrderType.EAT_IN.GetDescriptionFromEnum())
                {
                    report.InStoreAmount += item.FinalAmount;
                    report.TotalOrderInStore++;
                }
                else if (item.OrderType == OrderType.TAKE_AWAY.GetDescriptionFromEnum())
                {
                    report.TakeAwayAmount += item.FinalAmount;
                    report.TotalOrderTakeAway++;
                }
                else
                {
                    report.DeliAmount += item.FinalAmount;
                    report.TotalOrderDeli++;
                };
                if (item.PaymentType == PaymentTypeEnum.VISA.GetDescriptionFromEnum())
                {
                    report.VisaAmount += item.FinalAmount;
                    report.TotalVisa++;

                }
                else if (item.PaymentType == PaymentTypeEnum.MOMO.GetDescriptionFromEnum())
                {
                    report.MomoAmount += item.FinalAmount;
                    report.TotalMomo++;
                }
                else if (item.PaymentType == PaymentTypeEnum.BANKING.GetDescriptionFromEnum())
                {
                    report.BankingAmount += item.FinalAmount;
                    report.TotalBanking++;
                }
                else
                {
                    report.CashAmount += item.FinalAmount;
                    report.TotalCash++;
                };
                foreach (var cateReport in report.CategoryReports)
                {
                    foreach (var orderDetail in item.OrderDetails)
                    {
                        if (cateReport.Id.Equals(orderDetail.MenuProduct.Product.CategoryId))
                        {
                            var exitProduct = cateReport.ProductReports.FindIndex(element => element.Id.Equals(orderDetail.MenuProduct.ProductId));
                            if (exitProduct == -1)
                            {
                                cateReport.ProductReports.Add(new ProductReport(orderDetail.MenuProduct.Product.Id, orderDetail.MenuProduct.Product.Name, orderDetail.Quantity, orderDetail.TotalAmount, orderDetail.Discount, orderDetail.FinalAmount));
                                cateReport.TotalProduct += orderDetail.Quantity;
                                cateReport.TotalAmount += orderDetail.TotalAmount;
                                cateReport.TotalDiscount += orderDetail.Discount;
                                cateReport.FinalAmount += orderDetail.FinalAmount;
                                report.ProductCosAmount += orderDetail.MenuProduct.Product.HistoricalPrice * orderDetail.Quantity;
                                report.TotalProductDiscount += orderDetail.Discount;
                                report.TotalProduct += orderDetail.Quantity;
                                if (!string.IsNullOrEmpty(orderDetail.MenuProduct.Product.Size))
                                {
                                    if (orderDetail.MenuProduct.Product.Size.Equals(ProductSize.S.GetDescriptionFromEnum()))
                                    {
                                        report.TotalSizeS += orderDetail.Quantity;
                                        report.TotalAmountSizeS += orderDetail.FinalAmount;
                                    }
                                    else if (orderDetail.MenuProduct.Product.Size.Equals(ProductSize.M.GetDescriptionFromEnum()))
                                    {
                                        report.TotalSizeM += orderDetail.Quantity;
                                        report.TotalAmountSizeM += orderDetail.FinalAmount;
                                    }
                                    else
                                    {
                                        report.TotalSizeL += orderDetail.Quantity;
                                        report.TotalAmountSizeL += orderDetail.FinalAmount;
                                    }
                                }
                            }
                            else
                            {
                                cateReport.ProductReports[exitProduct].Quantity += orderDetail.Quantity;
                                cateReport.ProductReports[exitProduct].TotalAmount += orderDetail.TotalAmount;
                                cateReport.ProductReports[exitProduct].TotalDiscount += orderDetail.Discount;
                                cateReport.ProductReports[exitProduct].FinalAmount += orderDetail.FinalAmount;
                                cateReport.TotalProduct += orderDetail.Quantity;
                                cateReport.TotalAmount += orderDetail.TotalAmount;
                                cateReport.TotalDiscount += orderDetail.Discount;
                                cateReport.FinalAmount += orderDetail.FinalAmount;
                                report.ProductCosAmount += orderDetail.MenuProduct.Product.HistoricalPrice * orderDetail.Quantity;
                                report.TotalProductDiscount += orderDetail.Discount;
                                report.TotalProduct += orderDetail.Quantity;
                                if (!string.IsNullOrEmpty(orderDetail.MenuProduct.Product.Size))
                                {
                                    if (orderDetail.MenuProduct.Product.Size.Equals(ProductSize.S.GetDescriptionFromEnum()))
                                    {
                                        report.TotalSizeS += orderDetail.Quantity;
                                        report.TotalAmountSizeS += orderDetail.FinalAmount;
                                    }
                                    else if (orderDetail.MenuProduct.Product.Size.Equals(ProductSize.M.GetDescriptionFromEnum()))
                                    {
                                        report.TotalSizeM += orderDetail.Quantity;
                                        report.TotalAmountSizeM += orderDetail.FinalAmount;
                                    }
                                    else
                                    {
                                        report.TotalSizeL += orderDetail.Quantity;
                                        report.TotalAmountSizeL += orderDetail.FinalAmount;
                                    }
                                }
                            }

                        }

                    }
                }
                for (int i = 6; i < 24; i++)
                {
                    if (i == item.CheckOutDate.Hour)
                    {
                        report.TotalOrderTimeLine[i - 6]++;
                        report.TotalAmountTimeLine[i - 6] += item.FinalAmount;

                    }
                }
            }
            report.TotalPromotionDiscount = report.TotalDiscount - report.TotalProductDiscount;
            if (report.TotalOrder == 0)
            {
                report.AverageBill = 0;
            }
            else
            {
                report.AverageBill = report.FinalAmount / report.TotalOrder;
            }
            report.TotalRevenue = report.FinalAmount - report.ProductCosAmount;
            return report;
        }

        public async Task<SessionReport> GetSessionReportDetail(Guid sessionId)
        {
            Guid userStoreId = Guid.Parse(GetStoreIdFromJwt());
            if (sessionId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Session.EmptySessionIdMessage);

            Session session = await _unitOfWork.GetRepository<Session>()
                .SingleOrDefaultAsync(predicate: x => x.StoreId.Equals(userStoreId) && x.Id.Equals(sessionId));
            if (session == null) throw new BadHttpRequestException(MessageConstant.Session.SessionNotFoundMessage);


            List<Order> orders = (List<Order>)await _unitOfWork.GetRepository<Order>().GetListAsync(
               include: x => x.Include(order => order.Session),
               predicate: p => p.SessionId.Equals(sessionId) && p.Status.Equals(OrderStatus.PAID.GetDescriptionFromEnum())
               );
            SessionReport report = new SessionReport(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            foreach (var item in orders)
            {
                report.TotalAmount += item.TotalAmount;
                report.TotalDiscount += item.Discount;
                report.FinalAmount += item.FinalAmount;
                report.TotalOrder++;
                if (item.PaymentType == PaymentTypeEnum.VISA.GetDescriptionFromEnum())
                {
                    report.VisaAmount += item.FinalAmount;
                    report.TotalVisa++;

                }
                else if (item.PaymentType == PaymentTypeEnum.MOMO.GetDescriptionFromEnum())
                {
                    report.MomoAmount += item.FinalAmount;
                    report.TotalMomo++;
                }
                else if (item.PaymentType == PaymentTypeEnum.BANKING.GetDescriptionFromEnum())
                {
                    report.BankingAmount += item.FinalAmount;
                    report.TotalBanking++;
                }
                else
                {
                    report.CashAmount += item.FinalAmount;
                    report.TotalCash++;
                }
            }
            return report;

        }


    }
}

