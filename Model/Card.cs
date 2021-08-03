using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trello.API.Model
{
    public class Card
    {
        public int ID { get; set; }
        public int ListID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

    }
}
