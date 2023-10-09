using API.Contracts;
using API.DTOs.Bookings;
using API.DTOs.Rooms;
using API.Models;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks.Dataflow;

namespace API.Controllers;
[ApiController] //Menandadakan bahwa kelas ini merupakan sebuah controller API
[Route("api/[controller]")] //format route dari tiap endpoint pada controller ini
[Authorize]
//Deklarasi kelas BookingController yang merupakan turunan dari kelas ControllerBase
public class BookingController : ControllerBase
{
    //sebagai perantara untuk melakukan CRUD melalui contract yang telah dibuat
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public BookingController(IBookingRepository accountRepository, IRoomRepository roomRepository,
                             IEmployeeRepository employeeRepository)
    {
        _bookingRepository = accountRepository;
        _roomRepository = roomRepository;
        _employeeRepository = employeeRepository;
    }

    //Endpoint dengan menggunakan method HTTP Get untuk medapatkan data ruangan yang digunakan hari ini
    [HttpGet("booking-today")]
    public IActionResult GetBookingToday()
    {
        //Mendapatkan semua data booking pada hari ini
        var bookedToday = _bookingRepository.GetBookedToday();

        //Mendapatakan seluruh data rooms
        var rooms = _roomRepository.GetAll();

        //Mengecek apakah terdapat data pada variabel bookedToday dan akan mereturn response NotFound
        if (!bookedToday.Any())
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }

        //Join data bookedToday dan rooms

        var bookedTodayDetails = (from boo in bookedToday
                                  join roo in rooms on boo.RoomGuid equals roo.Guid
                                  select new BookingTodayDto 
                                  {
                                      BookingGuid = boo.Guid,
                                      RoomName = roo.Name,
                                      Status = boo.Status.ToString(),
                                      Floor = roo.Floor,
                                      //Mendapatkan nama Employee melalui method GetNameByGuid
                                      BookedBy = _employeeRepository.GetNameByGuid(boo.EmployeeGuid) 
                                  }).ToList();

