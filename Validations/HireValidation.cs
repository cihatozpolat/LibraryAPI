using FluentValidation;

namespace KütüphaneAPI.Validations
{
    public class HireValidation : AbstractValidator<hireRequest>
    {
        public HireValidation()
        {

            CascadeMode = CascadeMode.Continue;

            RuleFor(x => x.bookid)
            .Cascade(CascadeMode.Continue)
            .NotEmpty().WithMessage("BookId boş olamaz!")
            .GreaterThan(0).WithMessage("BookId 1'den küçük olamaz!");

            RuleFor(x => x.userid)
            .Cascade(CascadeMode.Continue)
            .NotEmpty().WithMessage("UserId boş olamaz!")
            .GreaterThan(0).WithMessage("UserId 1'den küçük olamaz!");

            RuleFor(x => x.startdate)
            .Cascade(CascadeMode.Continue)
            .NotEmpty().WithMessage("StartDate boş olamaz!")
            .GreaterThanOrEqualTo(System.DateTime.Now).WithMessage("StartDate geçmiş bir zaman olamaz!");

            RuleFor(x => x.enddate)
            .Cascade(CascadeMode.Continue)
            .NotEmpty().WithMessage("EndDate boş olamaz!")
            .GreaterThanOrEqualTo(x => x.startdate).WithMessage("EndDate, StartDate'den önceki bir zaman olamaz!");

            RuleFor(x => x.deliverydate)
            .Cascade(CascadeMode.Continue)
            .GreaterThan(x => x.startdate).WithMessage("DeliveryDate, StartDate'den önceki bir zaman olamaz!")
            .LessThan(x => x.enddate).WithMessage("DeliveryDate, EndDate'den sonraki bir zaman olamaz!");
        }
    }
}