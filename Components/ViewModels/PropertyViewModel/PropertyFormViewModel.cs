using Contract;

namespace ViewModels
{
    public class PropertyFormViewModel : ComponentBaseViewModel
	{
		[CascadingParameter] private MudDialogInstance? MudDialog { get; set; }
		protected const string ValidationMessage = "Field is required.";

		[Parameter]
		public PropertyCreateDto? PropertyCreate { get; set; }

		[Parameter]
		public PropertyUpdateDto? PropertyUpdate { get; set; }

		public IEnumerable<string> PropertyTypes { get; set; } = new List<string> { "House", "Apartment", "Office" };
		public IEnumerable<string> StatusOptions { get; set; } = new List<string> { "Available", "Reserved", "Sold" };

		public string? SelectedPropertyType { get; set; }
		public string? SelectedStatus { get; set; }

		protected override async Task OnInitializedAsync()
		{
			if (PropertyUpdate != null)
			{
				SelectedPropertyType = PropertyUpdate.PropertyType;
				SelectedStatus = PropertyUpdate.Status;
				StateHasChanged();
			}
		}

		public async Task CreateOrUpdate()
		{
			try
			{
				var response = new GeneralResponseDto();
				if (PropertyCreate != null)
				{
					PropertyCreate.PropertyType = SelectedPropertyType;
					PropertyCreate.Status = SelectedStatus;
					// Do not check PropertyId for creation
					response = await PropertyService!.Create(PropertyCreate);
				}
				else if (PropertyUpdate != null)
				{
					PropertyUpdate.PropertyType = SelectedPropertyType;
					PropertyUpdate.Status = SelectedStatus;
					response = await PropertyService!.Update(PropertyUpdate.PropertyId, PropertyUpdate);
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
			(PropertyCreate != null && string.IsNullOrWhiteSpace(PropertyCreate.Title)) ||
			(PropertyUpdate != null && string.IsNullOrWhiteSpace(PropertyUpdate.Title));
	}
}
