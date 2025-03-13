using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace BankingProductsData;

public partial class User : TableEntity
{
    public User(string firstName, string lastName, string password, string email, string mobile, bool active, DateTime joinedOn)
    {
        PartitionKey = email;
        RowKey = firstName;

        FirstName = firstName;
        LastName = lastName;
        Password = password;
        Email = email;
        Mobile = mobile;
        Active = active;
        JoinedOn = joinedOn;
    }

    public User() { }

    public string ID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public bool Active { get; set; }
    public DateTime JoinedOn { get; set; }
}