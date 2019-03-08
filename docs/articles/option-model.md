# The option model

The option model represents a value which may, or may not exist. It's a C# adaptation of common 
functional pattern. The base of the option model is build of three types: `IOption<T>`, `Some<T>` 
and `None<T>`. `Some<T>` and `None<T>` structs are implementing `IOption<T>` interface.

## `IOption<T>` construction
A `Some<T>` instance can be constructed using `Some()` extension method called from any type, while
`None<T>` can be instantiated using a static factory method `Optional.None<T>()`.

```csharp
IOption<string> some = "some value".Some();
IOption<string> none = Optional.None<string>();
```

## Unwrapping value
`Some<T>` and `None<T>` instances can be directly cast to `T` type. `Some<T>` will return a wrapped
value of `T`, while `None<T>` will always return `default(T)`. This feature allows you to use both of
those structures as a method parameter directly. In the case of `IOption<T>` interface instance,
its `Unwrap()` method can be used.

```csharp
Some<string> some = "some value".Some();
string value = some;

None<string> none = Optional.None<string>();
string willBeNull = none;

IOption<string> option = some;
string unwrapped = some.Unwrap();
```

## Chaining
The `IOption<T>` type is extended by a few `Map()` extension methods, which can help you in situations,
when one optional value is a result of an operation on another one.

```csharp
IOption<int> someInt = "100".Some().Map(s => Convert.ToInt32(s).Some());
```

The example above is equivalent to:

```csharp
IOption<string> someString = "100".Some();

IOption<T> optionalInt1;
switch(someString)
{
    case Some<string> s:
        optionalInt1 = Convert.ToInt32(s).Some();
        break;
    default:
        optionalInt1 = Optional.None<int>();
}
```