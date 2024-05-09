class MainClass
{
      static async Task Main(string[] args)
      {
            await Run();
      }

      static async Task Run()
      {
            // URL сервера Prometheus
            string prometheusUrl = "http://localhost:9090";

            // Создаем экземпляры классов типов метрик
            CPUAndMemoryMetrics cpuAndMemoryMetrics = new CPUAndMemoryMetrics(prometheusUrl);
            NetworkMetrics networkMetrics = new NetworkMetrics(prometheusUrl);
            FileSystemMetrics fileSystemMetrics = new FileSystemMetrics(prometheusUrl);

            #pragma warning disable 8602

            // Метрики: CPU и память
            var process_resident_memory_bytes = await cpuAndMemoryMetrics.GetProcessResidentMemoryBytes(); // Резидентный объем памяти, используемый процессом
            var PRMB = process_resident_memory_bytes["data"]["result"][0]["value"][1].ToString();

            var node_process_cpu_seconds_total = await cpuAndMemoryMetrics.GetProcessCpuSecondsTotal(); // Общее количество времени процессора, затраченного на выполнение процесса
            var PCST = node_process_cpu_seconds_total["data"]["result"][0]["value"][1].ToString();

            var node_memory_MemTotal_bytes = await cpuAndMemoryMetrics.GetNodeMemoryMemTotalBytes(); // Общее количество физической памяти на узле
            var MMTB = node_memory_MemTotal_bytes["data"]["result"][0]["value"][1].ToString();


            // Метрики: сеть
            var node_network_receive_bytes_total = await networkMetrics.GetNodeNetworkReceiveBytesTotal(); // Общее количество принятых байтов через сетевой интерфейс
            var NRBT = node_network_receive_bytes_total["data"]["result"][0]["value"][1].ToString();

            var node_network_transmit_bytes_total = await networkMetrics.GetNodeNetworkTransmitBytesTotal(); // Общее количество переданных байтов через сетевой интерфейс
            var NTBT = node_network_transmit_bytes_total["data"]["result"][0]["value"][1].ToString();


            // Метрики: файловая система
            var node_filesystem_size_bytes = await fileSystemMetrics.GetNodeFileSystemSizeBytes(); // Размер файловой системы
            var FSB = node_filesystem_size_bytes["data"]["result"][0]["value"][1].ToString();

            var node_filesystem_free_bytes = await fileSystemMetrics.GetNodeFileSystemFreeBytes(); // Свободное место на файловой системе
            var FFB = node_filesystem_free_bytes["data"]["result"][0]["value"][1].ToString();

            #pragma warning restore 8602

            var currentDateTime = DateTime.Now; // Текущая дата и время
            Console.WriteLine(currentDateTime);
            Console.WriteLine("Тип метрик: CPU и память");
            Console.WriteLine($"Общее количество физической памяти на узле: {MMTB} байт");
            Console.WriteLine($"Общее количество времени процессора, затраченного на выполнение процесса: {PCST} сек");
            Console.WriteLine($"Резидентный объем памяти, используемый процессом: {PRMB} байт");
            Console.WriteLine("--------------------------------------------------------------------------------------");

            Console.WriteLine("Тип метрик: сеть");
            Console.WriteLine($"Общее количество принятых байтов через сетевой интерфейс: {NRBT} байт");
            Console.WriteLine($"Общее количество переданных байтов через сетевой интерфейс: {NTBT} байт");
            Console.WriteLine("--------------------------------------------------------------------------------------");

            Console.WriteLine("Тип метрик: файловая система");
            Console.WriteLine($"Размер файловой системы: {FSB} байт");
            Console.WriteLine($"Свободное место на файловой системе: {FFB} байт");
            Console.WriteLine("--------------------------------------------------------------------------------------");

            // Работа с БД
            /*string tableName = "main_node";
            var db = new Storage(tableName);
            await db.CreateTable();
            await db.Insert(currentDateTime.ToString(),
                  Convert.ToInt64(PRMB),
                  Convert.ToInt64(MMTB),
                  Convert.ToDouble(PCST.Replace('.', ',')),
                  Convert.ToInt64(NRBT),
                  Convert.ToInt64(NTBT),
                  Convert.ToInt64(FSB),
                  Convert.ToInt64(FFB));
            Console.WriteLine(await db.GetMetricsRecords());*/
      }
}