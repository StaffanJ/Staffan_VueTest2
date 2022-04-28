using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Abstractions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WeatherForecast.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        List<Employee> employees = new List<Employee>();

        // GET: api/<EmployeeController>
        [HttpGet]
        public IEnumerable<Employee> Get()
        {

            employees = Employee.GetEmployees();

            return employees.ToArray();
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<EmployeeController>
        [HttpPost]
        public Employee[] Post([FromBody] Employee value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return Enumerable.Range(1, 1).Select(index => new Employee
            {
                Name = value.Name
            })
            .ToArray();
        }

        // Roll api/<EmployeeController>
        // Rolls for what employee we are going to randomize.
        [HttpGet("Roll")]
        public ActionResult<Employee> Roll()
        {
            Employee ChoosenOne = new Employee();
            employees = Employee.GetEmployees();
            int MinNumOfTimes = 0;
            //int MaxNumOfTimes = 0;

            foreach (var employee in employees)
            {
                //MinNumOfTimes = employee.NumOfTimes;
                if (MinNumOfTimes == 0)
                {
                    MinNumOfTimes = employee.NumOfTimes;
                }

                if (employee.NumOfTimes < MinNumOfTimes)
                {
                    MinNumOfTimes = employee.NumOfTimes;
                }
            }

            employees.RemoveAll(x => x.NumOfTimes > MinNumOfTimes);

            Random rnd = new Random();

            int EmployeeId = employees.OrderBy(x => rnd.Next()).First().Id;

            ChoosenOne = employees.Find(x => x.Id == EmployeeId);

            ChoosenOne.NumOfTimes++;

            employees.Clear();
            employees = Employee.GetEmployees();

            employees.Find(x => x.Id == EmployeeId).NumOfTimes = ChoosenOne.NumOfTimes;

            using (StreamWriter r = new StreamWriter("employee - kopia.json"))
            {
                r.Write(JsonSerializer.Serialize(employees, new JsonSerializerOptions { WriteIndented = true }));
            }

            return ChoosenOne;
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // PUT api/<EmployeeController>/5
        [HttpPost("ChangeStatus")]
        public ActionResult ChangeStatus([FromBody] Employee ChangeEmployee)
        {
            Employee ChangeStatusEmployee = new Employee();
            employees = Employee.GetEmployees();

            if (ChangeEmployee.IsAvailable == 1)
            {
                employees.Find(x => x.Id == ChangeEmployee.Id).IsAvailable = 0;
            }
            else
            {
                employees.Find(x => x.Id == ChangeEmployee.Id).IsAvailable = 1;
            }

            using (StreamWriter r = new StreamWriter("employee - kopia.json"))
            {
                r.Write(JsonSerializer.Serialize(employees, new JsonSerializerOptions { WriteIndented = true }));
            }

            employees.Clear();

            return new JsonResult(ChangeEmployee);
            //return employee;
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

