namespace Core.Utilities.PasswordUtilities.Base
{
    public interface IPasswordUtility
    {
        (byte[] Hash, byte[] Salt) HashPassword(string password);
        bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt);
    }
}
