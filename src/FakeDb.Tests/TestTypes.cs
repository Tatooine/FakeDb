using System;

namespace FakeDb.Tests
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
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