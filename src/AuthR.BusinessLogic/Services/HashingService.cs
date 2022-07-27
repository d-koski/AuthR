using System.Security.Cryptography;
using AuthR.BusinessLogic.Abstractions.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace AuthR.BusinessLogic.Services;

public class HashingService : IHashingService
{
    private const int HashLength = 256;
    private const int SaltLength = 128;
    private const int SaltedHashLength = HashLength + SaltLength;
    private const int EncodedSaltedHashLength = SaltedHashLength * 4 / 3;
    private const int IterationCount = 64000;
    
    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltLength);
        var hash = HashPassword(password, salt);

        var saltedHashBytes = new byte[SaltedHashLength];
        Buffer.BlockCopy(hash, 0, saltedHashBytes, 0, HashLength);
        Buffer.BlockCopy(salt, 0, saltedHashBytes, HashLength, SaltLength);

        var saltedHash = Convert.ToBase64String(saltedHashBytes);
        return saltedHash;
    }
    
    public bool VerifyPassword(string passwordHash, string password)
    {
        if (passwordHash.Length != EncodedSaltedHashLength)
            throw new ArgumentException("Invalid password hash length.", nameof(passwordHash));
        
        var saltedHash = Convert.FromBase64String(passwordHash);
        var hash = saltedHash[..HashLength];
        var salt = saltedHash[HashLength..];

        var hash2 = HashPassword(password, salt);
        return hash.SequenceEqual(hash2);
    }

    private static byte[] HashPassword(string password, byte[] salt)
    {
        var hash = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: IterationCount,
            numBytesRequested: HashLength);
        return hash;
    }
}