using CRUD_Firebase.Models;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace CRUD_Firebase.Controllers
{
    public class TodoController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            //AuthSecret = "Firebase Database Secret",
            BasePath = "https://learnproject-b1443-default-rtdb.asia-southeast1.firebasedatabase.app/"
        };
        IFirebaseClient client;
        public IActionResult Index()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Todos");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Todo>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<Todo>(((JProperty)item).Value.ToString()));
                }
            }

            return View(list);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Todo todo)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
                var data = todo;
                PushResponse response = client.Push("Todos/", data);
                data.Id = response.Result.name;
                SetResponse setResponse = client.Set("Todos/" + data.Id, data);

                if (setResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ModelState.AddModelError(string.Empty, "Added Succesfully");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong!!");
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View();
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Todos/" + id);
            Todo data = JsonConvert.DeserializeObject<Todo>(response.Body);
            return View(data);
        }
        [HttpPost]
        public ActionResult Edit(Todo todo)
        {
            client = new FireSharp.FirebaseClient(config);
            SetResponse response = client.Set("Todos/" + todo.Id, todo);
            return RedirectToAction("Index");
        }
        public ActionResult Delete(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Delete("Todos/" + id);
            return RedirectToAction("Index");
        }
    }
}
