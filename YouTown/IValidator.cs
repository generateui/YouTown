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
        private Dictionary<IValidator, object> _valueByValidator = new Dictionary<IValidator, object>();

        public ValidateAll With<TValidator>(object value)
            where TValidator : IValidator, new()
        {
            var validator = new TValidator();
            _valueByValidator[validator] = value;
            return this;
        }

        public IValidationResult Validate()
        {
            var invalidDescriptions = new List<string>();
            foreach (KeyValuePair<IValidator, object> pair in _valueByValidator)
            {
                var validator = pair.Key;
                var value = pair.Value;
                var result = validator.Validate(value);
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
        IValidationResult Validate(object toValidate);
    }

    /// <summary>
    /// Actual validation of a value of given generic type
    /// </summary>
    /// <typeparam name="T">Type of value to validate</typeparam>
    public interface IValidator<in T> : IValidator
    {
        IValidationResult Validate(T toValidate);
    }

    /// <summary>
    /// Used to prevent boilerplat implementation of non-generic <see cref="IValidator"/> type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ValidatorBase<T> : IValidator<T>
    {
        public virtual IValidationResult Validate(T player)
        {
            throw new NotImplementedException();
        }

        public IValidationResult Validate(object toValidate)
        {
            return Validate((T)toValidate);
        }
    }

    public class HasCityInStock : ValidatorBase<IPlayer>
    {
        public override IValidationResult Validate(IPlayer player)
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
        public override IValidationResult Validate(IPlayer player)
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
        public override IValidationResult Validate(IList list)
        {
            if (list.Count == 0)
            {
                // TODO: specify name of the list
                return new Invalid("list is empty");
            }
            return Validator.Valid;
        }
    }

    public class CanPayPiece : ValidatorBase<Tuple<IPlayer, IPiece>>
    {
        public override IValidationResult Validate(Tuple<IPlayer, IPiece> pair)
        {
            var player = pair.Item1;
            var piece = pair.Item2;
            if (!player.Hand.HasAtLeast(piece.Cost))
            {
                return new Invalid($"player {player.User.Name} does not have enough resources to pay for a {piece.PieceType}");
            }
            return Validator.Valid;
        }
    }
}
