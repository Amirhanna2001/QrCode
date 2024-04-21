namespace LimitlessCareDrPortal.Services.Common;

public class BaseResponseModel
{
	public BaseResponseModel()
	{
		ErrorList = new List<ErrorListModel>();
		Message = string.Empty;
	}

	public BaseResponseModel(string message, ICollection<ErrorListModel> errorList)
	{
		this.Message = message;
		this.ErrorList = errorList;
	}

	public string Message { get; set; }

	public ICollection<ErrorListModel> ErrorList { get; private set; }
}

public class ErrorListModel
{
	public int Id { get; set; }

	public string Message { get; set; }
}
