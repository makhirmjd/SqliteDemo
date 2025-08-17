using Microsoft.EntityFrameworkCore;

namespace SqliteDemo.Shared.Models.Annotations;

[AttributeUsage(AttributeTargets.Property)]
public class DeleteActionAttribute(DeleteBehavior deleteBehavior) : Attribute
{
    public DeleteBehavior DeleteBehavior => deleteBehavior;
}
