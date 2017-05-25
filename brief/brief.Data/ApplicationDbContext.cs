﻿namespace brief.Data
{
    using System.Data.Entity;
    using System.Reflection;
    using System.Threading.Tasks;

    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(string connectionString) : base(connectionString)
        {
            Database.SetInitializer<ApplicationDbContext>(new CreateDatabaseIfNotExists<ApplicationDbContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        public Task<int> Commit()
            => SaveChangesAsync();
    }
}
