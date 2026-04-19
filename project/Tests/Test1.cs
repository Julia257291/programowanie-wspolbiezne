using Data;
using Logic;

[TestClass]
public class LogicApiTests
{
    [TestMethod]
    public void GenerateBalls_AmountOfBalls()
    {
        LogicAbstractApi logic = LogicAbstractApi.CreateApi();
        int expectedNum = 5;

        logic.GenerateBalls(expectedNum, 100, 100);

        List<Ball> result = logic.GetBalls();

        Assert.IsNotNull(result);
        Assert.AreEqual(expectedNum, result.Count); 
    }

    [TestMethod]
    public void GenerateBalls_CorrectRadius()
    {
        var logic = LogicAbstractApi.CreateApi();
        logic.GenerateBalls(1, 100, 100);
        var result = logic.GetBalls();

        Assert.IsTrue(result.Count > 0);
        Assert.AreEqual(10, result[0].Radius);
    }

    [TestMethod]
    public void GenerateBalls_BallsWithinBounds()
    {
        var logic = LogicAbstractApi.CreateApi();
        double maxX = 100;
        double maxY = 100;
        logic.GenerateBalls(10, maxX, maxY);
        var balls = logic.GetBalls();

        foreach (var ball in balls)
        {
            Assert.IsTrue(ball.X >= 0 && ball.X <= maxX - ball.Radius, "Ball X out of bounds");
            Assert.IsTrue(ball.Y >= 0 && ball.Y <= maxY - ball.Radius, "Ball Y out of bounds");
        }
    }
}