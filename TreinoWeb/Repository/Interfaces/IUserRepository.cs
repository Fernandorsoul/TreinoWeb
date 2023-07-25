using Microsoft.AspNetCore.Mvc;
using TreinoWeb.Models;

namespace TreinoWeb.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<List<UserModel>> GetUsers();
        Task<ActionResult<UserModel>> GetUserById(int id);
        Task<ActionResult<UserModel>> GetUpdate(UserModel user,int id);
        Task<ActionResult<UserModel>> GetCreated(UserModel user);
        Task<bool> Delete(int id); 
    }
}
