# Mayhap [![Build status](https://ci.appveyor.com/api/projects/status/7dd0enuihjr8dwgj?svg=true)](https://ci.appveyor.com/project/pmartynski/mayhap) [![Nuget package](https://img.shields.io/nuget/v/mayhap.svg)](https://www.nuget.org/packages/Mayhap)

Mayhap is a tiny C# library inspired by Scott Wlaschins [Railway Oriented Programming](https://fsharpforfunandprofit.com/rop/). Its goal is to simplify the typical success/fail logic flows.

## How to use it?
Make sure that all parts of your code, that can either return a successful or faulty result, will return Maybe<TValue> struct. 
"Maybe" operations are able to be chained using "Continue" extension method.
Take a look at code samples [here](https://github.com/pmartynski/mayhap/blob/master/samples/Mayhap.Samples/RailwayOriented/CustomerService.cs).
    
`Maybe<T>` struct gives you also implicit conversions to `bool` and `T`. In result, `Maybe<T>` can be used directly as an `if` statement condition, or `T` typed argument.

## The problem details model

Mayhap comes with a [RFC7807](https://tools.ietf.org/html/rfc7807) `Problem` struct, which is the default error model. 
You may create your own error model by implementing `IProblem` interface.

Along with the `Problem` struct, Mayhap delivers also a few `Problem` construction patters. You may either create it from a public constructor, providing its properties explicitly, or use `string` or `System.Enum` extension methods.
A `Problem` instance created using `System.Enum` extension can be customized via attributes. Either way, final shape of `Problem` instance can be achieved using `ProblemBuilder` object.

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

## Why to use Mayhap?
Writing a chunk of business logic code, especially when dealing with remote resources, involves a lot of success checking on each step.
The goal of this library is to make those parts of code more readable and concise.

Lets consider the following example:

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

The flow contains a null check after each operation that could fail. Lets ommit the fact that treating null as a special business value is an abuse.

The example below shows how the same operation could be written using Mayhap, including error message passing:

```csharp
public Maybe<CustomerDto> Deposit(Guid id, decimal amount)
{
    return _repository.Find(id.ToString("n"))
        .Continue(customerDto => _converter.ToEntity(customerDto), out var customer)
        .Continue(c => c.Deposit(amount))
        .Continue(_ => _converter.ToDto(customer))
        .Continue(customerDto => _repository.Update(customerDto));
}
```