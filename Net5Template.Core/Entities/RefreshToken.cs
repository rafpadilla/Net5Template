using Net5Template.Core.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Core.Entities
{
    public class RefreshToken : Entity<Guid>
    {
        public string Token { get; set; }//sha256
        public Guid JwtId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Invalidated { get; set; }
        public Guid UserId { get; set; }

        public virtual AspNetUser User { get; set; }
    }
}
