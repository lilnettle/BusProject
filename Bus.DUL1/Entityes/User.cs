using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bus.DAL.Entitayes.Base;

namespace Bus.DAL.Entitayes
{



    public class User : Entity 
    { 
    
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Email { get; set; } 
        public virtual ICollection<Ticket>? Tickets { get; set; }

    }
}
