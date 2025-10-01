using ControlRH.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ControlRH.Controllers
{
    public abstract class SimpleController : Controller
    {
        protected void ShowToast(string message, ToastType type = ToastType.Success)
        {
            TempData["ToastType"] = type.ToString().ToLower();
            TempData["ToastMessage"] = message;
        }
    }
}
