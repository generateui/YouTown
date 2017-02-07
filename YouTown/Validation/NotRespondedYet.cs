using System.Linq;

namespace YouTown.Validation
{
    public class NotRespondedYet : ValidatorBase<IPlayer, TradeOffer>
    {
        public override IValidationResult Validate(IPlayer player, TradeOffer tradeOffer, string text = null)
        {
            var responsesOfPlayer = tradeOffer.Responses.Where(tr => tr.Player.Equals(player));
            if (responsesOfPlayer.Any())
            {
                return new Invalid($"player {player.User.Name} already responded");
            }
            return Validator.Valid;
        }
    }
}
