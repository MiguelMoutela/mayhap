# Mayhap [![Build status](https://ci.appveyor.com/api/projects/status/7dd0enuihjr8dwgj?svg=true)](https://ci.appveyor.com/project/pmartynski/mayhap) [![Nuget package](https://img.shields.io/nuget/v/mayhap.svg)](https://www.nuget.org/packages/Mayhap)

Mayhap is a tiny C# library inspired by Scott Wlaschins [Railway Oriented Programming](https://fsharpforfunandprofit.com/rop/). Its goal is to simplify the typical success/fail logic flows.

## How to use it?
Make sure that all parts of your code, that can either return a successful or faulty result, will return Maybe<TValue> struct. 
"Maybe" operations are able to be chained using "Continue" extension method.
Take a look at code samples [here](https://github.com/pmartynski/mayhap/blob/master/samples/Mayhap.Samples/RailwayOriented/CustomerService.cs).

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
