using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace DuDuDay
{
    public static class DdayStorage
    {
        private static string defaultFilePath = "ddays.json";

        // 기본 저장 (기본 경로)
        public static void Save(List<DdayItem> ddays) => Save(ddays, defaultFilePath);

        // 경로 지정 저장
        public static void Save(List<DdayItem> ddays, string path)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(ddays, options);
                File.WriteAllText(path, json);
                Debug.WriteLine($"[DdayStorage] Saved {ddays.Count} items to {path}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DdayStorage] Save error: {ex}");
                throw;
            }
        }

        // 기본 로드 (기본 경로)
        public static List<DdayItem> Load() => Load(defaultFilePath);

        // 경로 지정 로드 (오버로드)
        public static List<DdayItem> Load(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                    path = defaultFilePath;

                Debug.WriteLine($"[DdayStorage] Loading from: {path}");

                if (!File.Exists(path))
                {
                    Debug.WriteLine("[DdayStorage] File not found. Returning empty list.");
                    return new List<DdayItem>();
                }

                string json = File.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(json))
                {
                    Debug.WriteLine("[DdayStorage] File empty. Returning empty list.");
                    return new List<DdayItem>();
                }

                var list = JsonSerializer.Deserialize<List<DdayItem>>(json);
                Debug.WriteLine($"[DdayStorage] Loaded {list?.Count ?? 0} items.");
                return list ?? new List<DdayItem>();
            }
            catch (JsonException jex)
            {
                Debug.WriteLine($"[DdayStorage] JSON parse error: {jex}");
                return new List<DdayItem>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DdayStorage] Load error: {ex}");
                return new List<DdayItem>();
            }
        }
    }
}
