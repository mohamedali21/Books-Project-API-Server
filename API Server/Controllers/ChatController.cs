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
    public class ChatController : ApiController
    {
        BooksEntities db = new BooksEntities();
        [ResponseType(typeof(Message))]
        public IHttpActionResult PostReview(Message msg)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            msg.Msg_Date = DateTime.Now;
            db.Messages.Add(msg);
            db.SaveChanges();
            return Ok(msg);
        }
        [ResponseType(typeof(MessageVM))]
        [HttpGet]
        [Route("api/Chat/{senderID:int}/{bookID:int}/{recieverID:int}")]
        public IHttpActionResult GetProduct(int senderID, int bookID ,int recieverID)
        {
            var msgs = (from m in db.Messages
                        where m.Book_Id == bookID && (m.Sender_Id==senderID||m.Sender_Id==recieverID) && (m.Receiver_Id==recieverID|| m.Receiver_Id == senderID)
                        select m).ToList();
            List<MessageVM> msgsList = new List<MessageVM>();
            foreach (var item in msgs)
            {
                MessageVM msgVM = new MessageVM();
                if(item.Sender_Id== senderID)
                {
                    msgVM.Type = "Sent";
                }
                else
                {
                    msgVM.Type = "Received";
                }
                msgVM.Message1 = item.Message1;
                msgVM.Book_Id = item.Book_Id;
                msgVM.Msg_Date = item.Msg_Date;
                msgVM.Sender_Id = item.Sender_Id;
                msgVM.Receiver_Id = item.Receiver_Id;
                msgsList.Add(msgVM);
            }
            return Ok(msgsList);
        }
    }
}
