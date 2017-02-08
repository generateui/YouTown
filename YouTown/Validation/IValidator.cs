using System;
using System.Collections.Generic;
using System.Linq;

namespace YouTown.Validation
{
    public interface IValidationResult
    {
        bool IsValid { get; }
        string InvalidDescription { get; }
    }
    public class Valid : IValidationResult
    {
        public bool IsValid => true;
        public string InvalidDescription => null;
    }
    public class Invalid : IValidationResult
    {
        public Invalid(string invalidDescription)
        {
            InvalidDescription = invalidDescription;
        }

        public bool IsValid => false;
        public string InvalidDescription { get; }
    }

    public static class Validator
    {
        public static readonly Valid Valid = new Valid();
    }

    /// <summary>
    /// Allows fluent api for validating objects against validators in a typesafe way
    /// </summary>
    public class ValidateAll
    {
        private class ValidatorValue
        {
            public IValidator Validator { get; set; }
            public object Value1 { get; set; }
            public object Value2 { get; set; }
            public string Text { get; set; }
        }

        private List<ValidatorValue> _validatorValues = new List<ValidatorValue>();


        public ValidateAll WithObject<TValidator>(object value, string text = null)
            where TValidator : IValidator, new()
        {
            var validatorValue = new ValidatorValue
            {
                Validator = new TValidator(),
                Value1 = value,
                Text = text
            };
            _validatorValues.Add(validatorValue);
            return this;
        }

        public ValidateAll With<TValidator, TValue>(TValue value, string text = null)
            where TValidator : IValidator<TValue>, new()
        {
            var validatorValue = new ValidatorValue
            {
                Validator = new TValidator(),
                Value1 = value,
                Text = text
            };
            _validatorValues.Add(validatorValue);
            return this;
        }

        public ValidateAll With<TValidator, TValue1, TValue2>(TValue1 value1, TValue2 value2, string text = null)
            where TValidator : IValidator<TValue1, TValue2>, new()
        {
            var validatorValue = new ValidatorValue
            {
                Validator = new TValidator(),
                Value1 = value1,
                Value2 = value2,
                Text = text
            };
            _validatorValues.Add(validatorValue);
            return this;
        }

        public IValidationResult Validate()
        {
            var invalidDescriptions = new List<string>();
            foreach (ValidatorValue validatorValue in _validatorValues)
            {
                var validator = validatorValue.Validator;
                var value1 = validatorValue.Value1;
                var value2 = validatorValue.Value2;
                var text = validatorValue.Text;
                var result = validator.Validate(value1, value2, text);
                if (!result.IsValid)
                {
                    invalidDescriptions.Add(result.InvalidDescription);
                }
            }
            if (invalidDescriptions.Any())
            {
                var invalidDescription = string.Join(Environment.NewLine, invalidDescriptions);
                return new Invalid(invalidDescription);
            }
            return Validator.Valid;
        }
    }

    /// <summary>
    /// C# can't do higher order generics, so we need this to have the
    /// <see cref="ValidateAll"/> class work nicely
    /// </summary>
    public interface IValidator
    {
        IValidationResult Validate(object value1, object value2 = null, string text = null);
    }

    /// <summary>
    /// Actual validation of a value of given generic type
    /// </summary>
    /// <typeparam name="TValue">Type of value to validate</typeparam>
    public interface IValidator<in TValue> : IValidator
    {
        IValidationResult Validate(TValue toValidate, string text = null);
    }

    /// <summary>
    /// Actual validation of a value of given generic type
    /// </summary>
    /// <typeparam name="TValue1"></typeparam>
    /// <typeparam name="TValue2"></typeparam>
    public interface IValidator<in TValue1, in TValue2> : IValidator
    {
        IValidationResult Validate(TValue1 player, TValue2 value2 = default(TValue2), string text = null);
    }

    /// <summary>
    /// Used to prevent boilerplat implementation of non-generic <see cref="IValidator"/> type
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class ValidatorBase<TValue> : IValidator<TValue>
    {
        public virtual IValidationResult Validate(TValue player, string text = null)
        {
            throw new NotImplementedException();
        }

        public IValidationResult Validate(object value1, object value2 = null, string text = null)
        {
            return Validate((TValue)value1, text: text);
        }
    }

    /// <summary>
    /// Used to prevent boilerplat implementation of non-generic <see cref="IValidator"/> type
    /// </summary>
    /// <typeparam name="TValue1"></typeparam>
    /// <typeparam name="TValue2"></typeparam>
    public abstract class ValidatorBase<TValue1, TValue2> : IValidator<TValue1, TValue2>
    {

        public virtual IValidationResult Validate(TValue1 player, TValue2 value2 = default(TValue2), string text = null)
        {
            throw new NotImplementedException();
        }

        public IValidationResult Validate(object value1, object value2, string text = null)
        {
            return Validate((TValue1)value1, (TValue2)value2, text);
        }
    }
}
