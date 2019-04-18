public static class GenericVoidDelegate
{
    public delegate void ParamlessDelegate();
    public delegate void RefDelegate<V>(ref V eventParameter);
    
    public delegate void Delegate<V>(V eventParameter);
    public delegate void ParamsDelegate<V>(params V[] eventParameters);
}

public static class GenericDelegate<T>
{
    public delegate T ParamlessDelegate();
    public delegate T RefDelegate<V>(ref V eventParameter);

    public delegate T Delegate<V>(V eventParameter);
    public delegate T ParamsDelegate<V>(params V[] eventParameters);
}

public abstract class EventParameter
{
    public abstract object Value { get; set; }

    public T ConvertedValue<T>()
    {
        return (T) Value;
    }
}

public class GenericEventParameter<T>: EventParameter
{
    private T value;

    public GenericEventParameter(T newValue)
    {
        value = newValue;
    }
    
    public override object Value
    {
        get { return value; }
        set { value = (T) value; }
    }
}