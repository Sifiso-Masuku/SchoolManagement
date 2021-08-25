using IdentitySample.Models;
using SchoolManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Models
{
    public class BILogic
    {
        private readonly static ApplicationDbContext db = new ApplicationDbContext();

        public static decimal GetRegistrationFee(int? residenceId)
        {
            var fee = db.Residences.Find(residenceId);
            return fee.RegistrationFee;
        }
        public static void ApplicationStatus(int? id, string statuse)
        {
            var dbRecord = db.ResidenceApplications.Find(id);
            dbRecord.Status = statuse;
            db.Entry(dbRecord).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            EmailSender.ResidenceApplication(dbRecord);
        }

        public static bool ChechRoomNumber(string roomNumber)
        {
            var rooms = db.Rooms.ToList();
            foreach (var item in rooms)
            {
                return roomNumber != item.RoomNumber;
            }
            return false;
        }

        public static bool CheckDOB(DateTime DOB)
        {
            var years = DateTime.Now.Year - DOB.Year;
            return years > 10;
        }
    }
}