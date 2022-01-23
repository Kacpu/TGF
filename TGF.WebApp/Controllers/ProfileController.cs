using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TGF.Core.Domain;
using TGF.WebApp.Models;

namespace TGF.WebApp.Controllers
{
    public class ProfileController : Controller
    {
        public IConfiguration Configuration;
        private readonly UserManager<AppUser> _userManager;

        public ProfileController(IConfiguration configuration, UserManager<AppUser> userManager)
        {
            Configuration = configuration;
            _userManager = userManager;
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
            //var tokenString = GenerateJSONWebToken();

            //string _restpath = "https://localhost:5001/profile";
            string _restpath = GetHostUrl().Content + CN();

            List<ProfileVM> profilesList = new List<ProfileVM>();

            using (var httpClient = new HttpClient())
            {
                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                using (var response = await httpClient.GetAsync(_restpath))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    profilesList = JsonConvert.DeserializeObject<List<ProfileVM>>(apiResponse);
                }
            }

            return View(profilesList);
        }

        [Authorize]
        public async Task<IActionResult> GetOne(int id, string username)
        {
            //var tokenString = GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            ProfileVM p = new ProfileVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

                    if(username != null)
                    {
                        using (var response = await httpClient.GetAsync($"{_restpath}/filter?username={username}"))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            p = JsonConvert.DeserializeObject<ProfileVM>(apiResponse);
                        }
                    }
                    else
                    {
                        using (var response = await httpClient.GetAsync($"{_restpath}/{id}"))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            p = JsonConvert.DeserializeObject<ProfileVM>(apiResponse);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            if(p == null)
            {
                if (User.IsInRole("Admin"))
                {
                    TempData["Message"] = "Brak profilu użytkownika";
                    TempData["Category"] = "danger";
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction(nameof(Create));
            }

            return View(p);
        }

        [Authorize(Roles = "Admin")]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(ProfileVM p)
        {
            if (!User.IsInRole("Admin"))
            {
                //p.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                p.UserId = _userManager.GetUserId(User);
            }
            
            // var tokenString = GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(p);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

                    using (var response = await httpClient.PostAsync(_restpath, content))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            TempData["Message"] = "Błędne id usera!";
                            TempData["Category"] = "danger";
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            if (User.IsInRole("Admin"))
            {
                TempData["Message"] = "Dodano nowy profil o nazwie: " + p.Name;
                TempData["Category"] = "success";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(GetOne), new { username = User.Identity.Name });
            }
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            //var tokenString = GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            ProfileVM p = new ProfileVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.GetAsync($"{_restpath}/{id}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        p = JsonConvert.DeserializeObject<ProfileVM>(apiResponse);
                    }
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            return View(p);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(ProfileVM p)
        {
            //var tokenString = GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            ProfileVM pResult = new ProfileVM();

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
                        pResult = JsonConvert.DeserializeObject<ProfileVM>(apiResponse);
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
                return RedirectToAction(nameof(GetOne), new { username = User.Identity.Name });
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            //var tokenString = GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            ProfileVM p = new ProfileVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.GetAsync($"{ _restpath}/{id}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        p = JsonConvert.DeserializeObject<ProfileVM>(apiResponse);
                    }
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            return View(p);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(ProfileVM p)
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
