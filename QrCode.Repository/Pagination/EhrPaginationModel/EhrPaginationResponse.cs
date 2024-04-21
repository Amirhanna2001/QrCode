namespace LimitlessCareDrPortal.Repository;

public class EhrPaginationResponse<T>
{
	public EhrPaginationResponse(ICollection<T> recentPrescriptions, ICollection<T> previousPrescriptions, PaginationModel pagination)
	{
		this.RecentPrescriptions = recentPrescriptions;
		this.PreviousPrescriptions = previousPrescriptions;
		this.Pagination = pagination;
	}

	public ICollection<T> RecentPrescriptions { get; private set; }

	public ICollection<T> PreviousPrescriptions { get; private set; }

	public PaginationModel Pagination { get; private set; }
}
