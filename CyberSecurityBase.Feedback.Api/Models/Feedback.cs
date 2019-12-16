using System.ComponentModel.DataAnnotations.Schema;

namespace CyberSecurityBase.Feedback.Api.Models
{
    public class Feedback
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string Email { get; set; }
        public string Created { get; set; }
    }
}
