    using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_ASP_pizzeria.Database;
using MVC_ASP_pizzeria.Models;
using MVC_ASP_pizzeria.Utils;

namespace MVC_ASP_pizzeria.Controllers
{
    [Authorize]
    public class PizzaController : Controller
    {
        public IActionResult Index()
        {
            using (PizzaContext db = new PizzaContext())
            {
                List<Pizza> listPizzas = db.Pizzas.ToList<Pizza>();
                return View("Index", listPizzas);
            }
        }

        public IActionResult Details(int id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                //Con include gli dico di non usare il lazy coding e di prendermi anche l'oggetto category.
                     Pizza pizzaFound = db.Pizzas
                    .Where(PizzaNelDb => PizzaNelDb.Id == id)
                    .Include(pizza => pizza.Category)
                    .Include(pizza => pizza.Tags)
                    .FirstOrDefault();

                if (pizzaFound != null)
                {
                    return View(pizzaFound);
                }


                return NotFound("La pizza non esiste!");
            }


        }

        // METODO GET
        [HttpGet]
        public IActionResult Create()
        {
            using (PizzaContext db = new PizzaContext())
            {
                List<Category> categoriesFromDb = db.Categories.ToList<Category>();

                PizzaView modelForView = new PizzaView();
                modelForView.Categories = categoriesFromDb;

                modelForView.Tags = TagConverter.getListTagsForMultipleSelect();

                return View("Create", modelForView);
            }

        }


        // METODO POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaView formData)
        {
            if (!ModelState.IsValid)
            {
                using (PizzaContext db = new PizzaContext())
                {
                    List<Category> categories = db.Categories.ToList<Category>();
                    formData.Categories = categories;

                    formData.Tags = TagConverter.getListTagsForMultipleSelect();
                }
                return View("Create", formData);
            }

            using (PizzaContext db = new PizzaContext())
            {

                if (formData.TagsSelectedFromMultipleSelect != null)
                {
                    formData.Pizza.Tags = new List<Tag>();

                    foreach (string tagId in formData.TagsSelectedFromMultipleSelect)
                    {
                        int tagIdIntFromSelect = int.Parse(tagId);

                        Tag tag = db.Tags.Where(tagDb => tagDb.Id == tagIdIntFromSelect).FirstOrDefault();

                        //si possono mettere altri controlli
                        //come controllo errori id ecc.


                        formData.Pizza.Tags.Add(tag);
                    }

                }
                db.Pizzas.Add(formData.Pizza);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // METODO PUT (GET + POST)

        [HttpGet]
        public IActionResult Update(int id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizzaToModify = db.Pizzas.Where(pizza => pizza.Id == id).Include(post => post.Tags).FirstOrDefault();

                if (pizzaToModify == null)
                {
                    return NotFound("La pizza non è stata trovata");
                }

                List<Category> categories = db.Categories.ToList<Category>();

                PizzaView modelForView = new PizzaView();
                modelForView.Pizza = pizzaToModify;
                modelForView.Categories = categories;

                List<Tag> listTagFromDb = db.Tags.ToList<Tag>();

                List<SelectListItem> listOptsForSelection = new List<SelectListItem>();

                foreach (Tag tag in listTagFromDb)
                {

                    // Ricerco se il tag che sto inserindo nella lista delle opzioni della select era già stato selezionato dall'utente
                    // all'interno della lista dei tag del post da modificare

                    bool itWasSelected = pizzaToModify.Tags.Any(tagsSelected => tagsSelected.Id == tag.Id);

                    SelectListItem singleOptSelection = new SelectListItem() { Text = tag.Title, Value = tag.Id.ToString(), Selected = itWasSelected };

                    listOptsForSelection.Add(singleOptSelection);

                }

                modelForView.Tags = listOptsForSelection;


                return View("Update", modelForView);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PizzaView formData)
        {
            if (!ModelState.IsValid)
            {
                using (PizzaContext db = new PizzaContext())
                {
                    List<Category> categories = db.Categories.ToList<Category>();
                    formData.Categories = categories;
                }

                return View("Update", formData);
            }

            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizzaToUpdate = db.Pizzas.Where(pizza => pizza.Id == id).Include(pizza => pizza.Tags).FirstOrDefault();

                if (pizzaToUpdate != null)
                {
                    pizzaToUpdate.Image = formData.Pizza.Image;
                    pizzaToUpdate.Name = formData.Pizza.Name;
                    pizzaToUpdate.Description = formData.Pizza.Description;
                    pizzaToUpdate.Price = formData.Pizza.Price;
                    pizzaToUpdate.CategoryId = formData.Pizza.CategoryId;


                    // rimuoviamo i tag e inseriamo i nuovi
                    pizzaToUpdate.Tags.Clear();

                    if (formData.TagsSelectedFromMultipleSelect != null)
                    {

                        foreach (string tagId in formData.TagsSelectedFromMultipleSelect)
                        {
                            int tagIdIntFromSelect = int.Parse(tagId);

                            Tag tag = db.Tags.Where(tagDb => tagDb.Id == tagIdIntFromSelect).FirstOrDefault();

                            //si possono mettere eventuali controlli su errori dell' id del tag ecc

                            pizzaToUpdate.Tags.Add(tag);
                        }
                    }

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound("La pizza che volevi modificare non è stata trovata");
                }
            }

        }


        //METODO DELETE (Post)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizzaToDelete = db.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

                if (pizzaToDelete != null)
                {
                    db.Pizzas.Remove(pizzaToDelete);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound("La pizza da eliminare non è stata trovata");
                }
            }
        }

    }
}
