using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realms;

namespace Contact_Manager.Realm
{
    public class ContactObject : RealmObject
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string SurName { get; set; }

        public string FullName { get; set; }

        public string Number { get; set; }

        public string Image { get; set; }
    }
}
