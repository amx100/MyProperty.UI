using Contract;

namespace Services.ReservationService
{
    public class ReservationService(IApiService apiService) : IReservationService
	{
		private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

		public async Task<GeneralResponseDto> Create(ReservationCreateDto reservationDto, CancellationToken cancellationToken = default)
		{
			try
			{
				var response = await apiService.Post($"{ApiEndpoints.ReservationController}/create", reservationDto);
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

		public async Task<GeneralResponseDto> Delete(int reservationId, CancellationToken cancellationToken = default)
		{
			try
			{
				var response = await apiService.Delete($"{ApiEndpoints.ReservationController}/{reservationId}");
				return response.IsSuccessStatusCode
					? new GeneralResponseDto { IsSuccess = true }
					: new GeneralResponseDto { IsSuccess = false };
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine(ex.Message);
				return new GeneralResponseDto { IsSuccess = false };
			}
		}

		public async Task<GeneralResponseDto> Update(int id, ReservationUpdateDto updateDto, CancellationToken cancellationToken = default)
		{
			try
			{
				var url = $"{ApiEndpoints.ReservationController}/update/{id}";
				
				Console.WriteLine("=== Starting Reservation Update ===");
				Console.WriteLine($"URL: {url}");
				Console.WriteLine($"Request Data: {JsonSerializer.Serialize(updateDto)}");

				var response = await apiService.Put(url, updateDto);
				var content = await response.Content.ReadAsStringAsync(cancellationToken);
				
				Console.WriteLine($"Response Status: {(int)response.StatusCode} {response.StatusCode}");
				Console.WriteLine($"Response Content: {content}");

				if (!response.IsSuccessStatusCode)
				{
					if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
					{
						return new GeneralResponseDto 
						{ 
							IsSuccess = false, 
							Message = "Unauthorized. Please check if you're logged in with admin rights." 
						};
					}

					return new GeneralResponseDto 
					{ 
						IsSuccess = false, 
						Message = $"Server error: {response.StatusCode} - {content}" 
					};
				}

				try
				{
					var result = JsonSerializer.Deserialize<GeneralResponseDto>(content, _options);
					return result ?? new GeneralResponseDto 
					{ 
						IsSuccess = true,
						Message = $"Reservation status updated to {updateDto.Status} successfully"
					};
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error deserializing response: {ex.Message}");
					return new GeneralResponseDto 
					{ 
						IsSuccess = false,
						Message = $"Error processing server response: {ex.Message}"
					};
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Exception in Update: {ex.Message}");
				Console.WriteLine($"Stack trace: {ex.StackTrace}");
				return new GeneralResponseDto 
				{ 
					IsSuccess = false, 
					Message = $"Error: {ex.Message}" 
				};
			}
		}

		public async Task<ObservableCollection<ReservationDto>> GetAll(CancellationToken cancellationToken = default)
		{
			try
			{
				Console.WriteLine($"Fetching reservations from: {ApiEndpoints.ReservationController}");
				var response = await apiService.Get($"{ApiEndpoints.ReservationController}");
				
				if (!response.IsSuccessStatusCode)
				{
					var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
					Console.WriteLine($"Error fetching reservations. Status: {response.StatusCode}, Content: {errorContent}");
					return new ObservableCollection<ReservationDto>();
				}

				await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
				var reservations = await JsonSerializer.DeserializeAsync<ObservableCollection<ReservationDto>>(
					responseStream, 
					_options,
					cancellationToken);

				if (reservations == null)
				{
					Console.WriteLine("No reservations found or deserialization failed");
					return new ObservableCollection<ReservationDto>();
				}

				Console.WriteLine($"Successfully fetched {reservations.Count} reservations");
				return reservations;
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine($"HTTP request error in GetAll: {ex.Message}");
				return new ObservableCollection<ReservationDto>();
			}
			catch (JsonException ex)
			{
				Console.WriteLine($"JSON deserialization error in GetAll: {ex.Message}");
				return new ObservableCollection<ReservationDto>();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Unexpected error in GetAll: {ex.Message}");
				return new ObservableCollection<ReservationDto>();
			}
		}

		public async Task<ReservationDto> GetById(int reservationId, CancellationToken cancellationToken = default)
		{
			try
			{
				var response = await apiService.Get($"{ApiEndpoints.ReservationController}/details/{reservationId}");
				if (!response.IsSuccessStatusCode) return null!;
				await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
				var res = await JsonSerializer.DeserializeAsync<ReservationDto>(responseStream, _options,
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
