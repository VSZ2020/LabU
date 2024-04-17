using LabU.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabU.Core.Interfaces
{
    public interface IAuthService
    {
        Task<UserEntity?> AuthUserAsync(string login, string password);

        int GetLoginAttemptsCount(string login, int maxAttemptsCount = 5);
    }
}
