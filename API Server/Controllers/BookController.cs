using API_Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace API_Server.Controllers
{
    public class BookController : ApiController
    {
        BooksEntities db = new BooksEntities();
        public IEnumerable<BookVM> GetBooks()
        {
            //var booksIDs = (from b in db.Seller_Approved_book
            //                where b.Approved == "Yes"
            //                select b);
            //List<BookVM> booksList = new List<BookVM>();
            //foreach (var item in booksIDs)
            //{
            //    BookVM bookVM = new BookVM();
            //    bookVM.BookID = item.Book_Id;
            //    bookVM.UserID = item.Seller_Id;
            //    var book = (from b in db.Books
            //                where b.Id == item.Book_Id
            //                select b).FirstOrDefault() ;
            //    var images = (from i in db.Images
            //                where i.Book_Id == item.Book_Id
            //                select i).ToList();
            //    bookVM.Images = new List<string>();
            //    for (int i = 0; i < images.Count; i++)
            //    {

            //        string Img_URL = images[i].Img_URL;
            //        bookVM.Images.Add(Img_URL);
            //    }
            //    bookVM.Name = book.Name;
            //    bookVM.Discription = book.Discription;
            //    bookVM.Points = book.Points;
            //    bookVM.Date_Uploaded = book.Date_Uploaded;
            //    booksList.Add(bookVM);

            var books =
                (from b in db.Books
                 select b).ToList();

            List<BookVM> bookVMlist = new List<BookVM>();
            foreach (var book in books)
            {
                bookVMlist.Add(ConvertClass.BookToVM(book));
            }
            return bookVMlist;
        }
        
        [ResponseType(typeof(BookVM))]
        public IHttpActionResult GetProduct(int id)
        {
            //var user = (from b in db.Seller_Approved_book
            //                where b.Approved == "Yes" && b.Book_Id==id
            //                select new {b.Seller_Id,b.User.UserName }).FirstOrDefault();
            //Book book = db.Books.Find(id);
            //if (book == null)
            //{
            //    return NotFound();
            //}
            //BookVM bookVM = new BookVM();
            //bookVM.BookID = book.Id;
            //bookVM.Name = book.Name;
            //bookVM.Points = book.Points;
            //bookVM.Date_Uploaded = book.Date_Uploaded;
            //bookVM.UserID =user.Seller_Id;
            //bookVM.User_Name = user.UserName;
            //bookVM.Discription = book.Discription;
            //bookVM.SubCategory = book.SubCategory.Name;
            //var images = (from i in db.Images
            //              where i.Book_Id == id
            //              select i).ToList();

            //bookVM.Images = new List<string>();
            //for (int i = 0; i < images.Count; i++)
            //{
            //    string Img_URL = images[i].Img_URL;
            //    bookVM.Images.Add(Img_URL);
            //}

            var book =
                (from b in db.Books
                 where b.Id == id
                 select b).FirstOrDefault();
            BookVM bookVm = new BookVM();
            bookVm = ConvertClass.BookToVM(book);
            return Ok(bookVm);
        }


        [HttpGet]
        [Route("api/book/Approved/{bookID:int}")]
        public IHttpActionResult UserActivate(int bookID)
        {
            var book = db.Seller_Approved_book.Where(b => b.Book_Id == bookID).FirstOrDefault();
            if (book.Approved == "Yes")
            {
                book.Approved = "No";
            }
            else
            {
                book.Approved = "Yes";
            }
            db.SaveChanges();
            if (book == null)
            {
                return Ok("user NotFound");
            }
            return Ok(book);
        }
    }
}
