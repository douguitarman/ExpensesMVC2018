using ExpensesMVC2018.Models;
using ExpensesMVC2018.Utilities;
using ExpensesMVC2018.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpensesMVC2018.Controllers
{
    public class ExpensesController : Controller
    {
        // GET: Expenses
        public ActionResult Index()
        {
            return View();
        }

        // GET: Expenses/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Expenses/Create
        public ActionResult Create()
        {
            Expense model = new Expense();
            model.ExpenseTypes = DataAccess.Expenses.GetExpenseTypes();

            return View(model);
        }

        // POST: Expenses/Create
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase file, Expense model)
        {
            // Learn to use the entire functionality of the dxFileUploader widget.
            // http://js.devexpress.com/Documentation/Guide/UI_Widgets/UI_Widgets_-_Deep_Dive/dxFileUploader/

            var myFile = Request.Files["ReceiptImage"];
            // this is a problem because we will not know the path to the file on users computer.
            // May need to save file, process, then delete. This affects System.Drawing.Image.FromFile. Thats how we compress our image.
            var targetLocation = Server.MapPath("~/Content/Uploads/");
            int maxFileSize = 10000000;

            //Check to see if the request holds a file and the file is not empty.
            //if (myFile == null || myFile.ContentLength <= 0)
            //{
            //    Response.StatusCode = 400;
            //    Response.StatusDescription = "Sorry but you did not choose a file";

            //    return new EmptyResult();
            //}

            ////Check to see if the file size matches the allowed size.
            //if (myFile.ContentLength > maxFileSize)
            //{
            //    Response.StatusCode = 400;
            //    Response.StatusDescription = "Sorry but that file is too large";

            //    return new EmptyResult();
            //}

            ////Check to see if the file is an image.
            //if (!myFile.ContentType.Contains("image"))
            //{
            //    Response.StatusCode = 400;
            //    Response.StatusDescription = "Sorry but you must choose an image file";

            //    return new EmptyResult();
            //}

            try
            {
                if (ModelState.IsValid)
                {
                    // Create receipt filename
                    Guid receiptFileName = Guid.NewGuid();
                    model.ReceiptFileName = receiptFileName.ToString();

                    // Save the image so it can be found and resized by System.Drawing.Image.FromFile
                    //string filename = Path.GetFileName(myFile.FileName);
                    //string path = Path.Combine(targetLocation, myFile.FileName);
                    //myFile.SaveAs(path);

                    // From Stream instead if no memory issues?
                    model.ConvertedReceiptImage = Helpers.ConvertResizeImage(myFile);

                    // Save data
                    DataAccess.Expenses exp = new DataAccess.Expenses();
                    string result = exp.AddExpense(model);

                    ModelState.Clear();

                    if (result == "-1")
                    {
                        TempData["Message"] = "Work request successfully created!";
                    }
                }

            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                Response.StatusDescription = "Sorry something went wrong. Please try again.";
                return new EmptyResult();
            }

            return RedirectToAction("Create");
        }

        public ActionResult GetExpenses()
        {
            ExpenseViewModel model = new ExpenseViewModel();
            DataAccess.Expenses exp = new DataAccess.Expenses();
            model.GetExpenses = exp.GetExpenses();

            return View(model);
        }

        // GET: Expenses/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Expenses/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Expenses/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Expenses/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}