using Deposita.Domain.Enums;
using Deposita.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deposita.Domain.Entities
{
    public class Transaction
    {
        public int TransactionId { get; private set; }
        public decimal Amount { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public TransactionType TransactionType { get; private set; }
        public DateTime DateCreated { get; private set; }

        // Required by EF Core to create instances when reading from the database
        private Transaction() { }

        public static Transaction Create(decimal amount, string description, TransactionType type)
        {
            var transaction = new Transaction();
            transaction.SetDetails(amount, description, type);
            return transaction;
        }

        public void Update(decimal amount, string description, TransactionType type)
            => SetDetails(amount, description, type);

        private void SetDetails(decimal amount, string description, TransactionType type)
        {
            if (amount <= 0)
                throw new DomainException("Amount must be greater than zero.");

            if (string.IsNullOrWhiteSpace(description))
                throw new DomainException("Description is required.");

            if (description.Length > 255)
                throw new DomainException("Description cannot exceed 255 characters.");

            if (!Enum.IsDefined(type))
                throw new DomainException($"Invalid transaction type '{type}'.");

            Amount = amount;
            Description = description.Trim();
            TransactionType = type;
        }
    }
}
