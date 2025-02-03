using Contract;
using Microsoft.AspNetCore.Components.Forms;

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

		[Parameter]
		public int PropertyId { get; set; }

		private MudForm? Form;
		private bool _processing = false;

		protected override async Task OnInitializedAsync()
		{
			if (PropertyImageCreate != null)
			{
				PropertyImageCreate.PropertyId = PropertyId;
			}
			if (PropertyImageUpdate != null)
			{
				PropertyImageUpdate.PropertyId = PropertyId;
			}
		}

		protected async Task OnFileSelected(InputFileChangeEventArgs e)
		{
			try
			{
				var file = e.File;
				if (file != null)
				{
					// Get the file extension
					var extension = Path.GetExtension(file.Name).ToLowerInvariant();
					
					// Check if file is an image
					if (extension != ".jpg" && extension != ".jpeg" && extension != ".png" && extension != ".gif")
					{
						Snackbar!.Add("Please select an image file (.jpg, .jpeg, .png, .gif)", Severity.Warning);
						return;
					}

					// Check file size (e.g., max 5MB)
					if (file.Size > 5 * 1024 * 1024)
					{
						Snackbar!.Add("File size must be less than 5MB", Severity.Warning);
						return;
					}

					// Create a temporary URL for preview
					using var stream = file.OpenReadStream(maxAllowedSize: 5 * 1024 * 1024);
					using var ms = new MemoryStream();
					await stream.CopyToAsync(ms);
					var imageData = ms.ToArray();
					var base64Image = Convert.ToBase64String(imageData);
					var imageUrl = $"data:{file.ContentType};base64,{base64Image}";

					// Update the image URL
					if (PropertyImageCreate != null)
					{
						PropertyImageCreate.ImageUrl = imageUrl;
					}
					else if (PropertyImageUpdate != null)
					{
						PropertyImageUpdate.ImageUrl = imageUrl;
					}

					StateHasChanged();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error uploading file: {ex.Message}");
				Snackbar!.Add("Error uploading file", Severity.Error);
			}
		}

		public async Task CreateOrUpdate()
		{
			try
			{
				_processing = true;
				StateHasChanged();

				if (string.IsNullOrEmpty(PropertyImageCreate?.ImageUrl) && string.IsNullOrEmpty(PropertyImageUpdate?.ImageUrl))
				{
					Snackbar!.Add("Please provide an image URL or upload an image", Severity.Warning);
					return;
				}

				var response = new GeneralResponseDto();
				if (PropertyImageCreate != null)
				{
					// Log the request details
					Console.WriteLine($"Creating image for property {PropertyId}");
					Console.WriteLine($"Image URL: {PropertyImageCreate.ImageUrl}");

					PropertyImageCreate.PropertyId = PropertyId;
					response = await PropertyImageService!.Create(PropertyId, PropertyImageCreate);
					
					if (response == null)
					{
						Snackbar!.Add("Failed to create image", Severity.Error);
						return;
					}
				}
				else if (PropertyImageUpdate != null)
				{
					// Ensure PropertyId is set
					PropertyImageUpdate.PropertyId = PropertyId;
					response = await PropertyImageService!.Update(PropertyId, PropertyImageUpdate.Id, PropertyImageUpdate);
					
					if (response == null)
					{
						Snackbar!.Add("Failed to update image", Severity.Error);
						return;
					}
				}

				if (response.IsSuccess)
				{
					Snackbar!.Add("Image saved successfully!", Severity.Success);
					MudDialog!.Close(DialogResult.Ok(true));
				}
				else
				{
					Snackbar!.Add(response.Message ?? "Failed to save image", Severity.Error);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error saving image: {ex.Message}");
				Snackbar!.Add($"Error saving image: {ex.Message}", Severity.Error);
			}
			finally
			{
				_processing = false;
				StateHasChanged();
			}
		}

		public void Cancel() => MudDialog!.Cancel();

		public bool Disabled =>
			(PropertyImageCreate != null && string.IsNullOrWhiteSpace(PropertyImageCreate.ImageUrl)) ||
			(PropertyImageUpdate != null && string.IsNullOrWhiteSpace(PropertyImageUpdate.ImageUrl));
	}
}
