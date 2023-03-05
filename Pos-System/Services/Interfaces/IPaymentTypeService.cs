using Pos_System.API.Payload.Response.PaymentTypes;
using Pos_System.Domain.Models;

namespace Pos_System.API.Services.Interfaces;

public interface IPaymentTypeService
{
	Task<IEnumerable<GetPaymentTypeDetailResponse>> GetAllPaymentTypesByBrandId();
}