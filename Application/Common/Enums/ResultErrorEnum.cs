namespace Application.Common.Enums;
// TODO: write all errors here
public enum ResultErrorEnum
{
    Unknown = -1,
    
    UserNotFound,

    InvalidLogin,
    InvalidPassword,
    InvalidToken,
    SecretKeyMismatch,

    UserAlreadyExists,
    UserIsNotInRole,
    UserIsInRole,
    UserIsNotAuthorized,
    UserIsAuthorized,
    UserIsNotDeleted,
    UserIsDeleted,
    AccessDenied,
    UnableToConnectToDatabase,
    InvalidDbContext
}