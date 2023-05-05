﻿using Domain.Entities;

namespace Application.Models
{
    public class DateResponse
    {
        public int DateId { get; set; }
        public int MatchId { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public DateTime Time { get; set; }
        public int State { get; set; } // 3 Estados, 0 -> esperando confirmacion; 1 -> aceptado; 2 -> rechazado.
        public int UserId { get; set; }
        public Match Match { get; set; }
    }
}
