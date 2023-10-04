using AzafranML_V3.Models;
using AzafranML_V3.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace AzafranML_V3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Predict()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Predict(PredictionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Handle validation failures
                return View(model);
            }

            // Your prediction code here

            // Assuming you want to display the results on the same page
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> RoleManagement()
        {
            var roles = _roleManager.Roles.ToList();
            var users = _userManager.Users.ToList();
            var userRoles = new Dictionary<string, IList<string>>();

            foreach (var user in users)
            {
                userRoles[user.Id] = await _userManager.GetRolesAsync(user);
            }

            var model = new RoleManagementViewModel
            {
                Roles = roles,
                Users = users,
                UserRoles = userRoles
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RoleManagement(RoleManagementViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _roleManager.RoleExistsAsync(model.RoleName))
                {
                    ModelState.AddModelError("", "Role already exists.");
                }
                else
                {
                    var role = new IdentityRole(model.RoleName);
                    var result = await _roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("RoleManagement");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            model.Roles = _roleManager.Roles.ToList();
            model.Users = _userManager.Users.ToList();
            model.UserRoles = new Dictionary<string, IList<string>>();
            foreach (var user in model.Users)
            {
                model.UserRoles[user.Id] = await _userManager.GetRolesAsync(user);
            }

            return View(model);
        }

        public async Task<IActionResult> EditUserRoles(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();
            var model = new EditUserRolesViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                UserRoles = userRoles,
                AllRoles = allRoles
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserRoles(string userId, List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError($"User with ID {userId} not found.");
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var addedRoles = roles.Except(userRoles);
            var removedRoles = userRoles.Except(roles);

            var addResult = await _userManager.AddToRolesAsync(user, addedRoles);
            if (!addResult.Succeeded)
            {
                _logger.LogError($"Error adding roles for user {userId}: {string.Join(", ", addResult.Errors.Select(e => e.Description))}");
                foreach (var error in addResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            var removeResult = await _userManager.RemoveFromRolesAsync(user, removedRoles);
            if (!removeResult.Succeeded)
            {
                _logger.LogError($"Error removing roles for user {userId}: {string.Join(", ", removeResult.Errors.Select(e => e.Description))}");
                foreach (var error in removeResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return RedirectToAction("RoleManagement");
        }
        [HttpGet]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                _logger.LogError($"Role with ID {id} not found.");
                return NotFound();
            }
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRoleConfirmed(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                _logger.LogError($"Role with ID {id} not found.");
                return NotFound();
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("RoleManagement");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View("DeleteRole", role);
        }

    }
}
