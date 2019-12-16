using Microsoft.EntityFrameworkCore;

namespace CyberSecurityBase.Feedback.Api.Models
{

    public class FeedbackContext : DbContext
    {
        public FeedbackContext()
        { }

        public FeedbackContext(DbContextOptions<FeedbackContext> options)
            : base(options)
        { }

        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0");
            }
        }
    }
}
