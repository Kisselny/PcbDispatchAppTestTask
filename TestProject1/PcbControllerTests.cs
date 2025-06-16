using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PcbDispatchService.Controllers;
using PcbDispatchService.Controllers.Dto;
using PcbDispatchService.Domain.Logic;
using PcbDispatchService.Domain.Models;
using PcbDispatchService.Services;
using Xunit;

using Assert = Xunit.Assert;
using TheoryAttribute = Xunit.TheoryAttribute;

namespace TestProject1
{
    public class PcbControllerTests
    {
        #region Fields & Ctor
        private readonly Mock<IPcbService> _pcbServiceMock;
        private readonly PcbController _controller;

        public PcbControllerTests()
        {
            _pcbServiceMock = new Mock<IPcbService>();
            _controller = new PcbController(_pcbServiceMock.Object);
        } 
        #endregion

        #region CreateCircuitBoard Tests
        [Fact]
        public async Task CreateCircuitBoard_ReturnsCreatedAtAction()
        {
            // Arrange
            var boardName = "TestBoard";
            var newId = 42;
            _pcbServiceMock.Setup(s => s.CreateCircuitBoard(boardName)).ReturnsAsync(newId);

            // Act
            var result = await _controller.CreateCircuitBoard(boardName);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetCircuitBoardInfo), createdResult.ActionName);

            var expectedJson = JsonSerializer.Serialize(new { id = newId });
            var actualJson = JsonSerializer.Serialize(createdResult.Value);
            Assert.Equal(expectedJson, actualJson);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task CreateCircuitBoard_NameIsNullOrEmpty_ReturnsBadRequest(string? boardName)
        {
            // Act
            var result = await _controller.CreateCircuitBoard(boardName);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        } 
        #endregion

