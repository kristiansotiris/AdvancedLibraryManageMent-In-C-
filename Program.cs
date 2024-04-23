using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;

namespace Program
{
    class Accounts
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long UserCode { get; set; }
        public List<Books>RentedBooks { get; set; }

        public Accounts(string firstName, string lastName, long userCode)
        {
            FirstName = firstName;
            LastName = lastName;
            UserCode = userCode;
            RentedBooks = new List<Books>();
        }

        //List for Saving the Accounts
        public static List<Accounts> AccountList = new List<Accounts>();
    }

    class Books
    {

        public string Name { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public long  ISBN { get; set; }
        public Accounts RentedBy { get; set; }

        public bool IsRented = false;

        //Constructor 
        public Books(string name, string author, string category, long isbn)
        {
            Name = name;
            Author = author;
            Category = category;
            ISBN = isbn;
        }

        //List For Saving the Books
        public static List<Books> BookList = new List<Books>();
        public static List<string> CategoryList = new List<string> { "Fiction", "Non-fiction", "Science Fiction", "Fantasy", "Mystery", "Thriller", "Romance", "Biography", "History" };

    }


    class DeletedAccounts
    {
        public string Name { get; set; }
        public string Reason { get; set; }

        //Constructor
        public DeletedAccounts(string name, string reason)
        {
            Name = name;
            Reason = reason;
        }

        public static List<DeletedAccounts> Deletedaccounts = new List<DeletedAccounts>();
    }

    class LibraryAdvanced
    {
        static void Main(string[] args)
        {
            Console.Title = "CHRIS LIBRARY SYSTEM";
            Console.ForegroundColor = ConsoleColor.Magenta;

            StartingMenu();

            Console.ReadKey();
        }

        public static void StartingMenu()
        {
            Console.WriteLine("WELCOME TO CHRIS LIBRARY !");
            Console.WriteLine();
            int choice;

            do
            {
                Console.WriteLine("Enter The Above choices");
                Console.WriteLine();
                Thread.Sleep(1000);
                Console.WriteLine("1.Create Account");
                Console.WriteLine("2.Enter Manager Menu");
                Console.WriteLine("3.Enter User Account");
                Console.WriteLine("4.Talk With Administrator");//IS NOT READY !!!
                Console.WriteLine("5.Exit Program");
                Console.WriteLine();
                Console.Write("Enter Your Choice: ");

                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            CreateAccount();
                            break;


                        case 2:
                            ManagerMenu();
                            break;

                        case 3:
                            SignedUser();
                            break;


                        case 4:
                            //MethodOFTalkingWithAdmin...
                            break;

                        case 5:
                            Console.WriteLine("Program Will Shut Down !\n");
                            Console.WriteLine("Please Press Enter To SHUT DOWN !");
                            Thread.Sleep(1000);
                            Environment.Exit(0);
                            break;

                        default:
                            Console.WriteLine();
                            Console.WriteLine("Please Make Sure You Choose The Right Option ");
                            Console.WriteLine();
                            continue;
                    }
                }
            } while (true);
        }



        public static void CreateAccount()
        {
            bool AccountCreated = false;

            Console.WriteLine();
            Console.Write("Enter Your Name: ");

            do
            {
                string userName = Console.ReadLine();

                if (string.IsNullOrEmpty(userName))
                {
                    Console.Write("Please Enter Your Name: ");
                }

                else if (IAlphabetic(userName))
                {
                    Console.Write("Your Name Can't Have Symbols Please Try Again: ");
                }

                else if (ContainsNumbers(userName))
                {
                    Console.Write("Your Name Can't Have Numbers Please Try Again: ");
                }

                else if (userName.Length < 2)
                {
                    Console.Write("Enter A Valid Name Please: ");
                }

                else
                {
                    do
                    {
                        Console.Write("Enter Your LastName:");
                        string userLastName = Console.ReadLine();

                        if (string.IsNullOrEmpty(userLastName))
                        {
                            Console.Write("Please Enter your LastName: ");
                        }

                        else if (userLastName.Length < 3)
                        {
                            Console.Write("Enter A Valid LastName: ");
                        }

                        else if (IAlphabetic(userLastName))
                        {
                            Console.WriteLine("Your LastName Can't Have Symbols Please Try Again !");
                        }

                        else if (ContainsNumbers(userLastName))
                        {
                            Console.WriteLine("Your LastName Can't Have Numbers Please Try Again !");
                        }

                        else
                        {
                            Console.WriteLine("Please Wait For Saving Your Informations...");

                            Thread.Sleep(3000);
                            AccountCreated = true;
                            long userCode = GenerateRandomUserCode();
                            Accounts account = new Accounts(userName, userLastName, userCode);
                            Accounts.AccountList.Add(account);

                            Console.WriteLine("Your Account Have Been Successfully Added !");
                            Console.WriteLine();

                            Console.WriteLine("Displaying The Information of Your Account: ");
                            Console.WriteLine();

                            Console.WriteLine($"Name: {userName}");
                            Console.WriteLine($"LastName: {userLastName}");
                            Console.WriteLine($"CodeAccount: {userCode}");
                            Console.WriteLine();
                            StartingMenu();
                            return;
                        }
                    } while (true);
                }
            } while (true);
        }

        public static long GenerateRandomUserCode()
        {
            Random rnd = new Random();
            string codePrefix = "528";
            int randomSuffix = rnd.Next(0, 1000); // Generate a random number between 0 and 9999
            string userCodeStr = codePrefix + randomSuffix.ToString("D4");
            return long.Parse(userCodeStr);
        }

        //METHODS FOR SYMBOLS & NUMBERS CONTAINED IN NAME & LAST NAME

        static bool ContainsNumbers(string input)
        {
            foreach (char c in input)
            {
                if (char.IsDigit(c))
                {
                    return true;
                }
            }
            return false;
        }

        static bool IAlphabetic(string input)
        {
            foreach (char c in input)
            {
                if (char.IsLetterOrDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        public static void RentBook(Accounts account)
        {
            Console.WriteLine("Available Books For Rent");

            foreach (var book in Books.BookList)
            {
                if (book.RentedBy == null)
                {
                    Console.WriteLine($"Book Name: {book.Name}, Author: {book.Author}, Category: {book.Category}, ISBN: {book.ISBN}");
                }

                else
                {
                    Console.WriteLine("No Books Available For Renting Please Try Again Later ! ");
                    StartingMenu();
                    return;
                }

            }

            Console.Write("Enter The ISBN Code of the Book That You Want To Rent:");

            if (long.TryParse(Console.ReadLine(), out long isbn))
            {
                var bookToRent = Books.BookList.Find(book => book.ISBN == isbn && book.RentedBy == null);

                if (bookToRent != null)
                {
                    bookToRent.RentedBy = account;
                    account.RentedBooks.Add(bookToRent);
                    Console.WriteLine($"Book '{bookToRent.Name}' rented successfully.");
                }
                else
                {
                    Console.WriteLine("Book not found or already rented.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ISBN. Please enter a valid ISBN.");
            }
        }

        public static void ReturnBook(Accounts accounts)
        {
            if(accounts.RentedBooks.Count == 0)
            {
                Console.WriteLine("You Have No Books Rented Yet !");
                return;
            }

            Console.WriteLine("Books currently rented by you:");
            foreach(var book in accounts.RentedBooks)
            {
                Console.WriteLine($"Book Name: {book.Name}, Author: {book.Author}, Category: {book.Category}, ISBN: {book.ISBN}");
            }

            Console.Write("Enter the ISBN Code of the book you want to return: ");
            if(long.TryParse(Console.ReadLine(), out long isbn))
            {
                var bookToReturn = accounts.RentedBooks.Find(book => book.ISBN == isbn);

                if (bookToReturn != null)
                {
                    bookToReturn.RentedBy = null; // Mark the book as available for rent
                    accounts.RentedBooks.Remove(bookToReturn); // Remove the book from the account's rented books list
                    Console.WriteLine($"Book '{bookToReturn.Name}' returned successfully.");
                }

                else
                {
                    Console.WriteLine("Book not found among your rented books.");
                }

            }

            else
            {
                Console.WriteLine("Invalid ISBN. Please enter a valid ISBN.");
            }
        }

        public static void ManagerMenu()
        {
            string managerCode = "Manager123";
            int attempts = 0;
            int maxattempts = 3;
            Console.WriteLine();
            Console.WriteLine("Welcome To Manager Menu");
            Console.WriteLine();
            do
            {
                Console.Write("Please Enter The Manager's Password: ");
                string ManagerInput = ReadPasswordInput();

                static string ReadPasswordInput()
                {
                    string password = "";
                    ConsoleKeyInfo key;

                    do
                    {
                        key = Console.ReadKey(true);

                        // Check if the key pressed is not Enter or Backspace
                        if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
                        {
                            password += key.KeyChar;
                            Console.Write("*"); // Display asterisk (*) for each character entered
                        }

                        else if(key.Key == ConsoleKey.Backspace && password.Length > 0)
                        {
                            password = password[0..^1]; // Remove the last character
                            Console.Write("\b \b"); // Clear the character from the console
                        }

                    } while (key.Key != ConsoleKey.Enter);

                    Console.WriteLine();
                    return password;
                }

                if(ManagerInput == managerCode)
                {
                    do
                    {
                        Console.WriteLine("");
                        Console.WriteLine("1.Create Book");
                        Console.WriteLine("2.Delete Book");
                        Console.WriteLine("3.Display Books");
                        Console.WriteLine("4.Display Books For Rent");
                        Console.WriteLine("5.Display Accounts");
                        Console.WriteLine("6.Update Accounts");
                        Console.WriteLine("7.Delete Accounts");
                        Console.WriteLine("8.Display Deleted Accounts Reasons");
                        Console.WriteLine("9.Display RentedBooks From Accounts");
                        Console.WriteLine("10.Return to Main Menu");

                        int choice;

                        if(int.TryParse(Console.ReadLine(), out choice))
                        {
                            switch (choice)
                            {

                                case 1:
                                    CreateBook();
                                    break;


                                case 2:
                                    DeleteBook();
                                    break;


                                case 3:
                                    DisplayBooks();
                                    break;


                                case 4:
                                    DisplayNonRentedBooks();
                                    break;

                                case 5:
                                    DisplayAccounts();
                                    break;


                                case 6:
                                    UpdateAccounts();
                                    break;


                                case 7:
                                    DeleteAccounts();
                                    break;

                                case 8:
                                    DisplayDeletedAccounts();
                                    break;

                                case 9:
                                    DisplayRentedBooks();
                                    break;

                                case 10:
                                    StartingMenu();
                                return;
                                 
                                default:
                                    Console.WriteLine("Enter A Valid Choide Please !");
                                    return;
                            }
                        }

                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Invalid Choice Please Try Again");
                        }

                    } while (true);
                }

                else
                {
                    attempts++;
                    if(attempts >= maxattempts)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Too Many Attempts Returning Back...");
                        Thread.Sleep(2500);
                        StartingMenu();
                        return;
                    }

                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Attempts Left: {attempts} - {maxattempts}");
                        Console.WriteLine();
                    }
                }

            } while (true);
        }


        //Methods for the Books & for the Managers Menu Too
        public static void CreateBook()
        {
            bool BookCreated = false;

            Console.WriteLine();
            Console.Write("Enter Book Name: ");

            do
            {
                string bookNameInput = Console.ReadLine();

                if (string.IsNullOrEmpty(bookNameInput))
                {
                    Console.WriteLine("You Didn't Enter Anything Please Try Again !");
                }

                else if(bookNameInput.Length < 5)
                {
                    Console.WriteLine("You Didn't Enter a Valid Name of A Book");
                }

                else if (IAlphabetic(bookNameInput))
                {
                    Console.Write($"Your Input: {bookNameInput} Can't Have Symbols Please Try Again: ");
                }

                else if (ContainsNumbers(bookNameInput))
                {
                    Console.Write($"Your Input: {bookNameInput} Can't Have Numbers Please Try Again: ");
                }

                else
                {
                    Console.WriteLine();
                    Console.Write($"Enter The Author of the {bookNameInput}: ");

                    do
                    {

                        string authorNameInput = Console.ReadLine();

                        if (string.IsNullOrEmpty(authorNameInput))
                        {
                            Console.WriteLine("You Didn't Enter Anything Please Try Again !");
                        }

                        else if(authorNameInput.Length < 3)
                        {
                            Console.WriteLine($"Your Author Name: {authorNameInput} is not Valid Please Try Again !");
                        }

                        else if (IAlphabetic(bookNameInput))
                        {
                            Console.Write($"Your Input: {authorNameInput} Can't Have Symbols Please Try Again:");
                        }

                        else if (ContainsNumbers(bookNameInput))
                        {
                            Console.Write($"Your Input: {authorNameInput} Can't Have Numbers Please Try Again: ");
                        }

                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine($"Available Categories for the {bookNameInput}");

                            foreach(var category in Books.CategoryList)
                            {
                                Console.WriteLine(category);
                            }

                            Console.WriteLine();
                            Console.Write($"Enter The Categories Above for the Book: {bookNameInput}: ");

                            do
                            {
                                string categoryNameInput = Console.ReadLine();

                                if (string.IsNullOrEmpty(categoryNameInput))
                                {
                                    Console.WriteLine("You Didn't Enter Anything Please Try Again !");
                                }

                                else if (IAlphabetic(bookNameInput))
                                {
                                    Console.Write($"Your Input: {categoryNameInput} Can't Have Symbols Please Try Again:");
                                }

                                else if (ContainsNumbers(bookNameInput))
                                {
                                    Console.Write($"Your Input: {categoryNameInput} Can't Have Numbers Please Try Again: ");
                                }

                                else if (Books.CategoryList.Contains(categoryNameInput))
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("Please Wait For Saving the Informations...");
                                    Thread.Sleep(2500);

                                    long isbn = GenerateRandomISBN();
                                    Books books = new Books(bookNameInput, authorNameInput, categoryNameInput, isbn);
                                    Books.BookList.Add(books);
                                    Console.WriteLine();
                                    Console.WriteLine("Your Book Has Been Succesfully Created !");
                                    BookCreated = true;
                                    Console.WriteLine();

                                    Console.WriteLine("Displaying Book Informations...");
                                    Console.WriteLine();
                                    Thread.Sleep(1500);

                                    Console.WriteLine($"Book Name: {bookNameInput}");
                                    Console.WriteLine($"Book Author of {bookNameInput}: {authorNameInput}");
                                    Console.WriteLine($"Book Category: {categoryNameInput}");
                                    Console.WriteLine($"Book ISBN : {isbn}");
                                }

                                else
                                {
                                    Console.WriteLine($"Category {categoryNameInput} doesn't exist. Please choose from the available categories.");
                                }

                            } while (!BookCreated);

                            ManagerMenu();
                            return;
                        }

                    } while (true);
                }

            }while (true);
        }


        // FROM HERE AND DOWN THERE WOULD BE SHOWN THE METHODS FOR THE MANAGERMENU !
        static void DisplayBooks()
        {
            if(Books.BookList.Count == 0)
            {
                Console.WriteLine("No Books Available Try Again Later !");
                return;
            }

            else
            {
                Console.WriteLine("List of The Books");
                Console.WriteLine();

                foreach(var book in Books.BookList)
                {
                    Console.WriteLine($"Book Name: {book.Name}");
                    Console.WriteLine($"Author: {book.Author}");
                    Console.WriteLine($"Category: {book.Category}");
                    Console.WriteLine($"ISBN: {book.ISBN}");
                    Console.WriteLine();
                }

                ManagerMenu();
                return;
            }
        }


        static void DeleteBook()
        {
            if (Books.BookList.Count == 0)
            {
                Console.WriteLine("No Books Available to Delete.");
                return;
            }

            Console.WriteLine("List of Books:");
            Console.WriteLine();

            // Display the list of books with their indices starting from 1
            for (int i = 0; i < Books.BookList.Count; i++)
            {
                Console.WriteLine($"Book Number: {i + 1}, Book Name: {Books.BookList[i].Name}");
            }

            Console.WriteLine();
            Console.Write("Enter the Book Number above of the book you want to delete: ");
            if (int.TryParse(Console.ReadLine(), out int index))
            {
                if (index < 1 || index > Books.BookList.Count)
                {
                    Console.WriteLine("Invalid index. Please enter a valid index.");
                    return;
                }

                Console.WriteLine($"Deleting book: {Books.BookList[index - 1].Name}");
                Books.BookList.RemoveAt(index - 1);
                Console.WriteLine("Book Deleted Successfully.");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid index.");
            }
        }

        public static void DisplayAccounts()
        {
            if(Accounts.AccountList.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("No Accounts Here Please Try Again Later !");
                return;
            }

            else
            {
                Console.WriteLine();
                Console.WriteLine("Accounts That Are Being Registered");
                Console.WriteLine();

                foreach(var account in Accounts.AccountList)
                {
                    Console.WriteLine($"Account Name : {account.FirstName}");
                    Console.WriteLine($"Account LastName: {account.LastName}");
                    Console.WriteLine($"Account Code: {account.UserCode}");
                    Console.WriteLine();
                }

                ManagerMenu();
                return;
            }
        }

        static void DisplayNonRentedBooks()
        {
            if (Books.BookList.Count == 0)
            {
                Console.WriteLine("No Books Available.");
                return;
            }

            Console.WriteLine("List of Non-Rented Books:");
            Console.WriteLine();

            bool foundNonRentedBooks = false;

            foreach (var book in Books.BookList)
            {
                if (!book.IsRented)
                {
                    Console.WriteLine($"Book Name: {book.Name}");
                    Console.WriteLine($"Author: {book.Author}");
                    Console.WriteLine($"Category: {book.Category}");
                    Console.WriteLine($"ISBN: {book.ISBN}");
                    Console.WriteLine();

                    foundNonRentedBooks = true;
                }
            }

            if (!foundNonRentedBooks)
            {
                Console.WriteLine("No Non-Rented Books Available.");
            }
        }


        public static void DeleteAccounts()
        {
            Console.WriteLine();
            Console.Write("Enter The Name of The Account to Delete: ");
            string userNameDelete = Console.ReadLine();

            if (string.IsNullOrEmpty(userNameDelete))
            {
                Console.WriteLine("You Didn't Enter Any Name to Delete. Please Try Again!");
                return;
            }

            // Search for the account in the AccountList
            Accounts accountToDelete = Accounts.AccountList.Find(account => account.FirstName == userNameDelete);

            if (accountToDelete == null)
            {
                Console.WriteLine($"Account with the name {userNameDelete} not found.");
                return;
            }

            Console.Write("Enter the Reason for Deleting the Account: ");
            string reason = Console.ReadLine();

            // Remove the account from AccountList
            Accounts.AccountList.Remove(accountToDelete);

            // Add the deleted account to DeletedAccounts list
            DeletedAccounts deletedAccount = new DeletedAccounts(userNameDelete, reason);
            DeletedAccounts.Deletedaccounts.Add(deletedAccount);

            Console.WriteLine($"Account {userNameDelete} deleted successfully.");
        }

        public static void DisplayDeletedAccounts()
        {
            Console.WriteLine();
            Console.WriteLine("List Of the Deleted Accounts With Reasons Too");
            Console.WriteLine();

            if(DeletedAccounts.Deletedaccounts.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("No Accounts Deleted Yet !");
                return;
            }

            else
            {
                foreach(var deletedaccounts in DeletedAccounts.Deletedaccounts)
                {
                    Console.WriteLine($"Account Name: {deletedaccounts.Name}");
                    Console.WriteLine($"Deletion Reason: {deletedaccounts.Reason}");
                    Console.WriteLine();
                }
            }
        }

        public static void UpdateAccounts()
        {
            Console.WriteLine();
            Console.Write("Enter the Name of the Account to Update: ");
            string accountName;
            Accounts accountToUpdate = null;

            do
            {
                accountName = Console.ReadLine();

                // Check if the account exists
                accountToUpdate = Accounts.AccountList.Find(account => account.FirstName.Equals(accountName, StringComparison.OrdinalIgnoreCase));
                if (accountToUpdate == null)
                {
                    Console.WriteLine($"Account with name '{accountName}' does not exist.");
                    Console.Write("Enter a valid account name: ");
                }
            } while (accountToUpdate == null);

            Console.WriteLine("Enter the updated information:");

            string newFirstName = string.Empty;
            string newLastName = string.Empty;

            do
            {
                Console.Write("New First Name (press Enter to keep the current value): ");
                string input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    newFirstName = accountToUpdate.FirstName;
                    break;
                }

                else if (input.Length < 2)
                {
                    Console.WriteLine("New First Name must be at least 2 characters long.");
                }

                else
                {
                    newFirstName = input;
                    break;
                }

            } while (true);

            do
            {
                Console.Write("New Last Name (press Enter to keep the current value): ");
                string input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    newLastName = accountToUpdate.LastName;
                    break;
                }

                else if (input.Length < 2)
                {
                    Console.WriteLine("New Last Name must be at least 2 characters long.");
                }

                else
                {
                    newLastName = input;
                    break;
                }
            } while (true);

            // Update account information
            accountToUpdate.FirstName = newFirstName;
            accountToUpdate.LastName = newLastName;

            Console.WriteLine("Account updated successfully.");
        }
        public static void DisplayRentedBooks()
        {
            Console.WriteLine("Displaying Rented Book");

            foreach (var account in Accounts.AccountList)
            {
                Console.WriteLine($"Account: {account.FirstName} {account.LastName}");

                if (account.RentedBooks.Count == 0)
                {
                    Console.WriteLine("No Books Rented !");
                }

                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Rented Books");

                    foreach (var book in account.RentedBooks)
                    {
                        Console.WriteLine($"Book Name: {book.Name}, Author: {book.Author}, Category: {book.Category}, ISBN: {book.ISBN}");
                        Console.WriteLine($"Rented by: {account.FirstName} {account.LastName}"); // Display the renting account
                    }
                }

                Console.WriteLine();
            }

            ManagerMenu();
            return;
        }


        // ISBN CODE FOR THE BOOKS
        public static long GenerateRandomISBN()
        {
            Random rnd = new Random();
            return (long)(rnd.Next(1000000000, int.MaxValue) * 10L + rnd.Next(0, 10)); // Generate a 10-digit random ISBN as a long
        }


        public static void SignedUser()
        {
            int attempts = 0;
            int maxAttempts = 3;
            long userCode;

            do
            {
                Console.Write("Please Enter your Code: ");

                if (long.TryParse(Console.ReadLine(), out userCode))
                {
                    var account = Accounts.AccountList.Find(accounts => accounts.UserCode == userCode);

                    if (account != null)
                    {
                        Console.WriteLine($"Welcome, {account.FirstName} {account.LastName}!");
                        Console.WriteLine();

                        while (true)
                        {
                            Console.WriteLine("1. Rent a Book");
                            Console.WriteLine("2. Return a Book");
                            Console.WriteLine("3. See Available Books for Renting");
                            Console.WriteLine("4. Return Back");
                            Console.Write("Enter your choice: ");

                            if (int.TryParse(Console.ReadLine(), out int choice))
                            {
                                switch (choice)
                                {
                                    case 1:
                                        RentBook(account);
                                        break;

                                    case 2:
                                        ReturnBook(account);
                                        break;

                                    case 3:
                                        DisplayNonRentedBooks();
                                        break;

                                    case 4:
                                        StartingMenu();
                                        return; // Exit the SignedUser method

                                    default:
                                        Console.WriteLine("Invalid choice. Please try again.");
                                        break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please enter a number.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid user code. Please try again.");
                        attempts++;

                        if (attempts >= maxAttempts)
                        {
                            Console.WriteLine("Maximum attempts exceeded. Exiting sign-in process.");
                            return;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid user code.");
                    Console.WriteLine($"Attempts Left: {attempts} - {maxAttempts}");
                    attempts++;

                    if (attempts >= maxAttempts)
                    {
                        Console.WriteLine("Maximum attempts exceeded. Exiting sign-in process.");
                        return;
                    }
                }

            } while (true);
        }
    }
}
// --- YOU CAN ADD MORE THINGS TO THE CODE IF YOU WANT FROM YOUR PREVILAGES AND OTHER TASKS THAT THEY WANT TO ADD TO THIS CODE ----//