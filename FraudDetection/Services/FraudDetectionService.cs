using FraudDetection.Models;

namespace FraudDetection.Services
{
    public class FraudDetectionService
    {
        private const int MaxTransactionsPerMinute = 5;
        private const double MaxTransactionAmount = 10000.00;
        private readonly List<TransactionDTO> transactionHistory;

        public FraudDetectionService()
        {
            transactionHistory = new List<TransactionDTO>(); // Historico de transações
        }

        //Análise de Padrões de Transações (Frequência, Valor, Localização)
        public bool CheckTransactionFrequency(TransactionDTO transaction)
        {
            var recentTransactions = transactionHistory
                .Where(t => t.Id == transaction.Id && t.Timestamp > DateTime.Now.AddMinutes(-1))
                .ToList();

            return recentTransactions.Count >= MaxTransactionsPerMinute;
        }

        public bool CheckTransactionAmount(TransactionDTO transaction)
        {
            return transaction.Amount > MaxTransactionAmount;
        }

        public bool CheckTransactionLocation(TransactionDTO transaction)
        {
            var lastTransaction = transactionHistory
                .Where(t => t.Id == transaction.Id)
                .OrderByDescending(t => t.Timestamp)
                .FirstOrDefault();

            return lastTransaction != null && lastTransaction.Location != transaction.Location;
        }

        public string IsFraudulentTransaction(TransactionDTO transaction)
        {
            if(transaction.Amount > 100000)
            {
                //Detecta transações de valor alto como suspeitas
                return "Transação com valor acima do padrão, risco de fraude.";
            }

            if(transaction.AccountNumber.StartsWith("123"))
            {
                //Detecta transações de contas específicas como suspeitas
                return "Conta suspeita, risco de fraude.";
            }

            return "Transação válida.";
        }
    }

}
