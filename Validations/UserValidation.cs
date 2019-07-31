using FluentValidation;

namespace KütüphaneAPI.Validations
{
    public class UserValidation : AbstractValidator<userRequest>
    {
        public UserValidation()
        {
            RuleFor(x => x.name)
            .Cascade(CascadeMode.Continue)
            .NotEmpty().WithMessage("UserName boş bırakılamaz")
            .MinimumLength(1).WithMessage("UserName 1 karakterden küçük olamaz")
            .MaximumLength(25).WithMessage("UserName 25 karakterden fazla olamaz");

            RuleFor(x => x.lastname)
            .Cascade(CascadeMode.Continue)
            .NotEmpty().WithMessage("UserLastName boş bırakılamaz")
            .MinimumLength(3).WithMessage("UserLastName 3 karakterden küçük olamaz")
            .MaximumLength(25).WithMessage("UserLastName 25 karakterden fazla olamaz");

            RuleFor(x => x.email)
            .Cascade(CascadeMode.Continue)
            .EmailAddress().WithMessage("Lütfen düzgün bir e-mail adresi giriniz");

            RuleFor(x => x.adress)
            .Cascade(CascadeMode.Continue)
            .NotEmpty().WithMessage("Adress bilgisi boş geçilemez")
            .MinimumLength(5).WithMessage("Adress bilgisi 5 karakterden az olamaz")
            .MaximumLength(30).WithMessage("Adress bilgisi 30 karakterden fazla olamaz");
        }
    }
}