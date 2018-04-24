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
    public class RequestController : ApiController
    {
        BooksEntities db = new BooksEntities();
        // POST: api/Products
        public IEnumerable<RequestVM> GetRequests()
        {
            var requests = db.Requests.Select(x => x);
            List<RequestVM> requestList = new List<RequestVM>();
            foreach (var item in requests)
            {
                RequestVM requestVM = ConvertClass.RequestToVM(item);
                requestList.Add(requestVM);
            }
            return requestList;
        }
        [ResponseType(typeof(Request))]
        public IHttpActionResult PostRequest(Request request)
        {
            if (request == null)
            {
                // save the item here
                return Ok(request);
            }
            var requestCheck = db.Requests
                      .Where(b => b.Requester_Id == request.Requester_Id && b.Book_Id == request.Book_Id && b.Seller_Id == request.Seller_Id)
                      .FirstOrDefault();

            if (requestCheck != null)
            {
                // save the item here
                return Ok(requestCheck);
            }
            request.Date = DateTime.Now;
            request.User = db.Users.Find(request.Requester_Id);
            request.Seller_Approved_book = (from AB in db.Seller_Approved_book
                                            where AB.Book_Id == request.Book_Id
                                            select AB).FirstOrDefault();
            request.Acctepted = "No";
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Requests.Add(request);
            db.SaveChanges();

            return Ok(request);
        }
        [HttpPost]
        [Route("api/Request/check")]
        public Object CheckRequest(Request request)
        {

            var requestCheck = db.Requests
                        .Where(b => b.Requester_Id == request.Requester_Id && b.Book_Id == request.Book_Id)
                        .FirstOrDefault();

            if (requestCheck == null)
            {
                // save the item here
                return new { IsExist = false };
            }
            else
            {
                return new { IsExist = true };
            }
        }
        [Route("api/User/Requests/{userID:int}/{bookID:int}")]
        public IEnumerable<RequestVM> GetRequests(int userID, int bookID)
        {
            var requests = db.Requests.Where(x => x.Seller_Id == userID && x.Book_Id == bookID && (x.Acctepted == "Yes" || x.Acctepted == "No")).Select(x => x).OrderByDescending(x=>x.Date);
            List<RequestVM> requestList = new List<RequestVM>();
            foreach (var item in requests)
            {
                RequestVM requestVM = ConvertClass.RequestToVM(item);
                requestList.Add(requestVM);
            }
            return requestList;
        }

        [Route("api/User/Request/{userID:int}/{bookID:int}")]
        public IHttpActionResult GetRequest(int userID, int bookID)
        {
            var request = db.Requests.Where(x => x.Requester_Id == userID && x.Book_Id == bookID ).FirstOrDefault();
            if (request == null)
            {
                return NotFound();
            }
            RequestVM requestVM = ConvertClass.RequestToVM(request);
            return Ok(requestVM);
        }
        [Route("api/MyRequests/{userSendID:int}")]
        public IEnumerable<RequestVM> GetMyRequests(int userSendID)
        {
            var requests = db.Requests.Where(x => x.Requester_Id == userSendID).Select(x => x);
            List<RequestVM> requestList = new List<RequestVM>();
            foreach (var item in requests)
            {
                RequestVM requestVM = ConvertClass.RequestToVM(item);
                requestList.Add(requestVM);
            }
            return requestList;
        }
        [HttpGet]
        [Route("api/Operation/Confirm/{bookID:int}/{userID:int}")]
        public IHttpActionResult OPConfirm(int bookID, int userID)
        {
            var OP = db.Seller_Buyer_Book.Where(Op => Op.Book_Id == bookID && Op.Seller_Id == userID).FirstOrDefault();
            if (OP == null)
            {
                OP = db.Seller_Buyer_Book.Where(Op => Op.Book_Id == bookID && Op.Buyer_Id == userID).FirstOrDefault();
                if (OP == null)
                {
                    return Ok(OP);
                }
                OP.BuyerConfirm = "Yes";
                db.SaveChanges();
                return Ok("Conirmed");
            }
            OP.SellerConfirm = "Yes";
            db.SaveChanges();
            return Ok("Conirmed");
        }
        [HttpPost]
        [Route("api/Request/Cancel")]
        public IHttpActionResult CancelRequest(RequestVM request)
        {
            var req = db.Requests.Where(r => r.Book_Id == request.Book_Id && r.Requester_Id == request.Requester_Id).FirstOrDefault();
            if (req == null)
            {
                return NotFound();
            }
            var sr = db.Seller_Buyer_Book.Where(s => s.Book_Id == req.Book_Id && s.Buyer_Id == req.Requester_Id && s.Seller_Id == req.Seller_Id).FirstOrDefault();
            if (sr != null)
            {
                db.Seller_Buyer_Book.Remove(sr);
                db.SaveChanges();

            }
            req.Acctepted = "Refuse";
            db.SaveChanges();
            var msg = new { Message = "Request Refused" };
            return Ok(msg);
        }
        [HttpGet]
        [Route("api/Operation/Check/{bookID:int}")]
        public IHttpActionResult OPCheck(int bookID)
        {
            var OP = db.Seller_Buyer_Book.Where(Op => Op.Book_Id == bookID && Op.BuyerConfirm == "Yes" && Op.SellerConfirm == "Yes").FirstOrDefault();
            if (OP == null)
            {
                OP = db.Seller_Buyer_Book.Where(Op => Op.Book_Id == bookID && Op.SellerConfirm == "Yes").FirstOrDefault();
                if (OP == null)
                {
                    OP = db.Seller_Buyer_Book.Where(Op => Op.Book_Id == bookID && Op.BuyerConfirm == "Yes").FirstOrDefault();
                    if (OP == null)
                    {
                        return Ok(OP);
                    }
                    return Ok("BuyerConfirm");
                }
                return Ok("SellerConfirm");
            }
            return Ok("Conirmed");
        }

        [ResponseType(typeof(Request))]
        [Route("api/Request/delete/{bookID:int}/{requesterID:int}")]
        public IHttpActionResult DeleteRequest(int bookID, int requesterID)
        {
            Request request = db.Requests.Where(x => x.Book_Id == bookID && x.Requester_Id == requesterID).FirstOrDefault();
            if (request == null)
            {
                return NotFound();
            }

            db.Requests.Remove(request);
            db.SaveChanges();
            return Ok(request);
        }
        [HttpGet]
        [Route("api/Accepted/Request/{bookID:int}")]
        public IHttpActionResult AcceptedRequest(int BookID)
        {
            var req = db.Requests.Where(r => r.Book_Id == BookID && r.Acctepted == "Yes").FirstOrDefault();
            if (req == null)
            {
                return Ok(req);
            }
            RequestVM reqVM = ConvertClass.RequestToVM(req);
            return Ok(reqVM);
        }
        [HttpPost]
        [Route("api/Request/Accept")]
        public IHttpActionResult AcceptRequest(RequestVM request)
        {
            var req = db.Requests.Where(r => r.Book_Id == request.Book_Id && r.Requester_Id == request.Requester_Id).FirstOrDefault();
            if (req == null)
            {
                return NotFound();
            }

            req.Acctepted = "Yes";
            db.SaveChanges();
            var msg = new { Message = "Request cofirmed" };
            return Ok(msg);
        }
    }
}
