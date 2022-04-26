using System.Text.Json;

namespace WeatherForecast
{
    public class Employee
    {
        //Id of the employee.
        public int Id { get; set; }

        //Name of the employee.
        public string? Name { get; set; }

        //Age of the employee.
        public int Age { get; set; }

        //Number of times this has been choosen.
        public int NumOfTimes { get; set; }

        public int IsAvailable { get; set; }

        public static List<Employee> GetEmployees()
        {

            List<Employee> employees = new List<Employee>();

            using (StreamReader r = new StreamReader("employee - kopia.json"))
            {
                string jsonString = r.ReadToEnd();
                employees = JsonSerializer.Deserialize<List<Employee>>(jsonString);
            }

            return employees;
        }
    }
}
