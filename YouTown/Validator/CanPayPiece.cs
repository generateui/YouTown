namespace YouTown.Validator
{
    public class CanPayPiece : ValidatorBase<IPlayer, IPiece>
    {
        public override IValidationResult Validate(IPlayer player, IPiece piece, string text = null)
        {
            if (!player.Hand.HasAtLeast(piece.Cost))
            {
                return new Invalid($"player {player.User.Name} does not have enough resources to pay for a {piece.PieceType}");
            }
            return Validator.Valid;
        }
    }
}