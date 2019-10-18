namespace Sphix.ViewModels.User
{
   public class AssociationsList
    {
        public long CommunityId { get; set; }
        public string Name { get; set; }
        public string Associations { get; set; }
        public string Color { get; set; }
    }
    public class AssociationsModel
    {
        public long EditId { get; set; }
        public int GroupId { get; set; }
        public int AssociationId { get; set; }
        public int Type1Id { get; set; }
        public int Type2Id { get; set; }
        public string InterestIds { get; set; }
        public long UserId { get; set; }
        public int CommunityId { get; set; }
    }
}
