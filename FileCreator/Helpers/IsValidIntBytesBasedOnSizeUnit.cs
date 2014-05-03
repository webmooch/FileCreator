using FileCreator.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace FileCreator.Helpers
{
    internal class IsValidIntBytesBasedOnSizeUnit : ValidationAttribute
    {
        private static readonly int MaxIntArrayLengthx64 = int.MaxValue - 56;
        public string SizeUnitPropertyName { get; private set; }

        public IsValidIntBytesBasedOnSizeUnit(string sizeUnitPropertyName)
        {
            this.SizeUnitPropertyName = sizeUnitPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationContext == null)
                throw new ArgumentException("validationContext is null.");

            if (validationContext.ObjectInstance == null)
                throw new ArgumentException("validationContext.ObjectInstance is null.");

            var sizeUnit = validationContext.ObjectInstance.GetPropertyValue(SizeUnitPropertyName) as Size?;
            if (sizeUnit == null)
                throw new ArgumentException(string.Format("Specified property '{0}' is not of enum type 'Size'.", SizeUnitPropertyName));

            int intValue;
            switch (sizeUnit)
            {
                case Size.B:
                    if (!int.TryParse(value as string, out intValue) || intValue > MaxIntArrayLengthx64)
                        return new ValidationResult(string.Format("Size in Bytes must be between 1 and {0}.", MaxIntArrayLengthx64));
                    break;

                case Size.KB:
                    if (!int.TryParse(value as string, out intValue) || (intValue > (MaxIntArrayLengthx64 / 1024)))
                        return new ValidationResult(string.Format("Size in KiloBytes must be between 1 and {0}.", (MaxIntArrayLengthx64 / 1024)));
                    break;

                case Size.MB:
                    if (!int.TryParse(value as string, out intValue) || (intValue > (MaxIntArrayLengthx64 / 1024 / 1024)))
                        return new ValidationResult(string.Format("Size in MegaBytes must be between 1 and {0}.", (MaxIntArrayLengthx64 / 1024 / 1024)));
                    break;

                case Size.GB:
                    if (!int.TryParse(value as string, out intValue) || (intValue > (MaxIntArrayLengthx64 / 1024 / 1024 / 1024)))
                        return new ValidationResult("Size in GigaBytes can only be '1'.");
                    break;

                default:
                    throw new ArgumentOutOfRangeException("Unexpected size unit.");
            }

            if (intValue < 1)
                return new ValidationResult("Value must be greater than zero.");

            return null;
        }
    }
}