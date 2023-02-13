using MVC_ASP_pizzeria.Database;
using MVC_ASP_pizzeria.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC_ASP_pizzeria.Utils
{
    public static class TagConverter
    {

        public static List<SelectListItem> getListTagsForMultipleSelect()
        {
            using (PizzaContext db = new PizzaContext())
            {
                List<Tag> tagsFromDb = db.Tags.ToList<Tag>();

                // Creare una lista di SelectListItem e tradurci al suo interno tutti i nostri Tag che provengono da Db

                List<SelectListItem> listForMultipleSelection = new List<SelectListItem>();

                foreach (Tag tag in tagsFromDb)
                {
                    SelectListItem singleOptMultipleSelection = new SelectListItem() { Text = tag.Title, Value = tag.Id.ToString() };

                    listForMultipleSelection.Add(singleOptMultipleSelection);
                }

                return listForMultipleSelection;
            }
        }
    }
}
