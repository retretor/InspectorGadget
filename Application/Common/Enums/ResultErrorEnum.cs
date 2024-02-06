namespace Application.Common.Enums;
// TODO: write all errors here
public enum ResultErrorEnum
{
    UserNotFound,
    
    InvalidLogin,
    InvalidPassword,
    InvalidToken,
    
    UserAlreadyExists,
    UserIsNotInRole,
    UserIsInRole,
    UserIsNotAuthorized,
    UserIsAuthorized,
    UserIsNotDeleted,
    UserIsDeleted
}