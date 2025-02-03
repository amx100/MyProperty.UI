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
				// First, try to get the property to check if it has images
				var property = await GetById(propertyId, cancellationToken);
				if (property != null && property.Images.Any())
				{
					// Delete all images associated with the property
					foreach (var image in property.Images)
					{
						try
						{
							await apiService.Delete($"{ApiEndpoints.PropertyImageController}/{propertyId}/images/delete/{image.Id}");
						}
						catch (Exception ex)
						{
							Console.WriteLine($"Error deleting image {image.Id}: {ex.Message}");
							// Continue with other images even if one fails
						}
					}
				}

				// Now delete the property
				var response = await apiService.Delete($"{ApiEndpoints.PropertyController}/delete/{propertyId}");
				var content = await response.Content.ReadAsStringAsync(cancellationToken);
				
				if (!response.IsSuccessStatusCode)
				{
					Console.WriteLine($"Delete failed with status code: {response.StatusCode}");
					Console.WriteLine($"Response content: {content}");
					
					return new GeneralResponseDto 
					{ 
						IsSuccess = false,
						Message = !string.IsNullOrEmpty(content) 
							? content 
							: $"Failed to delete property. Status code: {response.StatusCode}"
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
					Message = "Property and associated images successfully deleted"
				};
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine($"Network error during delete: {ex.Message}");
				return new GeneralResponseDto 
				{ 
					IsSuccess = false,
					Message = $"Network error: {ex.Message}"
				};
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Unexpected error during delete: {ex.Message}");
				return new GeneralResponseDto 
				{ 
					IsSuccess = false,
					Message = "An unexpected error occurred while deleting the property"
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
				var response = await apiService.Get($"{ApiEndpoints.PropertyController}/details/{propertyId}");
				if (!response.IsSuccessStatusCode) return null!;
				await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
				var res = await JsonSerializer.DeserializeAsync<PropertyDto>(responseStream, _options,
					cancellationToken);
				return res ?? null!;
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine(ex.Message);
				return null!;
			}
		}

	}

}

//test