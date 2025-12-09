using System.Text.Json.Serialization;

namespace GarageManagement.API.Models;

public class GovernmentApiResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("result")]
    public ResultData? Result { get; set; }
}

public class ResultData
{
    [JsonPropertyName("records")]
    public List<GarageRecord>? Records { get; set; }
}

public class GarageRecord
{
    [JsonPropertyName("שם")]
    public string? Name { get; set; }

    [JsonPropertyName("כתובת")]
    public string? Address { get; set; }

    [JsonPropertyName("עיר")]
    public string? City { get; set; }

    [JsonPropertyName("טלפון")]
    public string? Phone { get; set; }

    [JsonPropertyName("אימייל")]
    public string? Email { get; set; }

    [JsonPropertyName("_id")]
    public int? Id { get; set; }
}
