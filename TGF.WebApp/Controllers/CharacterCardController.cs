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
    public class CharacterCardController : Controller
    {
        public IConfiguration Configuration;
        private JWToken JWToken;

        public CharacterCardController(IConfiguration configuration, JWToken jWToken)
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

            //string _restpath = "https://localhost:5001/profile";
            string _restpath = GetHostUrl().Content + CN();

            List<CharacterCardVM> characterCardsList = new List<CharacterCardVM>();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                using (var response = await httpClient.GetAsync(_restpath))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    characterCardsList = JsonConvert.DeserializeObject<List<CharacterCardVM>>(apiResponse);
                }
            }

            return View(characterCardsList);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CharacterCardVM c)
        {
            var tokenString = JWToken.GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(c);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

                    var response = await httpClient.PostAsync(_restpath, content);
                    if (!response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Błędne id postaci!";
                        TempData["Category"] = "danger";
                        if (User.IsInRole("Admin"))
                        {
                            return RedirectToAction("Index");
                        }
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var tokenString = JWToken.GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            CharacterCardVM c = new CharacterCardVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.GetAsync($"{_restpath}/{id}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        c = JsonConvert.DeserializeObject<CharacterCardVM>(apiResponse);
                    }
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            return View(c);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(CharacterCardVM c)
        {
            var tokenString = JWToken.GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            CharacterCardVM cResult = new CharacterCardVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(c);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

                    using (var response = await httpClient.PutAsync($"{ _restpath}/{c.Id}", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        cResult = JsonConvert.DeserializeObject<CharacterCardVM>(apiResponse);
                    }
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            return View(cResult);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var tokenString = JWToken.GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            CharacterCardVM c = new CharacterCardVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.GetAsync($"{ _restpath}/{id}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        c = JsonConvert.DeserializeObject<CharacterCardVM>(apiResponse);
                    }
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            return View(c);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(CharacterCardVM c)
        {
            var tokenString = JWToken.GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using var response = await httpClient.DeleteAsync($"{ _restpath}/{c.Id}");
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task CreateCard(CharacterCardVM c)
        {
            var tokenString = JWToken.GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + "CharacterCard";

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(c);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

                    using var response = await httpClient.PostAsync(_restpath, content);
                }
            }
            catch (Exception e)
            {
            }
        }

        public async Task EditCard(CharacterCardVM c)
        {
            string _restpath = GetHostUrl().Content + "CharacterCard";
            var tokenString = JWToken.GenerateJSONWebToken();

            CharacterCardVM cResult = new CharacterCardVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(c);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

                    var response = await httpClient.PutAsync($"{ _restpath}/{c.Id}", content);
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    cResult = JsonConvert.DeserializeObject<CharacterCardVM>(apiResponse);
                }
            }
            catch (Exception e)
            {
            }
        }

        public async Task DeleteCard(CharacterCardVM c)
        {
            var tokenString = JWToken.GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + "CharacterCard";

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using var response = await httpClient.DeleteAsync($"{ _restpath}/{c.Id}");
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}
