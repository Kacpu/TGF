using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TGF.WebApp.Models;

namespace TGF.WebApp.Controllers
{
    public class PostController : Controller
    {
        public IConfiguration Configuration;
        private JWToken JWToken;

        public PostController(IConfiguration configuration, JWToken jWToken)
        {
            Configuration = configuration;
            JWToken = jWToken;
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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var tokenString = JWToken.GenerateJSONWebToken();

            //string _restpath = "https://localhost:5001/post";
            string _restpath = GetHostUrl().Content + CN();

            List<PostVM> postsList = new List<PostVM>();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                using (var response = await httpClient.GetAsync(_restpath))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    postsList = JsonConvert.DeserializeObject<List<PostVM>>(apiResponse);
                }
            }

            foreach(var post in postsList)
            {
                if(post.Content != null)
                {
                    post.Short = post.Content.Substring(0, post.Content.Length < 200 ? post.Content.Length : 200);
                }
            }

            return View(postsList);
        }

        [Authorize]
        public async Task<IActionResult> Create(int story)
        {
            var tokenString = JWToken.GenerateJSONWebToken();

            PostVM post = new PostVM()
            {
                StoryId = story
            };

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.GetAsync($"{GetHostUrl().Content}profile/filter?username={User.Identity.Name}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        ProfileVM pr = JsonConvert.DeserializeObject<ProfileVM>(apiResponse);
                        if (pr != null)
                        {
                            post.ProfileId = pr.Id;
                            post.Profile = pr;
                        }
                        else
                        {
                            TempData["Message"] = "Nie udało się pobrać profilu użytkownika!";
                            TempData["Category"] = "danger";
                            if (User.IsInRole("Admin"))
                            {
                                return RedirectToAction("Index");
                            }
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            return View(post);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(PostVM p)
        {
            var tokenString = JWToken.GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(p);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

                    var response = await httpClient.PostAsync(_restpath, content);
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            if (User.IsInRole("Admin"))
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction("Get", "Story", new { id = p.StoryId });
            }
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var tokenString = JWToken.GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            PostVM p = new PostVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
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

            if (p == null)
            {
                TempData["Message"] = "Nie można pobrać postu!";
                TempData["Category"] = "danger";
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("GetOne", "Profile", new { username = User.Identity.Name });
                }
            }

            return View(p);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(PostVM p)
        {
            var tokenString = JWToken.GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            PostVM pResult = new PostVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(p);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

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

            if (User.IsInRole("Admin"))
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction("Get", "Story", new { id = pResult.StoryId });
            }
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var tokenString = JWToken.GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            PostVM p = new PostVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
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

            if (p == null)
            {
                TempData["Message"] = "Nie można pobrać postu!";
                TempData["Category"] = "danger";
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("GetOne", "Profile", new { username = User.Identity.Name });
                }
            }

            return View(p);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(PostVM p)
        {
            var tokenString = JWToken.GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using var response = await httpClient.DeleteAsync($"{ _restpath}/{p.Id}");
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            if (User.IsInRole("Admin"))
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction("Get", "Story", new { id = p.StoryId });
            }
        }
    }
}
