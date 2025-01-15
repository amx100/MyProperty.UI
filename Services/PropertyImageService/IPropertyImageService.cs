using Contract;

namespace Services;

public interface IPropertyImageService
{
	Task<ObservableCollection<PropertyImageDto>> GetAll(int propertyId, CancellationToken cancellationToken = default);
	Task<PropertyImageDto> GetById(int propertyId, int imageId, CancellationToken cancellationToken = default);
	Task<GeneralResponseDto> Create(int propertyId, PropertyImageCreateDto imageDto, CancellationToken cancellationToken = default);
	Task<GeneralResponseDto> Update(int propertyId, int imageId, PropertyImageUpdateDto imageDto, CancellationToken cancellationToken = default);
	Task<GeneralResponseDto> Delete(int imageId, CancellationToken cancellationToken = default);

}