using AutoMapper;
using Pos_System.API.Constants;
using Pos_System.API.Enums;
using Pos_System.API.Payload.Request.Categories;
using Pos_System.API.Payload.Response.Categories;
using Pos_System.API.Services.Interfaces;
using Pos_System.API.Utils;
using Pos_System.Domain.Models;
using Pos_System.Domain.Paginate;
using Pos_System.Repository.Interfaces;

namespace Pos_System.API.Services.Implements;

public class CategoryService : BaseService<CategoryService>, ICategoryService
{
	public CategoryService(IUnitOfWork<PosSystemContext> unitOfWork, ILogger<CategoryService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
	{
	}


	public async Task<bool> CreateNewCategoryRequest(CreateNewCategoryRequest request)
	{
		_logger.LogInformation($"Start create new category: {request}");
		Guid brandId = Guid.Parse(GetBrandIdFromJwt());
		if (brandId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Brand.EmptyBrandIdMessage);
		Brand brand = await _unitOfWork.GetRepository<Brand>().SingleOrDefaultAsync(
			predicate: x => x.Id.Equals(brandId));
		if (brand == null) throw new BadHttpRequestException(MessageConstant.Brand.BrandNotFoundMessage);
		Category newCategory = new Category()
		{
			Id = Guid.NewGuid(),
			Name = request.Name.Trim(),
			Code = request.Code.Trim(),
			Type = EnumUtil.GetDescriptionFromEnum(request.CategoryType),
			DisplayOrder = request.DisplayOrder,
			Status = EnumUtil.GetDescriptionFromEnum(CategoryStatus.Active),
			PicUrl = request?.PicUrl,
			Description = request.Description.Trim(),
			BrandId = brandId
		};
		await _unitOfWork.GetRepository<Category>().InsertAsync(newCategory);
		bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
		return isSuccessful;
	}

	public async Task<IPaginate<GetCategoryResponse>> GetCategories(string? name, int page, int size)
	{
		Guid brandId = Guid.Parse(GetBrandIdFromJwt());
		_logger.LogInformation($"Get Categories from Brand: {brandId}");
		name = name?.Trim();
		IPaginate<GetCategoryResponse> categoryResponse =
			await _unitOfWork.GetRepository<Category>().GetPagingListAsync(
				selector: x => new GetCategoryResponse(x.Id, x.Code, x.Name, x.Type, x.DisplayOrder, x.Description,
					x.Status, x.BrandId.Value),
				predicate: string.IsNullOrEmpty(name)
					? x => x.BrandId.Equals(brandId)
					: x => x.BrandId.Equals(brandId) && x.Name.Contains(name),
				orderBy: x => x.OrderByDescending(x => x.DisplayOrder),
				page: page,
				size: size
				);
		return categoryResponse;
	}

	public async Task<GetCategoryResponse> GetCategoryById(Guid id)
	{
		if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Category.EmptyCategoryIdMessage);
		Guid brandId = Guid.Parse(GetBrandIdFromJwt());
		GetCategoryResponse categoryResponse = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
		selector: x => new GetCategoryResponse(x.Id, x.Code, x.Name, x.Type, x.DisplayOrder, x.Description, x.Status, x.BrandId.Value),
		predicate: x => x.Id.Equals(id) && x.BrandId.Equals(brandId)
		);
		return categoryResponse;
	}
}