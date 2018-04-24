using API_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace API_Server.Controllers
{
    public class ReviewController : ApiController
    {
        BooksEntities db = new BooksEntities();
        [ResponseType(typeof(Review))]
        public IHttpActionResult PostReview(ReviewVM reviewVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            reviewVM.Date = DateTime.Now;
            Review review = ConvertClass.ReviewVMToModel(reviewVM);
            db.Reviews.Add(review);
            db.SaveChanges();
            return Ok(review);
        }
        [ResponseType(typeof(ReviewVM))]
        [HttpGet]
        [Route("api/Review/{buyerID:int}/{bookID:int}")]
        public IHttpActionResult GetProduct(int buyerID,int bookID)
        {
            Review review = db.Reviews.Where(r=>r.Book_Id==bookID&&r.Buyer_Id==buyerID).FirstOrDefault();
            if (review == null)
            {
                return Ok(review);
            }
            ReviewVM reviewVM=ConvertClass.ReviewToVM(review);
            return Ok(reviewVM);
        }
        [ResponseType(typeof(ReviewVM))]
        [HttpGet]
        [Route("api/Reviews/{userID:int}/")]
        public IHttpActionResult GetReviews(int userID)
        {
            var reviews = db.Reviews.Where(r => r.Seller_Id == userID).OrderByDescending(r => r.Date).ToList(); ;
            if (reviews == null)
            {
                return Ok(reviews);
            }
            List<ReviewVM> listOfReviews = new List<ReviewVM>();
            foreach (var item in reviews)
            {
                ReviewVM reviewVM = ConvertClass.ReviewToVM(item);
                listOfReviews.Add(reviewVM);
            }
            
            return Ok(listOfReviews);
        }
    }
}
