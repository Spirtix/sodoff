using System;

namespace sodoff.Schema;

public enum DeleteProfileStatus {
    SUCCESS = 1,
    NOT_A_PROFILE_ACCOUNT = 3,
    OWNER_ID_NOT_FOUND = 4,
    PROFILE_NOT_OWNED_BY_THIS_OWNER = 5,
    PROFILE_NOT_FOUND = 6
}
