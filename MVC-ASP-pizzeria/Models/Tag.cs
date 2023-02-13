using System.Text.Json.Serialization;

namespace MVC_ASP_pizzeria.Models

{
    public class Tag
    {   
        public int Id { get; set; }
        public string Title { get; set; }


        [JsonIgnore]
        public List<Pizza> Pizza { get; set; }


        public Tag() { }

    }
}
