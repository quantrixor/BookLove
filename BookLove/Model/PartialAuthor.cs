namespace BookLove.Model
{
    public partial class Author
    {
        public string GetFullName
        {
            get
            {
                return $"{firstName} {lastName} {secondName}";
            }
        }
    }
}
