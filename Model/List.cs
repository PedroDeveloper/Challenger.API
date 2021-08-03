using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trello.API.Model
{
    public class List
    {
        public int ID { get; set; }
        public int BoardID { get; set; }
        public string Title { get; set; }

    }
}
