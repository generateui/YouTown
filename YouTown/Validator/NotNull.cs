namespace YouTown.Validator
{
    public class NotNull : IValidator
    {
        public IValidationResult Validate(object value1, object value2 = null, string objectName = null)
        {
            if (value1 == null)
            {
                return new Invalid($"object {objectName} cannot be null");
            }
            return Validator.Valid;
        }
    }
}