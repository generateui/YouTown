namespace YouTown.Validator
{
    public class HasResources : ValidatorBase<IPlayer, IResourceList>
    {
        public override IValidationResult Validate(IPlayer player, IResourceList resources, string text = null)
        {
            if (!player.Hand.HasAtLeast(resources))
            {
                return new Invalid($"player {player.User.Name} does not have given resources: {resources}");
            }
            return Validator.Valid;
        }
    }
}