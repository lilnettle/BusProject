using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClientBus.Models.TicketsModel;

namespace ClientBus.Models
{
    internal class User
    {
       
            public int Id { get; set; }
            public string PhoneNumber { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
            public virtual ICollection<TicketsModel> Tickets { get; set; }
        


    }
}
