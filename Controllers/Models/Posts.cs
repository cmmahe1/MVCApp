using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication8.Models
{
  public class Posts
  {

    public int Id { get; set; }
    public int user_id { get; set; }

    public string title { get; set; }

    public string body { get; set; }
    public DateTime Created_at { get; set; }
    public DateTime Updated_at { get; set; }

  }





}