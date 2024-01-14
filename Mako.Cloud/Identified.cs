namespace Mako.Cloud;

public interface Identified<T>
{
    public string Id { get; set; }
    public T Content { get; set; }
}
