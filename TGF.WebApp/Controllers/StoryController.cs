using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TGF.WebApp.Models;

namespace TGF.WebApp.Controllers
{
    public class StoryController : Controller
    {
        public IConfiguration Configuration;

        public StoryController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public ContentResult GetHostUrl()
        {
            var result = Configuration["RestApiUrl:HostUrl"];
            return Content(result);
        }

        private string CN()
        {
            return ControllerContext.RouteData.Values["controller"].ToString();
        }

        public async Task<IActionResult> Index()
        {
            //var tokenString = GenerateJSONWebToken();

            //string _restpath = "https://localhost:5001/profile";
            string _restpath = GetHostUrl().Content + CN();

            List<StoryVM> storiesList = new List<StoryVM>();

            using (var httpClient = new HttpClient())
            {
                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                using (var response = await httpClient.GetAsync(_restpath))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    storiesList = JsonConvert.DeserializeObject<List<StoryVM>>(apiResponse);
                }
            }

            return View(storiesList);
        }

        //[Authorize]
        public IActionResult Create()
        {
            return View();
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(StoryVM s)
        {
            // var tokenString = GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(s);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

                    using var response = await httpClient.PostAsync(_restpath, content);
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            return RedirectToAction(nameof(Index));
        }

        //[Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            //var tokenString = GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            StoryVM s = new StoryVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.GetAsync($"{_restpath}/{id}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        s = JsonConvert.DeserializeObject<StoryVM>(apiResponse);
                    }
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            return View(s);
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(StoryVM s)
        {
            //var tokenString = GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            StoryVM sResult = new StoryVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(s);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

                    using (var response = await httpClient.PutAsync($"{ _restpath}/{s.Id}", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        sResult = JsonConvert.DeserializeObject<StoryVM>(apiResponse);
                    }
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            return View(sResult);
        }

        //[Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            //var tokenString = GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            StoryVM s = new StoryVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.GetAsync($"{ _restpath}/{id}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        s = JsonConvert.DeserializeObject<StoryVM>(apiResponse);
                    }
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            return View(s);
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(StoryVM p)
        {
            //var tokenString = GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using var response = await httpClient.DeleteAsync($"{ _restpath}/{p.Id}");
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
