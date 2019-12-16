using System;
using System.Linq;
using System.Security.Claims;
using CyberSecurityBase.Feedback.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CyberSecurityBase.Feedback.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private FeedbackContext _context;

        public FeedbackController(FeedbackContext context)
        {
            _context = context;
        }

        private string WhoAmI()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var sub = claimsIdentity.FindFirst("sub");
            return sub.Value;
        }

        // GET api/feedback
        [HttpGet]
        [Route("search")]
        [Authorize]
        public IActionResult Search(string keywords)
        {
            var sub = WhoAmI();
            var query = $"SELECT * FROM Feedbacks WHERE UserId = '{sub}' AND Content LIKE '%{keywords}%'";
            var results = _context.Feedbacks.FromSql(query).ToList();
            return Ok(results);
        }

        // GET api/feedback
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            var sub = WhoAmI();
            var feedbacks = _context.Feedbacks.Where(x => x.UserId == sub).ToList();
            return Ok(feedbacks);
        }

        // GET api/feedback
        [HttpGet]
        [Route("admin")]
        [Authorize]
        public IActionResult GetAllFeedbacks()
        {
            var feedbacks = _context.Feedbacks.ToList();
            return Ok(feedbacks);
        }

        // GET api/feedback/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            throw new NotImplementedException();
        }

        // POST api/feedback
        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] Models.Feedback feedback)
        {
            var sub = WhoAmI();

            feedback.UserId = sub;

            _context.Feedbacks.Add(feedback);
            var id = _context.SaveChanges();
            return Ok(id);
        }

        // PUT api/feedback/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/feedback/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
