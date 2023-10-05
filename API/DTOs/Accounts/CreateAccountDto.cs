using API.Models;
namespace API.DTOs.Accounts;
public class CreateAccountDto
{
    //Deklarasi atribut yang dibutuhkan sebagai DTO
    public Guid Guid {  get; set; }
    public string Password { get; set; }

    /*
     *<summary>Implicit opertor untuk mapping dari AccountDto ke Account secara implisit<summary>
     *<param name="accountDto>Object AccountDto yang akan di Mapping</param>
     *<return>Hasil mapping berupa object Account</return>
     */
    public static implicit operator Account(CreateAccountDto createAccountDto)
    {
        return new Account
        {
            Guid = createAccountDto.Guid,
            Password = createAccountDto.Password,
            IsUsed = true,
            Otp = 123456,
            ExpiredTime = DateTime.Now.AddDays(-30),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }
}
