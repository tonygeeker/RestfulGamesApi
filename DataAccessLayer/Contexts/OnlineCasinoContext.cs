using Microsoft.EntityFrameworkCore;
using RestfulGamesApi.DataAccessLayer.Models;

namespace RestfulGamesApi.DataAccessLayer.Contexts
{
    public class OnlineCasinoContext: DbContext
    {
        public OnlineCasinoContext(DbContextOptions<OnlineCasinoContext> options)
            : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<GameCollection> GameCollections { get; set; }
        public DbSet<Image> GameThumbnails { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AuthenticationToken> AuthenticationTokens { get; set; }
    }
}
