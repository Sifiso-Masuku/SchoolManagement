
using System.Web.Mvc;
using SchoolManagement.Models.ViewModels;
using SchoolManagement.Model.Entity;

namespace SchoolManagement.Controllers
{
    public class ScheduleController : Controller
    {
        resourceModel scheduleObj = new resourceModel();
        // GET: WorkSchedule
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetworkSchedules()
        {
            return new JsonResult { Data = scheduleObj.GetworkSchedules(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetResources()
        {

            return new JsonResult { Data = scheduleObj.GetResources(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetClassRooms()
        {

            return new JsonResult { Data = scheduleObj.GetClassroms(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetThemeColors()
        {
            return new JsonResult { Data = scheduleObj.GetThemeColors(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult checkWorkSchedule(WorkSchedule wrks)
        {
            return new JsonResult { Data = scheduleObj.checkWorkSchedule(wrks), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult SaveWorkSchedule(WorkSchedule wrks)
        {
            return new JsonResult { Data = new { status = scheduleObj.saveWork(wrks) } };
        }
    }
}