namespace LimitlessCareDrPortal.Services.Common;

public class GenericResponseModel<TResult> : BaseResponseModel
{
	public GenericResponseModel()
	{
		var t = typeof(TResult);
		if (t.GetConstructor(Type.EmptyTypes) != null)
		{
			Data = Activator.CreateInstance<TResult>();
		}
	}

	public TResult Data { get; set; }
}
