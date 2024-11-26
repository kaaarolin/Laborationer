using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Labb2_BookStoreDB;

class Program
{
    static string connectionString = "Server=localhost;Database=BookStoreDB;Trusted_Connection=True;";

    static void Main(string[] args)
    {
        var options = CreateDbContextOptions();

        using (var context = new BookStoreContext(options))
        {
            while (true)
            {
                ShowMainMenu();
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListInventory(context);
                        break;
                    case "2":
                        AddBookToInventoryWithAuthor(context);
                        break;
                    case "3":
                        RemoveBookFromInventory(context);
                        break;
                    case "4":
                        ListAllBooks(context);
                        break;
                    case "5":
                        AddNewBookWithAuthor(context);
                        break;
                    case "6":
                        DeleteBook(context);
                        break;
                    case "7":
                        EditBookTitleAndAuthor(context);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }
    }

    static void ShowMainMenu()
    {
        Console.Clear();
        Console.WriteLine("1. List Inventories"); // List all inventories for all stores
        Console.WriteLine("2. Add Book to a Store"); // Add a book to a store
        Console.WriteLine("3. Remove Book from a Store"); // Remove book from a store
        Console.WriteLine("4. List Books"); // List all books in the table books in the db
        Console.WriteLine("5. Add New Book"); // Add a new book to the table books and not to a specific store
        Console.WriteLine("6. Delete Book"); // Delete book from the books table 
        Console.WriteLine("7. Edit Title or Author"); // Edit title or author 
        Console.WriteLine("0. Exit");
        Console.Write("Choose an option: ");
    }

    public static DbContextOptions<BookStoreContext> CreateDbContextOptions()
    {
        var optionsBuilder = new DbContextOptionsBuilder<BookStoreContext>();
        optionsBuilder.UseSqlServer(connectionString);
        return optionsBuilder.Options;
    }

    // 1. Method to list the whole inventory 
    static void ListInventory(BookStoreContext context)
    {
        while (true)
        {
            Console.Clear();

            try
            {
                var inventory = context.Inventory
                    .Include(i => i.Store)
                    .Include(i => i.Book)
                    .ThenInclude(b => b.Author)
                    .AsNoTracking()
                    .OrderBy(i => i.Store.StoreName)
                    .ToList();

                if (inventory.Count == 0)
                {
                    Console.WriteLine("No inventory available.");
                }
                else
                {
                    Console.WriteLine("All inventory:");
                    string currentStore = null;

                    foreach (var item in inventory)
                    {
                        if (item.Store != null && item.Book != null && item.Book.Author != null)
                        {
                            if (currentStore != item.Store.StoreName)
                            {
                                if (currentStore != null)
                                    Console.WriteLine();

                                currentStore = item.Store.StoreName;
                            }

                            // Standardize the ISBN format before printing it
                            string standardizedIsbn = StandardizeISBN(item.ISBN13);

                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"Store: ");

                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.Write($"{item.Store.StoreName}, ");

                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write($"ISBN: ");

                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write($"{standardizedIsbn}, ");  // Use the standardized ISBN

                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write($"Title: ");

                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write($"{item.Book.Title}");

                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(", Author: ");

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write($"{item.Book.Author.FirstName} {item.Book.Author.LastName}");

                            Console.Write(", Quantity: ");

                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine(item.Quantity);
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine($"Data is incomplete for inventory item ISBN: {item.ISBN13}. Missing related data.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching the inventory: {ex.Message}");
            }

            Console.WriteLine("\nPress Enter to return to the menu or type 'exit' to close the application.");
            string input = Console.ReadLine()?.Trim().ToLower();

            if (input == "exit")
            {
                Console.WriteLine("Exiting the application. Goodbye!");
                Environment.Exit(0);
            }
            else if (string.IsNullOrEmpty(input))
            {
                break;
            }
        }
    }

    // 4. Method to list all the books 
    static void ListAllBooks(BookStoreContext context)
    {
        while (true)
        {
            Console.Clear();

            try
            {
                var books = context.Books
                    .Include(b => b.Author)
                    .AsNoTracking()
                    .OrderBy(b => b.Author.LastName)
                    .ToList();

                if (books.Count == 0)
                {
                    Console.WriteLine("No books available.");
                }
                else
                {
                    Console.WriteLine("All books:");
                    string currentAuthor = null;

                    foreach (var book in books)
                    {
                        if (book.Author != null && currentAuthor != $"{book.Author.FirstName} {book.Author.LastName}")
                        {
                            if (currentAuthor != null)
                                Console.WriteLine();

                            currentAuthor = $"{book.Author.FirstName} {book.Author.LastName}";
                        }

                        Console.ForegroundColor = ConsoleColor.White;

                        string standardizedIsbn = StandardizeISBN(book.ISBN13); // Standardize ISBN 

                        Console.Write($"ISBN: ");
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write($"{standardizedIsbn}, ");

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($"Title: ");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write($"{book.Title}");

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(", Price: ");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write($"{book.Price:C}");

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(", Author: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"{book.Author.FirstName} {book.Author.LastName}");

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($", Publication Date: ");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"{book.PublicationDate:yyyy-MM-dd}");
                        Console.ResetColor();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching the books: {ex.Message}");
            }

            Console.WriteLine("\nPress Enter to return to the menu or type 'exit' to close the application.");
            string input = Console.ReadLine()?.Trim().ToLower();

            if (input == "exit")
            {
                Console.WriteLine("Exiting the application. Goodbye!");
                Environment.Exit(0);
            }
            else if (string.IsNullOrEmpty(input))
            {
                break;
            }
        }
    }

    // 2. Add book to a store
    static void AddBookToInventoryWithAuthor(BookStoreContext context)
    {
        Console.Clear();

        // Step 1: Display existing stores and let the user select one
        Console.WriteLine("Select a store to add books:");
        var stores = context.Stores.OrderBy(s => s.StoreName).ToList();

        if (stores.Count == 0)
        {
            Console.WriteLine("No stores available.");
            return;
        }

        int storeIndex = 1;
        foreach (var store in stores)
        {
            Console.WriteLine($"{storeIndex}. Store: {store.StoreName}");
            storeIndex++;
        }

        Console.WriteLine("\nEnter the number of the store where you want to add a book: ");
        int selectedStoreIndex = int.Parse(Console.ReadLine()) - 1;

        if (selectedStoreIndex < 0 || selectedStoreIndex >= stores.Count)
        {
            Console.WriteLine("Invalid store selection. Exiting.");
            return;
        }

        var selectedStore = stores[selectedStoreIndex];

        Console.WriteLine("Select a book from the inventory by entering its number:");

        var books = context.Books.Include(b => b.Author).OrderBy(b => b.Title).ToList();

        if (books.Count == 0)
        {
            Console.WriteLine("No books available in the inventory.");
            return;
        }

        int bookIndex = 1;
        foreach (var book in books)
        {
            Console.WriteLine($"{bookIndex}. ISBN: {book.ISBN13}, Title: {book.Title}, Author: {book.Author.FirstName} {book.Author.LastName}");
            bookIndex++;
        }

        Console.WriteLine("\nEnter the number of the book you want to add to the inventory: ");
        int selectedBookIndex = int.Parse(Console.ReadLine()) - 1;

        if (selectedBookIndex < 0 || selectedBookIndex >= books.Count)
        {
            Console.WriteLine("Invalid book selection. Exiting.");
            return;
        }

        var selectedBook = books[selectedBookIndex];
        string isbn = selectedBook.ISBN13;

        Console.WriteLine("Enter quantity to add: ");
        int quantity = int.Parse(Console.ReadLine());

        var inventoryItem = context.Inventory
            .FirstOrDefault(i => i.StoreID == selectedStore.StoreID && i.ISBN13 == isbn);

        if (inventoryItem != null)
        {
            inventoryItem.Quantity += quantity;
            Console.WriteLine($"Updated the inventory for {selectedBook.Title} in {selectedStore.StoreName} with {quantity} more copies.");
        }
        else
        {
            context.Inventory.Add(new Inventory
            {
                StoreID = selectedStore.StoreID,
                ISBN13 = isbn,
                Quantity = quantity
            });
            Console.WriteLine($"Added {quantity} copies of {selectedBook.Title} to the inventory in {selectedStore.StoreName}.");
        }
        context.SaveChanges();
    }

    // 5. Add new book to the table books in the db
    static void AddNewBookWithAuthor(BookStoreContext context)
    {
        Console.Clear();

        Console.WriteLine("Enter the title: ");
        string title = Console.ReadLine();

        Console.WriteLine("\nEnter the ISBN of the new book (without dashes): ");
        string isbn = Console.ReadLine();

        isbn = StandardizeISBN(isbn);

        decimal price;
        Console.WriteLine("\nEnter the price of the book: ");
        while (!decimal.TryParse(Console.ReadLine(), out price))
        {
            Console.WriteLine("Invalid input. Please enter a valid price (e.g., 19.99): ");
        }

        DateTime publicationDate;
        Console.WriteLine("\nEnter the publication date (YYYY-MM-DD): ");
        while (!DateTime.TryParse(Console.ReadLine(), out publicationDate))
        {
            Console.WriteLine("Invalid date format. Please enter the publication date in the format YYYY-MM-DD: ");
        }

        Console.WriteLine("Is it an already existing author or would you add a new one?");
        Console.WriteLine("1. Select an existing author");
        Console.WriteLine("2. Add a new author");
        string authorChoice = Console.ReadLine();

        Author selectedAuthor = null;

        if (authorChoice == "1")
        {
            Console.WriteLine("Existing authors:");
            ListAuthors(context);  // List all authors for selection

            Console.WriteLine("\nEnter the ID of the author from the list above, or type 'new' to add a new author:");
            string authorSelection = Console.ReadLine();

            if (authorSelection.ToLower() == "new")
            {
                selectedAuthor = AddNewAuthor(context);
            }
            else
            {
                int authorId;
                if (int.TryParse(authorSelection, out authorId))
                {
                    selectedAuthor = context.Authors.FirstOrDefault(a => a.AuthorID == authorId);
                    if (selectedAuthor == null)
                    {
                        Console.WriteLine("Author not found. Would you like to add a new author?");
                        selectedAuthor = AddNewAuthor(context);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input, please try again.");
                    return;
                }
            }
        }
        else if (authorChoice == "2")
        {
            selectedAuthor = AddNewAuthor(context); // Add a new author if the user chooses option 2
        }
        else
        {
            Console.WriteLine("Invalid choice, please try again.");
            return;
        }
        Console.WriteLine("Is it an already existing publisher or should we add a new one?");
        Console.WriteLine("1. Select an existing publisher");
        Console.WriteLine("2. Add a new publisher");
        string publisherChoice = Console.ReadLine();

        Publisher selectedPublisher = null;

        if (publisherChoice == "1")
        {
            Console.WriteLine("Existing publishers:");
            ListPublishers(context);

            Console.WriteLine("\nEnter the ID of the publisher from the list above, or type 'new' to add a new publisher:");
            string publisherSelection = Console.ReadLine();

            if (publisherSelection.ToLower() == "new")
            {
                selectedPublisher = AddNewPublisher(context);
            }
            else
            {
                int publisherId;
                if (int.TryParse(publisherSelection, out publisherId))
                {
                    selectedPublisher = context.Publishers.FirstOrDefault(a => a.PublisherID == publisherId);
                    if (selectedPublisher == null)
                    {
                        Console.WriteLine("Publisher not found. Would you like to add a new publisher?");
                        selectedPublisher = AddNewPublisher(context);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input, please try again.");
                    return;
                }
            }
        }
        else if (publisherChoice == "2")
        {
            selectedPublisher = AddNewPublisher(context);
        }
        else
        {
            Console.WriteLine("Invalid choice, please try again.");
            return;
        }

        // Add the new book to the Books table
        var newBook = new Book
        {
            Title = title,
            ISBN13 = isbn,
            Price = price,
            PublicationDate = publicationDate,
            AuthorID = selectedAuthor.AuthorID,
            PublisherID = selectedPublisher.PublisherID
        };

        context.Books.Add(newBook);
        context.SaveChanges();

        Console.WriteLine("New book added successfully!");

        Console.WriteLine("Press any key to continue");
        Console.ReadKey();
    }

    // 3. Remove book from inventory
    static void RemoveBookFromInventory(BookStoreContext context)
    {
        Console.Clear();
        Console.WriteLine("Select a store to remove books from:");
        var stores = context.Stores.OrderBy(s => s.StoreName).ToList();

        if (stores.Count == 0)
        {
            Console.WriteLine("No stores available.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        int storeIndex = 1;
        foreach (var store in stores)
        {
            Console.WriteLine($"{storeIndex}. Store: {store.StoreName}");
            storeIndex++;
        }

        Console.WriteLine("\nEnter the number of the store from which you want to remove a book: ");
        int selectedStoreIndex = int.Parse(Console.ReadLine()) - 1;

        if (selectedStoreIndex < 0 || selectedStoreIndex >= stores.Count)
        {
            Console.WriteLine("Invalid store selection. Exiting.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        var selectedStore = stores[selectedStoreIndex];

        Console.WriteLine("Select a book from the inventory by entering its number:");

        var books = context.Inventory
            .Where(i => i.StoreID == selectedStore.StoreID)
            .Include(i => i.Book)
            .ThenInclude(b => b.Author)
            .OrderBy(i => i.Book.Title)
            .ToList();

        if (books.Count == 0)
        {
            Console.WriteLine("No books available in the inventory for this store.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        int bookIndex = 1;
        foreach (var inventoryItem in books)
        {
            Console.WriteLine($"{bookIndex}. ISBN: {inventoryItem.ISBN13}, Title: {inventoryItem.Book.Title}, Author: {inventoryItem.Book.Author.FirstName} {inventoryItem.Book.Author.LastName}, Quantity: {inventoryItem.Quantity}");
            bookIndex++;
        }

        Console.WriteLine("\nEnter the number of the book you want to remove from the inventory: ");
        int selectedBookIndex = int.Parse(Console.ReadLine()) - 1;

        if (selectedBookIndex < 0 || selectedBookIndex >= books.Count)
        {
            Console.WriteLine("Invalid book selection. Exiting.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        var selectedInventoryItem = books[selectedBookIndex];

        int quantityToRemove = 0;
        while (true)
        {
            Console.WriteLine($"Enter quantity to remove (up to {selectedInventoryItem.Quantity}): ");
            quantityToRemove = int.Parse(Console.ReadLine());

            if (quantityToRemove <= 0)
            {
                Console.WriteLine("Invalid choice. Quantity must be greater than zero.");
            }
            else if (quantityToRemove > selectedInventoryItem.Quantity)
            {
                Console.WriteLine($"Invalid choice. Copies available: {selectedInventoryItem.Quantity}. Please enter a valid quantity.");
            }
            else
            {
                break;
            }
        }

        selectedInventoryItem.Quantity -= quantityToRemove; // Update the inventory

        if (selectedInventoryItem.Quantity == 0)
        {
            context.Inventory.Remove(selectedInventoryItem); // If quantity reaches zero, remove the book entirely from the inventory
            Console.WriteLine($"Removed all copies of {selectedInventoryItem.Book.Title} from the inventory in {selectedStore.StoreName}.");
        }
        else
        {
            Console.WriteLine($"Reduced the inventory for {selectedInventoryItem.Book.Title} in {selectedStore.StoreName} by {quantityToRemove}.");
        }

        int changesSaved = context.SaveChanges();

        if (changesSaved > 0)
        {
            Console.WriteLine("Inventory updated successfully.");
        }
        else
        {
            Console.WriteLine("The removal did not go through. Please try again.");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    // 5. Add new author
    static Author AddNewAuthor(BookStoreContext context)
    {
        Console.WriteLine("\nEnter the first name of the new author: ");
        string firstName = Console.ReadLine();

        Console.WriteLine("\nEnter the last name of the new author: ");
        string lastName = Console.ReadLine();

        // Create the new author object
        Author newAuthor = new Author
        {
            FirstName = firstName,
            LastName = lastName
        };

        // Add the new author to the database
        context.Authors.Add(newAuthor);
        context.SaveChanges();

        Console.WriteLine("New author added successfully!");
        return newAuthor;
    }

    // Method to add new publisher
    static Publisher AddNewPublisher(BookStoreContext context)
    {
        Console.WriteLine("\nEnter the publisher name: ");
        string publisherName = Console.ReadLine();

        // Create new publisher object
        Publisher newPublisher = new Publisher
        {
            PublisherName = publisherName
        };

        // Add the new publisher to the database
        context.Publishers.Add(newPublisher);
        context.SaveChanges();

        Console.WriteLine("New publisher added successfully!");
        return newPublisher;
    }

    // 6. Delete book
    static void DeleteBook(BookStoreContext context)
    {
        Console.Clear();
        Console.WriteLine("Enter ISBN of the book to delete: ");
        string isbn = Console.ReadLine();

        var book = context.Books.FirstOrDefault(b => b.ISBN13 == isbn);
        if (book != null)
        {
            context.Books.Remove(book);
            context.SaveChanges();
            Console.WriteLine("Book deleted.");
        }
        else
        {
            Console.WriteLine("Book not found.");
        }
    }

    // Helper method to list authors
    static void ListAuthors(BookStoreContext context)
    {
        var authors = context.Authors.ToList();
        int authorIndex = 1;

        foreach (var author in authors)
        {
            Console.WriteLine($"{authorIndex}. {author.FirstName} {author.LastName} (ID: {author.AuthorID})");
            authorIndex++;
        }
    }

    // Helper method to list publishers
    static void ListPublishers(BookStoreContext context)
    {
        var publishers = context.Publishers.ToList();
        int publisherIndex = 1;

        foreach (var publisher in publishers)
        {
            Console.WriteLine($"{publisherIndex}. {publisher.PublisherName} (ID: {publisher.PublisherID})");
            publisherIndex++;
        }
    }

    // Method to standardize the ISBN 
    public static string StandardizeISBN(string isbn)
    {
        string cleanIsbn = new string(isbn.Where(char.IsDigit).ToArray());
        if (cleanIsbn.Length == 13)
        {
            // Format it as ISBN-13 with dashes
            return $"{cleanIsbn.Substring(0, 3)}-{cleanIsbn.Substring(3, 1)}-{cleanIsbn.Substring(4, 5)}-{cleanIsbn.Substring(9, 3)}-{cleanIsbn.Substring(12, 1)}";
        }
        return isbn;
    }

    // Method to edit the name of the book or the name of the author 
    static void EditBookTitleAndAuthor(BookStoreContext context)
    {
        Console.Clear();

        try
        {
            var books = context.Books.Include(b => b.Author).AsNoTracking().ToList();

            Console.WriteLine("Available books:");
            foreach (var book in books)
            {
                if (book.Author != null)
                {
                    Console.WriteLine($"{StandardizeISBN(book.ISBN13)} - {book.Title} by {book.Author.FirstName} {book.Author.LastName}");
                }
                else
                {
                    Console.WriteLine($"{StandardizeISBN(book.ISBN13)} - {book.Title} by Unknown Author");
                }
            }

            Console.Write("\nEnter the ISBN of the book you want to modify (without dashes - ): ");
            string isbn = Console.ReadLine()?.Trim();

            var selectedBook = context.Books.Include(b => b.Author).FirstOrDefault(b => b.ISBN13 == isbn);

            if (selectedBook == null)
            {
                Console.WriteLine("Book not found.");
                return;
            }

            Console.WriteLine("\nDo you want to modify the Title or the Author?");
            Console.WriteLine("1. Modify Title");
            Console.WriteLine("2. Modify Author");
            Console.Write("Enter your choice (1 or 2): ");
            string choice = Console.ReadLine()?.Trim();

            if (choice == "1")
            {
                Console.Write("Enter the new title: ");
                string newTitle = Console.ReadLine()?.Trim();

                if (!string.IsNullOrEmpty(newTitle))
                {
                    selectedBook.Title = newTitle;
                    Console.WriteLine("Title updated successfully.");
                }
                else
                {
                    Console.WriteLine("Invalid title. No changes made.");
                }
            }
            else if (choice == "2")
            {
                Console.Write("Enter the new first name of the author: ");
                string newFirstName = Console.ReadLine()?.Trim();

                Console.Write("Enter the new last name of the author: ");
                string newLastName = Console.ReadLine()?.Trim();

                if (!string.IsNullOrEmpty(newFirstName) && !string.IsNullOrEmpty(newLastName))
                {
                    if (selectedBook.Author == null)
                    {
                        selectedBook.Author = new Author();
                    }

                    selectedBook.Author.FirstName = newFirstName;
                    selectedBook.Author.LastName = newLastName;
                    Console.WriteLine("Author updated successfully.");
                }
                else
                {
                    Console.WriteLine("Invalid author details. No changes made.");
                }
            }
            else
            {
                Console.WriteLine("Invalid choice.");
                return;
            }

            var inventoryItems = context.Inventory
                .Where(i => i.Book.ISBN13 == selectedBook.ISBN13)
                .ToList();

            foreach (var item in inventoryItems)
            {
                if (item.Book != null && item.Book.ISBN13 == selectedBook.ISBN13)
                {
                    item.Book.Title = selectedBook.Title;
                    item.Book.Author = selectedBook.Author;
                }
            }
            context.SaveChanges();
            Console.WriteLine("\nInventory and book details updated successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        Console.WriteLine("\nPress Enter to return to the menu or type 'exit' to close the application.");
        string input = Console.ReadLine()?.Trim().ToLower();
        if (input == "exit")
        {
            Console.WriteLine("Exiting the application. Goodbye!");
            Environment.Exit(0);
        }
    }
}