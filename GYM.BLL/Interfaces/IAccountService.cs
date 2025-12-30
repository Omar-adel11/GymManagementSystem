using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYM.BLL.ModelViews.AccountsModelView;
using GYM.DAL.Entities;

namespace GYM.BLL.Interfaces
{
    public interface IAccountService
    {
        Task<ApplicationUser?> ValidateUser(LoginViewModel loginViewModel);
    }
}
