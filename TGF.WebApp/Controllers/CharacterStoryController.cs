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
    public class CharacterStoryController : Controller
    {
        public IConfiguration Configuration;
        private JWToken JWToken;

        public CharacterStoryController(IConfiguration configuration, JWToken jWToken)
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

        //[Authorize]
        //public async Task<IActionResult> Get(int cId, int sId)
        //{
        //    //var tokenString = GenerateJSONWebToken();
        //    string _restpath = GetHostUrl().Content + CN();
        //    CharacterStoryVM cs = new CharacterStoryVM();

        //    try
        //    {
        //        using (var httpClient = new HttpClient())
        //        {
        //            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
        //            using (var response = await httpClient.GetAsync($"{_restpath}?cId={cId}&sId={sId}"))
        //            {
        //                string apiResponse = await response.Content.ReadAsStringAsync();
        //                cs = JsonConvert.DeserializeObject<CharacterStoryVM>(apiResponse);
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //       // return View(e);
        //    }

        //    //if (cs == null)
        //    //{
        //    //    TempData["Message"] = "Brak Historii!";
        //    //    TempData["Category"] = "danger";
        //    //    return RedirectToAction("Get", "Character", new { id = character });
        //    //}

        //    //return View(cs);
        //}

        [Authorize]
        public async Task<IActionResult> Create(int sId, int cId)
        {
            var tokenString = JWToken.GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            CharacterStoryVM cs = new CharacterStoryVM()
            {
                CharactersId = cId,
                StoriesId = sId
            };

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(cs);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

                    using var response = await httpClient.PostAsync(_restpath, content);
                }
            }
            catch (Exception)
            {
                TempData["Message"] = "Błąd dodawania postaci do historii!";
                TempData["Category"] = "danger";
                return RedirectToAction("Get", "Story", new { id = sId });
            }

            return RedirectToAction("Get", "Story", new { id = sId });
        }

        //[Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> Delete(int sId, int cId)
        {
            var tokenString = JWToken.GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using var response = await httpClient.DeleteAsync($"{_restpath}?cId={cId}&sId={sId}");
                }
            }
            catch (Exception)
            {
                TempData["Message"] = "Błąd usuwania postaci z historii!";
                TempData["Category"] = "danger";
                return RedirectToAction("Get", "Story", new { id = sId });
            }

            return RedirectToAction("Get", "Story", new { id = sId });
        }
    }
}
