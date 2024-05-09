## Пояснения некоторые
В файлах, оканчивающихся на ...Metrics.cs находятся классы и методы для запросов к prometheus.
В Storage.cs реализован основной функционал для работы с БД.
В Entities.cs хранятся необходимые сущности.
Programs.cs - основной файл

## Про базу данных
В конструктор Storage.cs передается название таблицы, т.к. я вижу это так => есть кластер с различными узлами и у каждого узла должна быть своя таблица со своими записями. В данном коде название у таблицы "main_node".

## Имена столбцов
Я использовал сокращения в названих столбцов(чтобы код меньше был), а так нужно думаю нормальнее назвать столбцы.
1. MMTB - Общее количество физической памяти на узле.
2. PCST - Общее количество времени процессора, затраченного на выполнение процесса.
3. PRMB - Резидентный объем памяти, используемый процессом.
4. NRBT - Общее количество принятых байтов через сетевой интерфейс.
5. NTBT - Общее количество переданных байтов через сетевой интерфейс.
6. FSB - Размер файловой системы.
7. FFB - Свободное место на файловой системе.
