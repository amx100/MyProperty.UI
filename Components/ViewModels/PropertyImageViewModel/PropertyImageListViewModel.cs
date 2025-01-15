using Components.Dialog;
using Contract;
using System.Collections.ObjectModel;

namespace ViewModels
{
    public class PropertyImageListViewModel : ComponentBaseViewModel
    {
        protected bool Loading;

        protected ObservableCollection<PropertyImageDto> PropertyImages { get; set; } = new ObservableCollection<PropertyImageDto>();

        protected string? SearchImageUrl { get; set; }

        [Parameter]
        public int PropertyId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadPropertyImages(PropertyId);
            Loading = false;
        }
        protected async Task CreateOrUpdatePropertyImage(PropertyImageDto propertyImageDto)
        {
            DialogParameters parameters = new DialogParameters();
            if (propertyImageDto.Id == 0)
            {
                var propertyImageCreate = propertyImageDto.Adapt<PropertyImageCreateDto>();
                parameters = new DialogParameters { ["PropertyImageCreate"] = propertyImageCreate };
            }
            else
            {
                var propertyImageUpdate = propertyImageDto.Adapt<PropertyImageUpdateDto>();
                parameters = new DialogParameters { ["PropertyImageUpdate"] = propertyImageUpdate };
            }

            var options = new DialogOptions
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Medium
            };

            var dialogTitle = propertyImageDto.Id == 0 ? "Create Property Image" : "Update Property Image";
            var dialog = await DialogService!.ShowAsync<PropertyImageFormViewModel>(dialogTitle, parameters, options);

            var result = await dialog.Result;
            if (!result!.Canceled)
            {
                StateHasChanged();
            }
        }

        private async Task LoadPropertyImages(int propertyId)
        {
            try
            {
                PropertyImages = await PropertyImageService!.GetAll(propertyId);
                StateHasChanged();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected async Task DeletePropertyImage(PropertyImageDto propertyImage)
        {
            var parameters = new DialogParameters();
            const string text = "Are you sure you want to delete this property image?";

            parameters.Add("ContentText", text);
            parameters.Add("ButtonText", "Delete");
            parameters.Add("Color", Color.Success);

            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };
            var dialog = await DialogService!.ShowAsync<ConfirmComponent>("Delete Property Image", parameters, options);
            var result = await dialog.Result;

            if (result!.Canceled)
            {
                return;
            }

            var response = await PropertyImageService!.Delete(propertyImage.Id);
            HandleResponse(response, propertyImage);
        }

        protected bool FilterFunc(PropertyImageDto element)
        {
            return string.IsNullOrWhiteSpace(SearchImageUrl) ||
                   element.ImageUrl!.Contains(SearchImageUrl, StringComparison.OrdinalIgnoreCase);
        }

        private void HandleResponse(GeneralResponseDto response, PropertyImageDto propertyImage)
        {
            if (response.IsSuccess)
            {
                PropertyImages.Remove(propertyImage);
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
