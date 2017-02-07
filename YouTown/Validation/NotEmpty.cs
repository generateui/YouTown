using System.Collections;

namespace YouTown.Validation
{
    public class NotEmpty : ValidatorBase<IList>
    {
        public override IValidationResult Validate(IList list, string listName)
        {
            if (list.Count == 0)
            {
                return new Invalid($"list {listName} is empty");
            }
            return Validation.Validator.Valid;
        }
    }
}