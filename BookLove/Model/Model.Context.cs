﻿

//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------


namespace BookLove.Model
{

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


public partial class dbBookLoveEntities : DbContext
{
    public dbBookLoveEntities()
        : base("name=dbBookLoveEntities")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public virtual DbSet<Author> Author { get; set; }

    public virtual DbSet<Basket> Basket { get; set; }

    public virtual DbSet<Book> Book { get; set; }

    public virtual DbSet<City> City { get; set; }

    public virtual DbSet<Client> Client { get; set; }

    public virtual DbSet<Genre> Genre { get; set; }

    public virtual DbSet<GuestBasket> GuestBasket { get; set; }

    public virtual DbSet<Login> Login { get; set; }

    public virtual DbSet<Order> Order { get; set; }

    public virtual DbSet<OrderDetail> OrderDetail { get; set; }

    public virtual DbSet<OrderStatus> OrderStatus { get; set; }

    public virtual DbSet<Provider> Provider { get; set; }

    public virtual DbSet<Publisher> Publisher { get; set; }

    public virtual DbSet<Registration> Registration { get; set; }

    public virtual DbSet<Role> Role { get; set; }

    public virtual DbSet<Supple> Supple { get; set; }

}

}

