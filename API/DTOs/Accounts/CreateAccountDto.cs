using API.Models;

namespace API.DTOs.Accounts;
public class CreateAccountDto
{
    //Deklarasi atribut yang dibutuhkan sebagai DTO
    public Guid guid {  get; set; }
    public string password { get; set; }
    public int otp { get; set; }
    public bool isUsed { get; set; }

    /*
     *<summary>Implicit opertor untuk mapping dari AccountDto ke Account secara implisit<summary>
     *<param name="accountDto>Object AccountDto yang akan di Mapping</param>
     *<return>Hasil mapping berupa object Account</return>
     */
    public static implicit operator Account(CreateAccountDto createAccountDto)
    {
        return new Account
        {
            Guid = createAccountDto.guid,
            Password = createAccountDto.password,
            Otp = createAccountDto.otp,
            IsUsed = createAccountDto.isUsed,
            ExpiredTime = DateTime.Now.AddYears(5),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }
}
