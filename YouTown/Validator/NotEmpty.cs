using System.Collections;

namespace YouTown.Validator
{
    public class NotEmpty : ValidatorBase<IList>
    {
        public override IValidationResult Validate(IList list, string listName)
        {
            if (list.Count == 0)
            {
                return new Invalid($"list {listName} is empty");
            }
            return Validator.Valid;
        }
    }
}