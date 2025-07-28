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
        LogoutSucceeded,
        LogoutFailed,

        LogoutAllSucceeded,
        LogoutAllFailed,
        
        LogoutOthersSucceeded,
        LogoutOthersFailed,
        
        PasswordChangeSucceeded,
        PasswordChangeFailed,
        
        PasswordResetSucceeded,
        PasswordResetFailed,
        
        PasswordResetRequestSucceeded,
        PasswordResetRequestFailed,
        
        SessionRefreshSucceeded,
        SessionRefreshFailed,
        
        VerificationResendSucceeded,
        VerificationResendFailed,
        
        SignInSucceeded,
        SignInFailed,
        
        SignUpSucceeded,
        SignUpFailed,
        
        VerificationSignInOTPSucceeded,
        VerificationSignInOTPFailed,

        VerificationOTPEmailFailed,
        VerificationOTPEmailSucceeded,

        VerificationEmailFailed,
        VerificationEmailSucceeded
    }

    public static class SecurityEventTypeGuids
    {
        public static readonly Dictionary<SecurityEventTypeGuid, Guid> EventGuids = new()
        {
            { SecurityEventTypeGuid.LogoutSucceeded,                Guid.Parse("026c77ce-25d0-4294-8c0f-9dd1ccb41b7c") },
            { SecurityEventTypeGuid.LogoutFailed,                   Guid.Parse("d6821ea9-0a7b-42e6-8b2e-3aa42c65902c") },
            { SecurityEventTypeGuid.LogoutAllSucceeded,             Guid.Parse("504c6de6-dfa9-4b2c-9cc5-7c4fe0e36ae0") },
            { SecurityEventTypeGuid.LogoutAllFailed,                Guid.Parse("df326243-ecea-4224-92ef-6930bba3124d") },
            { SecurityEventTypeGuid.LogoutOthersSucceeded,          Guid.Parse("982c8c6c-db72-4ac1-aa10-ab4863e4c3a3") },
            { SecurityEventTypeGuid.LogoutOthersFailed,             Guid.Parse("d5efa0ab-1239-4dd2-94aa-41f94c667113") },
            { SecurityEventTypeGuid.PasswordChangeSucceeded,        Guid.Parse("64bab248-57ad-4cc5-bc18-5290efdb3f31") },
            { SecurityEventTypeGuid.PasswordChangeFailed,           Guid.Parse("de3810fc-58a6-461f-a03c-5c0710440b36") },
            { SecurityEventTypeGuid.PasswordResetSucceeded,         Guid.Parse("c8a345b9-b563-459c-8b01-aa98fc36c27e") },
            { SecurityEventTypeGuid.PasswordResetFailed,            Guid.Parse("3fc5bed0-5532-4c63-b6a5-15ee46599acb") },
            { SecurityEventTypeGuid.PasswordResetRequestSucceeded,  Guid.Parse("b209b5d5-b507-4e17-940f-9e50b5917939") },
            { SecurityEventTypeGuid.PasswordResetRequestFailed,     Guid.Parse("b7b51abf-89ba-433c-92f7-86a80f72b60e") },
            { SecurityEventTypeGuid.SessionRefreshSucceeded,        Guid.Parse("f2e8ff03-e342-4289-862d-76a8cd98ba63") },
            { SecurityEventTypeGuid.SessionRefreshFailed,           Guid.Parse("9f50ecbd-0291-4dc5-a14a-d8be2971db07") },
            { SecurityEventTypeGuid.VerificationResendSucceeded,    Guid.Parse("ec46be4f-01dc-4842-81b2-a5be04fe4015") },
            { SecurityEventTypeGuid.VerificationResendFailed,       Guid.Parse("d072d951-a14f-4d6b-9b52-559dac455109") },
            { SecurityEventTypeGuid.SignInSucceeded,                Guid.Parse("b0c1f8d2-3a4e-4c5b-9f6d-7e1f3c8b2c5a") },
            { SecurityEventTypeGuid.SignInFailed,                   Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890") },
            { SecurityEventTypeGuid.SignUpSucceeded,                Guid.Parse("4eb65105-dbbb-474b-a025-9ec6b0c85d3e") },
            { SecurityEventTypeGuid.SignUpFailed,                   Guid.Parse("555ee376-d85d-4553-a80f-4d17ae54f34a") },
            { SecurityEventTypeGuid.VerificationSignInOTPSucceeded, Guid.Parse("e3cdf08d-7fb2-4d9a-ac74-fd50ec52e7af") },
            { SecurityEventTypeGuid.VerificationSignInOTPFailed,    Guid.Parse("bf008257-2060-48bb-8370-3b0c299dadec") },
            { SecurityEventTypeGuid.VerificationOTPEmailFailed,     Guid.Parse("f615cf02-9131-4148-9494-9f905eac121a") },
            { SecurityEventTypeGuid.VerificationOTPEmailSucceeded,  Guid.Parse("083901f1-9d89-4b09-9a50-f720c2e97782") },
            { SecurityEventTypeGuid.VerificationEmailFailed,        Guid.Parse("ade70474-5ae5-4654-ac1e-89055bc0b745") },
            { SecurityEventTypeGuid.VerificationEmailSucceeded,     Guid.Parse("8e72d526-dd81-4826-809d-9f45f4536eaa") },
        };
    }
}














