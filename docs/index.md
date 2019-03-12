# Mayhap

---
Mayhap is a tiny C# library inspired by Scott Wlaschins [Railway Oriented Programming](https://fsharpforfunandprofit.com/rop/).
The main motivation behind the Mayhap project is to provide a set of structures and functions,
to simplify processing of `optional` values. Mayhap usage helps to eliminate null-checking.
`Maybe<T>` and `IOption<T>` types semantically suggests, that you should check if wrapped value really exists.

Examples:
```csharp
public Maybe<CustomerDto> Deposit(Guid id, decimal amount)
{
    return _repository.Find(id.ToString("n"))
        .Map(customerDto => _converter.ToEntity(customerDto), out var customer)
        .Map(c => c.Deposit(amount))
        .Map(_ => _converter.ToDto(customer))
        .Map(customerDto => _repository.Update(customerDto));
}
```

```csharp
IOption<int> x = "1".Some().Map(s => Convert.ToInt32(s).Some());
// x == Some<int>{ Value = 1 }

IOption<int> y = Optional.None<string>().Map(s => Convert.ToInt32(s).Some());
// y == None<int>
```