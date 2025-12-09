using GarageManagement.API.Models;
using System.Net.Http.Json;

namespace GarageManagement.API.Services;

public class GovernmentApiService
{
    private readonly HttpClient _httpClient;
    private const string GovernmentApiUrl = "https://data.gov.il/api/3/action/datastore_search?resource_id=bb68386a-a331-4bbc-b668-bba2766d517d&limit=5";

    public GovernmentApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
    }

    public async Task<List<Garage>> GetGaragesFromGovernmentApiAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<GovernmentApiResponse>(GovernmentApiUrl);
            
            if (response?.Success == true && response.Result?.Records != null && response.Result.Records.Any())
            {
                return response.Result.Records
                    .Where(record => record != null)
                    .Select(record => new Garage
                    {
                        Name = record.Name,
                        Address = record.Address,
                        City = record.City,
                        Phone = record.Phone,
                        Email = record.Email,
                        GovernmentId = record.Id?.ToString()
                    })
                    .ToList();
            }

            return new List<Garage>();
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Failed to connect to government API: {ex.Message}", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new Exception("Request to government API timed out", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error processing government API response: {ex.Message}", ex);
        }
    }
}
