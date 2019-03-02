using System;
using Mayhap.Maybe;

namespace Mayhap.Samples.Traditional
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

        public static Customer Create(string name)
            => IsNameValid(name)
                ? new Customer(Guid.NewGuid(), name)
                : null;

        internal static Customer Load(Guid id, string name, decimal accountBalance)
        {
            if (!IsNameValid(name))
            {
                return null;
            }

            if (accountBalance < 0.0m)
            {
                return null;
            }

            return new Customer(id, name, accountBalance);
        }

        private static bool IsNameValid(string name)
            => !string.IsNullOrWhiteSpace(name);

        public decimal Deposit(decimal amount)
        {
            if (amount > 0)
            {
                AccountBalance += amount;
                return AccountBalance.Success();
            }

            return 0;
        }

        public decimal Withdraw(decimal amount)
        {
            if (AccountBalance >= amount)
            {
                AccountBalance -= amount;
                return AccountBalance.Success();
            }

            return 0;
        }
    }
}