namespace Tests;

public class Miscellaneous
{

    // used to verify that the test runner is setup correctly, change the expected value to fail the test.
    [Fact]
    public void AlwaysTrue()
    {
        Assert.True(true);
    }
}