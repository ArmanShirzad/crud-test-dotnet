using Xunit;
using Moq;
using Core.Application.Commands;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;

namespace UnitTests
{
    public class CreateCustomerCommandHandlerTests
    {
        private readonly Mock<ICustomerRepository> _repositoryMock;
        private readonly Mock<IValidator<CreateCustomerCommand>> _validatorMock;
        private readonly CreateCustomerCommandHandler _handler;

        public CreateCustomerCommandHandlerTests()
        {
            _repositoryMock = new Mock<ICustomerRepository>();
            _validatorMock = new Mock<IValidator<CreateCustomerCommand>>();
            _handler = new CreateCustomerCommandHandler(_repositoryMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateCustomer_WhenValidRequest()
        {
            // Arrange
            var command = new CreateCustomerCommand
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "1234567890",
                Email = "john.doe@example.com",
                BankAccountNumber = "123456789"
            };

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult()); // No validation errors

            _repositoryMock.Setup(r => r.IsEmailUniqueAsync(command.Email)).ReturnsAsync(true);
            _repositoryMock.Setup(r => r.IsCustomerUniqueAsync(command.FirstName, command.LastName, command.DateOfBirth)).ReturnsAsync(true);
            _repositoryMock.Setup(r => r.AddCustomerAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeEmpty();
            _repositoryMock.Verify(r => r.IsEmailUniqueAsync(command.Email), Times.Once);
            _repositoryMock.Verify(r => r.IsCustomerUniqueAsync(command.FirstName, command.LastName, command.DateOfBirth), Times.Once);
            _repositoryMock.Verify(r => r.AddCustomerAsync(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenValidationFails()
        {
            // Arrange
            var command = new CreateCustomerCommand
            {
                FirstName = "",
                LastName = "Doe",
                DateOfBirth = DateTime.Now,
                PhoneNumber = "invalid-phone-number",
                Email = "invalid-email",
                BankAccountNumber = "123456789"
            };

            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("FirstName", "First Name is required."),
                new ValidationFailure("PhoneNumber", "Invalid phone number format."),
                new ValidationFailure("Email", "Invalid email format.")
            };
            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(failures));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(e => e.PropertyName == "FirstName" && e.ErrorMessage == "First Name is required.") &&
                ex.Errors.Any(e => e.PropertyName == "PhoneNumber" && e.ErrorMessage == "Invalid phone number format.") &&
                ex.Errors.Any(e => e.PropertyName == "Email" && e.ErrorMessage == "Invalid email format."));
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenEmailNotUnique()
        {
            // Arrange
            var command = new CreateCustomerCommand
            {
                FirstName = "Jane",
                LastName = "Smith",
                DateOfBirth = new DateTime(1985, 5, 15),
                PhoneNumber = "0987654321",
                Email = "jane.smith@example.com",
                BankAccountNumber = "987654321"
            };

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult()); // No validation errors
            _repositoryMock.Setup(r => r.IsEmailUniqueAsync(command.Email)).ReturnsAsync(false);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Email must be unique.");
            _repositoryMock.Verify(r => r.IsEmailUniqueAsync(command.Email), Times.Once);
            _repositoryMock.Verify(r => r.IsCustomerUniqueAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Never);
            _repositoryMock.Verify(r => r.AddCustomerAsync(It.IsAny<Customer>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenCustomerNotUnique()
        {
            // Arrange
            var command = new CreateCustomerCommand
            {
                FirstName = "Alice",
                LastName = "Johnson",
                DateOfBirth = new DateTime(1995, 7, 20),
                PhoneNumber = "1122334455",
                Email = "alice.johnson@example.com",
                BankAccountNumber = "112233445566"
            };

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult()); // No validation errors
            _repositoryMock.Setup(r => r.IsEmailUniqueAsync(command.Email)).ReturnsAsync(true);
            _repositoryMock.Setup(r => r.IsCustomerUniqueAsync(command.FirstName, command.LastName, command.DateOfBirth)).ReturnsAsync(false);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Customer must be unique by First Name, Last Name, and Date of Birth.");
            _repositoryMock.Verify(r => r.IsEmailUniqueAsync(command.Email), Times.Once);
            _repositoryMock.Verify(r => r.IsCustomerUniqueAsync(command.FirstName, command.LastName, command.DateOfBirth), Times.Once);
            _repositoryMock.Verify(r => r.AddCustomerAsync(It.IsAny<Customer>()), Times.Never);
        }
    }
}
