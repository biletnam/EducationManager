﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EducationManager.Security;
using EducationManager.ViewModels.Admin;

namespace EducationManager.Controllers.Admin
{
    [CustomAuthorize(Roles = "admin")]
    public class AdminGeneralController : Controller
    {
        Models.DataModel.DataStorage data_storage = new Models.DataModel.DataStorage();
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.teachers = data_storage.Teachers.Where(s => s.SchoolId.Equals(UserSession.Uinform.Admin.SchoolId)).ToList();
            ViewBag.students = data_storage.Students.Where(s => s.SchoolId.Equals(UserSession.Uinform.Admin.SchoolId)).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult DeleteUser(DeleteUserViewModel dvm)
        {
            if (!ModelState.IsValid)
                return Content("Неверные данные");
            Models.OperationRegistryData.OperationRegistryStorage registry_storage = new Models.OperationRegistryData.OperationRegistryStorage();
            Models.DataModel.Teacher teacher = new Models.DataModel.Teacher();
            Models.DataModel.Student student = new Models.DataModel.Student();

            if (dvm.Role == "teacher")
            {
                var t = data_storage.Teachers.Where(a => a.TeacherId.Equals(dvm._ID));

                if (t.Count() == 0)
                    return Content("Такого пользователя не существует");
                teacher = t.First();
                data_storage.Entry(teacher).State = System.Data.Entity.EntityState.Deleted;
                data_storage.SaveChanges();
                registry_storage.Users.Add(new Models.OperationRegistryData.OperationRegistryUser()
                {
                    Addres = data_storage.Addresses.Where(a => a.AddresId.Equals(teacher.AddresId)).First().AddresValue,
                    UserId = teacher.UserId,
                    DateOfBirth = teacher.DateOfBirth,
                    ClassId = -1,
                    FirstName = teacher.FirstName,
                    MiddleName = teacher.MiddleName,
                    LastName = teacher.LastName,
                    Gender = teacher.Gender,
                    Role = "teacher",
                    SchoolId = teacher.SchoolId
                });
                registry_storage.SaveChangesAsync();
            }
            else if (dvm.Role == "student")
            {
                var s = data_storage.Students.Where(a => a.StudentId.Equals(dvm._ID));

                if (s.Count() == 0)
                    return Content("Такого пользователя не существует");
                student = s.First();
                data_storage.Entry(student).State = System.Data.Entity.EntityState.Deleted;
                data_storage.SaveChanges();
                registry_storage.Users.Add(new Models.OperationRegistryData.OperationRegistryUser()
                {
                    Addres = data_storage.Addresses.Where(a => a.AddresId.Equals(student.AddresId)).First().AddresValue,
                    UserId = student.UserId,
                    DateOfBirth = student.DateOfBirth,
                    ClassId = -1,
                    FirstName = student.FirstName,
                    MiddleName = student.MiddleName,
                    LastName = student.LastName,
                    Gender = student.Gender,
                    Role = "student",
                    SchoolId = student.SchoolId
                });
                registry_storage.SaveChangesAsync();
            }
            else
            {
                return Content("Действие не выполнено");
            }
            return Content("Действие выполнено успешно");
        }

    }
}
