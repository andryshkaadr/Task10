namespace Task10
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text.Json;

    class DataSerializer
    {
        public static void SaveBinary<T>(T data, string filePath)
        {
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении данных в файл {filePath}: {ex.Message}");
            }
        }

        public static T LoadBinary<T>(string filePath)
        {
            T data = default(T);
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    data = (T)formatter.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке данных из файла {filePath}: {ex.Message}");
            }
            return data;
        }

        public static void SaveJson<T>(T data, string filePath)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                string jsonData = JsonSerializer.Serialize(data, options);
                File.WriteAllText(filePath, jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении данных в файл {filePath}: {ex.Message}");
            }
        }

        public static T LoadJson<T>(string filePath)
        {
            T data = default(T);
            try
            {
                if (File.Exists(filePath))
                {
                    string jsonData = File.ReadAllText(filePath);
                    data = JsonSerializer.Deserialize<T>(jsonData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке данных из файла {filePath}: {ex.Message}");
            }
            return data;
        }
    }
}
