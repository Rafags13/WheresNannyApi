using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities
{
    public class CommentRank
    {
        public CommentRank(string comment, int personWhoCommentId, int nannyWhoRecieveTheCommentId, float rankStarsCounting) 
        {
            Comment = comment;
            PersonWhoCommentId = personWhoCommentId;
            NannyWhoRecieveTheCommentId = nannyWhoRecieveTheCommentId;
            RankStarsCounting = rankStarsCounting;
        }

        public CommentRank() { }
        public int Id { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int? PersonWhoCommentId { get; set; }
        public Person? PersonWhoComment { get; set; }
        public int? NannyWhoRecieveTheCommentId { get; set; }
        public Nanny? NannyWhoRecieveTheComment { get; set; }
        public float RankStarsCounting { get; set; }
    }
}
