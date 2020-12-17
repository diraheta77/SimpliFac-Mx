using simplifile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace simplifile.Data
{
    public class UserDbProvider : IUserPasswordStore<Usuario>, IUserEmailStore<Usuario>, IUserRoleStore<Usuario>
    {
        private readonly ApplicationDbContext _context;

        public UserDbProvider(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
        }

        public async Task<IdentityResult> CreateAsync(Usuario user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            _context.Usuarios.Add(user);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError() { Description = $"Could not create user {user.UsuCorreo}." });
        }

        public async Task<IdentityResult> DeleteAsync(Usuario user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            var userFromDb = await _context.Usuarios.FindAsync(user.UsuId);
            _context.Remove(userFromDb);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError() { Description = $"Could not delete user {user.UsuCorreo}." });
        }

        public async Task<Usuario> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _context.Usuarios.SingleOrDefaultAsync(u => u.UsuId.ToString().Equals(userId), cancellationToken);
        }

        public async Task<Usuario> FindByNameAsync(string normalizedUserName,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return  await _context.Usuarios.SingleOrDefaultAsync(u => u.UsuCorreo.Equals(normalizedUserName.ToLower()),cancellationToken);
        }

        
        public Task<string> GetNormalizedUserNameAsync(Usuario user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.UsuCorreo);
        }

        public Task<string> GetUserIdAsync(Usuario user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.UsuId.ToString());
        }

        public Task<string> GetUserNameAsync(Usuario user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.UsuCorreo);
        }

        public Task SetNormalizedUserNameAsync(Usuario user, string normalizedName,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult<object>(null);
        }

        public Task SetUserNameAsync(Usuario user, string userName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.UsuCorreo = userName;
            return Task.FromResult<object>(null);
        }

        public async Task<IdentityResult> UpdateAsync(Usuario user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            _context.Update(user);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError() { Description = $"Could not update user {user.UsuCorreo}." });
        }

        public Task<string> GetPasswordHashAsync(Usuario user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(Usuario user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(Usuario user, string passwordHash, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.PasswordHash = passwordHash;
            return Task.FromResult<object>(null);
        }

        public async Task<Usuario> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _context.Usuarios.SingleOrDefaultAsync(u => u.UsuCorreo.Equals(normalizedEmail),
                cancellationToken);
        }

        public Task<string> GetEmailAsync(Usuario user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.UsuCorreo);
        }

        public Task<bool> GetEmailConfirmedAsync(Usuario user, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(true);
        }

        public Task<string> GetNormalizedEmailAsync(Usuario user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.UsuCorreo);
        }

        public Task SetEmailAsync(Usuario user, string email, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.UsuCorreo = email;
            return Task.FromResult<object>(null);
        }

        public Task SetEmailConfirmedAsync(Usuario user, bool confirmed, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<object>(null);
        }

        public Task SetNormalizedEmailAsync(Usuario user, string normalizedEmail,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult<object>(null);
        }

        public Task AddToRoleAsync(Usuario user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(Usuario user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(Usuario user, CancellationToken cancellationToken)
        {
            IList<string> x = new List<string>() { user.UsuRol };

            return Task.FromResult(x);

        }

        public Task<bool> IsInRoleAsync(Usuario user, string roleName, CancellationToken cancellationToken)
        {
            if (user.UsuRol == roleName) return Task.FromResult(true);

            return Task.FromResult(false);
        }

        public Task<IList<Usuario>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        
    }
}
