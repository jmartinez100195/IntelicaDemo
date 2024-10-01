namespace Intelica.Authentication.API.Domain.AuthenticationAggregate.Domain
{
    public class AccessInformation(Guid businessUserID, string ip,  DateTime expirationDate)
    {
        public Guid AccessInformationID { get; private set; } = Guid.NewGuid();
        public Guid BusinessUserID { get; private set; } = businessUserID;
        public string IP { get; private set; } = ip;
        public DateTime GenerationDate { get; private set; } = DateTime.Now;
        public DateTime ExpirationDate { get; private set; } = expirationDate;
    }
}