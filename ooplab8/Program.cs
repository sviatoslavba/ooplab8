using System;
using System.Collections.Generic;
using System.Linq;

namespace ooplab8
{
    public abstract class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }

        protected Book(int id, string title)
        {
            Id = id;
            Title = title;
        }
    }

    public class Document : Book
    {
        public string Author { get; set; }

        public Document(int id, string title, string author) : base(id, title)
        {
            Author = author;
        }
    }

    public class Journal : Book
    {
        public string Publisher { get; set; }

        public Journal(int id, string title, string publisher) : base(id, title)
        {
            Publisher = publisher;
        }
    }

    public class Newspaper : Book
    {
        public string Publisher { get; set; }
        public DateTime PublishDate { get; set; }

        public Newspaper(int id, string title, string publisher, DateTime publishDate) : base(id, title)
        {
            Publisher = publisher;
            PublishDate = publishDate;
        }
    }

    public class IssueRecord
    {
        public User User { get; set; }
        public Book Book { get; set; }
        public bool IsReturned { get; set; }

        public IssueRecord(User user, Book book)
        {
            User = user;
            Book = book;
            IsReturned = false;
        }

        public void Return()
        {
            IsReturned = true;
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public User(int id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public virtual void DisplayMenu()
        {
            Console.WriteLine("Base user menu");
        }
    }

    public class Student : User
    {
        public string AcademicGroup { get; set; }
        public List<Book> BorrowedBooks { get; set; }

        public Student(int id, string firstName, string lastName, string academicGroup) : base(id, firstName, lastName)
        {
            AcademicGroup = academicGroup;
            BorrowedBooks = new List<Book>();
        }

        public override void DisplayMenu()
        {
            Console.WriteLine("1. View My Details");
            Console.WriteLine("2. Borrow a Book");
            Console.WriteLine("3. Return a Book");
            Console.WriteLine("0. Exit");
        }

        public void BorrowBook(Book book, Library library)
        {
            if (BorrowedBooks.Count < 5)
            {
                BorrowedBooks.Add(book);
                library.RecordIssue(this, book);
                Console.WriteLine("Book borrowed successfully.");
            }
            else
            {
                Console.WriteLine("Cannot borrow more than 5 books.");
            }
        }

        public void ReturnBook(Book book, Library library)
        {
            if (BorrowedBooks.Remove(book))
            {
                library.RecordReturn(this, book);
                Console.WriteLine("Book returned successfully.");
            }
            else
            {
                Console.WriteLine("You don't have this book.");
            }
        }

        public void ViewDetails()
        {
            Console.WriteLine($"ID: {Id}, Name: {FirstName} {LastName}, Academic Group: {AcademicGroup}");
            Console.WriteLine("Borrowed Books:");
            foreach (var book in BorrowedBooks)
            {
                Console.WriteLine($"ID: {book.Id}, Title: {book.Title}");
            }
        }
    }

    public class Manager : User
    {
        public Manager(int id, string firstName, string lastName) : base(id, firstName, lastName)
        {
        }

        public override void DisplayMenu()
        {
            Console.WriteLine("1. Add User");
            Console.WriteLine("2. Remove User");
            Console.WriteLine("3. Update User");
            Console.WriteLine("4. View User");
            Console.WriteLine("5. View All Users");
            Console.WriteLine("6. Sort Users by First Name");
            Console.WriteLine("7. Sort Users by Last Name");
            Console.WriteLine("8. Sort Users by Academic Group");
            Console.WriteLine("9. Add Document");
            Console.WriteLine("10. Remove Document");
            Console.WriteLine("11. Update Document");
            Console.WriteLine("12. View Document");
            Console.WriteLine("13. View All Documents");
            Console.WriteLine("14. Sort Documents by Title");
            Console.WriteLine("15. Sort Documents by Author");
            Console.WriteLine("16. Issue Document");
            Console.WriteLine("17. Return Document");
            Console.WriteLine("18. Search Users");
            Console.WriteLine("19. Search Documents");
            Console.WriteLine("20. Get Book Borrower");
            Console.WriteLine("21. View User Borrowed Documents");
            Console.WriteLine("0. Exit");
        }
    }

    public class Library
    {
        private List<User> users = new List<User>();
        private List<Book> books = new List<Book>();
        private List<IssueRecord> issueRecords = new List<IssueRecord>();

        public Library()
        {
            users = new List<User>
            {
                new Student(1111, "student", "student", "ip33"),
                new Manager(2222, "manager", "manager")
            };

        }
        public void AddUser(User user)
        {
            users.Add(user);
        }

        public void RemoveUser(int userId)
        {
            var user = users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                users.Remove(user);
            }
        }

        public void UpdateUser(User updatedUser)
        {
            var user = users.FirstOrDefault(u => u.Id == updatedUser.Id);
            if (user != null)
            {
                user.FirstName = updatedUser.FirstName;
                user.LastName = updatedUser.LastName;
                if (user is Student student && updatedUser is Student updatedStudent)
                {
                    student.AcademicGroup = updatedStudent.AcademicGroup;
                }
            }
        }

        public User GetUser(int userId)
        {
            return users.FirstOrDefault(u => u.Id == userId);
        }

        public List<User> GetAllUsers()
        {
            return users;
        }

        public void AddBook(Book book)
        {
            books.Add(book);
        }

        public void RemoveBook(int bookId)
        {
            var book = books.FirstOrDefault(b => b.Id == bookId);
            if (book != null)
            {
                books.Remove(book);
            }
        }

        public void UpdateBook(Book updatedBook)
        {
            var book = books.FirstOrDefault(b => b.Id == updatedBook.Id);
            if (book != null)
            {
                book.Title = updatedBook.Title;
                if (book is Document document && updatedBook is Document updatedDocument)
                {
                    document.Author = updatedDocument.Author;
                }
                else if (book is Journal journal && updatedBook is Journal updatedJournal)
                {
                    journal.Publisher = updatedJournal.Publisher;
                }
                else if (book is Newspaper newspaper && updatedBook is Newspaper updatedNewspaper)
                {
                    newspaper.Publisher = updatedNewspaper.Publisher;
                    newspaper.PublishDate = updatedNewspaper.PublishDate;
                }
            }
        }

        public Book GetBook(int bookId)
        {
            return books.FirstOrDefault(b => b.Id == bookId);
        }

        public List<Book> GetAllBooks()
        {
            return books;
        }

        public void RecordIssue(User user, Book book)
        {
            issueRecords.Add(new IssueRecord(user, book));
        }

        public void RecordReturn(User user, Book book)
        {
            var record = issueRecords.FirstOrDefault(ir => ir.User.Id == user.Id && ir.Book.Id == book.Id && !ir.IsReturned);
            if (record != null)
            {
                record.Return();
            }
        }

        public List<User> SearchUsers(string keyword)
        {
            return users.Where(u => u.FirstName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >=0 ||
                                    u.LastName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                    (u is Student student && student.AcademicGroup.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)).ToList();
        }

        public List<Book> SearchBooks(string keyword)
        {
            return books.Where(b => b.Title.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >=0 ||
                                    (b is Document document && document.Author.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) ||
                                    (b is Journal journal && journal.Publisher.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) ||
                                    (b is Newspaper newspaper && newspaper.Publisher.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)).ToList();
        }

        public List<User> SortUsersByFirstName()
        {
            return users.OrderBy(u => u.FirstName).ToList();
        }

        public List<User> SortUsersByLastName()
        {
            return users.OrderBy(u => u.LastName).ToList();
        }

        public List<User> SortUsersByAcademicGroup()
        {
            return users.OfType<Student>().OrderBy(s => s.AcademicGroup).Cast<User>().ToList();
        }

        public List<Book> SortBooksByTitle()
        {
            return books.OrderBy(b => b.Title).ToList();
        }

        public List<Document> SortBooksByAuthor()
        {
            return books.OfType<Document>().OrderBy(d => d.Author).ToList();
        }
        public User GetDocumentBorrower(int documentId)
        {
            var issueRecord = issueRecords.FirstOrDefault(ir => ir.Book.Id == documentId && ir.IsReturned == false);
            return issueRecord?.User;
        }
    }
    
    public class LibrarySystem
    {
        private Library library;
        private User currentUser;

        public LibrarySystem()
        {
            library = new Library();
        }

        static void Main(string[] args)
        {
            LibrarySystem librarySystem = new LibrarySystem();
            librarySystem.Run();
        }

        public void Run()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Library System");
                Console.WriteLine("1. Login as Student");
                Console.WriteLine("2. Login as Manager");
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");

                if (int.TryParse(Console.ReadLine(), out int option) && option >= 0 && option <= 2)
                {
                    switch (option)
                    {
                        case 0:
                            exit = true;
                            break;
                        case 1:
                            LoginAsStudent();
                            break;
                        case 2:
                            LoginAsManager();
                            break;
                        default:
                            Console.WriteLine("Invalid option!");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input!");
                    Console.ReadLine();
                }
            }
        }

        private void LoginAsStudent()
        {
            Console.Write("Enter Student ID: ");
            int userId = int.Parse(Console.ReadLine());
            var user = library.GetUser(userId) as Student;

            if (user != null)
            {
                currentUser = user;
                StudentMenu();
            }
            else
            {
                Console.WriteLine("Student not found.");
                Console.ReadLine();
            }
        }

        private void LoginAsManager()
        {
            Console.Write("Enter Manager ID: ");
            int userId = int.Parse(Console.ReadLine());
            var user = library.GetUser(userId) as Manager;

            if (user != null)
            {
                currentUser = user;
                ManagerMenu();
            }
            else
            {
                Console.WriteLine("Manager not found.");
                Console.ReadLine();
            }
        }

        private void StudentMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                currentUser.DisplayMenu();
                Console.Write("Select an option: ");

                if (int.TryParse(Console.ReadLine(), out int option) && option >= 0 && option <= 3)
                {
                    switch (option)
                    {
                        case 0:
                            exit = true;
                            break;
                        case 1:
                            (currentUser as Student).ViewDetails();
                            break;
                        case 2:
                            BorrowBook();
                            break;
                        case 3:
                            ReturnBook();
                            break;
                        default:
                            Console.WriteLine("Invalid option!");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input!");
                }
                Console.ReadLine();
            }
        }

        private void ManagerMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                currentUser.DisplayMenu();
                Console.Write("Select an option: ");

                if (int.TryParse(Console.ReadLine(), out int option) && option >= 0 && option <= 21)
                {
                    switch (option)
                    {
                        case 0:
                            exit = true;
                            break;
                        case 1:
                            AddUser();
                            break;
                        case 2:
                            RemoveUser();
                            break;
                        case 3:
                            UpdateUser();
                            break;
                        case 4:
                            ViewUser();
                            break;
                        case 5:
                            ViewAllUsers();
                            break;
                        case 6:
                            SortUsersByFirstName();
                            break;
                        case 7:
                            SortUsersByLastName();
                            break;
                        case 8:
                            SortUsersByAcademicGroup();
                            break;
                        case 9:
                            AddBook();
                            break;
                        case 10:
                            RemoveBook();
                            break;
                        case 11:
                            UpdateBook();
                            break;
                        case 12:
                            ViewBook();
                            break;
                        case 13:
                            ViewAllBooks();
                            break;
                        case 14:
                            SortBooksByTitle();
                            break;
                        case 15:
                            SortBooksByAuthor();
                            break;
                        case 16:
                            IssueBook();
                            break;
                        case 17:
                            ReturnDocument();
                            break;
                        case 18:
                            SearchUsers();
                            break;
                        case 19:
                            SearchBooks();
                            break;
                        case 20:
                            GetDocumentBorrower();
                            break;
                        case 21:
                            ViewUserBorrowedDocuments();
                            break;
                        default:
                            Console.WriteLine("Invalid option!");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input!");
                }
                Console.ReadLine();
            }
        }

        private void AddUser()
        {
            Console.Write("Enter User Type (Student/Manager): ");
            string userType = Console.ReadLine();
            Console.Write("Enter User ID: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Enter First Name: ");
            string firstName = Console.ReadLine();
            Console.Write("Enter Last Name: ");
            string lastName = Console.ReadLine();

            if (userType.ToLower() == "student")
            {
                Console.Write("Enter Academic Group: ");
                string academicGroup = Console.ReadLine();
                var student = new Student(id, firstName, lastName, academicGroup);
                library.AddUser(student);
            }
            else if (userType.ToLower() == "manager")
            {
                var manager = new Manager(id, firstName, lastName);
                library.AddUser(manager);
            }
            else
            {
                Console.WriteLine("Invalid user type.");
            }
        }

        private void RemoveUser()
        {
            Console.Write("Enter User ID: ");
            int id = int.Parse(Console.ReadLine());
            library.RemoveUser(id);
        }

        private void UpdateUser()
        {
            Console.Write("Enter User Type (Student/Manager): ");
            string userType = Console.ReadLine();
            Console.Write("Enter User ID: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Enter First Name: ");
            string firstName = Console.ReadLine();
            Console.Write("Enter Last Name: ");
            string lastName = Console.ReadLine();

            if (userType.ToLower() == "student")
            {
                Console.Write("Enter Academic Group: ");
                string academicGroup = Console.ReadLine();
                var student = new Student(id, firstName, lastName, academicGroup);
                library.UpdateUser(student);
            }
            else if (userType.ToLower() == "manager")
            {
                var manager = new Manager(id, firstName, lastName);
                library.UpdateUser(manager);
            }
            else
            {
                Console.WriteLine("Invalid user type.");
            }
        }

        private void ViewUser()
        {
            Console.Write("Enter User ID: ");
            int id = int.Parse(Console.ReadLine());
            var user = library.GetUser(id);
            if (user != null)
            {
                if (user is Student student)
                {
                    student.ViewDetails();
                }
                else if (user is Manager manager)
                {
                    Console.WriteLine($"ID: {manager.Id}, Name: {manager.FirstName} {manager.LastName}");
                }
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }

        private void ViewAllUsers()
        {
            var users = library.GetAllUsers();
            foreach (var user in users)
            {
                if (user is Student student)
                {
                    student.ViewDetails();
                }
                else if (user is Manager manager)
                {
                    Console.WriteLine($"ID: {manager.Id}, Name: {manager.FirstName} {manager.LastName}");
                }
            }
        }

        private void SortUsersByFirstName()
        {
            var users = library.SortUsersByFirstName();
            foreach (var user in users)
            {
                Console.WriteLine($"ID: {user.Id}, First Name: {user.FirstName}, Last Name: {user.LastName}");
            }
        }

        private void SortUsersByLastName()
        {
            var users = library.SortUsersByLastName();
            foreach (var user in users)
            {
                Console.WriteLine($"ID: {user.Id}, First Name: {user.FirstName}, Last Name: {user.LastName}");
            }
        }

        private void SortUsersByAcademicGroup()
        {
            var users = library.SortUsersByAcademicGroup();
            foreach (var user in users.OfType<Student>())
            {
                Console.WriteLine($"ID: {user.Id}, First Name: {user.FirstName}, Last Name: {user.LastName}, Academic Group: {user.AcademicGroup}");
            }
        }

        private void AddBook()
        {
            Console.Write("Enter Book Type (Document/Journal/Newspaper): ");
            string bookType = Console.ReadLine();
            Console.Write("Enter Book ID: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Enter Title: ");
            string title = Console.ReadLine();

            if (bookType.ToLower() == "document")
            {
                Console.Write("Enter Author: ");
                string author = Console.ReadLine();
                var document = new Document(id, title, author);
                library.AddBook(document);
            }
            else if (bookType.ToLower() == "journal")
            {
                Console.Write("Enter Publisher: ");
                string publisher = Console.ReadLine();
                var journal = new Journal(id, title, publisher);
                library.AddBook(journal);
            }
            else if (bookType.ToLower() == "newspaper")
            {
                Console.Write("Enter Publisher: ");
                string publisher = Console.ReadLine();
                Console.Write("Enter Publish Date (yyyy-MM-dd): ");
                DateTime publishDate = DateTime.Parse(Console.ReadLine());
                var newspaper = new Newspaper(id, title, publisher, publishDate);
                library.AddBook(newspaper);
            }
            else
            {
                Console.WriteLine("Invalid book type.");
            }
        }

        private void RemoveBook()
        {
            Console.Write("Enter Book ID: ");
            int id = int.Parse(Console.ReadLine());
            library.RemoveBook(id);
        }

        private void UpdateBook()
        {
            Console.Write("Enter Book Type (Document/Journal/Newspaper): ");
            string bookType = Console.ReadLine();
            Console.Write("Enter Book ID: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Enter Title: ");
            string title = Console.ReadLine();

            if (bookType.ToLower() == "document")
            {
                Console.Write("Enter Author: ");
                string author = Console.ReadLine();
                var document = new Document(id, title, author);
                library.UpdateBook(document);
            }
            else if (bookType.ToLower() == "journal")
            {
                Console.Write("Enter Publisher: ");
                string publisher = Console.ReadLine();
                var journal = new Journal(id, title, publisher);
                library.UpdateBook(journal);
            }
            else if (bookType.ToLower() == "newspaper")
            {
                Console.Write("Enter Publisher: ");
                string publisher = Console.ReadLine();
                Console.Write("Enter Publish Date (yyyy-MM-dd): ");
                DateTime publishDate = DateTime.Parse(Console.ReadLine());
                var newspaper = new Newspaper(id, title, publisher, publishDate);
                library.UpdateBook(newspaper);
            }
            else
            {
                Console.WriteLine("Invalid book type.");
            }
        }

        private void ViewBook()
        {
            Console.Write("Enter Book ID: ");
            int id = int.Parse(Console.ReadLine());
            var book = library.GetBook(id);
            if (book != null)
            {
                if (book is Document document)
                {
                    Console.WriteLine($"ID: {document.Id}, Title: {document.Title}, Author: {document.Author}");
                }
                else if (book is Journal journal)
                {
                    Console.WriteLine($"ID: {journal.Id}, Title: {journal.Title}, Publisher: {journal.Publisher}");
                }
                else if (book is Newspaper newspaper)
                {
                    Console.WriteLine($"ID: {newspaper.Id}, Title: {newspaper.Title}, Publisher: {newspaper.Publisher}, Publish Date: {newspaper.PublishDate.ToShortDateString()}");
                }
            }
            else
            {
                Console.WriteLine("Book not found.");
            }
        }

        private void ViewAllBooks()
        {
            var books = library.GetAllBooks();
            foreach (var book in books)
            {
                if (book is Document document)
                {
                    Console.WriteLine($"ID: {document.Id}, Title: {document.Title}, Author: {document.Author}");
                }
                else if (book is Journal journal)
                {
                    Console.WriteLine($"ID: {journal.Id}, Title: {journal.Title}, Publisher: {journal.Publisher}");
                }
                else if (book is Newspaper newspaper)
                {
                    Console.WriteLine($"ID: {newspaper.Id}, Title: {newspaper.Title}, Publisher: {newspaper.Publisher}, Publish Date: {newspaper.PublishDate.ToShortDateString()}");
                }
            }
        }

        private void SortBooksByTitle()
        {
            var books = library.SortBooksByTitle();
            foreach (var book in books)
            {
                Console.WriteLine($"ID: {book.Id}, Title: {book.Title}");
            }
        }

        private void SortBooksByAuthor()
        {
            var documents = library.SortBooksByAuthor();
            foreach (var document in documents)
            {
                Console.WriteLine($"ID: {document.Id}, Title: {document.Title}, Author: {document.Author}");
            }
        }

        private void BorrowBook()
        {
            Console.Write("Enter Book ID: ");
            int bookId = int.Parse(Console.ReadLine());
            var book = library.GetBook(bookId);

            if (book != null && currentUser is Student student)
            {
                student.BorrowBook(book, library);
            }
            else
            {
                Console.WriteLine("Book not available or invalid user.");
            }
        }

        private void ReturnBook()
        {
            Console.Write("Enter Book ID: ");
            int bookId = int.Parse(Console.ReadLine());
            var book = library.GetBook(bookId);

            if (book != null && currentUser is Student student)
            {
                student.ReturnBook(book, library);
            }
            else
            {
                Console.WriteLine("Book not found or invalid user.");
            }
        }

        private void IssueBook()
        {
            Console.Write("Enter User ID: ");
            int userId = int.Parse(Console.ReadLine());
            Console.Write("Enter Document ID: ");
            int documentId = int.Parse(Console.ReadLine());

            var user = library.GetUser(userId);
            var document = library.GetBook(documentId);

            if (user is Student student && document != null)
            {
                student.BorrowBook(document, library);
                Console.WriteLine("Document issued successfully.");
            }
            else
            {
                Console.WriteLine("Failed to issue document.");
            }
        }

        private void ReturnDocument()
        {
            Console.Write("Enter User ID: ");
            int userId = int.Parse(Console.ReadLine());
            Console.Write("Enter Document ID: ");
            int documentId = int.Parse(Console.ReadLine());

            var user = library.GetUser(userId);
            var document = library.GetBook(documentId);

            if (user is Student student && document != null)
            {
                student.ReturnBook(document, library);
                Console.WriteLine("Document returned successfully.");
            }
            else
            {
                Console.WriteLine("Failed to return document.");
            }
        }

        private void ViewUserBorrowedDocuments()
        {
            Console.Write("Enter User ID: ");
            int userId = int.Parse(Console.ReadLine());
            var user = library.GetUser(userId);

            if (user is Student student)
            {
                student.ViewDetails();
            }
            else
            {
                Console.WriteLine("User not found or not a student.");
            }
        }

        private void GetDocumentBorrower()
        {
            Console.Write("Enter Document ID: ");
            int documentId = int.Parse(Console.ReadLine());

            var document = library.GetBook(documentId);

            if (document != null && document is Document)
            {
                var borrower = library.GetDocumentBorrower(documentId);
                if (borrower != null)
                {
                    Console.WriteLine($"User ID: {borrower.Id}, Name: {borrower.FirstName} {borrower.LastName}");
                }
                else
                {
                    Console.WriteLine("Document is in the library.");
                }
            }
            else
            {
                Console.WriteLine("Document not found or not a document.");
            }
        }

        private void SearchBooks()
        {
            Console.Write("Enter keyword: ");
            string keyword = Console.ReadLine();
            var documents = library.SearchBooks(keyword);
            foreach (var document in documents)
            {
                if (document is Document)
                {
                    Console.WriteLine($"ID: {document.Id}, Title: {document.Title}, Author: {(document as Document).Author}");
                }
                else
                {
                    Console.WriteLine($"ID: {document.Id}, Title: {document.Title}");
                }
            }
        }

        private void SearchUsers()
        {
            Console.Write("Enter keyword: ");
            string keyword = Console.ReadLine();
            var users = library.SearchUsers(keyword);
            foreach (var user in users)
            {
                if (user is Student student)
                {
                    Console.WriteLine($"ID: {user.Id}, Name: {user.FirstName} {user.LastName}, Academic Group: {student.AcademicGroup}");
                }
                else
                {
                    Console.WriteLine($"ID: {user.Id}, Name: {user.FirstName} {user.LastName}");
                }
            }
        }

    }
}
