using Logic;

namespace Tests
{
    [TestClass]
    public class LogicApiTests
    {
        [TestMethod]
        public void CreateBalls_AmountOfBalls() //Sprawdzamy czy poprawnie generowana jest liczba kul
        {
            var logic = new LogicApi();
            int expectedNum = 5;
            var result = logic.CreateBalls(expectedNum);
            Assert.IsNotNull(result); // czy lista nie jest pusta
            Assert.AreEqual(expectedNum, result.Count); //czy stworzyło 5 kul
        }
        [TestMethod]
        public void CreateBalls_CorrectRadius() //Sprawdzamy czy promień kul jest poprawny
        {
            var logic = new LogicApi();
            var result = logic.CreateBalls(1);
            Assert.IsTrue(result.Count > 0);
            Assert.AreEqual(10, result[0].Radius); // Sprawdzamy promień pierwszej kuli na liście
        }
    }
}
