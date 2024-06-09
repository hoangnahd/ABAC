using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using ABAC_Fe.Models;
using System.Collections.Generic;

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

        // GET: Edit Profile
        public async Task<ActionResult> EditProfile()
        {
            // Check if user is authenticated
            if (Session["AuthToken"] == null)
            {
                return RedirectToAction("Login");
            }

            // Call the API to get user profile information
            var profileInfo = await GetProfileInfo();

            // Pass the profileInfo to the view
            return View(profileInfo);
        }

        // POST: Edit Profile
        [HttpPost]
        public async Task<ActionResult> EditProfile(ProfileInfo model)
        {
            // Check if user is authenticated
            if (Session["AuthToken"] == null)
            {
                return RedirectToAction("Login");
            }

            var token = Session["AuthToken"] as string;
            var success = await UpdateProfileInfo(model, token);

            if (success)
            {
                TempData["SuccessMessage"] = "Profile updated successfully.";
                return RedirectToAction("Profile");
            }
            else
            {
                ModelState.AddModelError("", "Failed to update profile");
                return View(model);
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
            using (var client = new HttpClient())
            {
                // Get the token from session
                var token = Session["AuthToken"] as string;

                // Add authorization token to request headers
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Call the API to get profile information
                var response = await client.GetAsync("http://localhost:5291/api/profileInfo/profile");
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var profileInfo = JsonConvert.DeserializeObject<ProfileInfo>(responseBody);

                return profileInfo;
            }
        }

        private async Task<bool> UpdateProfileInfo(ProfileInfo profileInfo, string token)
        {
            using (var httpClient = new HttpClient())
            {
                // Add authorization token to request headers
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var content = new StringContent(JsonConvert.SerializeObject(profileInfo), System.Text.Encoding.UTF8, "application/json");

                var response = await httpClient.PutAsync("http://localhost:5291/api/profileInfo/profile-edit", content);

                return response.IsSuccessStatusCode;
            }
        }

        // Action to display the IT page
        public ActionResult IT()
        {
            return View();
        }
        
        public ActionResult HR()
        {
            return View();
        }

        public ActionResult Finance()
        {
            return View();
        }
        // Action to fetch resource content
        public async Task<ActionResult> GetResourceContent(int resourceId)
        {
            if (Session["AuthToken"] == null)
            {
                return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);
            }

            var token = Session["AuthToken"] as string;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync($"http://localhost:5291/api/access/resource/{resourceId}?action=read");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return Json(new { success = true, content }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Failed to fetch content" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }

    public class Resource
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Sensitivity { get; set; }
        public string Content { get; set; }
        public string Department { get; set; }
        public List<int> Owner { get; set; }
    }

    public class LoginResponse
    {
        public Token Token { get; set; }
    }

    public class Token
    {
        public bool Success { get; set; }
        public string TokenValue { get; set; }
        public string Message { get; set; }
    }

    public class ProfileInfo
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
