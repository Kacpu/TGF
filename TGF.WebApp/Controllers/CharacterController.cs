using Microsoft.AspNetCore.Authorization;
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
    public class CharacterController : Controller
    {
        public IConfiguration Configuration;
        public CharacterCardController _characterCardController;

        public CharacterController(IConfiguration configuration, CharacterCardController characterCardController)
        {
            Configuration = configuration;
            _characterCardController = characterCardController;
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

            string _restpath = GetHostUrl().Content + CN();

            List<CharacterVM> charactersList = new List<CharacterVM>();

            using (var httpClient = new HttpClient())
            {
                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

                using (var response = await httpClient.GetAsync(_restpath))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    charactersList = JsonConvert.DeserializeObject<List<CharacterVM>>(apiResponse);
                }
            }

            return View(charactersList);
        }

        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            //var tokenString = GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            CharacterVM c = new CharacterVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.GetAsync($"{_restpath}/{id}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        c = JsonConvert.DeserializeObject<CharacterVM>(apiResponse);
                    }
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            if (c == null)
            {
                TempData["Message"] = "Brak karty postaci!";
                TempData["Category"] = "danger";
                return RedirectToAction();
            }

            return View(c);
        }

        //[Authorize]
        public IActionResult Create()
        {
            return View();
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CharacterVM c)
        {
            // var tokenString = GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(c);
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

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            //var tokenString = GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            CharacterVM c = new CharacterVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.GetAsync($"{_restpath}/{id}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        c = JsonConvert.DeserializeObject<CharacterVM>(apiResponse);
                    }
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            return View(c);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(CharacterVM c)
        {
            //var tokenString = GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            CharacterVM cResult = new CharacterVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(c);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

                    using (var response = await httpClient.PutAsync($"{ _restpath}/{c.Id}", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        cResult = JsonConvert.DeserializeObject<CharacterVM>(apiResponse);
                    }
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            await _characterCardController.EditCard(c.CharacterCard);

            if (User.IsInRole("Admin"))
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Get), new { id = cResult.Id });
            }
        }

        //[Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            //var tokenString = GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            CharacterVM c = new CharacterVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.GetAsync($"{ _restpath}/{id}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        c = JsonConvert.DeserializeObject<CharacterVM>(apiResponse);
                    }
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            return View(c);
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(CharacterVM c)
        {
            //var tokenString = GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            await _characterCardController.DeleteCard(c.CharacterCard);

            try
            {
                using (var httpClient = new HttpClient())
                {
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    var response = await httpClient.DeleteAsync($"{ _restpath}/{c.Id}");
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
                return RedirectToAction("GetOne", "Profile", new { username = User.Identity.Name });
            }
        }
    }
}
