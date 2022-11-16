using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.Services
{
    public interface IAccountService
    {
        Task<bool> SignInAsync(string login, string password);
    }

    public class AccountServiceMoq : IAccountService
    {
        public async Task<bool> SignInAsync(string login, string password)
        {
            bool isSuccess = !string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password);
            await Task.Delay(2000);

            return await Task.FromResult(isSuccess);
        }
    }
}
