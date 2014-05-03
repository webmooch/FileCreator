using FileCreator.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace FileCreator.Helpers
{
    internal class IntPropertyCannotBeGreaterThanLongProperty : ValidationAttribute
    {
        public string NameOfSmallerIntProperty { get; private set; }
        public string NameOfLargerLongProperty { get; private set; }

        public IntPropertyCannotBeGreaterThanLongProperty(string smallerIntPropertyName, string largerLongPropertyName)
        {
            if (string.IsNullOrWhiteSpace(smallerIntPropertyName))
                throw new ArgumentException("smallerIntPropertyName must be specified.");

            if (string.IsNullOrWhiteSpace(largerLongPropertyName))
                throw new ArgumentException("largerLongPropertyName must be specified.");

            this.NameOfSmallerIntProperty = smallerIntPropertyName;
            this.NameOfLargerLongProperty = largerLongPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationContext == null)
                throw new ArgumentException("validationContext is null.");

            if (validationContext.ObjectInstance == null)
                throw new ArgumentException("validationContext.ObjectInstance is null.");

            var smallerType = validationContext.ObjectInstance.GetPropertyType(NameOfSmallerIntProperty);
            if (smallerType != typeof(int))
                throw new ArgumentException(string.Format("Specified 'smaller' property '{0}' must have a type of int.", NameOfSmallerIntProperty));

            var smallerValue = (int)validationContext.ObjectInstance.GetPropertyValue(NameOfSmallerIntProperty);

            var largerType = validationContext.ObjectInstance.GetPropertyType(NameOfLargerLongProperty);
            if (largerType != typeof(long))
                throw new ArgumentException(string.Format("Specified 'larger' property '{0}' must have a type of long.", NameOfLargerLongProperty));

            var largerValue = (long)validationContext.ObjectInstance.GetPropertyValue(NameOfLargerLongProperty);

            return ((long)largerValue < (int)smallerValue) ? new ValidationResult(string.IsNullOrWhiteSpace(this.ErrorMessage) ? "int value cannot be larger than long value." : this.ErrorMessage) : null;
        }
    }
}
