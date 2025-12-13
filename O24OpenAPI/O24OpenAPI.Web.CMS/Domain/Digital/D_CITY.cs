namespace O24OpenAPI.Web.CMS.Domain;

public class D_CITY : BaseEntity
{
		public string CityCode { get; set; }
		public string CityName { get; set; }
		public string SearchCode { get; set; }

		public string Description { get; set; }
		public int Ord { get; set; }
		public int RegionId { get; set; }
		public string CountryId { get; set; }
		public string Status { get; set; }
		public string UserCreated { get; set; }
		public DateTime? DateCreated { get; set; } = DateTime.UtcNow;
    public string UserModified { get; set; }
		public DateTime? LastModified { get; set; }
		public string UserApproved { get; set; }
		public DateTime? DateApproved { get; set; }
		public string CityNameMM { get; set; }
	}
