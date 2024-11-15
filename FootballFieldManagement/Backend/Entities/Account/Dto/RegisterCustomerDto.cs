using Backend.Entities.Customer.Dto;

namespace Backend.Entities.Account.Dto
{
    public class RegisterCustomerDto
    {
        public AccountDto Account { get; set; }
        public CustomerDto Customer { get; set; }
    }
}
