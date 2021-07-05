using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Shop18.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
