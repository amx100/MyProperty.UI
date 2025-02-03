using Contract;

namespace ViewModels
{
    public class ReservationFormViewModel : ComponentBaseViewModel
	{
		[CascadingParameter] private MudDialogInstance? MudDialog { get; set; }
		protected const string ValidationMessage = "Field is required.";

		[Parameter]
		public ReservationCreateDto? ReservationCreate { get; set; }

		[Parameter]
		public ReservationUpdateDto? ReservationUpdate { get; set; }

		protected override async Task OnInitializedAsync()
		{
			if (ReservationUpdate != null)
			{
				StateHasChanged();
			}
		}

		public async Task CreateOrUpdate()
		{
			try
			{
				var response = new GeneralResponseDto();
				if (ReservationCreate != null)
				{
					response = await ReservationService!.Create(ReservationCreate);
				}
				else if (ReservationUpdate != null)
				{
                    response = await ReservationService!.Update(ReservationUpdate.Id, ReservationUpdate);
				}

				HandleResponse(response);
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine(ex.Message);
				MudDialog!.Close(DialogResult.Ok(true));
			}
		}

		private void HandleResponse(GeneralResponseDto response)
		{
			var isSuccess = response?.IsSuccess == true;
			Snackbar!.Add(isSuccess ? "Success!" : "Error!", isSuccess ? Severity.Success : Severity.Error);
			MudDialog!.Close(DialogResult.Ok(true));
		}

		public void Cancel() => MudDialog!.Cancel();

		public bool Disabled =>
			(ReservationCreate != null && string.IsNullOrWhiteSpace(ReservationCreate.AccountId));
	}
}
