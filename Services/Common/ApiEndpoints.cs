namespace Services
{
    public static class ApiEndpoints
    {
        public static string BaseUrl => GetBaseUrl();
        
        public static string AccountController => $"{BaseUrl}/accounts";
        public static string CategoryController => $"{BaseUrl}/categories";
        public static string PropertyController => $"{BaseUrl}/properties";
        public static string PropertyImageController => $"{BaseUrl}/propertyimages";
        public static string ReservationController => $"{BaseUrl}/reservations";
        
        private static string GetBaseUrl()
        {
            var isProduction = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("RENDER"));
            return isProduction 
                ? "https://myproperty-api.onrender.com/api"
                : "https://localhost:5000/api";
        }
    }
}