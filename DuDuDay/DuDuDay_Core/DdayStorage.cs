using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace DuDuDay_Core
{
    public static class DdayStorage
    {
        private static readonly string FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DuDuDay");
        private static readonly string FilePath = Path.Combine(FolderPath, "ddays.json");

        static DdayStorage()
        {
            /* 내문서>DuDuDay 폴더 없으면 자동 생성 */
            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
                Debug.WriteLine($"[DdayStorage] Created folder: {FolderPath}");
            }
        }

        // 저장 (기본 경로)
        public static void Save(List<DdayItem> ddays) => Save(ddays, FilePath);

        // 경로 지정 저장
        public static void Save(List<DdayItem> ddays, string path)
        {
            try
            {
                if (!Directory.Exists(FolderPath))
                    Directory.CreateDirectory(FolderPath);

                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(ddays, options);
                File.WriteAllText(path, json);

                Console.WriteLine($"[DdayStorage] Saved {ddays.Count} items to {path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DdayStorage] Save error: {ex}");
                throw;
            }
        }

        // 로드 (기본 경로)
        public static List<DdayItem> Load() => Load(FilePath);

        // 경로 지정 로드
        public static List<DdayItem> Load(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                    path = FilePath;

                Console.WriteLine($"[DdayStorage] Loading from: {path}");

                if (!File.Exists(path))
                {
                    Console.WriteLine("[DdayStorage] File not found. Returning empty list.");
                    return new List<DdayItem>();
                }

                string json = File.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(json))
                {
                    Console.WriteLine("[DdayStorage] File empty. Returning empty list.");
                    return new List<DdayItem>();
                }

                var list = JsonSerializer.Deserialize<List<DdayItem>>(json);
                Console.WriteLine($"[DdayStorage] Loaded {list?.Count ?? 0} items.");
                return list ?? new List<DdayItem>();
            }
            catch (JsonException jex)
            {
                Console.WriteLine($"[DdayStorage] JSON parse error: {jex}");
                return new List<DdayItem>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DdayStorage] Load error: {ex}");
                return new List<DdayItem>();
            }
        }
    }
}
