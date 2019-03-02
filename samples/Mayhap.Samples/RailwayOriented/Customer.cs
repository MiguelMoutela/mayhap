using System;
using Mayhap.Maybe;

namespace Mayhap.Samples.RailwayOriented
{
    public class Customer
    {
        private Customer(Guid id, string name, decimal accountBalance = 0.0m)
        {
            Id = id;
            Name = name;
            AccountBalance = accountBalance;
        }

        public Guid Id { get; }

        public string Name { get; }

        public decimal AccountBalance { get; private set; }

        public static Maybe<Customer> Create(string name) 
            => IsNameValid(name) 
                ? new Customer(Guid.NewGuid(), name).Success() 
                : $"EMPTY.{nameof(name)}".Fail<Customer>();

        internal static Maybe<Customer> Load(Guid id, string name, decimal accountBalance)
        {
            if (!IsNameValid(name))
            {
                return $"EMPTY.{nameof(name)}".Fail<Customer>();
            }

            if (accountBalance < 0.0m)
            {
                return $"TOO_LOW.{nameof(accountBalance)}".Fail<Customer>();
            }

            return new Customer(id, name, accountBalance).Success();
        }

        private static bool IsNameValid(string name)
            => !string.IsNullOrWhiteSpace(name);

        public Maybe<decimal> Deposit(decimal amount)
        {
            if (amount > 0)
            {
                AccountBalance += amount;
                return AccountBalance.Success();
            }

            return $"TOO_LOW.{nameof(amount)}".Fail<decimal>();
        }

        public Maybe<decimal> Withdraw(decimal amount)
        {
            if (AccountBalance >= amount)
            {
                AccountBalance -= amount;
                return AccountBalance.Success();
            }

            return $"TOO_HIGH.{nameof(amount)}".Fail<decimal>();
        }
    }
}