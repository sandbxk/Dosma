using Backend.Helpers;

namespace Backend.Tests;

public class InfrastructureTests
{
    public class ReflectionTestClass
    {
        private static int _idCounter = 0;

        public readonly int id;

        public string TestProperty { get; set; }

        public ReflectionTestClass(string testProperty)
        {
            TestProperty = testProperty;
            id = _idCounter++;
        }
    }

    [Fact]
    public void ReflectionTestSingleTrue()
    {
        var test = new ReflectionTestClass("value1");

        Assert.True(ReflectionHelpers.FieldEquals(test, nameof(test.TestProperty), "value1"));
    }
    
    [Fact]
    public void ReflectionTestSingleFalse()
    {
        var test = new ReflectionTestClass("value1");

        Assert.False(ReflectionHelpers.FieldEquals(test, nameof(test.TestProperty), "value2"));
    }

    [Fact]
    public void ReflectionTestListed()
    {
        List<ReflectionTestClass> testList = new List<ReflectionTestClass>() {
            new ReflectionTestClass("value1"),
            new ReflectionTestClass("value2"),
            new ReflectionTestClass("value3"),
            new ReflectionTestClass("value2"),
        };
        
        testList.Where(t => ReflectionHelpers.FieldEquals(t, nameof(t.TestProperty), "value2")).ToList().ForEach(t => Assert.Equal("value2", t.TestProperty));
    }
}