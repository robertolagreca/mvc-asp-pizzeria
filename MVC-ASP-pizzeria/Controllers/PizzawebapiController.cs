using MVC_ASP_pizzeria.Database;
using MVC_ASP_pizzeria.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace MVC_ASP_pizzeria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzawebapiController : ControllerBase
    {

        [HttpGet]
        public IActionResult Get(string? search)
        {
            using (PizzaContext db = new PizzaContext())
            {
                //REFACTORING per aggiungere funzione search.
                // List<Pizza> pizzasWebApi = db.Pizzas.Include(pizza => pizza.Tags).ToList<Pizza>();

                List<Pizza> pizzasWebApi = new List<Pizza>();

                if (search is null || search == "")
                {
                    pizzasWebApi = db.Pizzas.Include(pizza => pizza.Tags).ToList<Pizza>();
                }
                else
                {
                    // converto tutto in stringa minuscola, non mi interessano le lettere maiuscole
                    search = search.ToLower();

                    pizzasWebApi = db.Pizzas.Where(pizza => pizza.Name.ToLower().Contains(search))
                                       .Include(pizza => pizza.Tags)
                                       .ToList<Pizza>();
                }

                //HO INSERITO JsonIgnore su modello pizza.
                //Ma volendo posso installare Newtonsoft
                //Scrivere qua per dirgli cosa non prendere
                //per far in modo che Json non serializzi
                //in un doppio ciclo

                return Ok(pizzasWebApi);
            }
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {

            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizza = db.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

                if (pizza is null)
                {
                    return NotFound("La pizza con questo id non è stato trovata!");
                }

                return Ok(pizza);
            }
        }

    }
}
