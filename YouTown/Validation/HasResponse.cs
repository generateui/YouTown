namespace YouTown.Validation
{
    public class HasResponse : ValidatorBase<TradeOffer, Accept>
    {
        public override IValidationResult Validate(TradeOffer tradeOffer, Accept accept, string text = null)
        {
            var hasAccept = tradeOffer.Responses.Contains(accept);
            if (!hasAccept)
            {
                return new Invalid($"trade offer {tradeOffer} does not contain acceptance {accept}");
            }
            return Validator.Valid;
        }
    }
}
