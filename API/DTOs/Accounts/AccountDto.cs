using API.Models;

namespace API.DTOs.Accounts;
public class AccountDto
{
    //Deklarasi atribut yang dibutuhkan sebagai DTO
    public Guid Guid { get; set; }
    public string Password { get; set; }
    public int Otp { get; set; }
    public bool IsUsed { get; set; }
    public DateTime ExpiredTime {  get; set; }

    /*
     *<summary>Implicit opertor untuk mapping dari AccountDto ke Account secara implisit<summary>
     *<param name="accountDto>Object AccountDto yang akan di Mapping</param>
     *<return>Hasil mapping berupa object Account</return>
     */
    public static implicit operator Account(AccountDto accountDto)
    {
        return new Account
        {
            Guid = accountDto.Guid,
            Password = accountDto.Password,
            Otp = accountDto.Otp,
            IsUsed = accountDto.IsUsed,
            ExpiredTime = accountDto.ExpiredTime,
            ModifiedDate = DateTime.Now
        };
    }

    /*
     *<summary>Explicit opertor untuk mapping dari Account ke AccountDto secara eksplisit<summary>
     *<param name="account>Object Account yang akan di Mapping</param>
     *<return>Hasil mapping berupa object AccountDto</return>
     */
    public static explicit operator AccountDto(Account account)
    {
        return new AccountDto
        {
            Guid = account.Guid,
            Password = account.Password,
            Otp = account.Otp,
            IsUsed = account.IsUsed,
            ExpiredTime = account.ExpiredTime

        };
    }
}