        #region GetCircuitBoardInfo Tests
        [Fact]
        public async Task GetCircuitBoardInfo_BoardExists_ReturnsOk()
        {
            // Arrange
            var id = 42;
            var boardComponents = new List<BoardComponent> { };
            var dto = new BoardInfoDto(
                Id: id,
                Name: "TestBoard",
                ComponentNumber: boardComponents.Sum(c => c.Quantity),
                BoardComponents: boardComponents,
                CurrentStatus: "TestBusinessStatus",
                QualityControlStatus: "TestQualityStatus"
            );
            var pcb = new PrintedCircuitBoard();
            _pcbServiceMock.Setup(s => s.GetCircuitBoardById(id)).ReturnsAsync(pcb);
            _pcbServiceMock.Setup(s => s.FormatBoardDto(pcb)).Returns(dto);

            // Act
            var result = await _controller.GetCircuitBoardInfo(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(dto, okResult.Value);
        }

        [Fact]
        public async Task GetCircuitBoardInfo_BoardNotFound_ReturnsNotFound()
        {
            // Arrange
            var id = 1;
            _pcbServiceMock.Setup(s => s.GetCircuitBoardById(id)).ReturnsAsync((PrintedCircuitBoard)null);

            // Act
            var result = await _controller.GetCircuitBoardInfo(id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        } 
        #endregion

        #region GetAllBoards Tests
        [Fact]
        public async Task GetAllBoards_BoardsExist_ReturnsOk()
        {
            // Arrange
            var boards = new List<PrintedCircuitBoard> { new PrintedCircuitBoard() };
            var boardComponents = new List<BoardComponent> { };
            var dto = new BoardInfoDto(
                Id: 42,
                Name: "TestBoard",
                ComponentNumber: boardComponents.Sum(c => c.Quantity),
                BoardComponents: boardComponents,
                CurrentStatus: "TestBusinessStatus",
                QualityControlStatus: "TestQualityStatus"
            );
            _pcbServiceMock.Setup(s => s.GetAllBoards()).ReturnsAsync(boards);
            _pcbServiceMock.Setup(s => s.FormatBoardDto(It.IsAny<PrintedCircuitBoard>())).Returns(dto);

            // Act
            var result = await _controller.GetAllBoards();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedList = Assert.IsType<List<BoardInfoDto>>(okResult.Value);
            Assert.Single(returnedList);
        }

        [Fact]
        public async Task GetAllBoards_NoBoards_ReturnsNotFound()
        {
            // Arrange
            _pcbServiceMock.Setup(s => s.GetAllBoards()).ReturnsAsync(new List<PrintedCircuitBoard>());

            // Act
            var result = await _controller.GetAllBoards();

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
        #endregion

        #region AddComponentToCircuitBoard Tests
        [Fact]
        public async Task AddComponentToCircuitBoard_ReturnsNoContent()
        {
            // Arrange
            var dto = new BoardComponentDto("TestTypeName", 10);
            int id = 42;
            _pcbServiceMock.Setup(s => s.GetCircuitBoardById(id)).ReturnsAsync(new PrintedCircuitBoard());
            _pcbServiceMock.Setup(s => s.AddComponent(id, dto.ComponentTypeName, dto.Quantity)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddComponentToCircuitBoard(id, dto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task AddComponentToCircuitBoard_DtoIsNull_ReturnsBadRequest()
        {
            // Arrange
            BoardComponentDto? dto = null;
            int id = 42;

            // Act
            var result = await _controller.AddComponentToCircuitBoard(id, dto!);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Theory]
        [InlineData(null, 1)]           // dto == null
        [InlineData("", 1)]             // ComponentTypeName is empty
        [InlineData("   ", 1)]          // ComponentTypeName is whitespace
        [InlineData("ValidName", 0)]    // Quantity is zero
        [InlineData("ValidName", -5)]   // Quantity is negative
        public async Task AddComponentToCircuitBoard_InvalidInput_ReturnsBadRequest(string? componentTypeName, int quantity)
        {
            int id = 42;
            BoardComponentDto? dto = new BoardComponentDto(componentTypeName, quantity);

            var result = await _controller.AddComponentToCircuitBoard(id, dto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task AddComponentToCircuitBoard_PcbNotFound_ReturnsNotFound()
        {
            // Arrange
            BoardComponentDto? dto = new BoardComponentDto("ValidTypeName", 2);
            int id = 42;

            // Act
            _pcbServiceMock.Setup(s => s.GetCircuitBoardById(id)).ReturnsAsync(null as PrintedCircuitBoard);
            var result = await _controller.AddComponentToCircuitBoard(id, dto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
        #endregion

        #region RenameBoard Tests
        [Fact]
        public async Task RenameBoard_ReturnsNoContent()
        {
            // Arrange
            var id = 1;
            var newName = "NewName";
            _pcbServiceMock.Setup(s => s.RenameBoard(id, newName)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.RenameBoard(id, newName);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task RenameBoard_NameIsNullOrEmpty_ReturnsBadRequest(string newName)
        {
            // Arrange
            var id = 1;
            _pcbServiceMock.Setup(s => s.RenameBoard(id, newName)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.RenameBoard(id, newName);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion

        #region RemoveAllComponentsFromBoard Tests
        [Fact]
        public async Task RemoveAllComponentsFromBoard_ReturnsNoContent()
        {
            // Arrange
            var id = 1;
            _pcbServiceMock.Setup(s => s.RemoveAllComponentsFromBoard(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.RemoveAllComponentsFromBoard(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task RemoveAllComponentsFromBoard_WrongBusinessState_ReturnsConflict()
        {
            // Arrange
            var id = 1;
            _pcbServiceMock
                .Setup(s => s.RemoveAllComponentsFromBoard(id))
                .ThrowsAsync(new BusinessException("Ошибка бизнес-статуса"));

            // Act
            var result = await _controller.RemoveAllComponentsFromBoard(id);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
        }

        [Fact]
        public async Task RemoveAllComponentsFromBoard_PcbNotFound_ReturnsNotFound()
        {
            // Arrange
            var id = 1;
            _pcbServiceMock
                .Setup(s => s.RemoveAllComponentsFromBoard(id))
                .ThrowsAsync(new ApplicationException("Плата не найдена"));

            // Act
            var result = await _controller.RemoveAllComponentsFromBoard(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
        #endregion

        #region DeleteBoard Tests
        [Fact]
        public async Task DeleteBoard_ReturnsNoContent()
        {
            // Arrange
            var id = 1;
            _pcbServiceMock.Setup(s => s.DeleteBoard(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteBoard(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        #endregion

        #region MoveToNextBusinessState Tests
        [Fact]
        public async Task MoveToNextBusinessState_ReturnsNoContent()
        {
            // Arrange
            var id = 1;
            _pcbServiceMock.Setup(s => s.AdvanceToNextStatus(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.MoveToNextBusinessState(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task MoveToNextBusinessState_PcbNotFound_ReturnsNotFound()
        {
            // Arrange
            var id = 1;
            _pcbServiceMock.Setup(s => s.AdvanceToNextStatus(id)).ThrowsAsync(new ApplicationException("Плата не найдена"));

            // Act
            var result = await _controller.MoveToNextBusinessState(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task MoveToNextBusinessState_WrongState_ReturnsConflict()
        {
            // Arrange
            var id = 1;
            _pcbServiceMock.Setup(s => s.AdvanceToNextStatus(id)).ThrowsAsync(new BusinessException("Не удалось перевести плату в новое состояние: нарушены бизнес-правила"));

            // Act
            var result = await _controller.MoveToNextBusinessState(id);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
        }
        #endregion
    }
}
