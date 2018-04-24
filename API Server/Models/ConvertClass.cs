using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Server.Models
{
    public class ConvertClass
    {
        static BooksEntities db = new BooksEntities();

        public static RequestVM RequestToVM(Request request)
        {
            RequestVM requestVM = new RequestVM();
            requestVM.SellerName = (from s in db.Users
                              where s.Id == request.Seller_Id
                              select s.UserName).FirstOrDefault();
            var requesterName= (from s in db.Users
                                where s.Id == request.Requester_Id
                                select s.UserName).FirstOrDefault();
            requestVM.RequesterName = requesterName;
            var book= (from b in db.Books
                                 where b.Id == request.Book_Id
                                 select new { b.Name ,b.Points}).FirstOrDefault();
            requestVM.BookImgUrl = (from i in db.Images
                                    where i.Book_Id==request.Book_Id
                                    select i.Img_URL).FirstOrDefault();
            requestVM.ReqDate = request.Date;
            requestVM.Points = book.Points;
            requestVM.Offer_Points = request.Offer_Points;
            requestVM.BookName= book.Name;
            requestVM.Message = request.Message ;
            requestVM.Book_Id = request.Book_Id;
            requestVM.Requester_Id = request.Requester_Id;
            requestVM.Seller_Id = request.Seller_Id;
            return requestVM;
        }
        public static CategoryVM CategoryToVM(Category cat)
        {
            CategoryVM catVM = new CategoryVM();
            catVM.Name = cat.Name;
            catVM.Id = cat.Id;
            List<string> subName = new List<string>();
            foreach (var item in cat.SubCategories)
            {
                subName.Add(item.Name);
            }
            catVM.SubCategoryName = subName;
            return catVM;

        }
        public static SubCategoryVM SubcategoryToVM(SubCategory cat)
        {
            SubCategoryVM catVM = new SubCategoryVM();
            catVM.SubCategoryName = cat.Name;
            catVM.Description = cat.Description;
            return catVM;
        }

        public static BookVM BookToVM(Book book)
        {
            BooksEntities db = new BooksEntities();
            BookVM bookVM = new Models.BookVM();
            bookVM.BookID = book.Id;
            bookVM.Name = book.Name;
            bookVM.Points = book.Points;
            bookVM.Discription = book.Discription;
            
            if (book.City_Id == null)
                bookVM.CityId = null;
            else
            {
                bookVM.CityId = book.City_Id;
                bookVM.CityName = book.City.Name;
            }
            bookVM.Date_Uploaded = book.Date_Uploaded;
            bookVM.Subcategory_Id = book.Subcategory_Id;
            bookVM.Images = new List<string>();

            foreach (var img in book.Images)
            {
                bookVM.Images.Add(img.Img_URL);
            }


            var subcatName =
                (from c in db.SubCategories
                 where c.Id == book.Subcategory_Id
                 select c.Name).FirstOrDefault();

            bookVM.SubCategory = subcatName;

            var booksIDs =
                (from b in db.Seller_Approved_book
                 where b.Approved == "Yes" && b.Book_Id == book.Id
                 select b).FirstOrDefault();

            if (booksIDs != null)
            {
                bookVM.UserID = booksIDs.User.Id;
                bookVM.User_Name = booksIDs.User.UserName;
            }

            return bookVM;
        }

        public static AdVM ApprovedToAdsVm(Seller_Approved_book book)
        {
            AdVM adVM = new AdVM();
            adVM.BookID = book.Book_Id;
            adVM.UserID = book.Seller_Id;
            adVM.Description = book.Book.Discription;
            adVM.SubCatName = book.Book.SubCategory.Name;

            var b =
                (from bk in db.Books
                    where bk.Id == book.Book_Id
                select bk).FirstOrDefault();

            var images = 
                (from i in db.Images
                where i.Book_Id == book.Book_Id
                select i).FirstOrDefault();
            if (book.Approved == "no"|| book.Approved == "No")
                adVM.Requests = -2;
            else
            {
                Request request = (from r in db.Requests
                                   where r.Book_Id == book.Book_Id && r.Acctepted == "Yes"
                                   select r).FirstOrDefault();

                if (request == null)
                {
                    adVM.Requests = (from r in db.Requests
                                     where r.Book_Id == book.Book_Id && (r.Acctepted=="Yes"||r.Acctepted=="No")
                                     select r).Count();
                }
                else
                {
                    adVM.Requests = -1;
                }
            }

                adVM.Img_Url = images.Img_URL;
            adVM.Name = b.Name;
            adVM.Points = b.Points;
            adVM.Date_Uploaded = b.Date_Uploaded;
            adVM.CityName = b.City.Name;

            return adVM;
        }

        public static GovernateVM GovernateToVM(Governate gov)
        {
            GovernateVM govVM = new Models.GovernateVM();
            govVM.Id = gov.Id;
            govVM.Name = gov.Name;
            govVM.Cities = new List<string>();
            foreach (var item in gov.Cities)
            {
                govVM.Cities.Add(item.Name);
            }
            return govVM;
        }


        public static UserVM UserToVM(User u)
        {
            UserVM uVm = new UserVM();
            uVm.Id = u.Id;
            uVm.UserName = u.UserName;
            uVm.Email = u.Email;
            uVm.IsActive = u.IsActive;

            return uVm;
        }

        public static CategoryVM CategoryToVM(Category cat, List<SubCategory> subCatList)
        {
            CategoryVM catVM = new CategoryVM();
            catVM.Name = cat.Name;
            catVM.SubCategoryName = new List<string>();
            List<string> subcat = new List<string>();
            foreach (var item in subCatList)
            {
                subcat.Add(item.Name);
            }
            catVM.SubCategoryName = subcat;
            return catVM;
        }
        public static ReviewVM ReviewToVM(Review review)
        {
            ReviewVM reviewVM = new ReviewVM();
            reviewVM.Seller_Id = review.Seller_Id;
            reviewVM.Buyer_Id = review.Buyer_Id;
            reviewVM.Book_Id = review.Book_Id;
            var book_Seller = (from a in db.Seller_Approved_book
                             where a.Book_Id==review.Book_Id
                             select new {Book_Name=a.Book.Name,Seller_Name=a.User.UserName }).FirstOrDefault();
            var Buyer_Name = db.Users.Find(review.Buyer_Id).UserName;
            reviewVM.Seller_Name = book_Seller.Seller_Name;
            reviewVM.Book_Name = book_Seller.Book_Name;
            reviewVM.Buyer_Name = Buyer_Name;
            reviewVM.Rate = review.Rate;
            reviewVM.Review = review.User_Review;
            reviewVM.Date = reviewVM.Date;
            return reviewVM;
        }
        public static Review ReviewVMToModel(ReviewVM reviewVM)
        {
            Review review = new Review();
            review.Seller_Id = reviewVM.Seller_Id;
            review.Buyer_Id = reviewVM.Buyer_Id ;
            review.Book_Id=reviewVM.Book_Id  ;
            //review.Seller_Buyer_Book = (from SBB in db.Seller_Buyer_Book
            //                            where SBB.Book_Id == reviewVM.Book_Id
            //                            select SBB).FirstOrDefault();
            review.Rate = reviewVM.Rate ;
            review.User_Review = reviewVM.Review ;
            review.Date = reviewVM.Date;
            return review;
        }






        ///////////////////////////////////////
        public static SubCategory VMToSubCategory(SubCatVM2 subVM)
        {
            SubCategory subCat = new API_Server.SubCategory();
            subCat.Id = subVM.subId;
            subCat.Name = subVM.SubCatName;
            subCat.Description = subVM.SubDiscription;
            subCat.Category_Id = subVM.catID;
            return subCat;
        }


        public static Category VMToCategory(CategoryVM catVM)
        {
            Category cat = new API_Server.Category();
            cat.Id = catVM.Id;
            cat.Name = catVM.Name;
            cat.SubCategories = null;

            return cat;
        }
        ////////////////////////////////////////
    }
}