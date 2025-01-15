using Contract;

namespace ViewModels
{
    public class PropertyImageFormViewModel : ComponentBaseViewModel
	{
		[CascadingParameter] private MudDialogInstance? MudDialog { get; set; }
		protected const string ValidationMessage = "Field is required.";

		[Parameter]
		public PropertyImageCreateDto? PropertyImageCreate { get; set; }

		[Parameter]
		public PropertyImageUpdateDto? PropertyImageUpdate { get; set; }

		protected override async Task OnInitializedAsync()
		{
			if (PropertyImageUpdate != null)
			{
				StateHasChanged();
			}
		}

        public async Task CreateOrUpdate()
        {
            try
            {
                var response = new GeneralResponseDto();
                if (PropertyImageCreate != null)
                {
                    response = await PropertyImageService!.Create(PropertyImageCreate.PropertyId, PropertyImageCreate);
                }
                else if (PropertyImageUpdate != null)
                {
                    response = await PropertyImageService!.Update(PropertyImageUpdate.PropertyId, PropertyImageUpdate.Id, PropertyImageUpdate);
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
			(PropertyImageCreate != null && string.IsNullOrWhiteSpace(PropertyImageCreate.ImageUrl)) ||
			(PropertyImageUpdate != null && string.IsNullOrWhiteSpace(PropertyImageUpdate.ImageUrl));
	}
}
