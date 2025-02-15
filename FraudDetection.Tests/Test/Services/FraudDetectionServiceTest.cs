using FraudDetection.Models;
using FraudDetection.Services;

namespace FraudDetection.Tests
{
    
    public class FraudDetectionServiceTests
    {
        private FraudDetectionService _fraudDetectionService;

        
        public void Setup()
        {
            // Inicializa o serviço antes de cada teste
            _fraudDetectionService = new FraudDetectionService();
        }

        [Fact]
        public void CheckTransactionAmount_TransactionAmountAboveLimit_ReturnsTrue()
        {
            // Arrange
            var transaction = new TransactionDTO
            {
                Amount = 15000.00
            };

            // Act
            var result = _fraudDetectionService.CheckTransactionAmount(transaction);

            // Assert
            Assert.True(result, "O valor da transação deveria ser considerado como fraudulento.");
        }

        [Fact]
        public void CheckTransactionAmount_TransactionAmountBelowLimit_ReturnsFalse()
        {
            // Arrange
            var transaction = new TransactionDTO
            {
                Amount = 5000.00
            };

            // Act
            var result = _fraudDetectionService.CheckTransactionAmount(transaction);

            // Assert
            Assert.False(result, "O valor da transação deveria ser considerado válido.");
        }

        [Fact]
        public void IsFraudulentTransaction_AmountAbove100000_ReturnsFraudMessage()
        {
            // Arrange
            var transaction = new TransactionDTO
            {
                Amount = 150000.00
            };

            // Act
            var result = _fraudDetectionService.IsFraudulentTransaction(transaction);

            // Assert
            Assert.Equal("Transação com valor acima do padrão, risco de fraude.", result);
        }

        [Fact]
        public void IsFraudulentTransaction_AccountNumberStartsWith123_ReturnsFraudMessage()
        {
            // Arrange
            var transaction = new TransactionDTO
            {
                AccountNumber = "123456789",
                Amount = 1000.00
            };

            // Act
            var result = _fraudDetectionService.IsFraudulentTransaction(transaction);

            // Assert
            Assert.Equal("Conta suspeita, risco de fraude.", result);
        }

        [Fact]
        public void IsFraudulentTransaction_ValidTransaction_ReturnsValidMessage()
        {
            // Arrange
            var transaction = new TransactionDTO
            {
                Amount = 1000.00,
                AccountNumber = "987654321"
            };

            // Act
            var result = _fraudDetectionService.IsFraudulentTransaction(transaction);

            // Assert
            Assert.Equal("Transação válida.", result);
        }
    }
}
