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
    public class PostController : Controller
    {
        public IConfiguration Configuration;

        public PostController(IConfiguration configuration)
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

            List<PostVM> postsList = new List<PostVM>();

            using (var httpClient = new HttpClient())
            {
                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                using (var response = await httpClient.GetAsync(_restpath))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    postsList = JsonConvert.DeserializeObject<List<PostVM>>(apiResponse);
                }
            }

            return View(postsList);
        }

        //[Authorize]
        public IActionResult Create()
        {
            return View();
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(PostVM p)
        {
            // var tokenString = GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(p);
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

            PostVM p = new PostVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.GetAsync($"{_restpath}/{id}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        p = JsonConvert.DeserializeObject<PostVM>(apiResponse);
                    }
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            return View(p);
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(PostVM p)
        {
            //var tokenString = GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            PostVM pResult = new PostVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(p);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

                    using (var response = await httpClient.PutAsync($"{ _restpath}/{p.Id}", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        pResult = JsonConvert.DeserializeObject<PostVM>(apiResponse);
                    }
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            return View(pResult);
        }

        //[Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            //var tokenString = GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            PostVM p = new PostVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.GetAsync($"{ _restpath}/{id}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        p = JsonConvert.DeserializeObject<PostVM>(apiResponse);
                    }
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            return View(p);
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(PostVM p)
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
