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

        [Authorize(Roles = "Admin")]
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

        [Authorize]
        public async Task<IActionResult> Get(int id, int character)
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

                    using (var response = await httpClient.GetAsync($"{GetHostUrl().Content}character"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        s.OtherCharacters = JsonConvert.DeserializeObject<List<CharacterVM>>(apiResponse);
                        foreach(CharacterVM c in s.Characters)
                        {
                            s.OtherCharacters.Remove(s.OtherCharacters.First(x => x.Id == c.Id)); 
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            if (s == null)
            {
                TempData["Message"] = "Brak Historii!";
                TempData["Category"] = "danger";
                return RedirectToAction("Get", "Character", new { id = character });
            }

            return View(s);
        }

        [Authorize]
        public IActionResult Create(int character)
        {
            StoryVM characterVM = new StoryVM() { FromCharacter = character };

            return View(characterVM);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(StoryVM s)
        {
            // var tokenString = GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();
            int Id = 0;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(s);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

                    var response = await httpClient.PostAsync(_restpath, content);
                    if (!response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Błąd tworzenia story!";
                        TempData["Category"] = "danger";
                        if (User.IsInRole("Admin"))
                        {
                            return RedirectToAction("Index");
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        Id = int.Parse(response.Headers.Location.Segments[2]);
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
                return RedirectToAction("Create", "CharacterStory", new { sId = Id, cId = s.FromCharacter});
            }
        }

        [Authorize(Roles = "Admin")]
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

            if (s == null)
            {
                TempData["Message"] = "Nie można pobrać historii!";
                TempData["Category"] = "danger";
                return RedirectToAction("Index");
            }

            return View(s);
        }

        [Authorize(Roles = "Admin")]
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

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
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

            if (s == null)
            {
                TempData["Message"] = "Nie można pobrać historii!";
                TempData["Category"] = "danger";
                return RedirectToAction("Index");
            }

            return View(s);
        }

        [Authorize(Roles = "Admin")]
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

        //[Authorize]
        //public async Task<IActionResult> AddCharacter(int storyId, int characterId)
        //{
        //    //var tokenString = GenerateJSONWebToken();
        //    string _restpath = GetHostUrl().Content + CN();

        //    StoryVM sResult = new StoryVM();
        //    StoryVM s = new StoryVM();
        //    CharacterVM c = new CharacterVM();

        //    try
        //    {
        //        using (var httpClient = new HttpClient())
        //        {
        //            using (var response = await httpClient.GetAsync($"{_restpath}/{storyId}"))
        //            {
        //                string apiResponse = await response.Content.ReadAsStringAsync();
        //                s = JsonConvert.DeserializeObject<StoryVM>(apiResponse);
        //            }

        //            using (var response = await httpClient.GetAsync($"{GetHostUrl().Content}character/{characterId}"))
        //            {
        //                string apiResponse = await response.Content.ReadAsStringAsync();
        //                s = JsonConvert.DeserializeObject<StoryVM>(apiResponse);
        //            }


        //            string jsonString = System.Text.Json.JsonSerializer.Serialize(s);
        //            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
        //            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

        //            using (var response = await httpClient.PutAsync($"{ _restpath}/{s.Id}", content))
        //            {
        //                string apiResponse = await response.Content.ReadAsStringAsync();
        //                sResult = JsonConvert.DeserializeObject<StoryVM>(apiResponse);
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return View(e);
        //    }

        //    return RedirectToAction(nameof(Index));
        //}
    }
}
