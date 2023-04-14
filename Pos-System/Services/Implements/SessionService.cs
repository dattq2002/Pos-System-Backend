using AutoMapper;
using Pos_System.API.Constants;
using Pos_System.API.Extensions;
using Pos_System.API.Payload.Request.Sessions;
using Pos_System.API.Payload.Response.Sessions;
using Pos_System.API.Services.Interfaces;
using Pos_System.API.Utils;
using Pos_System.Domain.Models;
using Pos_System.Domain.Paginate;
using Pos_System.Repository.Interfaces;
using System.Linq.Expressions;

namespace Pos_System.API.Services.Implements
{
    public class SessionService : BaseService<SessionService>, ISessionService
    {
        public SessionService(IUnitOfWork<PosSystemContext> unitOfWork, ILogger<SessionService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<bool> CreateStoreSessions(Guid storeId, CreateStoreSessionsRequest createStoreSessionsRequest)
        {
            Guid userStoreId = Guid.Parse(GetStoreIdFromJwt());
            if (userStoreId != storeId) throw new BadHttpRequestException(MessageConstant.Store.CreateStoreSessionUnAuthorized);
            _logger.LogInformation($"Start create new store session with storeID: {storeId}");
            if (storeId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Store.EmptyStoreIdMessage);
            Store store = await _unitOfWork.GetRepository<Store>().SingleOrDefaultAsync(predicate: x => x.Id.Equals(storeId));
            if (store == null) throw new BadHttpRequestException(MessageConstant.Store.StoreNotFoundMessage);

            List<Session> sessionsToInsert = new List<Session>();
            if(createStoreSessionsRequest.Sessions.Count > 0)
            {
                createStoreSessionsRequest.Sessions.ForEach(session =>
                {
                    sessionsToInsert.Add(new Session
                    {
                        Id = Guid.NewGuid(),
                        StartDateTime = session.startTime,
                        EndDateTime = session.endTime,
                        NumberOfOrders = 0,
                        TotalAmount = 0,
                        TotalPromotion = 0,
                        TotalChangeCash = 0,
                        TotalDiscountAmount = 0,
                        TotalFinalAmount = 0,
                        StoreId = storeId,
                        Name = String.IsNullOrEmpty(session.Name) ? 
                            "Ca " + TimeUtils.GetHoursTime(session.startTime) + " - " + TimeUtils.GetHoursTime(session.endTime) 
                            : session.Name
                    });
                });
            }

            await _unitOfWork.GetRepository<Session>().InsertRangeAsync(sessionsToInsert);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<IPaginate<GetStoreSessionListResponse>> GetStoreSessions(Guid storeId, int page, int size, DateTime? startTime, DateTime? endTime)
        {
            Guid userStoreId = Guid.Parse(GetStoreIdFromJwt());
            if (userStoreId != storeId) throw new BadHttpRequestException(MessageConstant.Store.GetStoreSessionUnAuthorized);
            if (storeId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Store.EmptyStoreIdMessage);
            Store store = await _unitOfWork.GetRepository<Store>().SingleOrDefaultAsync(predicate: x => x.Id.Equals(storeId));
            if (store == null) throw new BadHttpRequestException(MessageConstant.Store.StoreNotFoundMessage);

            IPaginate<GetStoreSessionListResponse> sessionsInStore = await _unitOfWork.GetRepository<Session>().GetPagingListAsync(
                selector: x => new GetStoreSessionListResponse
                {
                    Id = x.Id,
                    StartDateTime = x.StartDateTime,
                    EndDateTime = x.EndDateTime,
                    Name = x.Name,
                    NumberOfOrders = x.NumberOfOrders,
                    CurrentCashInVault = x.TotalChangeCash ?? 0,
                    TotalFinalAmount = x.TotalFinalAmount ?? 0
                },
                predicate: BuildGetSessionsOfStoreQuery(storeId, startTime, endTime),
                orderBy: x => x.OrderByDescending(x => x.EndDateTime),
                page: page,
                size: size
                );

            return sessionsInStore;
        }

        public async Task<Guid> UpdateStoreSession(Guid storeId, Guid sessionId, UpdateStoreSessionRequest updateStoreSessionRequest)
        {
            Guid userStoreId = Guid.Parse(GetStoreIdFromJwt());
            if (userStoreId != storeId) throw new BadHttpRequestException(MessageConstant.Store.CreateStoreSessionUnAuthorized);
            _logger.LogInformation($"Start update store session with storeID: {storeId}");
            if (storeId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Store.EmptyStoreIdMessage);
            Store store = await _unitOfWork.GetRepository<Store>().SingleOrDefaultAsync(predicate: x => x.Id.Equals(storeId));
            if (store == null) throw new BadHttpRequestException(MessageConstant.Store.StoreNotFoundMessage);

            Session sessionToUpdate = await _unitOfWork.GetRepository<Session>().SingleOrDefaultAsync(predicate: x => x.Id.Equals(sessionId));
            if (sessionToUpdate == null) throw new BadHttpRequestException(MessageConstant.Session.SessionNotFoundMessage);

            sessionToUpdate.StartDateTime = updateStoreSessionRequest.startTime;
            sessionToUpdate.EndDateTime = updateStoreSessionRequest.endTime;
            sessionToUpdate.Name = updateStoreSessionRequest.Name ?? 
                "Ca " + TimeUtils.GetHoursTime(sessionToUpdate.StartDateTime) + " - " + TimeUtils.GetHoursTime(sessionToUpdate.EndDateTime);
            if (updateStoreSessionRequest.InitCashInVault != null)
            {
                double currentInitCashInVault = (double)(sessionToUpdate.TotalChangeCash - sessionToUpdate.TotalFinalAmount);
                sessionToUpdate.TotalChangeCash = sessionToUpdate.TotalChangeCash - currentInitCashInVault + updateStoreSessionRequest.InitCashInVault;
            }

            _unitOfWork.GetRepository<Session>().UpdateAsync(sessionToUpdate);
            await _unitOfWork.CommitAsync();
            return sessionId;
        }

        private Expression<Func<Session, bool>> BuildGetSessionsOfStoreQuery(Guid storeId, DateTime? startDate,
           DateTime? endDate)
        {
            Expression<Func<Session, bool>> filterQuery = p => p.StoreId.Equals(storeId);
            if (startDate != null)
            {
                filterQuery = filterQuery.AndAlso(p => p.StartDateTime >= startDate);
            }

            if (endDate != null)
            {
                filterQuery = filterQuery.AndAlso(p => p.EndDateTime <= endDate);
            }
            return filterQuery;
        }
    }
}
