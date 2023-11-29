namespace Task10
{
    using System;

    class DataIOManager
    {
        public void SaveData<T>(T data, string binaryFilePath, string jsonFilePath)
        {
            DataSerializer.SaveBinary(data, binaryFilePath);
            DataSerializer.SaveJson(data, jsonFilePath);
            Console.WriteLine("Данные сохранены в файлах (Binary и JSON).");
        }

        public T LoadData<T>(string jsonFilePath, string binaryFilePath)
        {
            T data = DataSerializer.LoadJson<T>(jsonFilePath);

            if (data == null)
            {
                data = DataSerializer.LoadBinary<T>(binaryFilePath);
            }

            return data;
        }
    }
}
