Тестовое приложение-сервис диспетчеризации в производстве печатных плат.

Запустить контейнер из официального образа postgres в репозитории docker:
    -смаппить порты 5432:5432
    -в environment variables прописать переменную с названием POSTGRES_PASSWORD и значением dev
    -запустить.
Открыть папу с проектом PcbDispatchService в терминале/powershell;
Запустить dotnet ef database update;
Не обращать внимания на "Failed executing DbCommand (12ms) [Parameters=[], CommandType='Text', CommandTimeout='30']"
Запустить dotnet run;
Перейти на http://localhost:5015/swagger/index.html

IN PROGRESS:
Делаю docker-compose, пока не работает.