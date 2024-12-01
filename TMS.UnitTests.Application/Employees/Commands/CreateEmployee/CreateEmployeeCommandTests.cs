using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using TMS.Application.Employees.Commands.CreateEmployee;
using TMS.UnitTests.Application.Common;
using Xunit;
using Xunit.Abstractions;

namespace TMS.UnitTests.Application.Employees.Commands.CreateEmployee
{
    public class CreateEmployeeCommandTests : CommandTestBase
    {
        private readonly ITestOutputHelper _output;

        public CreateEmployeeCommandTests(ITestOutputHelper output)
        {
            _output = output;
        }
        [Theory]
        [InlineData(null)]
        public async Task Handle_ShouldPersistEmployee(string fullname)
        {
            // Arrange
            var command = new CreateEmployeeCommand { FullName = fullname };
            var handler = new CreateEmployeeCommand.CreateEmployeeCommandHandler(Context);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            var entity = Context.Employees.Find(result);
            entity.ShouldNotBeNull("Employee entity should have been persisted but was not found.");

            if (entity.EmployeeId>0)
            {
                Assert.True(true, $"Test passed: Expected FullName to be '{fullname}', and got '{entity.FullName}'");
                _output.WriteLine($"Test passed: FullName '{fullname}' was successfully persisted.");
            }
            else
            {
                var failMessage = $"Test failed: Expected FullName to be '{fullname}', but got '{entity.FullName}'";
                _output.WriteLine(failMessage);
                Assert.True(false, failMessage); // This will fail the test and show the message in output.
            }
        }

    }
}
