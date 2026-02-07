using ExampleProject.Core.Entities;
using Xunit;

namespace ExampleProject.Core.Tests.Entities;

public class BaseEntityTests
{
    [Fact]
    public void Id_Is_Guid_Type()
    {
        var entity = new ConcreteEntity();
        Assert.Equal(default(Guid), entity.Id);
    }

    [Fact]
    public void Id_Can_Be_Set()
    {
        var id = Guid.NewGuid();
        var entity = new ConcreteEntity { Id = id };
        Assert.Equal(id, entity.Id);
    }

    private sealed class ConcreteEntity : BaseEntity { }
}
