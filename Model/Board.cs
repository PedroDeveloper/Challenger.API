using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trello.API.Model
{
    public class Board
    {
        public int ID { get; set; }
        public int DesktopID { get; set; }
        public string Name { get; set; }
        public List<List>lists { get; set; }
    }
}
