using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace birds.Controllers
{
    public class HelperClass
    {
        public static string GetErrorMessages(Exception e)
        {
            var messages = new List<string>();
            do
            {
                messages.Add(e.Message);
                e = e.InnerException;
            }
            while (e != null);
            var message = string.Join("\r\nInnerException: ", messages);
            return message;
        }

        //        try
        //                {
        //                    await _context.SaveChangesAsync();
        //    }
        //                catch (DbUpdateException ex)
        //                {
        //                    ModelState.AddModelError("", HelperClass.GetErrorMessages(ex));
        //                    return View(country);
        //}

        public class ViewLayoutAttribute : ResultFilterAttribute
        {
            private readonly string _layout;
            public ViewLayoutAttribute(string layout)
            {
                _layout = layout;
            }

            public override void OnResultExecuting(ResultExecutingContext context)
            {
                if (context.Result is ViewResult viewResult)
                {
                    viewResult.ViewData["Layout"] = _layout;
                }
            }
        }
    }
}
