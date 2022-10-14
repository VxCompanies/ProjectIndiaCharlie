using ProjectIndiaCharlie.Desktop.ViewModels;

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


    }
}