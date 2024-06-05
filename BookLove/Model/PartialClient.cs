namespace BookLove.Model
{
    public partial class Client
    {
        public string GetFullTitle
        {
            get
            {
                return $"Наименование: {title}, город: {City.title}, улица: {street}";
            }
        }
    }
}
