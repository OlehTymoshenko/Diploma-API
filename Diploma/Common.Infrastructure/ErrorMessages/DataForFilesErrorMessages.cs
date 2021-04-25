namespace Common.Infrastructure.ErrorMessages
{
    public class DataForFilesErrorMessages
    {
        #region Errors messages

        public const string ObjectWasNotFoundDuringUpdate = "The passed object was not found on updating";

        public const string ObjectWithIdWasNotFoundDuringDelete = "A object with the passed Id was not found on deleting";

        #endregion

        /*#region Publishing house errors messages

        public const string InvalidCredentials = "Invalid credintials";

        public const string RefreshTokensNotExists = "Refresh tokens for the provided credentials don't exist";

        public const string EmailAlreadyExistsInDB = "Email is already registred";

        public const string UserWithProvidedEmailDoesntExists = "User with provided email doesn't exists in the DB";

        public const string InvalidAccessToken = "Invalid access token";

        public const string InvalidRefreshToken = "Invalid refresh token";

        #endregion */

    }
}
