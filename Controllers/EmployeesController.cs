using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;


using EmployeeDataAccess;
namespace EmployeeService.Controllers
{
    public class EmployeesController : ApiController
    {


        [HttpGet]
        public HttpResponseMessage EmployeeDetails(string gender = "All")
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {

                switch (gender.ToLower())
                {
                    case "all":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.ToList());

                    case "male":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(e => e.Gender.ToLower() == "male").ToList());

                    case "female":

                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(e => e.Gender.ToLower() == "female").ToList());

                    default:
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Value for gender given " + gender + " is invalid");
                }
                
            }
        }

        public Employee Get(int id)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                return entities.Employees.FirstOrDefault(e => e.ID == id);
            }
        }

        public void Post([FromBody] Employee employee)
        {

            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                entities.Employees.Add(employee);
                entities.SaveChanges();
            }

        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {

                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {


                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee With the id" + id.ToString() + "not found to delete");

                    }
                    else
                    {

                        entities.Employees.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);


                    }

                }


            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }
        }


        public void Put(int id,  [FromBody]Employee employee )
        {
            using(EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                var entity = entities.Employees.FirstOrDefault(e => e.ID == id);

                entity.FirstName = employee.FirstName;
                entity.LastName = employee.LastName;
                entity.Gender = employee.Gender;
                entity.Salary = employee.Salary;

                entities.SaveChanges();


            }
        }
    }
}
