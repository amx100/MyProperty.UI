using Components.Dialog;
using Components.MyProperties;
using Components.PropertyImages;
using Contract;
using System.Collections.ObjectModel;

namespace ViewModels
{
    public class PropertyListViewModel : ComponentBaseViewModel
    {
        protected bool Loading;

        protected ObservableCollection<PropertyDto> Properties { get; set; } = new ObservableCollection<PropertyDto>();

        protected string? SearchPropertyName { get; set; }
        protected string? SelectedPropertyType { get; set; }
        protected string? SelectedStatus { get; set; }
        protected double? MinArea { get; set; }
        protected double? MaxArea { get; set; }

        protected IEnumerable<string> PropertyTypes { get; set; } = new List<string> { "House", "Apartment", "Office" };
        protected IEnumerable<string> StatusOptions { get; set; } = new List<string> { "Available", "Reserved" };

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
            if (!result.Canceled)
            {
                Loading = true;
                StateHasChanged();
                await LoadProperties();
                Loading = false;
                StateHasChanged();
            }
        }

        protected async Task LoadProperties()
        {
            try
            {
                var properties = await PropertyService!.GetAll();
                if (properties != null)
                {
                    Properties = properties;
                    StateHasChanged();
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                Snackbar!.Add("Error loading properties", Severity.Error);
            }
        }

        protected async Task DeleteProperty(PropertyDto property)
        {
            var parameters = new DialogParameters();
            const string text = "Are you sure you want to delete this property?";

            parameters.Add("ContentText", text);
            parameters.Add("ButtonText", "Delete");
            parameters.Add("Color", Color.Error);

            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };
            var dialog = await DialogService!.ShowAsync<ConfirmComponent>("Delete Property", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                Loading = true;
                StateHasChanged();
                
                var response = await PropertyService!.Delete(property.PropertyId);
                HandleResponse(response, property);
                
                Loading = false;
                StateHasChanged();
            }
        }

        protected bool FilterFunc(PropertyDto element)
        {
            if (string.IsNullOrWhiteSpace(SearchPropertyName) &&
                string.IsNullOrWhiteSpace(SelectedPropertyType) &&
                string.IsNullOrWhiteSpace(SelectedStatus) &&
                !MinArea.HasValue &&
                !MaxArea.HasValue)
            {
                return true;
            }

            bool matchesSearch = string.IsNullOrWhiteSpace(SearchPropertyName) ||
                               element.Title!.Contains(SearchPropertyName, StringComparison.OrdinalIgnoreCase);

            bool matchesType = string.IsNullOrWhiteSpace(SelectedPropertyType) ||
                             element.PropertyType == SelectedPropertyType;

            bool matchesStatus = string.IsNullOrWhiteSpace(SelectedStatus) ||
                               element.Status == SelectedStatus;

            bool matchesArea = true;
            if (MinArea.HasValue && element.Area < MinArea.Value)
            {
                matchesArea = false;
            }
            if (MaxArea.HasValue && element.Area > MaxArea.Value)
            {
                matchesArea = false;
            }

            return matchesSearch && matchesType && matchesStatus && matchesArea;
        }

        protected void ClearFilters()
        {
            SearchPropertyName = null;
            SelectedPropertyType = null;
            SelectedStatus = null;
            MinArea = null;
            MaxArea = null;
            StateHasChanged();
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
                Loading = true;
                StateHasChanged();
                await LoadProperties();
                Loading = false;
                StateHasChanged();
            }
        }
    }
}
