# Problem details model

Mayhap provides a [RFC7807](https://tools.ietf.org/html/rfc7807) `Problem` struct to represent error information.
`Maybe<TValue>` structure uses `IOption<Problem>` instance to indicate failed operation and to provide more details about the error.

## `Problem` structure construction

The `Problem` structure may be created using its constructor or using `ProblemBuilder` instance.
To simplify problem instance creation Mayhap comes with a set of custom attributes, to decorate
enum values.

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