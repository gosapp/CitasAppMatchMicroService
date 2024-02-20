using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class RankResponse2
    {
        public int UserId { get; set; }
        public int MatchQty { get; set; }
        public UserResponse UserResponse { get; set; }
    }
}
