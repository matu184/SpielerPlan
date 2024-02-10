using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpielerPlan.Models;
using System.Data;
using System.Diagnostics;

namespace SpielerPlan.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SpielerContext _context;

        public HomeController(ILogger<HomeController> logger, SpielerContext context)
        {
            _context = context;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View(_context.Spieler.ToList());

        }
        public IActionResult Statistik(ICollection<Spieler> spieler)

        {
            foreach (var typ in spieler)
            {
                var spielerToUpdate = _context.Spieler.FirstOrDefault(s => s.Id == typ.Id);

                if (spielerToUpdate != null)
                {
                    if (typ.Über1001)
                        spielerToUpdate.Über100++;

                    if (typ.Über1401)
                        spielerToUpdate.Über140++;

                    if (typ.MaxFinish1)
                    {
                        spielerToUpdate.MaxFinish++;
                        if (typ.HighFinish > spielerToUpdate.HighFinish)
                        {
                            spielerToUpdate.HighFinish = typ.HighFinish;
                        }
                    }
                    if (typ.Anzahl1801)
                        spielerToUpdate.Anzahl180++;

                    if (typ.Schwarz1)
                        spielerToUpdate.Schwarz++;

                    if (typ.Bezahlt1)
                        spielerToUpdate.Bezahlt += 5;
                }
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Create(Spieler spieler)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Spieler.Add(spieler);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Fehler beim Hinzufügen des Teilnehmers.");
                    return RedirectToAction("Error");
                }
            }
            return View(spieler);






            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var teilnehmer = _context.Spieler.FirstOrDefault(m => m.Id == id);
            if (teilnehmer == null)
            {
                return NotFound();
            }
            return View(teilnehmer);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                var teilnehmer = _context.Spieler.Find(id);

                if (teilnehmer == null)
                {
                    return NotFound();
                }

                _context.Spieler.Remove(teilnehmer);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Fehler beim Löschen des Teilnehmers.");
                return RedirectToAction("Error");
            }
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var spieler = _context.Spieler.FirstOrDefault(s => s.Id == id);
            if (spieler != null)
            {
                return View(spieler);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Edit(Spieler spieler)
        {
            if (ModelState.IsValid)
            {
                var spielerToUpdate = _context.Spieler.FirstOrDefault(s => s.Id == spieler.Id);
                if (spielerToUpdate != null)
                {
                    spielerToUpdate.Name = spieler.Name;
                    spielerToUpdate.Vorname = spieler.Vorname;
                    spielerToUpdate.Über100 = spieler.Über100;
                    spielerToUpdate.Über140 = spieler.Über140;
                    spielerToUpdate.MaxFinish = spieler.MaxFinish;
                    spielerToUpdate.HighFinish = spieler.HighFinish;
                    spielerToUpdate.Anzahl180 = spieler.Anzahl180;
                    spielerToUpdate.Schwarz = spieler.Schwarz;
                    spielerToUpdate.Bezahlt = spieler.Bezahlt;
                    _context.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(spieler);
        }
        public IActionResult Spiele()
        {
            return View(_context.Spieler.ToList());
        }
        public IActionResult Dart(ICollection<Spieler> spiler)
        {
            foreach (var typ in spiler)
            {
                var spielerToUpdate = _context.Spieler.FirstOrDefault(s => s.Id == typ.Id);
                if (spielerToUpdate != null)
                {
                    if (typ.Gewonnen1)
                        spielerToUpdate.Gewonnen++;

                    if (typ.Verloren1)
                        spielerToUpdate.Verloren++;

                    if (typ.Verloren1 || typ.Gewonnen1)
                    {
                        spielerToUpdate.Spiele++;
                    }
                    spielerToUpdate.Saetze += typ.Saetze;
                    spielerToUpdate.Saetze1 += typ.Saetze1;
                    if (spielerToUpdate.Spiele > 0)
                    {
                        spielerToUpdate.Gewinnquote = (spielerToUpdate.Gewonnen * 100) / spielerToUpdate.Spiele;
                    }
                }
            }
            _context.SaveChanges();
            return RedirectToAction("Spiele");
        }
    }
}