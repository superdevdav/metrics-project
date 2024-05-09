using System.Text.Json.Nodes;

class CPUAndMemoryMetrics
{
    private readonly string _prometheusUrl;
    private readonly HttpClient _httpClient;

    public CPUAndMemoryMetrics(string prometheusUrl)
    {
        _prometheusUrl = prometheusUrl;
        _httpClient = new HttpClient();
    }

    // Общее количество времени процессора, затраченного на различные режимы работы
    public async Task<JsonObject> GetNodeCpuSecondsTotal()
    {
        return await QueryMetric("node_cpu_seconds_total");
    }

    // Общее количество физической памяти на узле
    public async Task<JsonObject> GetNodeMemoryMemTotalBytes()
    {
        return await QueryMetric("node_memory_MemTotal_bytes");
    }

    // Общее количество времени процессора, затраченного на выполнение процесса
    public async Task<JsonObject> GetProcessCpuSecondsTotal()
    {
        return await QueryMetric("process_cpu_seconds_total");
    }

    // Резидентный объем памяти, используемый процессом
    public async Task<JsonObject> GetProcessResidentMemoryBytes()
    {
        return await QueryMetric("process_resident_memory_bytes");
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
