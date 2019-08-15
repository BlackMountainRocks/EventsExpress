﻿using EventsExpress.Core.DTOs;
using EventsExpress.Core.Infrastructure;
using EventsExpress.Core.IServices;
using EventsExpress.Db.Helpers;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;


namespace EventsExpress.Core.Services
{
    public class AuthServicre : IAuthServicre
    {
        private readonly IUserService _userService;
        private readonly IJwtSigningEncodingKey _signingEncodingKey;

        public AuthServicre(
            IUserService userSrv, 
            IJwtSigningEncodingKey signingEncodingKey)
        {
            _userService = userSrv;
            _signingEncodingKey = signingEncodingKey;
        }


        public OperationResult Authenticate(string email, string password)
        {
            var user = _userService.GetByEmail(email);
            if (user == null)
            {
                return new OperationResult(false, $"User with email: {email} not found", "email");
            }

            if (user.IsBlocked)
            {
                return new OperationResult(false, $"{email}, your account was blocked.", "email");
            }

            if (!user.EmailConfirmed)
            {
                return new OperationResult(false, $"{email} is not confirmed, please confirm", "");
            }

            if (!VerifyPassword(user, password))
            {
                return new OperationResult(false, "Invalid password", "Password");
            }

            var token = GenerateJwt(user);

            return new OperationResult(true, token, "");
        }


        public OperationResult FirstAuthenticate(UserDTO userDto)
        {
            if (userDto == null)
            {
                return new OperationResult(false, $"User with email: {userDto.Email} not found", "email");
            }
            var token = GenerateJwt(userDto);

            return new OperationResult(true, token, "");
        }

        public async Task<OperationResult> ChangePasswordAsync(UserDTO userDto, string oldPassword, string newPassword)
        {
            if (VerifyPassword(userDto, oldPassword))
            {
                userDto.PasswordHash = PasswordHasher.GenerateHash(newPassword);

                return await _userService.Update(userDto);
            }
            return new OperationResult(false, "Invalid password", "");
        }

        public UserDTO GetCurrentUser(ClaimsPrincipal userClaims)
        {
            string email = userClaims.FindFirst(ClaimTypes.Email).Value;
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }
            return _userService.GetByEmail(email);
        }


        private bool VerifyPassword(UserDTO user, string actualPassword) => 
            (user.PasswordHash == PasswordHasher.GenerateHash(actualPassword));


        private string GenerateJwt(UserDTO user)
        {
            var claims = new []
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.Name),
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(3000),
                signingCredentials: new SigningCredentials(
                        _signingEncodingKey.GetKey(),
                        _signingEncodingKey.SigningAlgorithm)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
