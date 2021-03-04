using System.Threading.Tasks;
using Refactorizando.Shared.Data.Models.Auth;

namespace Refactorizando.Client.Data.Services
{
    public interface ILoginService
    {
        Task Login(UserToken token);
        Task Logout();
        
    }
}