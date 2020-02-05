using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DatingApp.Api.Data;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

public interface IUserService
{
    Task<User> Login(string UserName, string password);
    Task<User> RegisterUser(User user, string password);
    Task<bool> UserExits(string username);
}

public class UserService : IUserService
{
    private readonly DataContext _context;

    public UserService(DataContext context)
    {
        _context = context;
    }

    public async Task<User> Login(string UserName, string password)
    {
        var user = _context.Users.FirstOrDefault(x => x.UserName == UserName);
        if (user == null)
            return null;

        if (!VarifyPassword(password, user.PasswordHash, user.PasswordSalt))
        {
            return null;
        }
        return user;
    }

    public async Task<bool> UserExits(string username)
    {
        return await _context.Users.AnyAsync(x => x.UserName == username);
    }
    private bool VarifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        byte[] computedhash;
        computedhash = KeyDerivation.Pbkdf2(
                    password: password,
                    salt: passwordSalt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8);
        for (int i = 0; i < computedhash.Length; i++)
        {
            if (computedhash[i] != passwordHash[i])
                return false;
        }
        return true;
    }

    public async Task<User> RegisterUser(User user, string password)
    {
        byte[] passwordHash, passwordSalt;
        CreatePasswordHash(password, out passwordHash, out passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        // generate a 128-bit salt using a secure PRNG
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        //Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");
        passwordSalt = salt;
        // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
        passwordHash = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8);
    }
}