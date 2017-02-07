namespace YouTown.Validation
{
    public class WaitedOneTurn : ValidatorBase<IDevelopmentCard, ITurn>
    {
        public override IValidationResult Validate(IDevelopmentCard developmentCard, ITurn turn, string text = null)
        {
            int turnNumber = turn.Number;
            int boughtTurnNumber = developmentCard.TurnBought.Number;
            if (boughtTurnNumber >= turnNumber)
            {
                return new Invalid($"developmentcard {developmentCard} is bought in turn {boughtTurnNumber} but played in {turnNumber}");
            }
            return Validator.Valid;
        }
    }
}