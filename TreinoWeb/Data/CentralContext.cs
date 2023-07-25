using Microsoft.EntityFrameworkCore;
using TreinoWeb.Models;

namespace TreinoWeb.Data
{
    public class CentralContext:DbContext
    {
        public CentralContext(DbContextOptions<CentralContext>option) : base(option)
        {
                      
        }
        public DbSet<UserModel> Usuarios { get; set; }
        public DbSet<TreinosModel> Treinos { get; set; }
        public DbSet<ExercicioModel> Exercicios { get; set; }

       
    }
}
