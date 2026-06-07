namespace StarCorp.Travel.Domain.Customers;
using StarCorp.Travel.Domain.Shared;

public class Customer : Entity
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Address { get; private set; }
    public string DocumentNumber { get; private set; }
    public bool IsActive { get; private set; }

    public Customer(string name, string email, string phoneNumber, string address, string documentNumber)
    {
        Guard.ValidateString(name, nameof(name));
        Guard.ValidateString(email, nameof(email));
        Guard.ValidateString(phoneNumber, nameof(phoneNumber));
        Guard.ValidateString(address, nameof(address));
        Guard.ValidateString(documentNumber, nameof(documentNumber));

        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        DocumentNumber = documentNumber;
        IsActive = true;
    }

    public static Customer Restore(Guid id, string name, string email, string phoneNumber, string address, string documentNumber, bool isActive, DateTime createdAt, DateTime updatedAt)
    {
        var customer = new Customer(name, email, phoneNumber, address, documentNumber);
        customer.SetPersistenceData(id, createdAt, updatedAt);
        customer.IsActive = isActive;
        return customer;
    }
}