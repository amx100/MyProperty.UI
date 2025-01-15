namespace Contract;

public class ReservationDto
{
	public int Id { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public int PropertyId { get; set; }
	public string AccountId { get; set; } = string.Empty;
	public string Status { get; set; } // "Pending", "Confirmed", "Declined"
}

public class ReservationCreateDto
{
	public int PropertyId { get; set; }
	public string AccountId { get; set; } = string.Empty;
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
}

public class ReservationUpdateDto
{
    public int Id { get; set; } //new
    public int PropertyId { get; set; }
	public string AccountId { get; set; } = string.Empty;  //ukloniti napraviti da owner i admin menjanju status rezervacije
	public string Status { get; set; } // "Pending", "Confirmed", "Declined"
}
