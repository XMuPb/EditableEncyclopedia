using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EditableEncyclopedia
{
    /// <summary>
    /// Handles exporting and importing custom encyclopedia descriptions as JSON files
    /// so players can share them across campaigns or with other players.
    /// </summary>
    public static class DescriptionExportImportService
    {
        private const string ExportFileName = "descriptions_export.json";
        private const int CurrentFormatVersion = 1;

        /// <summary>
        /// Returns the directory used for storing export files:
        /// Documents\Mount and Blade II Bannerlord\Configs\ModSettings\Global\EditableEncyclopedia\
        /// </summary>
        public static string GetExportDirectory()
        {
            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return Path.Combine(
                documents,
                "Mount and Blade II Bannerlord",
                "Configs", "ModSettings", "Global",
                "EditableEncyclopedia");
        }

        public static string GetExportFilePath()
        {
            return Path.Combine(GetExportDirectory(), ExportFileName);
        }

        /// <summary>
        /// Exports all descriptions from the current campaign to a JSON file.
        /// </summary>
        /// <returns>An <see cref="ExportResult"/> indicating success/failure and details.</returns>
        public static ExportResult ExportDescriptions()
        {
            var storage = DescriptionStorageBehavior.Instance;
            if (storage == null)
                return ExportResult.Failure("No active campaign. Load a campaign first.");

            var descriptions = storage.GetAllDescriptions();
            if (descriptions.Count == 0)
                return ExportResult.Failure("No custom descriptions to export.");

            try
            {
                string directory = GetExportDirectory();
                Directory.CreateDirectory(directory);

                var exportData = new DescriptionExportData
                {
                    Version = CurrentFormatVersion,
                    ExportedAt = DateTime.UtcNow.ToString("o"),
                    DescriptionCount = descriptions.Count,
                    Descriptions = descriptions
                };

                string json = JsonConvert.SerializeObject(exportData, Formatting.Indented);
                string filePath = GetExportFilePath();
                File.WriteAllText(filePath, json);

                return ExportResult.Success(descriptions.Count, filePath);
            }
            catch (Exception ex)
            {
                return ExportResult.Failure($"Export failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Imports descriptions from a JSON file and merges them into the current campaign.
        /// Supports both the versioned format (v1) and the legacy format.
        /// </summary>
        /// <returns>An <see cref="ImportResult"/> indicating success/failure and details.</returns>
        public static ImportResult ImportDescriptions()
        {
            return ImportDescriptions(GetExportFilePath());
        }

        /// <summary>
        /// Imports descriptions from a specific JSON file path.
        /// </summary>
        public static ImportResult ImportDescriptions(string filePath)
        {
            var storage = DescriptionStorageBehavior.Instance;
            if (storage == null)
                return ImportResult.Failure("No active campaign. Load a campaign first.");

            if (!File.Exists(filePath))
                return ImportResult.Failure($"File not found: {filePath}");

            try
            {
                string json = File.ReadAllText(filePath);
                var descriptions = ParseDescriptionsFromJson(json);

                if (descriptions == null || descriptions.Count == 0)
                    return ImportResult.Failure("The file contains no descriptions.");

                int merged = storage.MergeDescriptions(descriptions);
                return ImportResult.Success(merged, filePath);
            }
            catch (JsonException ex)
            {
                return ImportResult.Failure($"Invalid JSON format: {ex.Message}");
            }
            catch (Exception ex)
            {
                return ImportResult.Failure($"Import failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Parses descriptions from JSON, supporting both the v1 format and legacy format.
        ///
        /// V1 format:
        /// { "version": 1, "descriptions": { "id": "text", ... } }
        ///
        /// Legacy format:
        /// { "descriptions": { "id": "text", ... } }
        /// </summary>
        private static Dictionary<string, string> ParseDescriptionsFromJson(string json)
        {
            var root = JObject.Parse(json);
            var descriptionsToken = root["descriptions"];

            if (descriptionsToken == null || descriptionsToken.Type != JTokenType.Object)
                return null;

            var result = new Dictionary<string, string>();
            foreach (var prop in ((JObject)descriptionsToken).Properties())
            {
                string value = prop.Value?.ToString();
                if (!string.IsNullOrEmpty(value))
                    result[prop.Name] = value;
            }

            return result;
        }
    }

    /// <summary>
    /// Data model for the exported JSON file.
    /// </summary>
    internal class DescriptionExportData
    {
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("exportedAt")]
        public string ExportedAt { get; set; }

        [JsonProperty("descriptionCount")]
        public int DescriptionCount { get; set; }

        [JsonProperty("descriptions")]
        public Dictionary<string, string> Descriptions { get; set; }
    }

    public class ExportResult
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
        public int Count { get; private set; }
        public string FilePath { get; private set; }

        public static ExportResult Success(int count, string filePath)
        {
            return new ExportResult
            {
                IsSuccess = true,
                Count = count,
                FilePath = filePath,
                Message = $"Exported {count} description(s) to {filePath}"
            };
        }

        public static ExportResult Failure(string message)
        {
            return new ExportResult { IsSuccess = false, Message = message };
        }
    }

    public class ImportResult
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
        public int Count { get; private set; }
        public string FilePath { get; private set; }

        public static ImportResult Success(int count, string filePath)
        {
            return new ImportResult
            {
                IsSuccess = true,
                Count = count,
                FilePath = filePath,
                Message = $"Imported {count} description(s) from {filePath}"
            };
        }

        public static ImportResult Failure(string message)
        {
            return new ImportResult { IsSuccess = false, Message = message };
        }
    }
}
