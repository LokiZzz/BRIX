﻿namespace BRIX.GameService.Contracts.Account
{
    public class ResetPasswordRequest
    {
        public string UserId { get; set; } = string.Empty;
        
        public string Password { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;
    }
}
