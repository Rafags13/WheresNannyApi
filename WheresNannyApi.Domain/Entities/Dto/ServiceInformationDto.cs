namespace WheresNannyApi.Domain.Entities.Dto
{
    public class ServiceInformationDto
    {
        public ServiceInformationDto(
            string name,
            string cellphone,
            string email,
            DateTime birthdayDate,
            string pictureBase64,
            string cep,
            TimeSpan serviceFinishHour,
            CoordinateDto originCoordinates,
            CoordinateDto destinationCoordinates,
            decimal servicePrice) 
        {
            Name = name;
            Cellphone = cellphone;
            Email = email;
            BirthdayDate = birthdayDate;
            PictureBase64 = pictureBase64;
            Cep = cep;
            OriginCoordinates = originCoordinates;
            DestinationCoordinates = destinationCoordinates;
            ServiceFinishHour = serviceFinishHour;
            ServicePrice = servicePrice;
        }
        public ServiceInformationDto() { }
        
        public string Name { get; set; } = string.Empty;
        public string Cellphone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime BirthdayDate { get; set; }
        public string PictureBase64 { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public CoordinateDto OriginCoordinates { get; set; } = new CoordinateDto();
        public CoordinateDto DestinationCoordinates { get; set; } = new CoordinateDto();
        public TimeSpan ServiceFinishHour { get; set; }
        public decimal ServicePrice { get; set; }
        public double Distance { get; set; }

    }
}
