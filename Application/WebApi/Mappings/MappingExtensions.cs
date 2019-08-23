using AutoMapper;

public static class MappingExtensions
{

    public static TResult Map<TSource, TResult>(this TSource source)
    {
        return new MapperConfiguration(c =>
        {
            c.CreateMap<TSource, TResult>();
        }).CreateMapper().Map<TSource, TResult>(source);
    }
}