        //Mengembalikan nilai berupa response OK dengan membawa data bookedTodayDetails
        return Ok(new ResponseOKHandler<IEnumerable<BookingTodayDto>>(bookedTodayDetails));
    }



    //Endpoint dengan menggunakan method HTTP Get untuk medapatkan detail booking ruangan
    [HttpGet("details")]
    public IActionResult GetDetails()
    {
        //Mendapatkan seluruh data pada tabel untuk entitas Booking
        var bookings = _bookingRepository.GetAll();

        //Mengecek apakah variabel bookings memiliki data dan akan me-return response NotFound
        if (!bookings.Any())
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }

        //Mendaptakan seluruh data pada tabel untuk entitas Employee
        var employees = _employeeRepository.GetAll();

        //Mendaptakan seluruh data pada tabel untuk entitas Room
        var rooms = _roomRepository.GetAll();

        //Melakukan join pada data bookings, rooms, dan employees untuk mendapatkan data detail booking
        var bookingDetails = from boo in bookings
                             join roo in rooms on boo.RoomGuid equals roo.Guid
                             join emp in employees on boo.EmployeeGuid equals emp.Guid
                             select new BookingDetailDto
                             {
                                 Guid = boo.Guid,
                                 BookedNik = emp.Nik,
                                 BookedBy = emp.FirstName + " " + emp.LastName,
                                 RoomName = roo.Name,
                                 StartDate = boo.StartDate,
                                 EndDate = boo.EndDate,
                                 Status = boo.Status.ToString(),
                                 Remarks = boo.Remarks
                             };

        //Mengembalikan nilai berupa objek ResponseOKHandler dengan argument <IEnumerable<BookingDetailDto>
        return Ok(new ResponseOKHandler<IEnumerable<BookingDetailDto>>(bookingDetails));
    }

    //Endpoint dengan menggunakan method HTTP Get untuk medapatkan detail booking ruangan berdasarkan Booking GUID
    [HttpGet("details-by-id")]
    public IActionResult GetDetailsByGuid(Guid guid)
    {
        //Mendapatkan data Booking berdasarkan guid yang dinginkan
        var booking = _bookingRepository.GetByGuid(guid);

        //mengecek apakah data booking bernilai null dan akan mengembalikan response NotFound
        if (booking is null)
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }

        //Mendapatkan data employee berdasarkan Employee Guid
        var employee = _employeeRepository.GetByGuid(booking.EmployeeGuid);

        //Mendapatkan data Room berdasarkan Room Guid
        var room = _roomRepository.GetByGuid(booking.RoomGuid);

        //Instansiasi object Dto berdasarkan data yang ada
        var bookingDetail = new BookingDetailDto
        {
            Guid = booking.Guid,
            BookedBy = employee.FirstName + " " + employee.LastName,
            BookedNik = employee.Nik,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            RoomName = room.Name,
            Status = booking.Status.ToString(),
            Remarks = booking.Remarks
        };
        //Mengembalikan nilai berupa objek ResponseOKHandler dengan argument variabel bookingDetail
        return Ok(new ResponseOKHandler<BookingDetailDto>(bookingDetail));
    }


    //End Point menggunakan method HTTP Get untuk mendapatkan data ruangan yang tersedia (tidak di-booking)
    [HttpGet("available-rooms")]
    public IActionResult GetAvailableRooms()
    {
        //Mendapatkan data Booking hari ini yang didapat dari method GetBookedToday
        var bookedToday = _bookingRepository.GetBookedToday();

        //Mendapatkan seluruh data Room 
        var rooms = _roomRepository.GetAll();

        //Mendapatkan rooms yang tersedia dengan menggunakan LINQ.
        var availableRooms = rooms.Where(room => !bookedToday.Any(book => book.RoomGuid == room.Guid));


        //Jika availableRooms tidak memiliki data maka akan mengembalikan response NotFound
        if (!availableRooms.Any())
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }

        //Operator sxplicit untuk konversi dari Room ke RoomDto dan disimpan pada variabel data
        var data = availableRooms.Select(x => (RoomDto)x);
        return Ok(new ResponseOKHandler<IEnumerable<RoomDto>>(data));
    }

    //End Point untuk mendapatka durasi booking pada tiap data Booking
    [HttpGet("booking-length")]
    public IActionResult GetBookingLenght()
    {
        //Mendapatkan seluruh data entitas Booking 
        var bookings = _bookingRepository.GetAll();

        //Mendapatkan seluruh data entitas Room
        var rooms = _roomRepository.GetAll();

        //Mengecek apakah terdapat data pada variabel rooms dan mengembalikan response NotFound
        if (!rooms.Any())
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }

       //Join untuk menggabungkan data bookings dan rooms 
        var bookingLength = from boo in bookings
                            join roo in rooms on boo.RoomGuid equals roo.Guid
                            select new BookingLengthDto
                            {
                                RoomGuid = roo.Guid,
                                RoomName = roo.Name,

                                //Instansiasi nilai BookingLength dari return value method GenerateDayLength
                                BookingLength = GenerateHandler.GenerateDayLength(boo.StartDate, boo.EndDate) 
                            };

        //Mengembalikan nilai berupa objek ResponseOKHandler dengan argument variabel bookingLength
        return Ok(new ResponseOKHandler<IEnumerable<BookingLengthDto>>(bookingLength));
    }

    /*
     *<summary>request HTTP GET untuk mendpatkan data dari semua Booking</summary>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpGet]
    public IActionResult GetAll()
    {
        //Mendapatkan data Booking dan disimpan pada variabel result
        var result = _bookingRepository.GetAll();
        if (!result.Any())
        {
            //Mengembalikan nilai dengan response body berupa objek ResponseErrorHandler
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound, //Inisialisasi nilai atribut Code
                Status = HttpStatusCode.NotFound.ToString(), //Inisialisai nilai atribut Status
                Message = "Data Not Found" //Inisialisasi nilai atribut Message
            });
        }

        //mapping setiap item variabel result ke dalam object dari kelas BookingDto menggunakan explicit operator
        var data = result.Select(x => (BookingDto)x);

        //Mengembalikan nilai berupa objek ResponseOKHandler dengan argument <IEnumerable<BookingDto>
        return Ok(new ResponseOKHandler<IEnumerable<BookingDto>>(data));
    }

    /*
     *<summary>request HTTP GET untuk mendpatkan data berdasarkan Guid yang dimasukkan pada parameter</summary>
     *<param name="guid">guid yang didapatkan dari path<param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        //Mendapatkan data Booking berdasrkan Gui dan disimpan pada variabel result
        var result = _bookingRepository.GetByGuid(guid);

        //Mengecek apakah variabel result bernilai null
        if (result is null)
        {
            //Mengembalikan nilai dengan response body berupa objek ResponseErrorHandler
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound, //Inisialisasi nilai atribut Code
                Status = HttpStatusCode.NotFound.ToString(), //Inisialisai nilai atribut Status
                Message = "Data Not Found" //Inisialisasi nilai atribut Message
            });
        }

        //mapping variabel result ke BookingDto menggunakan explicit operator
        //Mengembalikan nilai berupa response OK dengan response body berupa objek ResponseOKHandler
        return Ok(new ResponseOKHandler<BookingDto>((BookingDto)result));
    }

    /*
     *<summary>request HTTP POST untuk menambahkan Booking baru</summary>
     *<param name="createBookingDto">Data yang akan ditambahkan, didapatkan dari request body<param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpPost]
    public IActionResult Create(CreateBookingDto createBookingDto)
    {
        try
        {
            //Mapping secara implisit pada createBookingDto untuk dijadikan objek Booking
            var result = _bookingRepository.Create(createBookingDto);

            //Mapping variabel result ke BookingDto menggunakan explicit operator
            //Mengembalikan nilai berupa response OK dengan response body berupa objek ResponseOKHandler
            return Ok(new ResponseOKHandler<BookingDto>((BookingDto)result));
        }
        catch (ExceptionHandler ex)
        {
            //Mengembalikan nilai dengan response body berupa objek ResponseErrorHandler
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,//Inisialisasi atribut Code dengan nilai 500
                Status = HttpStatusCode.InternalServerError.ToString(), //Inisialisasi atribut Status dengan nilai InternalServerError
                Message = "Failed to Create Data", //Inisialisasi nilai atribut Message
                Error = ex.Message  //Inisialisasi nilai atribut Error berupa Message dari ExceptionHandler
            });
        }
    }

    /*
     *<summary>request HTTP DELETE untuk menambahkan menghapus data berdasarkan Guid</summary>
     *<param name="guid">Guid dari data yang akan dihapus, didapatkan dari request body<param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        try
        {
            //Mendapatkan data Booking berdasrkan Guid dan disimpan pada variabel bookingByGuid
            var bookingByGuid = _bookingRepository.GetByGuid(guid);

            //Mengecek apakah bookingByGuid bernilai null
            if (bookingByGuid is null)
            {
                //Mengembalikan nilai NotFound dengan response body berupa objek ResponseErrorHandler
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound, //Inisialisasi atribut Code dengan nilai 500
                    Status = HttpStatusCode.NotFound.ToString(), //Inisialisasi atribut Status dengan nilai NotFound
                    Message = "Data Not Found" //Inisialisasi nilai atribut Message
                });
            }

            //Melakuan Delete data booking melalui perantara contract _bookingRepository
            _bookingRepository.Delete(bookingByGuid);

            //Mengembalikan nilai berupa response OK dengan response body berupa objek ResponseOKHandler
            return Ok(new ResponseOKHandler<string>("Data Deleted"));
        }
        catch (ExceptionHandler ex)
        {
            //Mengembalikan nilai berupa Respon Status 500 dan objek ResponseErrorHandler
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError, //Inisialiasi atribut Code dengan nilai 500
                Status = HttpStatusCode.InternalServerError.ToString(), //Inisialisasi atribut Status dengan nilai InternalServerError
                Message = "Failed to create data", //Inisialisasi atribut Message
                Error = ex.Message //Inisialisasi nilai atribut Error berupa Message dari ExceptionHandler
            });
        }
    }

    /*
     *<summary>request HTTP PUT untuk melakukan perubahan data Booking </summary>
     *<param name="bookingDto">Data yang akan dijadikan perubahan, didapatkan dari request body<param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpPut]
    public IActionResult UpdateByGuid(BookingDto bookingDto)
    {
        try
        {
            //Mendapatkan data Booking berdasarkan guid
            var bookingByGuid = _bookingRepository.GetByGuid(bookingDto.Guid);

            //Mengecek apakah bookingByGuid bernilai null
            if (bookingByGuid is null)
            {
                //Mengembalikan nilai response NotFound dengan response body berupa objek ResponseErrorHandler
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound, //Inisialisasi atribut Code dengan nilai 500
                    Status = HttpStatusCode.NotFound.ToString(), //Inisialisasi atribut Status dengan nilai NotFound
                    Message = "Data Not Found" //Inisialisasi nilai atribut Message
                });
            }

            //Menyimpan data dari parameter ke dalam objek toUpdate, serta dilakukan mapping secara implisit
            Booking toUpdate = bookingDto;

            //Inisialiasi nilai CreatedDate agar tidak ada perubahan dari data awal
            toUpdate.CreatedDate = bookingByGuid.CreatedDate;

            //Melakukan Update dengan parameter toUpdate
            var result = _bookingRepository.Update(toUpdate);

            //Mengembalikan nilai response OK dengan response body berupa objek ResponseOKHandler dengan argumen string
            return Ok(new ResponseOKHandler<string>("Data Updated"));
        }
        catch (ExceptionHandler ex)
        {
            //Mengembalikan nilai berupa Respon Status 500 dan objek ResponseErrorHandler
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError, //Inisialiasi atribut Code dengan nilai 500
                Status = HttpStatusCode.InternalServerError.ToString(), //Inisialisasi atribut Status dengan nilai InternalServerError
                Message = "Failed to create data", //Inisialisasi atribut Message
                Error = ex.Message //Inisialisasi nilai atribut Error berupa Message dari ExceptionHandler
            });
        }
    }

}
