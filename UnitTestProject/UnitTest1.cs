using ProjectIndiaCharlie.Desktop.ViewModels;
using ProjectIndiaCharlie.Desktop.Models;
namespace UnitTestProject
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void EsUsuarioValido()
        {
            LoginViewModel lvm = new LoginViewModel()
            { UserId = "1110408",
            Password = "qwerty123"};
            Assert.IsNotEmpty(lvm.UserId, "User id is empty");
            Assert.IsNotEmpty(lvm.Password, "Password is empty");

            Assert.Pass();
        }

        [Test]
        public void ValoresEmpty()
        {
            var DocNoTest = new List<object>() { "1", "2", "3", "4", null};
            var FistName = new List<object>() { "Steven", "Nikita", "Oliver", "Omar", null};
            var FirstSurnameList = new List<object>() { "A", "B", "C", null };
            var GenderTest = new List<object> { "Masculino", "Femenino", null };
            var EmailTest = new List<object>() { "nikita@gmail.com", null };
            var Career = new List<object>() { "Software", "Mercadeo", null };
        

            var vStudentTest = new VStudentDetail()
            {
                DocNo = (string)DocNoTest[new Random().Next(0,5)],
                FirstName = (string)FistName[new Random().Next(0, 5)],
                FirstSurname = (string)FirstSurnameList[new Random().Next(0, 4)],
                Gender = (string)GenderTest[new Random().Next(0, 3)],
                Email = (string)EmailTest[new Random().Next(0,2)],
                Career = (string)Career[new Random().Next(0, 3)],
            };

            
            Assert.IsNotNull(vStudentTest.DocNo);
            Assert.IsNotNull(vStudentTest.FirstName);
            Assert.IsNotNull(vStudentTest.FirstSurname);
            Assert.IsNotNull(vStudentTest.Gender);
            Assert.IsNotNull(vStudentTest.Email);
            Assert.IsNotNull(vStudentTest.Career);


        }

    }
}