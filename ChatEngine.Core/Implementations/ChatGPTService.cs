using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ChatEngine.Core;

public class ChatGPTService : IChatGPTService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public ChatGPTService(HttpClient httpClient)
    {
        _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
                  ?? throw new ArgumentNullException("API Key is missing");
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/engines/davinci/completions");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
    }

    public async Task<string> SendRequestAsync(string prompt, int maxTokens = 100, double temperature = 0.7)
    {
        var requestBody = new
        {
            prompt = prompt,
            max_tokens = maxTokens,
            temperature = temperature
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        try
        {
            // Realiza la solicitud a la API de OpenAI
            var response = await _httpClient.PostAsync("", jsonContent);

            // Si la respuesta no es exitosa, lanza una excepción
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error in request to ChatGPT: {response.StatusCode}");
            }

            var result = await response.Content.ReadAsStringAsync();

            // Procesa la respuesta (opcional: puedes extraer solo el texto necesario)
            using (JsonDocument doc = JsonDocument.Parse(result))
            {
                var root = doc.RootElement;
                var choices = root.GetProperty("choices");
                return choices[0].GetProperty("text").GetString();
            }
        }
        catch (Exception ex)
        {
            // Manejo de excepciones para casos de error
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw;
        }
    }
}

