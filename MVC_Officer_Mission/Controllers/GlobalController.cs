using MVC_Officer_Mission.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Officer_Mission.Controllers
{
    public class GlobalController : Controller
    {
        //checks if valid date
        public static bool IsValidDate(int year, int month, int day)
        {
            if (year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year)
                return false;

            if (month < 1 || month > 12)
                return false;

            return day > 0 && day <= DateTime.DaysInMonth(year, month);
        }
        //returns a selectlist of years
        public static SelectList GenerateYearsDDL(int selected_year = -1)
        {
            var items = new List<SelectListItem>();
            for (int i = 2018; i <= 2050; i++)
            {

                SelectListItem tmp = new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                };
                if (selected_year != -1 && Int32.Parse(tmp.Value) == selected_year)
                {
                    tmp.Selected = true;
                }
                items.Add(tmp);



            }
            return new SelectList(items, "Value", "Text", selected_year);
        }
        //return a selectedlist of months
        public static SelectList GenerateMonthsDDL(int selected_month = -1)
        {
            var items = new List<SelectListItem>();
            for (int i = 1; i <= 12; i++)
            {
                SelectListItem tmp = new SelectListItem
                {
                    Value = i.ToString(),
                    Text = ArabicMonthName(i)
                };
                if (selected_month != -1 && Int32.Parse(tmp.Value) == selected_month)
                {
                    tmp.Selected = true;
                }
                items.Add(tmp);
            }
            return new SelectList(items, "Value", "Text", selected_month);
        }
        //returns overriden required field message in order to unify all messages
        public static string overridenRequiredFieldMessage(string fieldname)
        {
            return "حقل " + fieldname + " مطلوب";
        }
        //compares two string dates
        public static int dateCompare(string d1, string d2)
        {
            return DateTime.Compare(DateTime.Parse(d1), DateTime.Parse(d2));
        }
        public static string ArabicMonthName(int month_number)
        {
            IDictionary<int, string> monthsList = new Dictionary<int, string>() {
                {1,"كانون الثاني" },
                {2,"شباط" },
                {3,"آذار" },
                {4,"نيسان" },
                {5,"أيار" },
                {6,"حزيران" },
                {7,"تموز" },
                {8,"آب" },
                {9,"أيلول" },
                {10,"تشرين الأول" },
                {11,"تشرين الثاني" },
                {12,"كانون الأول" },
            };
            return monthsList[month_number];
        }
        public static bool isImage(HttpPostedFileBase file)
        {
            string ext = Path.GetExtension(file.FileName).Replace(".", "").ToLower();
            List<string> imagesFileExtensions = new List<string> { "png", "jpg", "jpeg", "tif", "gif" };
            return imagesFileExtensions.Contains(ext);
        }

        public static string GetUserName(string id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationUser user = db.Users.Find(id);
            return user.Name;
        }

        public static int WordCount(string word)
        {
            char[] delimiters = new char[] { ' ', '\r', '\n' };
            return word.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        public static int DaysBetween(DateTime d1, DateTime d2)
        {
            TimeSpan span = d2.Subtract(d1);
            return (int)span.TotalDays;
        }
    }
}