namespace YouTown.Validator
{
    public class NotYetPlayedDevelopmentCard : ValidatorBase<IPlayTurnsTurn, IDevelopmentCard>
    {
        public override IValidationResult Validate(IPlayTurnsTurn turn, IDevelopmentCard developmentCard, string text = null)
        {
            if (developmentCard.MaxOnePerTurn && turn.HasPlayedDevelopmentCard)
            {
                return new Invalid($"turn {turn.Number} already has one development card played");
            }
            return Validator.Valid;
        }
    }
}