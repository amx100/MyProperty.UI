using Contract;

namespace Services
{
    public class PropertyImageService(IApiService apiService) : IPropertyImageService
	{
	

		private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

		
		public async Task<GeneralResponseDto> Create(int propertyId, PropertyImageCreateDto imageDto, CancellationToken cancellationToken = default)
		{
			try
			{
				imageDto.PropertyId = propertyId;
				
				var url = $"{ApiEndpoints.PropertyImageController}/{propertyId}/images/upload";
				Console.WriteLine($"Creating image for property {propertyId}");
				Console.WriteLine($"Request URL: {url}");
				Console.WriteLine($"Image URL length: {imageDto.ImageUrl?.Length ?? 0}");

				var response = await apiService.Post(url, imageDto);
				
				if (!response.IsSuccessStatusCode)
				{
					var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
					Console.WriteLine($"Error creating image. Status: {response.StatusCode}");
					Console.WriteLine($"Response content: {errorContent}");
					return new GeneralResponseDto 
					{ 
						IsSuccess = false, 
						Message = $"Failed to create image. Status: {response.StatusCode}. Details: {errorContent}" 
					};
				}

				await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
				var result = await JsonSerializer.DeserializeAsync<GeneralResponseDto>(responseStream, _options, cancellationToken);
				
				return result ?? new GeneralResponseDto 
				{ 
					IsSuccess = false, 
					Message = "Failed to deserialize response" 
				};
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine($"Network error creating image: {ex.Message}");
				return new GeneralResponseDto 
				{ 
					IsSuccess = false, 
					Message = $"Network error: {ex.Message}" 
				};
			}
		}

		public async Task<GeneralResponseDto> Update(int propertyId, int imageId, PropertyImageUpdateDto imageDto, CancellationToken cancellationToken = default)
		{
			try
			{
				var response = await apiService.Put($"{ApiEndpoints.PropertyImageController}/{propertyId}/images/update/{imageId}", imageDto);
				if (!response.IsSuccessStatusCode) return null!;
				await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
				var res = await JsonSerializer.DeserializeAsync<GeneralResponseDto>(responseStream, _options, cancellationToken);
				return res ?? null!;
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine(ex.Message);
				return null!;
			}
		}
		public async Task<GeneralResponseDto> Delete(int propertyImageId, CancellationToken cancellationToken = default)
		{
			try
			{
				var response = await apiService.Delete($"{ApiEndpoints.PropertyImageController}/images/delete/{propertyImageId}");
				var content = await response.Content.ReadAsStringAsync(cancellationToken);
				
				if (!response.IsSuccessStatusCode)
				{
					Console.WriteLine($"Delete image failed with status code: {response.StatusCode}");
					Console.WriteLine($"Response content: {content}");
					
					return new GeneralResponseDto 
					{ 
						IsSuccess = false,
						Message = !string.IsNullOrEmpty(content) 
							? content 
							: $"Failed to delete image. Status code: {response.StatusCode}"
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
					Message = "Image successfully deleted"
				};
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine($"Network error during image delete: {ex.Message}");
				return new GeneralResponseDto 
				{ 
					IsSuccess = false,
					Message = $"Network error: {ex.Message}"
				};
			}
		}

		public async Task<ObservableCollection<PropertyImageDto>> GetAll(int propertyId, CancellationToken cancellationToken = default)
		{
			try
			{
				var response = await apiService.Get($"{ApiEndpoints.PropertyImageController}/{propertyId}/images");
				if (!response.IsSuccessStatusCode) return null!;
				await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
				var res = await JsonSerializer.DeserializeAsync<ObservableCollection<PropertyImageDto>>(responseStream, _options, cancellationToken);
				return res ?? null!;
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine(ex.Message);
				return null!;
			}
		}

		public async Task<PropertyImageDto> GetById(int propertyId, int imageId, CancellationToken cancellationToken = default)
		{
			try
			{
				var response = await apiService.Get($"{ApiEndpoints.PropertyImageController}/{propertyId}/images/{imageId}");
				if (!response.IsSuccessStatusCode) return null!;
				await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
				var res = await JsonSerializer.DeserializeAsync<PropertyImageDto>(responseStream, _options,
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
