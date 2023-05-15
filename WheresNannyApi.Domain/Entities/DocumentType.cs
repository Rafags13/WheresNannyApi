using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities
{
    public class DocumentType
    {
        public DocumentType(int id, string name, string description) 
        {
            Id = id;
            Name = name;
            Description = description;
        }
        public DocumentType() { }
        public int Id { get; set;}
        public string Name { get; set; } = string.Empty;
        public string Description { get; set;} = string.Empty;
        public ICollection<Document>? DocumentsWhoHaveThisDocumentType { get; set; }
    }
}
