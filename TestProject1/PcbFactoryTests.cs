using NUnit.Framework;

using PcbDispatchService.Domain.Models;
using PcbDispatchService.Services;

namespace TestProject1
{
    [TestFixture]
    public class PcbFactoryTests
    {
        private PcbFactory _factory;

        [SetUp]
        public void Setup()
        {
            // Инициализация фабрики перед каждым тестом
            _factory = new PcbFactory();
        }

        [Test]
        public void CreateCircuitBoard_ReturnsBoard_WithCorrectName()
        {
            // Arrange
            var expectedName = "TestBoard";

            // Act
            var result = _factory.CreateCircuitBoard(expectedName);

            // Assert
            Assert.IsNotNull(result, "Созданная плата не должна быть null.");
            Assert.AreEqual(expectedName, result.Name, "Имя созданной платы должно совпадать с переданным.");
        }

        [Test]
        public void CreateCircuitBoard_ThrowsException_WhenNameIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _factory.CreateCircuitBoard(null), "Ожидается исключение при передаче null.");
        }

        [Test]
        public void CreateCircuitBoard_ThrowsException_WhenNameIsEmpty()
        {
            // Act & Assert
            Assert.Throws<PcbDispatchService.Domain.Logic.BusinessException>(() => _factory.CreateCircuitBoard(string.Empty), "Ожидается исключение при передаче пустой строки.");
        }
    }
}
