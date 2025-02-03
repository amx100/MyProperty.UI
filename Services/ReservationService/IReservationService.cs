using Contract;

namespace Services.ReservationService;

public interface IReservationService
{
	Task<ObservableCollection<ReservationDto>> GetAll(CancellationToken cancellationToken = default);

	Task<ReservationDto> GetById(int reservationId, CancellationToken cancellationToken = default);

	Task<GeneralResponseDto> Create(ReservationCreateDto reservationDto, CancellationToken cancellationToken = default);

	Task<GeneralResponseDto> Update(int id, ReservationUpdateDto updateDto, CancellationToken cancellationToken = default);

	Task<GeneralResponseDto> Delete(int reservationId, CancellationToken cancellationToken = default);
}