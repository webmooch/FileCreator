using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace FileCreator.Helpers
{
    internal class IsValidFullFileName : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is string))
                throw new ArgumentException("Value must be of type string.");

            var stringValue = value as string;

            if (string.IsNullOrWhiteSpace(stringValue))
                return new ValidationResult("Filename cannot be blank.");

            if (stringValue.Contains("/"))
                return new ValidationResult("Neither path nor filename can contain a forward slash '/'.");

            if (stringValue.EndsWith(@"\"))
                return new ValidationResult("Filename cannot be a directory.");

            if (Directory.Exists(stringValue))
                return new ValidationResult("Filename cannot be an existing directory.");

            string path;
            string file;
            ExtractPathAndFileName(stringValue, out path, out file);

            if (!string.IsNullOrWhiteSpace(path))
                if (!Directory.Exists(path))
                    return new ValidationResult(string.Format("Path '{0}' does not exist.", path));

            if (path.Any(x => Path.GetInvalidPathChars().Contains(x)))
                return new ValidationResult("Path contains invalid character(s).");

            if (file.Any(x => Path.GetInvalidFileNameChars().Contains(x)))
                return new ValidationResult("Filename contains invalid character(s).");

            return null;
        }

        private static void ExtractPathAndFileName(string fullFilePath, out string path, out string fileName)
        {
            var divided = fullFilePath.Split(new char[] { '\\' });
            path = string.Join("\\", divided, 0, divided.Length - 1);
            fileName = divided[divided.Length - 1];
        }
    }
}