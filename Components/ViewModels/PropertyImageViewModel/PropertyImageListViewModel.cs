using Components.Dialog;
using Components.PropertyImages;
using Contract;
using System.Collections.ObjectModel;

namespace ViewModels
{
    public class PropertyImageListViewModel : ComponentBaseViewModel
    {
        [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }
        [Parameter] public int PropertyId { get; set; }

        protected ObservableCollection<PropertyImageDto> PropertyImages { get; set; } = new();

        protected string? SearchImageUrl { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadPropertyImages();
        }

        private async Task LoadPropertyImages()
        {
            try
            {
                var images = await PropertyImageService!.GetAll(PropertyId);
                if (images != null)
                {
                    PropertyImages = images;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading images: {ex.Message}");
                Snackbar!.Add("Error loading images", Severity.Error);
            }
        }

        protected async Task CreateOrUpdatePropertyImage(PropertyImageDto propertyImage)
        {
            var parameters = new DialogParameters();
            if (propertyImage.Id == 0)
            {
                var createDto = new PropertyImageCreateDto { PropertyId = PropertyId };
                parameters.Add("PropertyImageCreate", createDto);
            }
            else
            {
                var updateDto = propertyImage.Adapt<PropertyImageUpdateDto>();
                parameters.Add("PropertyImageUpdate", updateDto);
            }
            parameters.Add("PropertyId", PropertyId);

            var options = new DialogOptions 
            { 
                CloseButton = true, 
                MaxWidth = MaxWidth.Medium,
                FullWidth = true
            };
            
            var dialog = await DialogService!.ShowAsync<PropertyImageFormDialog>(
                propertyImage.Id == 0 ? "Add New Image" : "Edit Image",
                parameters,
                options);

            var result = await dialog.Result;
            if (!result.Canceled)
            {
                await LoadPropertyImages();
            }
        }

        protected async Task DeletePropertyImage(PropertyImageDto image)
        {
            var parameters = new DialogParameters
            {
                ["ContentText"] = "Are you sure you want to delete this image?",
                ["ButtonText"] = "Delete",
                ["Color"] = Color.Error
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };
            var dialog = await DialogService!.ShowAsync<ConfirmComponent>("Delete Image", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                var response = await PropertyImageService!.Delete(image.Id);
                if (response.IsSuccess)
                {
                    await LoadPropertyImages();
                    Snackbar!.Add("Image deleted successfully", Severity.Success);
                }
                else
                {
                    Snackbar!.Add("Error deleting image", Severity.Error);
                }
            }
        }

        protected bool FilterFunc(PropertyImageDto element)
        {
            return string.IsNullOrWhiteSpace(SearchImageUrl) ||
                   element.ImageUrl!.Contains(SearchImageUrl, StringComparison.OrdinalIgnoreCase);
        }

        public void Cancel() => MudDialog?.Cancel();
    }
}
