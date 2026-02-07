using ExampleProject.Core.Exceptions;
using Xunit;

namespace ExampleProject.Core.Tests.Exceptions;

public class DomainExceptionTests
{
    [Fact]
    public void Message_Is_Preserved()
    {
        const string message = "Test domain error";
        var ex = new DomainException(message);
        Assert.Equal(message, ex.Message);
    }

    [Fact]
    public void Is_Exception()
    {
        var ex = new DomainException("x");
        Assert.IsAssignableFrom<Exception>(ex);
    }
}
