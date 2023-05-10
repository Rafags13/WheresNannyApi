using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities
{
    public class Nanny : Person
    {
        public Nanny(float servicePrice, int personId)
        {
            ServicePrice = servicePrice;
            PersonId = personId;
        }
        public Nanny() { }
        public float ServicePrice { get; set; }
        public int PersonId { get; set; }
        public Person? Person { get; set; }
        public ICollection<Service>? ServicesNanny { get; set; }
        public ICollection<CommentRank>? CommentsRankNanny { get; set; }
        public float RankAvegerageStars
        {
            get { return CommentsRank.Select(x => x.RankStarsCounting).Average(); }
            set { RankAvegerageStars = value; }
        }
    }
}
