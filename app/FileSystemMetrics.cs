using System.Text.Json.Nodes;

class FileSystemMetrics
{
    private readonly string _prometheusUrl;
    private readonly HttpClient _httpClient;

    public FileSystemMetrics(string prometheusUrl)
    {
        _prometheusUrl = prometheusUrl;
        _httpClient = new HttpClient();
    }

    // Размер файловой системы
    public async Task<JsonObject> GetNodeFileSystemSizeBytes()
    {
      return await QueryMetric("node_filesystem_size_bytes");
    }
    
    // Свободное место на файловой системе
    public async Task<JsonObject> GetNodeFileSystemFreeBytes()
    {
      return await QueryMetric("node_filesystem_free_bytes");
    }

    private async Task<JsonObject> QueryMetric(string metric)
    {
        // Формируем URL для запроса метрики
        string queryUrl = $"{_prometheusUrl}/api/v1/query?query={metric}";

        try
        {
            // Отправляем GET-запрос к серверу Prometheus
            HttpResponseMessage response = await _httpClient.GetAsync(queryUrl);

            // Проверяем успешность запроса
            if (response.IsSuccessStatusCode)
            {
                // Получаем ответ в виде строки
                string responseContent = await response.Content.ReadAsStringAsync();
                
                // Преобразование string в json
                JsonObject jsonResponse = (JsonObject)JsonNode.Parse(responseContent);

                return jsonResponse;
            }
            else
            {
                JsonObject jsonObj = new JsonObject { ["error"] = response.StatusCode.ToString() };
                return jsonObj;
            }
        }
        catch (Exception ex)
        {
            JsonObject jsonObj = new JsonObject { ["error"] = ex.ToString() };
            return jsonObj;
        }
    }
}