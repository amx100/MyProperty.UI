namespace Helpers
{
    public class CreateInquiryItem
    {
        public Guid DocumentId { get; set; }
        public string? Type { get; set; }
        public Guid InquirySectionId { get; set; }
    }
}