using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoWebApi.Models;
using Microsoft.EntityFrameworkCore;
using DemoWebApi.ViewModel;

namespace DemoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeptController : ControllerBase
    {
        _1026dbContext db = new _1026dbContext();
        [HttpGet]
        [Route("ListDept")]
        public IActionResult GetDept()
        {
            //var data = from dept in db.Depts select dept;
            var data = from dept in db.Depts select new 
            { 
                Id=dept.Id, 
                Name=dept.Name, 
                Location=dept.Location,
            };
            return Ok(data);
        }

        [HttpGet]
        [Route("ListDept/{id}")]
        public IActionResult GetDept(int? id)
        {
            if(id == null)
            {
                return BadRequest("Id Cannot be NULL!");
            }
            var data = (from dept in db.Depts where dept.Id==id select new
            {
                Id = dept.Id,
                Name = dept.Name,
                Location = dept.Location,
            }).FirstOrDefault();

            // ALTERNATE SYNTAX
            //var data = db.Depts.Where(d => d.Id == id).Select(d => new
            //{
            //    Id = dept.Id,
            //    Name = dept.Name,
            //    Location = dept.Location,
            //}).FirstOrDefault();

            if(data == null)
            {
                return NotFound($"Department {id} not present!");
            }
            return Ok(data);
        }

        [HttpGet]
        [Route("ListCity")]
        public IActionResult GetCity([FromQuery] string city)
        {
            var data = db.Depts.Where(d => d.Location == city);
            return Ok(data);
        }
        // ..../ListCity?city=chennai

        [HttpPost]
        [Route("AddDept")]
        public IActionResult PostDept(Dept dept)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //db.Depts.Add(dept);
                    //db.SaveChanges();
                    db.Database.ExecuteSqlInterpolated($"adddept {dept.Id},{dept.Name},{dept.Location}");
                }
                catch (Exception)
                {
                    return BadRequest("Something went wrong while saving record!");
                }
            }
            return Created("Record successfully added!", dept);
        }

        [HttpPut]
        [Route("EditDept/{id}")]
        public IActionResult PutDept(int id, Dept dept)
        {
            if (ModelState.IsValid)
            {
                Dept odept = db.Depts.Find(id);
                odept.Name = dept.Name;
                odept.Location = dept.Location;
                db.SaveChanges();
                return Ok("Record Updated Sucessfully");
            }
            return BadRequest("Something went wrong while updating record");
        }

        [HttpDelete]
        [Route("DeleteDept/{id}")]
        public IActionResult DeleteDept(int id)
        {
            var data = db.Depts.Find(id);
            db.Depts.Remove(data);
            db.SaveChanges();
            return Ok("Record Deleted Successfully!");
        }


        // ViewModel implementation
        [HttpGet]
        [Route("ShowEmp")]
        public IActionResult GetEmp()
        {
            var data = db.EmpDepts.FromSqlInterpolated<EmpDept>($"ShowEmp");
            return Ok(data);
        }

    }
}
