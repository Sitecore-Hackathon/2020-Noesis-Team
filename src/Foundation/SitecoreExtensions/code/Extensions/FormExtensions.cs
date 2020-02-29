using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Sitecore.ExperienceForms.Data;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Noesis.Foundation.SitecoreExtensions.Extensions
{
    public static class FormExtensions
    {
        public static string GetValue(FormSubmitContext formSubmitContext, string fieldName)
        {
            var fieldId = formSubmitContext.Fields.FirstOrDefault(i => i.Name == fieldName)?.ItemId;
            var values = string.Empty;

            if (fieldId == null)
            {
                return string.Empty;
            }

            var field = GetFieldById(new Guid(fieldId), formSubmitContext.Fields);
            var value = field?.GetType().GetProperty("Value")?.GetValue(field, null) as List<string>;
            if (value != null)
            {
                if (value.Count > 1)
                {
                    foreach (var str in value)
                    {
                        values = string.Join(values, "|", str, "|");
                    }
                    return values.Trim('|');
                }
                return value.FirstOrDefault();
            }

            return field?.GetType().GetProperty("Value")?.GetValue(field, null)?.ToString() ?? string.Empty;
        }

        public static List<byte[]> GetFileValue(FormSubmitContext formSubmitContext, string fieldName)
        {
            var files = new List<byte[]>();
            var fileStorageProvider = ServiceLocator.ServiceProvider.GetService<IFileStorageProvider>();

            var fieldId = formSubmitContext.Fields.FirstOrDefault(i => i.Name == fieldName)?.ItemId;

            if (fieldId == null)
            {
                return new List<byte[]>();
            }

            var field = GetFieldById(new Guid(fieldId), formSubmitContext.Fields);

            var storedFileInfo = field?.GetType().GetProperty("Value")?.GetValue(field, null) as List<Sitecore.ExperienceForms.Data.Entities.StoredFileInfo>;

            if (storedFileInfo != null && storedFileInfo.Any())
            {
                files.AddRange(storedFileInfo
                    .Select(storeFile => fileStorageProvider.GetFile(storeFile.FileId))
                    .Select(file => ConvertStream(file.File)));
            }

            return files;
        }

        private static byte[] ConvertStream(Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private static IViewModel GetFieldById(Guid id, IEnumerable<IViewModel> fields)
        {
            return fields.FirstOrDefault(f => Guid.Parse(f.ItemId) == id);
        }
    }
}