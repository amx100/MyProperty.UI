using Contract;

namespace Services
{
    public interface IPropertyService
	{
		Task<ObservableCollection<PropertyDto>> GetAll(CancellationToken cancellationToken = default);
		Task<PropertyDto> GetById(int propertyId, CancellationToken cancellationToken = default);
		Task<GeneralResponseDto> Create(PropertyCreateDto propertyDto, CancellationToken cancellationToken = default);
		Task<GeneralResponseDto> Update(int propertyId, PropertyUpdateDto propertyDto, CancellationToken cancellationToken = default);
		Task<GeneralResponseDto> Delete(int propertyId, CancellationToken cancellationToken = default);
	}
}


