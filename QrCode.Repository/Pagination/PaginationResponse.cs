namespace LimitlessCareDrPortal.Repository;

public class PaginationResponse<T>
{
	public PaginationResponse(ICollection<T> items) => this.Items = items;

	public PaginationResponse(ICollection<T> items, PaginationModel pagination)
	{
		this.Items = items;
		this.Pagination = pagination;
	}

	public ICollection<T> Items { get; private set; }

	public PaginationModel Pagination { get; private set; }
}
