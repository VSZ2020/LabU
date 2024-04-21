using LabU.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace LabU.Data.Repository
{
    public class UnitOfWork
    {
        public UnitOfWork(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        readonly DataContext _dataContext;

        IAuthService _authService;
        IUserService _userService;
        IPersonRepository _personService;
        ITaskRepository _taskService;
        ISubjectRepository _subjectService;
        ITaskResponseService _responseService;
        IRoleService _roleService;

        public IAuthService AuthService => _authService ??= new DefaultAuthService(_dataContext);
        public IUserService UserService => _userService ??= new DefaultUsersRepository(_dataContext);
        public IPersonRepository PersonService => _personService ??= new DefaultPersonRepository(_dataContext);
        public ITaskRepository TasksService => _taskService ??= new DefaultTaskRepository(_dataContext);
        public ISubjectRepository SubjectsService => _subjectService  ??= new DefaultSubjectsRepository(_dataContext);
        public ITaskResponseService ResponseService => _responseService  ??= new DefaultTaskResponseRepository(_dataContext);
        public IRoleService RoleService => _roleService ??= new DefaultRoleService(_dataContext);

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                await _dataContext.SaveChangesAsync();
              
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
