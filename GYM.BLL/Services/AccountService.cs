using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYM.BLL.Interfaces;
using GYM.BLL.ModelViews.AccountsModelView;
using GYM.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace GYM.BLL.Services
{
    public class AccountService(UserManager<ApplicationUser> _userManager) : IAccountService
    {

        async Task<ApplicationUser?> IAccountService.ValidateUser(LoginViewModel loginViewModel)
        {
            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
            if (user is null) return null;

            var isPasswordValid = await _userManager.CheckPasswordAsync(user,loginViewModel.Password);

            if (isPasswordValid)
            {
                return user;
            }

            return null;
        }
    }
}
