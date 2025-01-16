namespace BRIX.Web.Problems
{
    public static class ProblemCodes
    {
        public const string UnknownError = nameof(UnknownError);

        public static class Account
        {
            public const string InvalidCredentials = nameof(InvalidCredentials);
            public const string UserNotFound = nameof(UserNotFound);
            public const string WrongResetToken = nameof(WrongResetToken);
            public const string NeedToConfirmAccount = nameof(NeedToConfirmAccount);
        }
    }
}
