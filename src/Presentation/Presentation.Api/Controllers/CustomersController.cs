using Microsoft.AspNetCore.Mvc;
using MediatR;
using Core.Application.Commands;
using Core.Application.Queries;
using Core.Domain.Entities;
using System;
using System.Threading.Tasks;
using Presentation.Shared.Models;
using AutoMapper;

namespace Presentation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;


        public CustomersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _mediator.Send(new GetAllCustomersQuery());
            var customerDtos = _mapper.Map<List<CustomerDto>>(customers);
            return Ok(customerDtos);
        }


        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new CreateCustomerCommand
            {
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                DateOfBirth = customerDto.DateOfBirth,
                PhoneNumber = customerDto.PhoneNumber.ToString(), 
                Email = customerDto.Email,
                BankAccountNumber = customerDto.BankAccountNumber
            };

            var customerId = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetCustomerById), new { id = customerId }, null);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(Guid id)
        {
            var customer = await _mediator.Send(new GetCustomerByIdQuery { Id = id });
            if (customer == null)
            {
                return NotFound();
            }

            var customerDto = _mapper.Map<CustomerDto>(customer);
            return Ok(customerDto);
        }
        
 
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate and convert PhoneNumber
            if (!ulong.TryParse(customerDto.PhoneNumber, out ulong phoneNumber))
            {
                ModelState.AddModelError("PhoneNumber", "Invalid phone number format.");
                return BadRequest(ModelState);
            }

            var customerEntity = _mapper.Map<Customer>(customerDto);
            customerEntity.Id = id;
            // Create update command
            var command = new UpdateCustomerCommand
            {
                Id = id,
                FirstName = customerEntity.FirstName,
                LastName = customerEntity.LastName,
                DateOfBirth = customerEntity.DateOfBirth,
                PhoneNumber = customerEntity.PhoneNumber.ToString(), // Pass as string
                Email = customerEntity.Email,
                BankAccountNumber = customerEntity.BankAccountNumber
            };

            // Send command
            var result = await _mediator.Send(command);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var command = new DeleteCustomerCommand { Id = id };

            var result = await _mediator.Send(command);

            if (!result)
            {
                return NotFound($"Customer with ID {id} not found.");
            }

            return NoContent();
        }




    }
}
