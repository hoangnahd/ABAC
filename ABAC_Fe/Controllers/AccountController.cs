using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using ABAC_Fe.Models;

namespace ABAC_Fe.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(string username, string password)
        {
            var loginResponse = await LoginAsync(username, password);

            if (loginResponse.Token != null && loginResponse.Token.Success)
            {
                // If login is successful, save the token to session
                Session["AuthToken"] = loginResponse.Token.TokenValue;

                // Call an API endpoint to check the user's role
                var isSysAdmin = await CheckUserSysAdminAsync(username, Session["AuthToken"] as string);

                if (isSysAdmin)
                {
                    // Redirect to Admin page if the user is an administrator
                    return RedirectToAction("Admin", "Home");
                }
                else
                {
                    // Redirect to Profile page for regular users
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                // If login fails, return the view with the login response model
                return View(loginResponse);
            }
        }


        // Action to display user profile
        public async Task<ActionResult> Profile()
        {
            // Check if user is authenticated
            if (Session["AuthToken"] == null)
            {
                // If user is not authenticated, redirect to login page
                return RedirectToAction("Login");
            }

            // Call the API to get user profile information
            var profileInfo = await GetProfileInfo();

            // Pass the profileInfo to the view
            return View(profileInfo);
        }
        private async Task<bool> CheckUserSysAdminAsync(string username, string token)
        {
            using (var client = new HttpClient())
            {
                // Set up HttpClient with appropriate base address, headers, etc.
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Make a request to an API endpoint to check the user's role
                var response = await client.PostAsync($"http://localhost:5291/api/access/isSysAdmin", null);

                if (response.IsSuccessStatusCode)
                {
                    return true; // Assuming the API returns a boolean value indicating sys admin status
                }
                else
                {
                    // Handle unsuccessful API call
                    // For example, log the error or return a default value
                    return false;
                }
            }
        }
        private async Task<LoginResponse> LoginAsync(string username, string password)
        {
            using (var httpClient = new HttpClient())
            {
                var requestBody = new { Username = username, Password = password };
                var content = new StringContent(JsonConvert.SerializeObject(requestBody), System.Text.Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("http://localhost:5291/api/Auth/login", content);

                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseBody);

                return loginResponse;
            }
        }

        private async Task<ProfileInfo> GetProfileInfo()
        {
            using (var httpClient = new HttpClient())
            {
                // Get the token from session
                var token = Session["AuthToken"] as string;

                // Add authorization token to request headers
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Call the API to get profile information
                var response = await httpClient.GetAsync("http://localhost:5291/api/profileInfo/profile");
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var profileInfo = JsonConvert.DeserializeObject<ProfileInfo>(responseBody);

                return profileInfo;
            }
        }
    }
}
