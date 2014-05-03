using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;

namespace FileCreator.Models
{
    internal static class Extensions
    {
        public static object GetPropertyValue(this object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        public static Type GetPropertyType(this object src, string propName)
        {
            return src.GetType().GetProperty(propName).PropertyType;
        }

        public static string GenerateBandwidthString(this long bytesTransferredPerSecond)
        {
            var ordinals = new[] { "", "K", "M", "G", "T", "P", "E" };
            decimal rate = (decimal)bytesTransferredPerSecond;
            var ordinal = 0;
            while (rate > 1024)
            {
                rate /= 1024;
                ordinal++;
            }
            return string.Format("{0} {1}B/s", Math.Round(rate, 2, MidpointRounding.AwayFromZero), ordinals[ordinal]);
        }

        public static void OpenDirectory(this FileInfo file)
        {
            Process.Start("explorer.exe", string.Format("/select,\"{0}\"", file.FullName));
        }

        public static T DeserializeAsJsonObject<T>(this string json) where T : class
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                return serializer.ReadObject(ms) as T;
            }
        }

        public static bool IsCritical(this Exception ex)
        {
            if (ex is OutOfMemoryException) return true;
            if (ex is AppDomainUnloadedException) return true;
            if (ex is BadImageFormatException) return true;
            if (ex is CannotUnloadAppDomainException) return true;
            //if (ex is ExecutionEngineException) return true;
            if (ex is InvalidProgramException) return true;
            if (ex is ThreadAbortException) return true;
            return false;
        }
    }
}
