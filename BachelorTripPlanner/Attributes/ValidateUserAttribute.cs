using BusinessLogic.Accounts;
using CompositionRoot;
using System;
using System.ComponentModel.DataAnnotations;

namespace BachelorTripPlanner.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public class ValidateUserAttribute : ValidationAttribute
    {
        private readonly UserWorker _userWorker;

        public ValidateUserAttribute() : base()
        {
            var compositionRoot = CompositionRootBackend.Instance;
            _userWorker = compositionRoot.GetImplementation<UserWorker>();
        }

        public override bool IsValid(object value)
        {
            var userId = (value as int?).GetValueOrDefault();
            if (userId == 0)
            {
                return false;
            }

            var user = _userWorker.GetById(userId);
            if (user == null)
            {
                ErrorMessage = "The account could not be retrieved!";
                return false;
            }

            return true;
        }
    }
}