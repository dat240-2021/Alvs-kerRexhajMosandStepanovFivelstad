using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Domain.Authentication
{
    public class User : BaseEntity{
        public int Id {get; protected set;}
        public string Name {get; protected set;}
        public string Password {get; protected set;}

        public User(){}
        public User(string n ,string p )
        {
            Name = n;
            Password = p;

        }

    }

}

