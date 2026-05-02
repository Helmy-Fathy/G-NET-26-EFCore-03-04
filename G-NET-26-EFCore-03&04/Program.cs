using G_NET_26_EFCore_03_04.Enums;
using G_NET_26_EFCore_03_04.Models;
using Microsoft.EntityFrameworkCore;

namespace G_NET_26_EFCore_03_04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var bankContext = new BankContext())
                bankContext.Database.Migrate();


            while (true)
            {
                Console.Clear();
                Console.WriteLine("1) Add a new Customer");
                Console.WriteLine("2) Open a new Account for a Customer (Active / Closed)");
                Console.WriteLine("3) Update Account Status");
                Console.WriteLine("4) Remove an Account From a Customer");
                Console.WriteLine("5) List all Customers (With Accounts)");
                Console.WriteLine("0) Exit");

                Console.Write("Enter Choice: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid input.");
                    Console.WriteLine("Press any key to return to the menu...");
                    Console.ReadKey();
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        AddCustomer();
                        break;
                    case 2:
                        OpenNewAccount();
                        break;
                    case 3:
                        UpdateAccountStatus();
                        break;
                    case 4:
                        RemoveAccountFromCustomer();
                        break;
                    case 5:
                        ListAllCustomers();
                        break;
                    case 0:
                        Exit();
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                Console.WriteLine("Press any key to return to the menu...");
                Console.ReadKey();
            }
        }

        static void AddCustomer()
        {

            Console.WriteLine("\n--- Add New Customer ---");

            Console.Write("Full Name      : ");
            string fullName = Console.ReadLine()!.Trim();

            Console.Write("National ID    : ");
            string nationalId = Console.ReadLine()!.Trim();

            DateTime dob;
            while (true)
            {
                Console.Write("Date of Birth  : (yyyy-MM-dd) ");
                if (DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd",
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None, out dob))
                    break;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  Invalid date format. Use yyyy-MM-dd");
                Console.ResetColor();
            }

            Console.Write("Email          : ");
            string email = Console.ReadLine()!.Trim();

            Console.Write("Phone          : ");
            string phone = Console.ReadLine()!.Trim();

            Console.Write("Address        : ");
            string address = Console.ReadLine()!.Trim();

            Console.WriteLine("Customer Type:");
            Console.WriteLine("     1) Individual");
            Console.WriteLine("     2) Business");
            Console.Write("  Choice: ");
            int.TryParse(Console.ReadLine(), out int typeChoice);
            CustomerType customerType = typeChoice == 2 ? CustomerType.Business : CustomerType.Individual;

            using (var bankContext = new BankContext())
            {
                if (bankContext.Customers.Any(c => c.NationalID == nationalId))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n  National ID '{nationalId}' already exists.");
                    Console.ResetColor();
                }
                else
                {
                    var customer = new Customer
                    {
                        FullName = fullName,
                        NationalID = nationalId,
                        DateOfBirth = dob,
                        Email = email,
                        PhoneNumber = phone,
                        Address = address,
                        CustomerType = customerType
                    };

                    bankContext.Customers.Add(customer);
                    bankContext.SaveChanges();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nCustomer created successfully. CustomerId = {customer.Id}");
                    Console.ResetColor();
                }
            }

        }

        static void OpenNewAccount()
        {
            Console.WriteLine("\n--- Open New Account ---");

            Console.Write("Account Number : ");
            string accountNumber = Console.ReadLine()!.Trim();

            Console.WriteLine("Account Type:");
            Console.WriteLine("     1) Savings");
            Console.WriteLine("     2) Current");
            Console.WriteLine("     3) Business");
            Console.Write("  Choice: ");
            int.TryParse(Console.ReadLine(), out int accTypeChoice);
            AccountType accountType = accTypeChoice switch
            {
                2 => AccountType.Current,
                3 => AccountType.Business,
                _ => AccountType.Savings
            };

            Console.Write("Branch Code    : ");
            string branchCode = Console.ReadLine()!.Trim();

            Console.Write("Customer Id    : ");
            int.TryParse(Console.ReadLine(), out int customerId);

            Console.WriteLine("Ownership Role:");
            Console.WriteLine("     1) Primary");
            Console.WriteLine("     2) CoHolder");
            Console.Write("  Choice: ");
            int.TryParse(Console.ReadLine(), out int roleChoice);
            OwnershipType ownershipRole = roleChoice == 2 ? OwnershipType.CoHolder : OwnershipType.Primary;

            using (var bankContext = new BankContext())
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\nValidating branch '{branchCode}' and customer #{customerId}...");
                Console.ResetColor();

                var branch = bankContext.Branches.FirstOrDefault(b => b.Code == branchCode);
                var customer = bankContext.Customers.Find(customerId);

                if (branch is null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"  Branch '{branchCode}' not found.");
                    Console.ResetColor();
                }
                else if (customer is null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"  Customer #{customerId} not found.");
                    Console.ResetColor();
                }
                else if (bankContext.Accounts.Any(a => a.AccountNumber == accountNumber))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"  Account '{accountNumber}' already exists.");
                    Console.ResetColor();
                }
                else
                {
                    bankContext.Accounts.Add(new Account
                    {
                        AccountNumber = accountNumber,
                        AccountType = accountType,
                        CurrentBalance = 0m,
                        OpeningDate = DateTime.UtcNow,
                        BranchCode = branch.Code
                    });

                    bankContext.CustomerAccounts.Add(new CustomerAccount
                    {
                        CustomerId = customerId,
                        AccountNumber = accountNumber,
                        AccountStatus = AccountStatus.Active,
                        OwnershipType = ownershipRole,
                        OwnerShipStartDate = DateTime.UtcNow
                    });

                    bankContext.SaveChanges();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Account '{accountNumber}' created and linked to customer {customerId} as {ownershipRole} owner.");
                    Console.ResetColor();
                }
            }

        }

        static void UpdateAccountStatus()
        {
            Console.WriteLine("\n--- Update Account Status ---");

            Console.Write("Account Number : ");
            string accountNumber = Console.ReadLine()!.Trim();

            Console.Write("Customer Id    : ");
            int.TryParse(Console.ReadLine(), out int customerId);

            using (var bankContext = new BankContext())
            {
                var link = bankContext.CustomerAccounts
                             .Include(ca => ca.Account)
                             .FirstOrDefault(ca => ca.AccountNumber == accountNumber
                                                && ca.CustomerId == customerId);

                if (link is null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"  No link found between account '{accountNumber}' and customer #{customerId}.");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("New Status:");
                    Console.WriteLine("     1) Active");
                    Console.WriteLine("     2) Closed");
                    Console.Write("  Choice: ");
                    int.TryParse(Console.ReadLine(), out int statusChoice);

                    link.AccountStatus = statusChoice == 2 ? AccountStatus.Closed : AccountStatus.Active;
                    bankContext.SaveChanges();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Status updated to {link.AccountStatus}.");
                    Console.ResetColor();
                }
            }

        }

        static void RemoveAccountFromCustomer()
        {
            Console.WriteLine("\n--- Remove Account From Customer ---");

            Console.Write("Account Number : ");
            string accountNumber = Console.ReadLine()!.Trim();

            Console.Write("Customer Id    : ");
            int.TryParse(Console.ReadLine(), out int customerId);

            using (var bankContext = new BankContext())
            {
                var link = bankContext.CustomerAccounts
                             .FirstOrDefault(ca => ca.AccountNumber == accountNumber
                                                && ca.CustomerId == customerId);

                if (link is null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"  No link found between account '{accountNumber}' and customer #{customerId}.");
                    Console.ResetColor();
                }
                else
                {
                    bankContext.CustomerAccounts.Remove(link);
                    bankContext.SaveChanges();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("  Ownership link deleted.");

                    // If no other owners remain → delete the account itself
                    bool hasOtherOwners = bankContext.CustomerAccounts.Any(ca => ca.AccountNumber == accountNumber);
                    if (!hasOtherOwners)
                    {
                        var orphanAccount = bankContext.Accounts.Find(accountNumber);
                        if (orphanAccount is not null)
                        {
                            bankContext.Accounts.Remove(orphanAccount);
                            bankContext.SaveChanges();
                        }
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"      That was the last owner — account '{accountNumber}' was also removed.");
                    }

                    Console.ResetColor();
                }
            }

        }

        static void ListAllCustomers()
        {
            Console.WriteLine("\n--- All Customers ---\n");

            using var bankContext = new BankContext();

            var customers = bankContext.Customers
                              .Include(c => c.CustomerAccounts)
                                  .ThenInclude(ca => ca.Account)
                                      .ThenInclude(a => a.Branch)
                              .OrderBy(c => c.Id)
                              .ToList();

            foreach (var c in customers)
            {
                Console.WriteLine($"  #{c.Id} {c.FullName} ({c.CustomerType})");

                if (!c.CustomerAccounts.Any())
                {
                    Console.WriteLine("       (no accounts)");
                }
                else
                {
                    foreach (var ca in c.CustomerAccounts)
                    {
                        var a = ca.Account;
                        Console.WriteLine(
                            $"       {a.AccountNumber,-12} {a.AccountType,-10} " +
                            $"Balance: {a.CurrentBalance,12:N2}  " +
                            $"{ca.OwnershipType,-9} {ca.AccountStatus,-7} " +
                            $"@ {a.Branch?.Name ?? "Unknown"}");
                    }
                }
            }

        }

        static void Exit()
        {
            Console.WriteLine("Goodbye!");
            Environment.Exit(0);
        }
    }
}
