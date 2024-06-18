using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;  

namespace TSDLestate
{
	public class HttpClientTSD : IHttpService
	{
		private readonly string _username;
		private readonly string _password;
		private readonly string _baseUrl = "https://vervet-direct-vastly.ngrok-free.ap/test_retail_neti/hs/apitsd/";
		private readonly HttpClient _client;

		public HttpClientTSD(string username, string password)
		{
			_username = username;
			_password = password;
			_client = new HttpClient
			{
				BaseAddress = new Uri(_baseUrl)
			};
			var byteArray = Encoding.ASCII.GetBytes($"{_username}:{_password}");
			var base64Credentials = Convert.ToBase64String(byteArray);
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);
		}

		public async Task<List<Movementos>> GetMovementosAsync()
		{
			List<Movementos> movementos = new List<Movementos>();

			try
			{
				HttpResponseMessage response = await _client.GetAsync("movements");
				response.EnsureSuccessStatusCode();
				string responseData = await response.Content.ReadAsStringAsync();
				movementos = JsonConvert.DeserializeObject<List<Movementos>>(responseData);
			}
			catch (HttpRequestException e)
			{
				throw new Exception($"Request error: {e.Message}");
			}
			catch (Exception e)
			{
				throw new Exception($"Unexpected error: {e.Message}");
			}

			return movementos;
		}

		public async Task<List<MovementGoods>> GetMovementosGoodsAsync(string guid)
		{
			List<MovementGoods> movementGoods = new List<MovementGoods>();

			try
			{
				HttpResponseMessage response = await _client.GetAsync($"movementgoods/{guid}");
				response.EnsureSuccessStatusCode();
				string responseData = await response.Content.ReadAsStringAsync();
				movementGoods = JsonConvert.DeserializeObject<List<MovementGoods>>(responseData);
			}
			catch (HttpRequestException e)
			{
				throw new Exception($"Request error: {e.Message}");
			}
			catch (Exception e)
			{
				throw new Exception($"Unexpected error: {e.Message}");
			}

			return movementGoods;
		}

		public async Task SetMovementosGoodsAsync(string guid, string to1c)
		{
			try
			{
				string jsonContent = JsonConvert.SerializeObject(to1c);
				var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
				HttpResponseMessage response = await _client.PostAsync($"movementgoods/{guid}", content);
				response.EnsureSuccessStatusCode();
			}
			catch (HttpRequestException e)
			{
				throw new Exception($"Request error: {e.Message}");
			}
			catch (Exception e)
			{
				throw new Exception($"Unexpected error: {e.Message}");
			}
		}
	}
}
