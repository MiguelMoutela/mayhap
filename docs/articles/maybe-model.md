# Maybe model

The responsibility of the maybe model is to represent an operation result. To achieve that, `Maybe<T>`
structure composes features of the other two models: [problem model](problem-model.html) to represent
error information, and [option model](option-model.html) to represent value in case of success. To provide a fluent experience, `Maybe<T>` structure comes with a set of methods and extension methods.
The structure is also immutable.

## `Maybe<T>` structure construction
To create a `Maybe<T>` instance use `Success()`/`Fail()`extension methods. The `Success()` method,
can be called from any type, while `Fail()` extends only `IProblem`, `string`, and `System.Enum` values.

```csharp
Maybe<string> success = "some string".Success();
Maybe<int> fail = "some error".Fail<int>();
```

## Implicit casting
The `Maybe<T>` structure can be cast to `bool` and `T` types. Thanks to this feature, the `Maybe<T>`
value can be used directly as a method parameter or a condition in an `if` statement.

```csharp
bool isSuccess = "some value".Success(); // true
string val = "some value".Success(); // val == "some value"

// but be careful...
object obj = "some value".Success<object>(); // obj is Maybe<string> { Value = "some value" }
```

## Chaining
The `Maybe<T>` struct is most useful when dealing with more than one operation that may fail.
Mayhap delivers a bunch of `Map()` methods which enable `Maybe<T>` chaining.

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

The example above is equivalent to:

```csharp
public Maybe<CustomerDto> Deposit(Guid id, decimal amount)
{
    var customerDto = _repository.Find(id.ToString("n"));

    var customer = customer
        ? _converter.ToEntity(customerDto)
        : customer.Error.Fail<Customer>();

    customer = customer.Deposit();

    customerDto = customer
        ? _converter.ToDto(customer)
        : customer.Error.Fail<CustomerDto>();

    return customerDto
        ? _repository.Update(customerDto)
        : customerDto;
}
```