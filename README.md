# Mayhap [![Build status](https://ci.appveyor.com/api/projects/status/7dd0enuihjr8dwgj?svg=true)](https://ci.appveyor.com/project/pmartynski/mayhap) [![Nuget package](https://img.shields.io/nuget/v/mayhap.svg)](https://www.nuget.org/packages/Mayhap)

Mayhap is a tiny C# library inspired by Scott Wlaschins [Railway Oriented Programming](https://fsharpforfunandprofit.com/rop/).
The main motivation behind the Mayhap project is to provide a set of structures and functions,
to simplify processing of `optional` values. Mayhap usage helps to eliminate null-checking.
`Maybe<T>` and `IOption<T>` types semantically suggests, that you should check if wrapped value
really exists.

Full documentation can be found [here](https://pmartynski.github.io/mayhap/).

## Why to use Mayhap?
Writing a block of business logic code, especially when dealing with remote resources, involves
a lot of success checking on each step.
The goal of this library is to make those parts of code more readable and concise.

Let's consider the following example:

```csharp
public CustomerDto Deposit(Guid id, decimal amount)
{
    var customerDto = _repository.Find(id.ToString("N"));

    if (customerDto == null)
    {
        return null;
    }

    var customer = _converter.ToEntity(customerDto);
    if (customer == null)
    {
        return null;
    }

    var deposit = customer.Deposit(amount);
    if (deposit == 0)
    {
        return null;
    }

    customerDto = _converter.ToDto(customer);
    customerDto = _repository.Update(customerDto);
    return customerDto;
}
```

The flow contains a null check after each operation that could fail. Null was used only for
the sake of simplicity. In a real-world solution, some kind of result type should be created.

The example below shows how the same operation could be written using Mayhap, including error
message passing:

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

Each dependency method, which may fail, returns `Maybe<T>`, where `T` is the type of wrapped value.
In the happy path scenario, all consequent map functors will be applied, to produce the final result -
`CustomerDto` wrapped in a maybe struct. On the other hand, when the chain will fail on any step,
the failure will be passed all the way down, omitting further map functors, to finally return
`Maybe<CustomerDto>` which will contain error information.

The full example code may be found [here](https://github.com/pmartynski/mayhap/blob/master/samples/Mayhap.Samples/RailwayOriented/CustomerService.cs).

## Mayhap parts
Mayhap provides three models: **option** model, **problem details** model and **maybe** model.
The maybe model utilizes the other two models.

### Option model
The option model is a representation of a common functional option pattern. The `IOption<T>`
may be `Some<T>`, or `None<T>`. `IOption<T>` may be chained using  `Map` extension method.

Example:
```csharp
IOption<int> x = "1".Some().Map(Convert.ToInt32);
// x == Some<int>{ Value = 1 }

IOption<int> y = Optional.None<string>().Map(Convert.ToInt32);
// y == None<int>
```

### The problem details model
Mayhap comes with a [RFC7807](https://tools.ietf.org/html/rfc7807) `Problem` struct. 
It is used by maybe model to provide an error representation. 
Along with the `Problem` struct, Mayhap delivers also a few `Problem` construction and customization facilities.

Example:
```csharp
public enum ProblemType
{
    [ProblemType("VALIDATION_ERROR")]
    [ProblemTitle("The data input is invalid")]
    [ProblemStatus(400)]
    ValidationError
}

public Maybe<AccountBalance> Deposit(long accountId, decimal amount)
{
    if(amount <= 0)
    {
        return Problem.OfType(ProblemType.ValidationError)
            .WithDetail("The amount value is too low.")
            .WithInstance("/account/3215")
            .WithProperty(
                "correlationId", 
                "a910ff48df1b41fb884cf05e1e68741d")
            .Create()
            .Fail<AccountBalance>()
    }

    Maybe<Account> account = _accountRepository.Find(accountId);
    if(!account)
    {
        return Problem.New()
            .WithType("ACCOUNT_NOT_FOUND")
            .Create()
            .Fail<AccountBalance>();
    }

    // ...
}
```

### The maybe model
Mayhap comes with `Maybe<T>` struct. Its main purpose is to provide fault-aware operation chains.
To use it, make sure that all parts of your code, that can either return a successful or faulty
result, will return `Maybe<T>` struct.

`Maybe<T>` is chainable through `Map` extension method.
Take a look at code samples [here](https://github.com/pmartynski/mayhap/blob/master/samples/Mayhap.Samples/RailwayOriented/CustomerService.cs).

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
    
`Maybe<T>` struct gives you also implicit conversions to `bool` and `T`. In result, `Maybe<T>`
can be used directly as an `if` statement condition or a `T` typed argument.
