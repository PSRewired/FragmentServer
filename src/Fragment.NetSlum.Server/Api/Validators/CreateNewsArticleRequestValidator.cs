using FastEndpoints;
using FluentValidation;
using Fragment.NetSlum.Server.Api.Endpoints.News;

namespace Fragment.NetSlum.Server.Api.Validators;

public class CreateNewsArticleRequestValidator : Validator<CreateNewsArticleRequest>
{
    public CreateNewsArticleRequestValidator()
    {
        RuleFor(a => a.Title)
            .MaximumLength(33);
        RuleFor(a => a.Content)
            .MaximumLength(412);
    }
}
