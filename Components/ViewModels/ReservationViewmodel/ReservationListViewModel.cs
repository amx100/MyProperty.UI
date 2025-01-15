using Components.Dialog;
using Contract;
using System.Collections.ObjectModel;

namespace ViewModels
{
    public class ReservationListViewModel : ComponentBaseViewModel
    {
        protected bool Loading;

        protected ObservableCollection<ReservationDto> Reservations { get; set; } = new ObservableCollection<ReservationDto>();

        protected string? SearchReservationId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadReservations();
            Loading = false;
        }

        protected async Task CreateOrUpdateReservation(ReservationDto reservationDto)
        {
            DialogParameters parameters = new DialogParameters();
            if (reservationDto.Id == 0)
            {
                var reservationCreate = reservationDto.Adapt<ReservationCreateDto>();
                parameters = new DialogParameters { ["ReservationCreate"] = reservationCreate };
            }
            else
            {
                var reservationUpdate = reservationDto.Adapt<ReservationUpdateDto>();
                parameters = new DialogParameters { ["ReservationUpdate"] = reservationUpdate };
            }

            var options = new DialogOptions
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Medium
            };

            var dialogTitle = reservationDto.Id == 0 ? "Create Reservation" : "Update Reservation";
            var dialog = await DialogService!.ShowAsync<ReservationFormViewModel>(dialogTitle, parameters, options);

            var result = await dialog.Result;
            if (!result!.Canceled)
            {
                StateHasChanged();
            }
        }

        private async Task LoadReservations()
        {
            try
            {
                Reservations = await ReservationService!.GetAll();
                StateHasChanged();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected async Task DeleteReservation(ReservationDto reservation)
        {
            var parameters = new DialogParameters();
            const string text = "Are you sure you want to delete this reservation?";

            parameters.Add("ContentText", text);
            parameters.Add("ButtonText", "Delete");
            parameters.Add("Color", Color.Success);

            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };
            var dialog = await DialogService!.ShowAsync<ConfirmComponent>("Delete Reservation", parameters, options);
            var result = await dialog.Result;

            if (result!.Canceled)
            {
                return;
            }

            var response = await ReservationService!.Delete(reservation.Id);
            HandleResponse(response, reservation);
        }

        protected bool FilterFunc(ReservationDto element)
        {
            return string.IsNullOrWhiteSpace(SearchReservationId) ||
                   element.Id!.ToString().Contains(SearchReservationId, StringComparison.OrdinalIgnoreCase);
        }

        private void HandleResponse(GeneralResponseDto response, ReservationDto reservation)
        {
            if (response.IsSuccess)
            {
                Reservations.Remove(reservation);
                StateHasChanged();
                Snackbar!.Add("Success!", Severity.Success);
            }
            else
            {
                Snackbar!.Add("Error", Severity.Error);
            }
        }
    }
}
