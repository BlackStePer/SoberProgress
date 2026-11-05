namespace DIContainer
{
    /// <summary>
    /// Атрибут для внедрения зависимостей
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property)]
    public class InjectAttribute : Attribute
    {
    }
}
