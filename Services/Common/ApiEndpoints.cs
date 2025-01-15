namespace Services
{
    public static class ApiEndpoints
    {
        public const string BaseUrl = "https://localhost:5000/api";
        public const string AccountController = $"{BaseUrl}/accounts";
        public const string CategoryController = $"{BaseUrl}/categories";
        public const string PropertyController = $"{BaseUrl}/properties";
        public const string PropertyImageController = $"{BaseUrl}/propertyimages";
        public const string ReservationController = $"{BaseUrl}/reservations";
    }
}