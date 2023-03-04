using AutoMapper;
using Pos_System.API.Constants;
using Pos_System.API.Payload.Response.PaymentTypes;
using Pos_System.API.Services.Interfaces;
using Pos_System.Domain.Models;
using Pos_System.Repository.Interfaces;

namespace Pos_System.API.Services.Implements;

public class PaymentTypeService : BaseService<PaymentTypeService>, IPaymentTypeService
{
	public PaymentTypeService(IUnitOfWork<PosSystemContext> unitOfWork, ILogger<PaymentTypeService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
	{
	}

	public async Task<IEnumerable<GetPaymentTypeDetailResponse>> GetAllPaymentTypesByBrandId()
	{
		Guid storeId = Guid.Parse(GetStoreIdFromJwt());
		Store store = await _unitOfWork.GetRepository<Store>().SingleOrDefaultAsync(predicate: x => x.Id.Equals(storeId));
		if (store == null) throw new BadHttpRequestException(MessageConstant.Store.StoreNotFoundMessage);
		Guid brandId = store.BrandId;
		IEnumerable<GetPaymentTypeDetailResponse> paymentTypeDetailResponses =
			await _unitOfWork.GetRepository<PaymentType>().GetListAsync(
				selector: x => new GetPaymentTypeDetailResponse(x.Id, x.Name, x.PicUrl, x.IsDisplay, x.Position, x.BrandId),
				predicate: x => x.BrandId.Equals(brandId)
				);
		return paymentTypeDetailResponses;
	}
}