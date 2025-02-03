using Contract;

namespace Services
{
    public class PropertyService(IApiService apiService) : IPropertyService
	{
		private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

		public async Task<GeneralResponseDto> Create(PropertyCreateDto propertyDto, CancellationToken cancellationToken = default)
		{
			try
			{
				var response = await apiService.Post($"{ApiEndpoints.PropertyController}/create", propertyDto);
				var content = await response.Content.ReadAsStringAsync(cancellationToken);
				
				if (!response.IsSuccessStatusCode)
				{
					Console.WriteLine($"Create property failed with status code: {response.StatusCode}");
					Console.WriteLine($"Response content: {content}");
					
					return new GeneralResponseDto 
					{ 
						IsSuccess = false,
						Message = !string.IsNullOrEmpty(content) 
							? content 
							: $"Failed to create property. Status code: {response.StatusCode}"
					};
				}

				try 
				{
					var result = JsonSerializer.Deserialize<GeneralResponseDto>(content, _options);
					if (result != null)
					{
						return result;
					}
				}
				catch (JsonException ex)
				{
					Console.WriteLine($"Failed to deserialize response: {ex.Message}");
				}

				return new GeneralResponseDto 
				{ 
					IsSuccess = true,
					Message = "Property successfully created"
				};
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine($"Network error during create: {ex.Message}");
				return new GeneralResponseDto 
				{ 
					IsSuccess = false,
					Message = $"Network error: {ex.Message}"
				};
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Unexpected error during create: {ex.Message}");
				return new GeneralResponseDto 
				{ 
					IsSuccess = false,
					Message = "An unexpected error occurred while creating the property"
				};
			}
		}

		public async Task<GeneralResponseDto> Delete(int propertyId, CancellationToken cancellationToken = default)
		{
			try
			{
				Console.WriteLine($"Attempting to delete property with ID: {propertyId}");
				
				// Direct deletion without checking existence
				var deleteResponse = await apiService.Delete($"{ApiEndpoints.PropertyController}/delete/{propertyId}");
				var deleteContent = await deleteResponse.Content.ReadAsStringAsync(cancellationToken);
				
				Console.WriteLine($"Delete response status: {deleteResponse.StatusCode}");
				Console.WriteLine($"Delete response content: {deleteContent}");

				if (!deleteResponse.IsSuccessStatusCode)
				{
					try
					{
						var errorResponse = JsonSerializer.Deserialize<GeneralResponseDto>(deleteContent, _options);
						return new GeneralResponseDto 
						{ 
							IsSuccess = false,
							Message = errorResponse?.Message ?? $"Failed to delete property. Status: {deleteResponse.StatusCode}"
						};
					}
					catch
					{
						return new GeneralResponseDto 
						{ 
							IsSuccess = false,
							Message = $"Failed to delete property. Status: {deleteResponse.StatusCode}. {deleteContent}"
						};
					}
				}

				return new GeneralResponseDto 
				{ 
					IsSuccess = true,
					Message = "Property successfully deleted"
				};
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine($"Network error in Delete: {ex.Message}");
				return new GeneralResponseDto 
				{ 
					IsSuccess = false,
					Message = $"Network error while deleting property: {ex.Message}"
				};
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Unexpected error in Delete: {ex.Message}");
				Console.WriteLine($"Stack trace: {ex.StackTrace}");
				return new GeneralResponseDto 
				{ 
					IsSuccess = false,
					Message = $"An unexpected error occurred while deleting the property: {ex.Message}"
				};
			}
		}

		public async Task<GeneralResponseDto> Update(int propertyId, PropertyUpdateDto propertyDto, CancellationToken cancellationToken = default)
		{
			try
			{
				var response = await apiService.Put($"{ApiEndpoints.PropertyController}/update/{propertyId}", propertyDto);
				if (!response.IsSuccessStatusCode) return null!;
				await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
				var res = await JsonSerializer.DeserializeAsync<GeneralResponseDto>(responseStream, _options,
					cancellationToken);
				return res ?? null!;
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine(ex.Message);
				return null!;
			}
		}


		public async Task<ObservableCollection<PropertyDto>> GetAll(CancellationToken cancellationToken = default)
		{
			try
			{
				var response = await apiService.Get($"{ApiEndpoints.PropertyController}");
				if (!response.IsSuccessStatusCode) return null!;
				await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
				var res = await JsonSerializer.DeserializeAsync<ObservableCollection<PropertyDto>>(responseStream, _options,
					cancellationToken);
				return res ?? null!;
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine(ex.Message);
				return null!;
			}
		}

		public async Task<PropertyDto> GetById(int propertyId, CancellationToken cancellationToken = default)
		{
			try
			{
				Console.WriteLine($"GetById - Attempting to get property with ID: {propertyId}");
				var response = await apiService.Get($"{ApiEndpoints.PropertyController}/details/{propertyId}");
				var content = await response.Content.ReadAsStringAsync(cancellationToken);
				
				Console.WriteLine($"GetById - Response status: {response.StatusCode}");
				Console.WriteLine($"GetById - Response content: {content}");

				if (!response.IsSuccessStatusCode)
				{
					Console.WriteLine($"GetById - Failed to get property. Status: {response.StatusCode}");
					return null!;
				}

				try
				{
					var property = JsonSerializer.Deserialize<PropertyDto>(content, _options);
					if (property != null)
					{
						Console.WriteLine($"GetById - Successfully deserialized property with ID: {property.PropertyId}");
						return property;
					}
				}
				catch (JsonException ex)
				{
					Console.WriteLine($"GetById - Failed to deserialize response: {ex.Message}");
					Console.WriteLine($"GetById - Content that failed to deserialize: {content}");
				}

				return null!;
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine($"GetById - Network error: {ex.Message}");
				return null!;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"GetById - Unexpected error: {ex.Message}");
				return null!;
			}
		}

	}

}

//test