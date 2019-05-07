using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace MongoDbTest
{
    class Program
    {
        private static BookService _bookService = new BookService();
        static void Main(string[] args)
        {
            AddBooks();
            ShowAllBooks();
            Console.Read();
        }

        static void AddBooks()
        {
            var books = new List<Book>
            {
                new Book
                {
                    Author = "Толстой",
                    BookName = "Война и мир"
                },
                new Book
                {
                    Author = "Толстой",
                    BookName = "Война и мир"
                },
                new Book
                {
                    Author = "Толстой",
                    BookName = "Война и мир"
                },
                new Book
                {
                    Author = "Толстой",
                    BookName = "Война и мир"
                }
            };

            foreach (var book in books)
            {
                _bookService.Create(book);
            }
        }
        static void ShowAllBooks()
        {
            var books = _bookService.Get();

            foreach (var book in books)
            {
                Console.WriteLine($"{book.BookName}");
            }
        }
    }

    public class BookService
    {
        private readonly IMongoCollection<Book> _books;
        public BookService()
        {
            var client = new MongoClient("mongodb://root:123456@localhost:2707");
            var database = client.GetDatabase("BookstoreDb");
            _books = database.GetCollection<Book>("Books");
        }

        public List<Book> Get()
        {
            return _books.Find(book => true).ToList();
        }

        public Book Get(string id)
        {
            return _books.Find<Book>(book => book.Id == id).FirstOrDefault();
        }

        public Book Create(Book book)
        {
            _books.InsertOne(book);
            return book;
        }

        public void Update(string id, Book bookIn)
        {
            _books.ReplaceOne(book => book.Id == id, bookIn);
        }

        public void Remove(Book bookIn)
        {
            _books.DeleteOne(book => book.Id == bookIn.Id);
        }

        public void Remove(string id)
        {
            _books.DeleteOne(book => book.Id == id);
        }
    }

    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string BookName { get; set; }

        [BsonElement("Price")]
        public decimal Price { get; set; }

        [BsonElement("Category")]
        public string Category { get; set; }

        [BsonElement("Author")]
        public string Author { get; set; }
    }
}
