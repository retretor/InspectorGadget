using Application.Common.Enums;

namespace Application.Common.Models;

public class ResultError
{
    public ResultErrorEnum Code { get; init; }
    public string Message { get; init; }

    public ResultError(ResultErrorEnum code)
    {
        Code = code;
        Message = code.ToString();
    }

    public ResultError(string message)
    {
        Code = ResultErrorEnum.Unknown;
        Message = message;
    }
}