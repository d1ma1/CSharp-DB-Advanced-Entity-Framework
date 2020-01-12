namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                // DbInitializer.ResetDatabase(db);

                // // 1.	Age Restriction
                //var result = GetBooksByAgeRestriction(db, Console.ReadLine());
                //Console.WriteLine(result);

                // // 2.	Golden Books
                //Console.WriteLine(GetGoldenBooks(db));

                // // 3.	Books by Price
                // Console.WriteLine(GetBooksByPrice(db));

                // // 4.	Not Released In
                // var result = GetBooksNotReleasedIn(db, int.Parse(Console.ReadLine()));
                // Console.WriteLine(result);

                // // 5.	Book Titles by Category
                // var result = GetBooksByCategory(db, Console.ReadLine());
                // Console.WriteLine(result);

                // // 6.	Released Before Date
                // var result = GetBooksReleasedBefore(db, Console.ReadLine());
                // Console.WriteLine(result);

                // // 7.
                // var result = GetAuthorNamesEndingIn(db, Console.ReadLine());
                // Console.WriteLine(result);

                // // 8.
                // var result = GetBookTitlesContaining(db, Console.ReadLine());
                // Console.WriteLine(result);

                // // 9.
                // var result = GetBooksByAuthor(db, Console.ReadLine());
                // Console.WriteLine(result);

                // // 10.
                // var result = CountBooks(db, int.Parse(Console.ReadLine()));
                // Console.WriteLine(result);

                // // 11.
                //  Console.WriteLine(CountCopiesByAuthor(db));

                // // 12.
                // Console.WriteLine(GetTotalProfitByCategory(db));

                // // 13.
                //  Console.WriteLine(GetMostRecentBooks(db));

                // // 14.
                // IncreasePrices(db);

                // // 25.
                Console.WriteLine(RemoveBooks(db));
            }
        }

        // 1.	Age Restriction
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var ageRestriction = Enum.Parse<AgeRestriction>(command, true);

            var books = context.Books
                    .Where(b => b.AgeRestriction == ageRestriction)
                    .Select(t => t.Title)
                    .OrderBy(x=>x)
                    .ToList();

            return string.Join(Environment.NewLine, books);
        }

        // 2.	Golden Books
        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books
                    .Where(b => (int)b.EditionType == 2)
                    .Where(b => b.Copies < 5000)
                    .OrderBy(b => b.BookId)
                    .Select(t => t.Title)
                    .ToList();

            return string.Join(Environment.NewLine, books);
        }

        //3.	Books by Price
        public static string GetBooksByPrice(BookShopContext context)
        {
            var sb = new StringBuilder();

            var books = context.Books
                   .Where(t => t.Price > 40)
                   .OrderByDescending(p => p.Price)
                   .Select(t => new { t.Title, t.Price })
                   .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:F2}");
            }
            return sb.ToString().TrimEnd();
        }

        // 4.	Not Released In
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                    .Where(b => b.ReleaseDate.Value.Year != year)
                    .OrderBy(b => b.BookId)
                    .Select(t => t.Title)
                    .ToList();

            return string.Join(Environment.NewLine, books);
        }

        // 5.	Book Titles by Category
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var arr = input
                .ToLower()
                .Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            var books = context.Books
                    .Where(x => x.BookCategories.Select(y => y.Category.Name.ToLower()).Intersect(arr).Any())
                    .OrderBy(b => b.Title)
                    .Select(t => t.Title)
                    .ToList();

            return string.Join(Environment.NewLine, books);
        }

        //6.	Released Before Date
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var sb = new StringBuilder();

            var parsedDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                   .Where(t => t.ReleaseDate < parsedDate)
                   .OrderByDescending(p => p.ReleaseDate)
                   .Select(t => new { t.Title, t.Price, t.EditionType })
                   .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:F2}");
            }
            return sb.ToString().TrimEnd();
        }

        // 7.
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var sb = new StringBuilder();

            var authors = context.Authors
                    .Where(a => a.FirstName.EndsWith(input))
                    .OrderBy(a => a.FirstName)
                    .ThenBy(a => a.LastName)
                    .ToList();

            foreach (var author in authors)
            {
                sb.AppendLine($"{author.FirstName} {author.LastName}");
            }
            return sb.ToString().TrimEnd();
        }

        // 8.
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                    .Where(x => x.Title.ToLower().Contains(input.ToLower()))
                    .OrderBy(b => b.Title)
                    .Select(t => t.Title)
                    .ToList();

            return string.Join(Environment.NewLine, books);
        }

        // 9.
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var sb = new StringBuilder();

            var books = context.Books
                    .Include(a => a.Author)
                    .Where(a => a.Author.LastName.ToLower().StartsWith(input.ToLower()))
                    .OrderBy(b => b.BookId)
                    .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} ({book.Author.FirstName} {book.Author.LastName})");
            }
            return sb.ToString().TrimEnd();
        }

        // 10.
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            return context.Books
                    .Where(b => b.Title.Length > lengthCheck)
                    .Count();
        }

        // 11.
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var sb = new StringBuilder();

            var authors = context.Authors
                .Select(a => new
                {
                    Name = a.FirstName + " " + a.LastName,
                    Copies = a.Books.Select(b => b.Copies).Sum()
                })
                .OrderByDescending(a => a.Copies)
                .ToList();

            foreach (var author in authors)
            {
                sb.AppendLine($"{author.Name} - {author.Copies}");
            }
            return sb.ToString().TrimEnd();
        }

        // 12.
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var sb = new StringBuilder();

            var cats = context.Categories
                .Select(a => new
                {
                    profit = a.CategoryBooks.Select(b => b.Book.Price* b.Book.Copies).Sum(),
                    name = a.Name
                })
                 .OrderByDescending(a => a.profit)
                 .ThenBy(a => a.name)
                .ToList();

            foreach (var cat in cats)
            {
                sb.AppendLine($"{cat.name} ${cat.profit:F2}");
            }
            return sb.ToString().TrimEnd();
        }

        // 13
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var sb = new StringBuilder();

            var cats = context.Categories
                .Select(a => new
                {
                    books = a.CategoryBooks.Select(b => b.Book).OrderByDescending(b => b.ReleaseDate).Take(3),
                    name = a.Name
                })
                .OrderBy(a => a.name)
                .ToList();

            foreach (var cat in cats)
            {
                sb.AppendLine($"--{cat.name}");

                foreach (var book in cat.books)
                {
                    sb.AppendLine($"{book.Title} ({book.ReleaseDate.Value.Year})");
                }
            }
            return sb.ToString().TrimEnd();
        }

        // 14.
        public static void IncreasePrices(BookShopContext context)
        {
            context.Books.Where(a => a.ReleaseDate.Value.Year < 2015).ToList().ForEach(a => a.Price += 5);

            context.SaveChanges();
        }

        // 15.
        public static int RemoveBooks(BookShopContext context)
        {
            var toBeRemoved = context.Books.Where(b => b.Copies < 4200).ToList();

            context.RemoveRange(toBeRemoved);
            context.SaveChanges();

            return toBeRemoved.Count;
        }
    }
}
