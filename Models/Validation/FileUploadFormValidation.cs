using FluentValidation;
using Models.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Validation
{
    public class FileUploadFormValidation : AbstractValidator<FileModelDto>
    {
        
        public FileUploadFormValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(1, 100);

            RuleFor(x => x.File)
            .NotEmpty();

            When(x => x.File != null, () =>
            {
                RuleFor(x => x.File.Size).LessThanOrEqualTo(10485760).WithMessage("The maximum file size is 10 MB");
            });
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<FileModelDto>.CreateWithOptions((FileModelDto)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
        

    }
}
