namespace TSDLestate
{
	public interface IHttpService
	{
		Task<List<Movementos>> GetMovementosAsync();
		Task<List<MovementGoods>> GetMovementosGoodsAsync(string guid);
		Task SetMovementosGoodsAsync(string guid, string to1c);
	}
}
