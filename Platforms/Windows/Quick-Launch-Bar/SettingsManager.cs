using System;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;
using System.Net.Http;
using static System.Net.Mime.MediaTypeNames;

namespace Quick_Launch_Bar
{
    class SettingsManager
    {
        private readonly Windows.Storage.ApplicationDataContainer localSettings =
            Windows.Storage.ApplicationData.Current.LocalSettings;

        Windows.Storage.StorageFolder localFolder =
            Windows.Storage.ApplicationData.Current.LocalFolder;

        public bool SaveBoolSetting(string name, bool value)
        {
            localSettings.Values[name] = value;

            return true;
        }

        public bool CheckBoolSetting(string name)
        {
            Object value = localSettings.Values[name];

            if (value == null)
                return false;

            bool result = (bool)value;

            return result;
        }

        public async Task<bool> SaveViewModelToJsonFileAsync<T>(T data, string suggestedFileName)
        {
            try
            {
                // 1. 序列化数据
                string json = JsonSerializer.Serialize(data, new JsonSerializerOptions
                {
                    WriteIndented = true // 格式化 JSON
                });

                // 3. 保存文件
                // 创建JSON子文件夹（如果不存在）
                StorageFolder jsonFolder = await localFolder.CreateFolderAsync("JSON", CreationCollisionOption.OpenIfExists);

                // 构造完整的文件路径
                string filePath = Path.Combine(jsonFolder.Path, suggestedFileName);


                await File.WriteAllTextAsync(filePath, json);
                return true; // 保存成功
            }
            catch (Exception ex)
            {
                var builder = new AppNotificationBuilder()
                    .AddText("保存数据失败！")
                    .AddText($"失败原因：{ex.Message}");
                AppNotificationManager.Default.Show(builder.BuildNotification());
            }

            return false; // 保存失败
        }

        public async Task<T> LoadFromJsonFileAsync<T>(string fileName)
        {
            try
            {
                StorageFolder jsonFolder = await localFolder.CreateFolderAsync("JSON", CreationCollisionOption.OpenIfExists);

                // 2. 获取文件
                StorageFile file = await jsonFolder.GetFileAsync(fileName);

                // 3. 读取并反序列化
                string json = await FileIO.ReadTextAsync(file);
                T data = JsonSerializer.Deserialize<T>(json);

                return data;
            }
            catch (Exception ex)
            {
                var builder = new AppNotificationBuilder()
                    .AddText("数据加载失败！")
                    .AddText($"失败原因：{ex.Message}");
                AppNotificationManager.Default.Show(builder.BuildNotification());

                return default;
            }
        }
    }
}
