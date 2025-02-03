using Components.Dialog;
using Components.MyProperties;
using Contract;
using System.Collections.ObjectModel;

namespace ViewModels
{
    public class PropertyListViewModel : ComponentBaseViewModel
    {
        protected bool Loading;

        protected ObservableCollection<PropertyDto> Properties { get; set; } = new ObservableCollection<PropertyDto>();

        protected string? SearchPropertyName { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadProperties();
            Loading = false;
        }

        protected async Task CreateOrUpdateProperty(PropertyDto propertyDto)
        {
            DialogParameters parameters = new DialogParameters();
            if (propertyDto.PropertyId == 0)
            {
                var propertyCreate = propertyDto.Adapt<PropertyCreateDto>();
                parameters = new DialogParameters { ["PropertyCreate"] = propertyCreate };
            }
            else
            {
                var propertyUpdate = propertyDto.Adapt<PropertyUpdateDto>();
                parameters = new DialogParameters { ["PropertyUpdate"] = propertyUpdate };
            }

            var options = new DialogOptions
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Medium
            };

            var dialogTitle = propertyDto.PropertyId == 0 ? "Create Property" : "Update Property";
            var dialog = await DialogService!.ShowAsync<PropertyFormComponent>(dialogTitle, parameters, options);

            var result = await dialog.Result;
            if (!result!.Canceled)
            {
                StateHasChanged();
            }
        }

        private async Task LoadProperties()
        {
            try
            {
                Properties = await PropertyService!.GetAll();
                StateHasChanged();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected async Task DeleteProperty(PropertyDto property)
        {
            var parameters = new DialogParameters();
            const string text = "Are you sure you want to delete this property?";

            parameters.Add("ContentText", text);
            parameters.Add("ButtonText", "Delete");
            parameters.Add("Color", Color.Success);

            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };
            var dialog = await DialogService!.ShowAsync<ConfirmComponent>("Delete Property", parameters, options);
            var result = await dialog.Result;

            if (result!.Canceled)
            {
                return;
            }

            var response = await PropertyService!.Delete(property.PropertyId);
            HandleResponse(response, property);
        }

        protected bool FilterFunc(PropertyDto element)
        {
            return string.IsNullOrWhiteSpace(SearchPropertyName) ||
                   element.Title!.Contains(SearchPropertyName, StringComparison.OrdinalIgnoreCase);
        }

        public void HandleResponse(GeneralResponseDto response, PropertyDto property)
        {
            if (response.IsSuccess)
            {
                Properties.Remove(property);
                StateHasChanged();
                Snackbar!.Add("Property successfully deleted!", Severity.Success);
            }
            else
            {
                var errorMessage = !string.IsNullOrEmpty(response.Message) 
                    ? response.Message 
                    : "Failed to delete property. Please try again.";
                Snackbar!.Add(errorMessage, Severity.Error);
            }
        }

        protected async Task ManageImages(PropertyDto property)
        {
            var parameters = new DialogParameters
            {
                ["PropertyId"] = property.PropertyId
            };

            var options = new DialogOptions
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Large,
                FullWidth = true,
                CloseOnEscapeKey = true
            };

            var dialog = await DialogService!.ShowAsync<PropertyImageListComponent>(
                $"Manage Images - {property.Title}",
                parameters,
                options);

            var result = await dialog.Result;
            if (!result.Canceled)
            {
                await LoadProperties();
            }
        }
    }
}
