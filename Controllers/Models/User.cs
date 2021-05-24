using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication8.Models
{
  public class User
  {
    public int Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Gender { get; set; }

    public string Status { get; set; }

    public DateTime Created_at { get; set; }
    public DateTime Updated_at { get; set; }
  }

  public class ToDO
  {
    public int Id { get; set; }
    public int User_id { get; set; }

    public string Title { get; set; }
    public string Completed { get; set; }
    public DateTime Created_at { get; set; }
    public DateTime Updated_at { get; set; }
  }


  public class Comments
  {

    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string body { get; set; }
    public DateTime Created_at { get; set; }
    public DateTime Updated_at { get; set; }

  }

  public class Categories
  {

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }

  }

  public class Products
  {

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public double Price { get; set; }
    public double discount_amount { get; set; }
    public string Status { get; set; }

    List<Categories> categories { get; set; }


  }
}