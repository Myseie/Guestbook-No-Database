using Guestbook.Models;
using Microsoft.AspNetCore.Mvc;

namespace Guestbook.Controllers
{
    public class GuestbookController : Controller
    {
        private static List<GuestbookEntry> Entries = new List<GuestbookEntry>();

        [HttpGet]
        public IActionResult Index(string sortOrder)
        {
            var sortedEntries = sortOrder == "oldest"
            ? Entries.OrderBy(e => e.Date).ToList()
            : Entries.OrderByDescending(e => e.Date).ToList();
            ViewBag.EntryCount = sortedEntries.Count;

            return View(sortedEntries);
        }

        [HttpPost]
        public IActionResult AddEntry(string name, string message)
        {
            if(!string.IsNullOrEmpty(name) || string.IsNullOrEmpty(message))
            {
                ViewBag.Error = "Både namn och meddelande måste finnas";
                return View("Index", Entries);
            }

            if(message.Length < 10)
            {
                ViewBag.Error = "Meddelandet måste vara mer än 10 bokstäver långt.";
                return View("Index", Entries);
            }

            Entries.Add(new GuestbookEntry
            {
                Name = name,
                Message = message,
                Date = DateTime.Now

            });

            return RedirectToAction("Index");
        }

        public IActionResult RemoveEntry(int index)
        {
            if (index >= 0 && index < Entries.Count)
            {
                Entries.RemoveAt(index);
            }

            return RedirectToAction("Index");
        }
    }
}
