namespace ExperimentAgent.API.Validators.ExperimentRequest;

/// <summary>
/// Class for ExperimentRequest validation
/// </summary>
public class ExperimentRequestValidator : AbstractValidator<Models.ExperimentRequest>
{
    public ExperimentRequestValidator()
    {
        RuleFor(ex => ex.Query).NotEmpty();
        RuleFor(ex => ex.QueryExecutionNumber).Must(num => num > 0);
    }
}