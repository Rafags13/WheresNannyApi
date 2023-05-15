using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheresNannyApi.Domain.Entities
{
    public class Document
    {
        public Document(int personId, int documentTypeId, string fileInBase64)
        {
            PersonId = personId;
            DocumentTypeId = documentTypeId;
            FileInBase64 = fileInBase64;
        }

        public Document() { }
        public int Id { get; set; }
        public string FileInBase64 { get; set; } = string.Empty;
        public int PersonId { get; set; }
        public Person? PersonDocumentOwner { get; set;}
        public int DocumentTypeId { get; set; }
        public DocumentType? TypeFromDocument { get; set; }
    }
}
