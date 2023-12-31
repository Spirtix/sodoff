﻿namespace sodoff.Schema;
public enum MembershipUserStatus {
    Success,
    InvalidUserName,
    InvalidPassword,
    InvalidEmail,
    DuplicateUserName,
    DuplicateEmail,
    InvalidCulture,
    InvalidDOB,
    IsDeleted,
    InvalidApiToken,
    ValidationError,
    UserCreationFailed,
    TokenNotFound,
    InvalidChildUserName,
    InvalidChildDOB,
    InvalidPreRegisteredEmail,
    InvalidGuestChildUserName,
    GuestProfileNotFound,
    GuestAccountNotAnonymous,
    GuestAccountNotFound,
    FacebookAccessTokenFailed,
    DuplicateFacebookUserID,
    UnmatchedUserNameFacebookUserID,
    UnlinkedFacebookID,
    ValidateSignatureFailed,
    ProductGroupNotFound,
    DataDeserializationFailed,
    ChildNotAdded,
    NoChildData,
    InvalidSSOUserID,
    UnlinkedSSOUserID,
    ExternalCallFailed,
    IPAddressInvalid,
    EmailLengthInvalid,
    EmailBlocked,
    EmailCannotBeUsed,
    RegistrationFailedTryAgain,
    IPAddressBlocked,
    LoginAttemptsTooMany,
    UserIsBanned,
    GenderIsBlank,
    PasswordRecoveryAttemptsTooMany,
    UserNameLengthMismatch,
    UserNameRestriction,
    UserNameContainsProductName,
    UserNameContainsBadWord,
    PasswordLengthMismatch,
    PasswordContainsInvalidChars,
    PasswordShouldContainNumbers,
    PasswordIsOfSameCharacter,
    PasswordIsNotSecure,
    PasswordIsSameAsUserName,
    EmailContainsUserName,
    EmailDoesNotMatchWithRegisteredEmail,
    EmailUseExceededLimit,
    ExternalSystemFailed,
    FacebookAuthProviderMissing,
    FaceBookBusinessAPIError,
    FaceBookInvalidUserIDToken,
    FaceBookLoginSuccess,
    FaceBookLoginFailed,
    FaceBookUserEmpty,
    FaceBooktokenEmpty,
    FaceBookLinkExists,
    FaceBookNoLink,
    FaceBookUnderAge,
    FaceBookGraphError,
    UserNameDoesNotExists,
    EmailIsEmpty,
    EmailIsBad,
    EmailShouldBeInLowerCase,
    UserPolicyNotAccepted,
    ProviderError = 9999
}
