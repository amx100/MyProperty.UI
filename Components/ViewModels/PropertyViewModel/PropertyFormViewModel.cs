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

		private MudForm? form;

		public IEnumerable<string> PropertyTypes { get; set; } = new List<string> { "House", "Apartment", "Office" };
		public IEnumerable<string> StatusOptions { get; set; } = new List<string> { "Available", "Reserved", "Sold" };

		protected override async Task OnInitializedAsync()
		{
			if (PropertyCreate != null)
			{
				// Set default values for new properties
				PropertyCreate.PropertyType = PropertyTypes.First();
				PropertyCreate.Status = StatusOptions.First();
			}
			else if (PropertyUpdate != null)
			{
				// Keep existing values for updates
				PropertyUpdate.PropertyType ??= PropertyTypes.First();
				PropertyUpdate.Status ??= StatusOptions.First();
			}
		}

		public async Task CreateOrUpdate()
		{
			try
			{
				if (PropertyCreate != null)
				{
					// Log the property data being sent
					Console.WriteLine($"Creating property: Title={PropertyCreate.Title}, " +
									$"Type={PropertyCreate.PropertyType}, " +
									$"Status={PropertyCreate.Status}");

					// Ensure required fields are set
					if (string.IsNullOrWhiteSpace(PropertyCreate.Title) ||
						string.IsNullOrWhiteSpace(PropertyCreate.Description) ||
						string.IsNullOrWhiteSpace(PropertyCreate.Address))
					{
						Snackbar!.Add("Please fill in all required fields", Severity.Warning);
						return;
					}

					var response = await PropertyService!.Create(PropertyCreate);
					
					if (response != null && response.IsSuccess)
					{
						Snackbar!.Add("Property created successfully!", Severity.Success);
						MudDialog!.Close(DialogResult.Ok(true));
					}
					else
					{
						var errorMessage = response?.Message ?? "Failed to create property";
						Console.WriteLine($"Error creating property: {errorMessage}");
						Snackbar!.Add(errorMessage, Severity.Error);
					}
				}
				else if (PropertyUpdate != null)
				{
					var response = await PropertyService!.Update(PropertyUpdate.PropertyId, PropertyUpdate);
					if (response != null && response.IsSuccess)
					{
						Snackbar!.Add("Property updated successfully!", Severity.Success);
						MudDialog!.Close(DialogResult.Ok(true));
					}
					else
					{
						Snackbar!.Add(response?.Message ?? "Failed to update property", Severity.Error);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error saving property: {ex.Message}");
				Console.WriteLine($"Stack trace: {ex.StackTrace}");
				Snackbar!.Add($"Error saving property: {ex.Message}", Severity.Error);
			}
		}

		public void Cancel() => MudDialog!.Cancel();

		public bool Disabled =>
			(PropertyCreate != null && (
				string.IsNullOrWhiteSpace(PropertyCreate.Title) ||
				string.IsNullOrWhiteSpace(PropertyCreate.Description) ||
				string.IsNullOrWhiteSpace(PropertyCreate.Address))) ||
			(PropertyUpdate != null && (
				string.IsNullOrWhiteSpace(PropertyUpdate.Title) ||
				string.IsNullOrWhiteSpace(PropertyUpdate.Description) ||
				string.IsNullOrWhiteSpace(PropertyUpdate.Address)));
	}
}
