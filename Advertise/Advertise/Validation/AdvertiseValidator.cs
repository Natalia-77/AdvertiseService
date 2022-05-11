using AdvertisePublish.Models;
using FluentValidation;

namespace AdvertisePublish.Validation
{
    public class AdvertiseValidator : AbstractValidator<CreateAdvertiseViewModel>
    {
        public AdvertiseValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title can't be empty")
                .Length(2, 200).WithMessage("Title has range from 2 to 200 characters");
            RuleFor(y => y.Description)
                .NotEmpty().WithMessage("Description can't be empty")
                .Length(10, 1000).WithMessage("Description has range from 10 to 1000 characters");
            RuleFor(d => d.DateCreate)
                .NotEmpty().WithMessage("Date must be fill");
            RuleFor(i => i.Images).ListMustBeFewerThan(4);
        }
    }

    public static class CustomValidators
    {
        public static IRuleBuilderOptions<T, IList<TElement>> ListMustBeFewerThan<T, TElement>(this IRuleBuilder<T, IList<TElement>> ruleBuilder, int num)
        {
            return ruleBuilder.Must(list => list.Count < num).WithMessage("You can upload only three images");
        }
    }
}
