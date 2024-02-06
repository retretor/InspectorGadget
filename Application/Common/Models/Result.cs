namespace Application.Common.Models;

public class Result
{
    private Result(bool succeeded, IEnumerable<ResultError>? errors)
    {
        Succeeded = succeeded;
        Errors = errors?.ToArray() ?? Array.Empty<ResultError>();
    }

    public bool Succeeded { get; init; }

    public ResultError[] Errors { get; init; }

    public static Result Success()
    {
        return new Result(true, Array.Empty<ResultError>());
    }

    public static Result Failure(IEnumerable<ResultError>? errors)
    {
        return new Result(false, errors);
    }
}