using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication8.Models;

namespace WebApplication8.Controllers
{
  public class HomeController : Controller
  {
    public ActionResult Index(int page = 1)
    {
      int pageNum = 1;
      var client = GetClient();
      List<Posts> posts = new List<Posts>();
      using (client)
      {
        //Called Member default GET All Posts  
        var result = client.GetAsync("posts" + "?page=" + page).GetAwaiter().GetResult();
        //If success received   
        if (result.IsSuccessStatusCode)
        {
          var s = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
          JObject jObject = JObject.Parse(s);
          int.TryParse(jObject["meta"]["pagination"]["pages"].ToString(), out pageNum);

          JsonConvert.PopulateObject(jObject["data"].ToString(), posts);
        }
        else
        {
          ModelState.AddModelError(string.Empty, "Server error try after some time.");
        }
      }
      ViewData["pageNum"] = pageNum;
      ViewData["posts"] = posts;
      return View();
    }

    public ActionResult About(int userID)
    {
      var client = GetClient("users/" + userID + "/posts");
      List<Posts> posts = new List<Posts>();
      using (client)
      {
        var result = client.GetAsync(client.BaseAddress).GetAwaiter().GetResult();
        //If success received   
        if (result.IsSuccessStatusCode)
        {
          var s = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
          JObject jObject = JObject.Parse(s);
          JsonConvert.PopulateObject(jObject["data"].ToString(), posts);
        }
        else
        {
          ModelState.AddModelError(string.Empty, "Server error try after some time.");
        }
      }
      ViewData["posts"] = posts;

      return View();
    }

    public ActionResult Users()
    {
      var client = GetClient("users");
      List<User> users = new List<User>();
      using (client)
      {
        var result = client.GetAsync(client.BaseAddress).GetAwaiter().GetResult();

        //If success received   
        if (result.IsSuccessStatusCode)
        {
          var s = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
          JObject jObject = JObject.Parse(s);
          JsonConvert.PopulateObject(jObject["data"].ToString(), users);
        }
        else
        {
          ModelState.AddModelError(string.Empty, "Server error try after some time.");
        }
      }
      ViewData["users"] = users;

      return View();
    }

    public ActionResult Contact()
    {
      ViewBag.Message = "Your contact page.";

      return View();
    }

    public ActionResult ToDo(int page = 1)
    {
      int pageNum = 1;
      var client = GetClient("todos?page=" + page);
      List<ToDO> toDOs = new List<ToDO>();
      using (client)
      {
        var result = client.GetAsync(client.BaseAddress).GetAwaiter().GetResult();
        //If success received   
        if (result.IsSuccessStatusCode)
        {
          var s = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
          JObject jObject = JObject.Parse(s);
          int.TryParse(jObject["meta"]["pagination"]["pages"].ToString(), out pageNum);
          JsonConvert.PopulateObject(jObject["data"].ToString(), toDOs);
        }
        else
        {
          ModelState.AddModelError(string.Empty, "Server error try after some time.");
        }
      }
      ViewData["pageNum"] = pageNum;

      ViewData["toDOs"] = toDOs;

      return View();
    }

    public ActionResult Delete(int userID, int postid)
    {
      var client = GetClient("users/" + userID + "/posts/");
      using (client)
      {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "142e5fd279533942bc665559f3fc28463cbc3c30fb0453567735fcf663cc2b38");
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        ViewBag.Message = "Your contact page.";
        var postTask = client.DeleteAsync(client.BaseAddress).Result;

        var result = postTask.Content.ReadAsStringAsync().Result;
      }
      return View();
    }


    public ActionResult DeleteUser(int userID)
    {
      var client = GetClient("users/" + userID);
      using (client)
      {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "142e5fd279533942bc665559f3fc28463cbc3c30fb0453567735fcf663cc2b38");
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        ViewBag.Message = "Your contact page.";
        var postTask = client.DeleteAsync(client.BaseAddress).Result;

        var result = postTask.Content.ReadAsStringAsync().Result;
      }
      return RedirectToAction("Users", "Home");
    }


    public ActionResult CreateNewUser()
    {
      var newModel = new User();
      return View("CreateNewUser", newModel);
    }

    [HttpGet]
    public ActionResult UpdateUser(User user)
    {
      return View("CreateNewUser", user);
    }

    [HttpPost]
    public ActionResult CreateNewUser(User user)
    {
      string url = user.Id > 0 ? "users/" + user.Id : "users/";
      var client = GetClient(url);
      using (client)
      {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "142e5fd279533942bc665559f3fc28463cbc3c30fb0453567735fcf663cc2b38");
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //HTTP POST
        var content = new FormUrlEncodedContent(new[]
        {
             new KeyValuePair<string, string>("name", user.Name.ToString()),
            new KeyValuePair<string, string>("email", user.Email.ToString()),
            new KeyValuePair<string, string>("gender", user.Gender.ToString()),
            new KeyValuePair<string, string>("status", user.Status.ToString())
        });
        if (user.Id > 0)
        {
          var task = client.PutAsync("", content).Result;
          var result = task.Content.ReadAsStringAsync().Result;
          ViewBag.SuccessMsg = "successfully Updated";
        }
        else
        {
          var task = client.PostAsync("", content).Result;
          var result = task.Content.ReadAsStringAsync().Result;
          ViewBag.SuccessMsg = "successfully Created";
        }

        return View();
      }
    }

    public ActionResult CreateNewPOst(int userID = 1)
    {
      var newModel = new Posts();
      newModel.user_id = userID;

      return View("CreateNewPost", newModel);
    }

    [HttpPost]
    public ActionResult CreateNewPost(Posts posts)
    {
      var client = GetClient("users/" + posts.user_id + "/posts");
      using (client)
      {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "142e5fd279533942bc665559f3fc28463cbc3c30fb0453567735fcf663cc2b38");
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //HTTP POST

        var content = new FormUrlEncodedContent(new[]
        {
             new KeyValuePair<string, string>("user_id", posts.user_id.ToString()),
            new KeyValuePair<string, string>("title", posts.title.ToString()),
            new KeyValuePair<string, string>("body", posts.body.ToString())

        });

        var postTask = client.PostAsync("", content).Result;

        var result = postTask.Content.ReadAsStringAsync().Result;
        JObject jObject = JObject.Parse(result);
        JsonConvert.PopulateObject(jObject["data"].ToString(), posts);
        ViewBag.SuccessMsg = "successfully added";
        return View(posts);
      }
    }


    private HttpClient GetClient(string url = null)
    {
      var client = new HttpClient();
      if (string.IsNullOrEmpty(url))
      {
        client.BaseAddress = new Uri("https://gorest.co.in/public-api/");
      }
      else
      {
        client.BaseAddress = new Uri("https://gorest.co.in/public-api/" + url);
      }
      return client;
    }
  }
}