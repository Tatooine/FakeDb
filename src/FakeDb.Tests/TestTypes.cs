using System;

namespace FakeDb.Tests
{
    public class Car
    {
        public int Id { get; protected set; }
        public int CarId { get; protected set; }
    }

    public class Wheel
    {
        public short Id { get; set; }
    }

    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AddressId { get; set; }
        public string _AddressId { get; set; }
        public Address Address { get; set; }
    }

    public class Address
    {
        public string Id { get; set; }
    }

    public class User
    {
        public string Username { get; set; }
        public Profile Profile { get; set; }
    }

    public class Profile
    {
        public string Theme { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}