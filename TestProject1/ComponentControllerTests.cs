using Microsoft.AspNetCore.Mvc;
using Moq;
using PcbDispatchService.Controllers;
using PcbDispatchService.Dal;
using ComponentType = PcbDispatchService.Domain.Models.ComponentType;

namespace TestProject1
{
    [TestFixture]
    public class ComponentControllerTests
    {
        private Mock<IComponentTypesRepository> _mockRepository;
        private ComponentController _controller;

        [SetUp]
        public void Setup()
        {
            // Создаем мок репозитория
            _mockRepository = new Mock<IComponentTypesRepository>();
            // Создаем экземпляр контроллера с подставленным моком
            _controller = new ComponentController(_mockRepository.Object);
        }

        [Test]
        public async Task GetAllTypes_ReturnsOkResult_WithListOfComponents()
        {
            // Arrange
            var mockComponents = new List<ComponentType>()
            {
                ComponentType.Create("Resistor", 10),
                ComponentType.Create("Capacitor", 15),
                ComponentType.Create("Inductor", 20)
            };

            _mockRepository
                .Setup(repo => repo.GetAllComponents())
                .ReturnsAsync(mockComponents);

            // Act
            var result = await _controller.GetAllTypes();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            CollectionAssert.AreEqual(mockComponents, okResult.Value as List<ComponentType>);
            Assert.IsInstanceOf<List<ComponentType>>(okResult.Value);
            Assert.AreEqual(mockComponents, okResult.Value);

            var components = okResult.Value as List<ComponentType>;
            Assert.IsNotNull(components);
            Assert.AreEqual(3, components.Count);
            Assert.AreEqual("Resistor", components[0].Name);
            Assert.AreEqual(10, components[0].AvailableSupply);

        }

        [Test]
        public async Task GetAllTypes_ReturnsNotFound_WhenRepositoryReturnsNull()
        {
            // Arrange
            _mockRepository
                .Setup(repo => repo.GetAllComponents())
                .ReturnsAsync((List<ComponentType>)null);

            // Act
            var result = await _controller.GetAllTypes();

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public void GetAllTypes_ThrowsException_WhenRepositoryThrowsException()
        {
            // Arrange
            _mockRepository
                .Setup(repo => repo.GetAllComponents())
                .Throws(new ApplicationException("Test exception"));

            // Act & Assert
            Assert.ThrowsAsync<ApplicationException>(async () => await _controller.GetAllTypes());
        }

        [TearDown]
        public void TearDown()
        {
            // Освобождаем ресурсы
            _mockRepository = null;
            _controller.Dispose();
        }
    }
}