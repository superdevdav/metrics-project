using System.Text.Json.Nodes;

class NetworkMetrics
{
    private readonly string _prometheusUrl;
    private readonly HttpClient _httpClient;

    public NetworkMetrics(string prometheusUrl)
    {
        _prometheusUrl = prometheusUrl;
        _httpClient = new HttpClient();
    }

    // Общее количество принятых байтов через сетевой интерфейс
    public async Task<JsonObject> GetNodeNetworkReceiveBytesTotal()
    {
      return await QueryMetric("node_network_receive_bytes_total");
    }
    
    // Общее количество принятых байтов через сетевой интерфейс
    public async Task<JsonObject> GetNodeNetworkTransmitBytesTotal()
    {
      return await QueryMetric("node_network_transmit_bytes_total");
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