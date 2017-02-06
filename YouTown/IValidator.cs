using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace YouTown
{
    public interface IValidationResult
    {
        bool IsValid { get; }
        string InvalidDescription { get; }
    }
    public class Valid : IValidationResult
    {
        public bool IsValid => true;
        public string InvalidDescription => null;
    }
    public class Invalid : IValidationResult
    {
        public Invalid(string invalidDescription)
        {
            InvalidDescription = invalidDescription;
        }

        public bool IsValid => false;
        public string InvalidDescription { get; }
    }

    public static class Validator
    {
        public static readonly Valid Valid = new Valid();
    }

    /// <summary>
    /// Allows fluent api for validating objects against validators in a typesafe way
    /// </summary>
    public class ValidateAll
    {
        private class ValidatorValue
        {
            public IValidator Validator { get; set; }
            public object Value1 { get; set; }
            public object Value2 { get; set; }
            public string Text { get; set; }
        }

        private List<ValidatorValue> _validatorValues = new List<ValidatorValue>();


        public ValidateAll WithObject<TValidator>(object value, string text = null)
            where TValidator : IValidator, new()
        {
            var validatorValue = new ValidatorValue
            {
                Validator = new TValidator(),
                Value1 = value,
                Text = text
            };
            _validatorValues.Add(validatorValue);
            return this;
        }

        public ValidateAll With<TValidator, TValue>(TValue value, string text = null)
            where TValidator : IValidator, new()
        {
            var validatorValue = new ValidatorValue
            {
                Validator = new TValidator(),
                Value1 = value,
                Text = text
            };
            _validatorValues.Add(validatorValue);
            return this;
        }

        public ValidateAll With<TValidator, TValue1, TValue2>(TValue1 value1, TValue2 value2, string text = null)
            where TValidator : IValidator, new()
        {
            var validatorValue = new ValidatorValue
            {
                Validator = new TValidator(),
                Value1 = value1,
                Value2 = value2,
                Text = text
            };
            _validatorValues.Add(validatorValue);
            return this;
        }

        public IValidationResult Validate()
        {
            var invalidDescriptions = new List<string>();
            foreach (ValidatorValue validatorValue in _validatorValues)
            {
                var validator = validatorValue.Validator;
                var value1 = validatorValue.Value1;
                var value2 = validatorValue.Value1;
                var text = validatorValue.Text;
                var result = validator.Validate(value1, value2, text);
                if (!result.IsValid)
                {
                    invalidDescriptions.Add(result.InvalidDescription);
                }
            }
            if (invalidDescriptions.Any())
            {
                var invalidDescription = string.Join(Environment.NewLine, invalidDescriptions);
                return new Invalid(invalidDescription);
            }
            return Validator.Valid;
        }
    }

    /// <summary>
    /// C# can't do higher order generics, so we need this to have the
    /// <see cref="ValidateAll"/> class work nicely
    /// </summary>
    public interface IValidator
    {
        IValidationResult Validate(object value1, object value2 = null, string text = null);
    }

    /// <summary>
    /// Actual validation of a value of given generic type
    /// </summary>
    /// <typeparam name="TValue">Type of value to validate</typeparam>
    public interface IValidator<in TValue> : IValidator
    {
        IValidationResult Validate(TValue toValidate, string text = null);
    }

    /// <summary>
    /// Actual validation of a value of given generic type
    /// </summary>
    /// <typeparam name="TValue1"></typeparam>
    /// <typeparam name="TValue2"></typeparam>
    public interface IValidator<in TValue1, in TValue2> : IValidator
    {
        IValidationResult Validate(TValue1 player, TValue2 value2 = default(TValue2), string text = null);
    }

    /// <summary>
    /// Used to prevent boilerplat implementation of non-generic <see cref="IValidator"/> type
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public abstract class ValidatorBase<TValue> : IValidator<TValue>
    {
        public virtual IValidationResult Validate(TValue player, string text = null)
        {
            throw new NotImplementedException();
        }

        public IValidationResult Validate(object value1, object value2 = null, string text = null)
        {
            return Validate((TValue)value1, text: text);
        }
    }

    /// <summary>
    /// Used to prevent boilerplat implementation of non-generic <see cref="IValidator"/> type
    /// </summary>
    /// <typeparam name="TValue1"></typeparam>
    /// <typeparam name="TValue2"></typeparam>
    public abstract class ValidatorBase<TValue1, TValue2> : IValidator<TValue1, TValue2>
    {

        public virtual IValidationResult Validate(TValue1 player, TValue2 value2 = default(TValue2), string text = null)
        {
            throw new NotImplementedException();
        }

        public IValidationResult Validate(object value1, object value2 = null, string text = null)
        {
            return Validate((TValue1)value1, (TValue2)value2, text);
        }
    }

    public class HasCityInStock : ValidatorBase<IPlayer>
    {
        public override IValidationResult Validate(IPlayer player, string text = null)
        {
            if (!player.Stock[City.CityType].Any())
            {
                return new Invalid($"player {player.User.Name} does not have a city in stock");
            }
            return Validator.Valid;
        }
    }

    public class IsOnTurn : ValidatorBase<IPlayer>
    {
        public override IValidationResult Validate(IPlayer player, string text = null)
        {
            if (!player.IsOnTurn)
            {
                return new Invalid($"it's not player {player.User.Name}'s turn");
            }
            return Validator.Valid;
        }
    }

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

    public class HasTownAt : ValidatorBase<IPlayer, Point>
    {
        public override IValidationResult Validate(IPlayer player, Point point, string text = null)
        {
            if (player.PointPieces.ContainsKey(point))
            {
                // TODO: have nicer description of location e.g. "3ore, 9wheat, 6clay"
                return new Invalid($"player {player.User.Name} does not have a town at {point}");
            }
            return Validator.Valid;
        }
    }

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

    public class LoosesCorrectAmount : ValidatorBase<IPlayer, Int32>
    {
        public override IValidationResult Validate(IPlayer player, int amount, string text = null)
        {
            var amountToLoose = player.Hand.HalfCount();
            if (amountToLoose != amount)
            {
                return new Invalid($"player {player.User.Name} is expected to loose {amountToLoose} but wants to loose {amount}");
            }
            return Validator.Valid;
        }
    }

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

    public class HasHexAt : ValidatorBase<IBoard, Location>
    {
        public override IValidationResult Validate(IBoard board, Location location, string text = null)
        {
            if (!board.HexesByLocation.ContainsKey(location))
            {
                return new Invalid($"board does not have a hex at location {location}");
            }
            return Validator.Valid;
        }
    }

    public class CanPlaceRobberAt : ValidatorBase<IBoard, Location>
    {
        public override IValidationResult Validate(IBoard board, Location location, string text = null)
        {
            var hex = board.HexesByLocation[location];
            if (!hex.CanHaveRobber)
            {
                return new Invalid($"hex {hex} at location {location} does not support placing a robber on it");
            }
            return Validator.Valid;
        }
    }
}
