# Mayhap
Mayhap is a tiny C# library inspired by Scott Wlaschins [Railway Oriented Programming](https://fsharpforfunandprofit.com/rop/). Its goal is to simplify typical success/fail logic flows.

## How to use it?
Make sure that all parts of your code, that can either return a successful or faulty result, will return Maybe<TValue> struct. "Maybe" operations are able to be chained using "Continue" extension method.
Take a look at code samples [here](https://github.com/pmartynski/mayhap/blob/master/samples/Maybe.Samples/CustomerService.cs).
