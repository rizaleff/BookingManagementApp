using API.Contracts;
using API.DTOs.Roles;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;

namespace API.Controllers;

[ApiController] //Menandadakan bahwa kelas ini merupakan sebuah controller API
[Route("api/[controller]")] //format route dari tiap endpoint pada controller ini

//Deklarasi kelas RoleController yang merupakan turunan dari kelas ControllerBase
public class RoleController : ControllerBase
{
    //sebagai perantara untuk melakukan CRUD melalui contract yang telah dibuat
    private readonly IRoleRepository _roleRepository;

    public RoleController(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    /*
     *<summary>request HTTP GET untuk mendpatkan data dari semua Role</summary>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpGet]
    public IActionResult GetAll()
    {
        //Mendapatkan data Role dan disimpan pada variabel result
        var result = _roleRepository.GetAll();
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }

        //mapping setiap item variabel result ke dalam object dari kelas RoleDto menggunakan explicit operator
        var data = result.Select(x => (RoleDto)x);
        
        return Ok(data);
    }

    /*
     *<summary>request HTTP GET untuk mendpatkan data berdasarkan Guid yang dimasukkan pada parameter</summary>
     *<param name="guid">guid yang didapatkan dari path</param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _roleRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        //mapping variabel result ke RoleDto menggunakan explicit operator
        return Ok((RoleDto)result);
    }

    /*
     *<summary>request HTTP POST untuk menambahkan Role baru</summary>
     *<param name="createRoleDto">Data yang akan ditambahkan, didapatkan dari request body</param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpPost]
    public IActionResult Create(CreateRoleDto createRoleDto)
    {
        //Mapping secara implisit pada createRoleDto untuk dijadikan objek Role
        var result = _roleRepository.Create(createRoleDto);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        //Mapping variabel result ke RoleDto menggunakan explicit operator
        return Ok((RoleDto)result);
    }


    /*
     *<summary>request HTTP DELETE untuk menambahkan menghapus data berdasarkan Guid</summary>
     *<param name="guid">Guid dari data yang akan dihapus, didapatkan dari request body</param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var roleByGuid = _roleRepository.GetByGuid(guid);
        if (roleByGuid is null)
        {
            return NotFound("ID Not Found");
        }
        var result = _roleRepository.Delete(roleByGuid);
        if (!result)
        {
            return BadRequest("Failed to delete data");
        }
        return Ok("Data Deleted");
    }

    /*
     *<summary>request HTTP PUT untuk melakukan perubahan data Role </summary>
     *<param name="roleDto">Data yang akan dijadikan perubahan, didapatkan dari request body</param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpPut]
    public IActionResult UpdateByGuid(RoleDto roleDto)
    {
        //Mendapatkan data Role berdasarkan guid
        var roleByGuid = _roleRepository.GetByGuid(roleDto.Guid);
        if (roleByGuid is null)
        {
            return NotFound("ID Not Found");
        }

        //Menyimpan data dari parameter ke dalam objek toUpdate, serta dilakukan mapping secara implisit
        Role toUpdate = roleDto;
        
        //Inisialiasi nilai CreatedDate agar tidak ada perubahan dari data awal
        toUpdate.CreatedDate = roleByGuid.CreatedDate;

        //Melakukan Update dengan parameter toUpdate
        var result = _roleRepository.Update(toUpdate);
        if (!result)
        {
            return BadRequest("Failed to Update Date");

        }
        return Ok("Data Updated");
    }


}
