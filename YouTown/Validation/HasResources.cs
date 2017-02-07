namespace YouTown.Validation
{
    public class HasResources : ValidatorBase<IResourceList, IResourceList>
    {
        public override IValidationResult Validate(IResourceList resources, IResourceList minimum, string name = null)
        {
            if (!resources.HasAtLeast(minimum))
            {
                return new Invalid($"player {name} does not have given resources: {resources}");
            }
            return Validator.Valid;
        }
    }
}