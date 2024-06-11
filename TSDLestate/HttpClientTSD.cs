using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;  

namespace TSDLestate
{
	public class HttpClientTSD
	{
		public static string username;
		public static string password;
		public static string postGUID;
		private string url = "https://vervet-direct-vastly.ngrok-free.app";

		public static async Task<List<Movementos>> GetMovementos(MainPage mainPage)
		{
			var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
			var base64Credentials = Convert.ToBase64String(byteArray);

			List<Movementos> movementos = new List<Movementos>();

			using (HttpClient client = new HttpClient())
			{
				try
				{
					client.BaseAddress = new Uri("https://vervet-direct-vastly.ngrok-free.ap/test_retail_neti/hs/apitsd/");
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic ", base64Credentials);
					HttpResponseMessage response = await client.GetAsync("movements");
					response.EnsureSuccessStatusCode();
					string responseData = await response.Content.ReadAsStringAsync();

					var data = JsonConvert.DeserializeObject<List<Movementos>>(responseData);
					
					return movementos;
				}

				catch (HttpRequestException e)
				{
					mainPage.Error(e.Message);
					return movementos;
				}
				catch (Exception e)
				{
					mainPage.Error(e.Message);
					return movementos;
				}
			}
		}

		public static async Task<List<MovementGoods>> GetMovementosGoods(string guid, MoveGoods moveGoods)
		{
			var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
			var base64Credentials = Convert.ToBase64String(byteArray);

			List<MovementGoods> movementGoods = new List<MovementGoods>();

			using (HttpClient client = new HttpClient())
			{
				try
				{
					client.BaseAddress = new Uri("https://vervet-direct-vastly.ngrok-free.ap/test_retail_neti/hs/apitsd/");
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);
					HttpResponseMessage response = await client.GetAsync($"movementgoods/{guid}");
					response.EnsureSuccessStatusCode();
					string responseData = await response.Content.ReadAsStringAsync();

					var data = JsonConvert.DeserializeObject<List<MovementGoods>>(responseData);
					
					return movementGoods;
				}

				catch (HttpRequestException e)
				{
					moveGoods.Error(e.Message);
					return movementGoods;
				}
				catch (Exception e)
				{
					moveGoods.Error(e.Message);
					return movementGoods;
				}
			}
		}

		public static async Task SetMovementosGoods(string guid, MoveGoods moveGoods, string to1c)
		{
			var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
			var base64Credentials = Convert.ToBase64String(byteArray);

			List<MovementGoods> movementGoods = new List<MovementGoods>();

			using (HttpClient client = new HttpClient())
			{
				try
				{
					// Установка базового URL (если необходимо)
					client.BaseAddress = new Uri("https://vervet-direct-vastly.ngrok-free.ap/test_retail_neti/hs/apitsd/");

					// Добавление заголовка авторизации
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);

					
					// Сериализация списка в JSON
					string jsonContent = JsonConvert.SerializeObject(to1c);
					var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

					// Отправка POST-запроса с JSON-данными
					HttpResponseMessage response = await client.PostAsync($"movementgoods/{guid}", content);

					// Проверка успешности ответа
					response.EnsureSuccessStatusCode();

					// Чтение содержимого ответа как строки (если сервер возвращает ответ)
					string responseData = await response.Content.ReadAsStringAsync();

					moveGoods.SendResult();
				}
				catch (HttpRequestException e)
				{
					moveGoods.Error(e.Message);
				}
				catch (Exception e)
				{
					moveGoods.Error(e.Message);
				}
			}
		}
	}
}
