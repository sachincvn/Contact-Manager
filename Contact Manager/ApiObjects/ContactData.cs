using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contact_Manager.ApiObjects
{
    public partial class ContactData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Image { get; set; }
        public string Number { get; set; }
        public object Email { get; set; }
        public object Address { get; set; }
    }

}
