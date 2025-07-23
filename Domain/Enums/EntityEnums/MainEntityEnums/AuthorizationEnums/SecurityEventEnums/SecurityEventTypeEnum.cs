using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Domain.Enums.EntityEnums.MainEntityEnums.AuthorizationEnums.SecurityEventEnums
{
    public enum SecurityEventTypeGuid
    {
        AccountCreated,
        LogginAttemptWithEmail,
        LogginAttemptWithPhone,
        LoginSuccess,
        LoginFailure,
        SessionCreated,
        SessionRefreshed,
        SessionTerminated,
        VerificationOTPResend,
        VerificationOTPSuccess,
        VerificationOTPFailed,
        Logout,
        LogoutAll,
        LogoutOthers,
        PasswordChange,
        PasswordResetRequest,
        PasswordResetSuccess,
        PasswordResetFailed,
        VerificationEmail,
        VerificationPhone,
    }

    public static class SecurityEventTypeGuids
    {
        public static readonly Dictionary<SecurityEventTypeGuid, Guid> EventGuids = new()
        {
            { SecurityEventTypeGuid.AccountCreated, Guid.Parse("026c77ce-25d0-4294-8c0f-9dd1ccb41b7c") },
            { SecurityEventTypeGuid.LogginAttemptWithEmail, Guid.Parse("d6821ea9-0a7b-42e6-8b2e-3aa42c65902c") },
            { SecurityEventTypeGuid.LogginAttemptWithPhone, Guid.Parse("504c6de6-dfa9-4b2c-9cc5-7c4fe0e36ae0") },
            { SecurityEventTypeGuid.LoginSuccess, Guid.Parse("df326243-ecea-4224-92ef-6930bba3124d") },
            { SecurityEventTypeGuid.LoginFailure, Guid.Parse("982c8c6c-db72-4ac1-aa10-ab4863e4c3a3") },
            { SecurityEventTypeGuid.SessionCreated, Guid.Parse("d5efa0ab-1239-4dd2-94aa-41f94c667113") },
            { SecurityEventTypeGuid.SessionRefreshed, Guid.Parse("64bab248-57ad-4cc5-bc18-5290efdb3f31") },
            { SecurityEventTypeGuid.SessionTerminated, Guid.Parse("de3810fc-58a6-461f-a03c-5c0710440b36") },
            { SecurityEventTypeGuid.VerificationOTPResend, Guid.Parse("c8a345b9-b563-459c-8b01-aa98fc36c27e") },
            { SecurityEventTypeGuid.VerificationOTPSuccess, Guid.Parse("3fc5bed0-5532-4c63-b6a5-15ee46599acb") },
            { SecurityEventTypeGuid.VerificationOTPFailed, Guid.Parse("b209b5d5-b507-4e17-940f-9e50b5917939") },
            { SecurityEventTypeGuid.Logout, Guid.Parse("b7b51abf-89ba-433c-92f7-86a80f72b60e") },
            { SecurityEventTypeGuid.LogoutAll, Guid.Parse("f2e8ff03-e342-4289-862d-76a8cd98ba63") },
            { SecurityEventTypeGuid.LogoutOthers, Guid.Parse("9f50ecbd-0291-4dc5-a14a-d8be2971db07") },
            { SecurityEventTypeGuid.PasswordChange, Guid.Parse("ec46be4f-01dc-4842-81b2-a5be04fe4015") },
            { SecurityEventTypeGuid.PasswordResetRequest, Guid.Parse("d072d951-a14f-4d6b-9b52-559dac455109") },
            { SecurityEventTypeGuid.PasswordResetSuccess, Guid.Parse("b0c1f8d2-3a4e-4c5b-9f6d-7e1f3c8b2c5a") },
            { SecurityEventTypeGuid.PasswordResetFailed, Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890") },
            { SecurityEventTypeGuid.VerificationEmail, Guid.Parse("4eb65105-dbbb-474b-a025-9ec6b0c85d3e") },
            { SecurityEventTypeGuid.VerificationPhone, Guid.Parse("555ee376-d85d-4553-a80f-4d17ae54f34a") },
        };
    }
}














