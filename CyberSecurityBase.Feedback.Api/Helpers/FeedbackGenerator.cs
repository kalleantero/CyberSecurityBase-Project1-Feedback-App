using CyberSecurityBase.Feedback.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace CyberSecurityBase.Feedback.Api.Helpers
{
    public class FeedbackGenerator
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new FeedbackContext(
                serviceProvider.GetRequiredService<DbContextOptions<FeedbackContext>>()))
            {
                var feedbacks = context.Feedbacks.ToList();

                context.RemoveRange(feedbacks);

                context.SaveChanges();

                context.Feedbacks.AddRange(
                    new Models.Feedback
                    {
                        UserId = "1",
                        FirstName = "Jane",
                        LastName = "Doe",
                        Subject = "Good job!",
                        Content = "Your service is great. Check my new service <a href='https://localhost:44361/'>here</a>"
                    },
                    new Models.Feedback
                    {
                        UserId = "2",
                        FirstName = "John",
                        LastName = "Doe",
                        Subject = "A question",
                        Content = "When your service is open? I didn't find any information from your web site."
                    },
                    new Models.Feedback
                    {
                        UserId = "2",
                        FirstName = "John",
                        LastName = "Doe",
                        Subject = "Feedback",
                        Content = "Nice and useful app!"
                    },
                    new Models.Feedback
                    {
                        UserId = "2",
                        FirstName = "John",
                        LastName = "Doe",
                        Subject = "Security issues",
                        Content = "I have found some security holes from the feedback app. When can I call you?"
                    },
                    new Models.Feedback
                    {
                        UserId = "1",
                        FirstName = "Jane",
                        LastName = "Doe",
                        Subject = "Lorem Ipsum",
                        Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s"
                    },
                    new Models.Feedback
                    {
                        UserId = "1",
                        FirstName = "Jane",
                        LastName = "Doe",
                        Subject = "Hi",
                        Content = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable"
                    }
                    );

                context.SaveChanges();
            }
        }
    }
}
