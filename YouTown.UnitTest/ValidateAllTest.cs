using Microsoft.VisualStudio.TestTools.UnitTesting;
using YouTown.Validation;

namespace YouTown.UnitTest
{
    /// <summary>
    /// We cannot mock the classes here using moq since the 
    /// <see cref="ValidateAll.With{TValidator,TValue}"/> methods require concrete types 
    /// with a parameterless constructor.
    /// </summary>
    [TestClass]
    public class ValidateAllTest
    {
        private class FailTwoArgValidator : ValidatorBase<TestClassAttribute, TestMethodAttribute>
        {
            public override IValidationResult Validate(TestClassAttribute player, TestMethodAttribute value2 = null, string text = null)
            {
                return new Invalid("fail");
            }
        }
        private class FailOneArgValidator : ValidatorBase<TestClassAttribute>
        {
            public override IValidationResult Validate(TestClassAttribute player, string text = null)
            {
                return new Invalid("fail");
            }
        }
        private class FailObjectArgValidator : IValidator
        {
            public IValidationResult Validate(object value1, object value2 = null, string text = null)
            {
                return new Invalid("fail");
            }
        }

        private class SuccessTwoArgValidator : ValidatorBase<TestClassAttribute, TestMethodAttribute>
        {
            public override IValidationResult Validate(TestClassAttribute player, TestMethodAttribute value2 = null, string text = null)
            {
                return Validator.Valid;
            }
        }
        private class SuccessOneArgValidator : ValidatorBase<TestClassAttribute>
        {
            public override IValidationResult Validate(TestClassAttribute player, string text = null)
            {
                return Validator.Valid;
            }
        }
        private class SuccessObjectArgValidator : IValidator
        {
            public IValidationResult Validate(object value1, object value2 = null, string text = null)
            {
                return Validator.Valid;
            }
        }

        [TestMethod]
        public void OnlyFailValidators_ValidationFails()
        {
            var result = new ValidateAll()
                .With<FailTwoArgValidator, TestClassAttribute, TestMethodAttribute>(null, null)
                .With<FailOneArgValidator, TestClassAttribute>(null)
                .WithObject<FailObjectArgValidator>(null)
                .Validate();

            Assert.IsFalse(result.IsValid, result.InvalidDescription);
        }

        [TestMethod]
        public void OnlySuccessValidators_ValidationSucceeds()
        {
            var result = new ValidateAll()
                .With<SuccessTwoArgValidator, TestClassAttribute, TestMethodAttribute>(null, null)
                .With<SuccessOneArgValidator, TestClassAttribute>(null)
                .WithObject<SuccessObjectArgValidator>(null)
                .Validate();

            Assert.IsTrue(result.IsValid, result.InvalidDescription);
        }

        [TestMethod]
        public void OneFailValidator_ValidationFails()
        {
            var result = new ValidateAll()
                .With<FailTwoArgValidator, TestClassAttribute, TestMethodAttribute>(null, null)
                .With<SuccessOneArgValidator, TestClassAttribute>(null)
                .WithObject<SuccessObjectArgValidator>(null)
                .Validate();

            Assert.IsFalse(result.IsValid, result.InvalidDescription);
        }
    }
}
