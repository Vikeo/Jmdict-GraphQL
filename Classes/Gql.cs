namespace gql
{
    public class Book
    {
        public required string Title { get; set; }
        public required Author Author { get; set; }
    }

    public class Author
    {
        public required string Name { get; set; }
    }
}