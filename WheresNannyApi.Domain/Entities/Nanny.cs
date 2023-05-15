using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities
{
    public class Nanny
    {
        public Nanny(float servicePrice, int personId)
        {
            ServicePrice = servicePrice;
            PersonId = personId;
        }
        public Nanny() { }
        public int Id { get; set; }
        public float ServicePrice { get; set; }
        public bool ApprovedToWork { get; set; } = false;
        public int PersonId { get; set; }
        public Person? Person { get; set; }
        public ICollection<Service>? ServicesNanny { get; set; }
        public ICollection<CommentRank>? CommentsRankNanny { get; set; }

        [NotMapped]
        public float RankAvegerageStars
        {
            get {
                var average = CommentsRankNanny?.Select(x => x.RankStarsCounting);
                return average is null ? 0.0f : average.Average(); }
            set { RankAvegerageStars = value; }
        }
    }
}
