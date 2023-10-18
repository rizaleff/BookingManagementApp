// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
/*$.ajax({
    url: "https://pokeapi.co/api/v2/pokemon/",
    //success: (result) => {
    //    console.log(result);
    //}
}).done((result) => {
    console.log(result)
    let temp = "";
    $.each(result.results, (key, val) => {
        temp += `<tr>
                    <td>${key + 1}</td>
                    <td>${val.name}</td>
                    <td><button type="button" onclick="detail('${val.url}')" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#modalPoke">Detail</button></td>
                </tr >`;
    });
    $("#tbodyPoke").html(temp);


}).fail((error) => {
    console.log(error);
})
*/

let employeesTable = $("#table-employee").DataTable({
    ajax: {
        url: "https://localhost:7155/api/Employee",
        dataSrc: "data",
        dataType: "JSON"
    },
    dom: 'Bfrtip',

    columns: [
        {
            data: null,
            render: function (data, type, row, meta) {
                return meta.row + 1;
            }
        },
        { data: "nik" },
        { data: "firstName" },
        { data: "lastName" },
        {
            data: "birthDate",
            render: function (data) {
                return new Date(data).toISOString().slice(0, 10);
            }
        },
        {
            data: "gender",
            render: function (data) {
                // Menentukan teks yang akan ditampilkan berdasarkan nilai gender
                return data === 1 ? "Male" : "Female";
            }   
        },
        {
            data: "hiringDate",
            render: function (data) {
                return new Date(data).toISOString().slice(0, 10)
            }
        },
        { data: "email" },
        { data: "phoneNumber" },
        {
            data: null,
            render: function (data, type, row) {
                return `<button type="button" 
                                class="btn btn-success edit-button"

                                data-guid='${row.guid}'
                                data-emp='${JSON.stringify(row)}'
                                data-bs-toggle="modal"
                                data-bs-target="#editEmployeeModal">Edit</button>

                        <button type="button" 
                                class="btn btn-danger delete-button"

                                data-guid="${row.guid}">Hapus</button>`
            }

        }
    ],
    buttons: [
        {
            extend: 'excelHtml5',
            className: 'btn btn-success',
            exportOptions: {
                columns: ':visible'
            }
        },
        {
            extend: 'pdfHtml5',
            className: 'btn btn-danger',
            exportOptions: {
                columns: [0, 1, 2, 4, 6, 7, 8, 9]
            }
        },
        {
            extend: 'colvis',
            className: 'btn btn-primary'
        }
    ]
});
/*
$("#table-employee-details").DataTable({
    ajax: {
        url: "https://localhost:7155/api/Employee/details",
        dataSrc: "data",
        dataType: "JSON"
    },
    dom: 'Bfrtip',
   
    columns: [
        {
            data: null,
            render: function (data, type, row, meta) {
                return meta.row + 1;
            }
        },
        { data: "nik" },
        { data: "fullName" },
        {
            data: "birthDate",
            render: function (data) {
                return formatDateTime(data);
            }
        },
        { data: "gender" },
        {
            data: "hiringDate",
            render: function (data) {
                return formatDateTime(data);
            }
        },
        { data: "email" },
        { data: "phoneNumber" },
        { data: "major" },
        { data: "degree" },
        { data: "gpa" },
        { data: "university" },
        {   
            data: null,
            render: function (data, type, row) {

      
                return `<button type="button" class="btn btn-success edit-button" data-id="${row.guid}">Edit</button>
                                <button type="button" class="btn btn-danger delete-button" data-id="${row.guid}">Hapus</button>`
                
            }

        }
    ],
    buttons: [
        {
            extend: 'excelHtml5',
            className: 'btn btn-success',
            exportOptions: {
                columns: ':visible'
            }
        },
        {
            extend: 'pdfHtml5',
            className: 'btn btn-danger',
            exportOptions: {
                columns: [0, 1, 2, 4, 6, 7, 8, 9]
            }
        },
        {
            extend: 'colvis',
            className: 'btn btn-primary'
        }
    ]
});*/
$('.dt-buttons').removeClass('dt-buttons');

function Insert() {
    var obj = new Object();
    obj.firstName = $("#firstName").val();
    obj.lastName = $("#lastName").val();
    obj.birthDate = new Date($("#birth-date").val()).toISOString();
    obj.gender = parseInt($("input[name='genderRadioButton']:checked").val());
    obj.hiringDate = new Date($("#hiring-date").val()).toISOString();
    obj.email = $("#email").val();
    obj.phoneNumber = $("#phone-number").val(); 


    $.ajax({
        url: "https://localhost:7155/api/Employee",
        type: "POST",
        data: JSON.stringify(obj), // Menggunakan 'data' untuk mengirimkan objek JSON
        contentType: "application/json", // Mengatur header 'Content-Type'
        dataType: "json", // Tipe data yang diharapkan dari respons server
        success: function (result) {
            employeesTable.ajax.reload();
            $('#addEmployeeModal').modal('hide');
            Swal.fire({
                icon: 'success',
                title: 'Success!',
                text: result.message,
                
            })
        },
        error: function (error) {
            console.log(error);
            let errorMessage;
            if (Array.isArray(error.responseJSON.error)) {
                errorMessage = error.responseJSON.error[0];
            } else {
                errorMessage = error.responseJSON.error;

            }
            Swal.fire({
                icon: 'error',
                title: 'Failed!',
                text: errorMessage,

            });
        }
    });
}

function Delete(guid) {
    //var guid = $('#deleteEmployeeModal').attr('data-guid');
    $.ajax({
        url: "https://localhost:7155/api/Employee?guid="+guid,
        type: "DELETE",
        success: function (result) {

            employeesTable.ajax.reload();
            Swal.fire(
                'Deleted!',
                'Your file has been deleted.',
                'success'
            )
        },
        error: function (error) {
            Swal.fire({
                title: "Failed to delete data!!",
                text: error.responseJSON.error[0],
                icon: "error",
                button: "Close",
            });
        }

    })
}

function Edit() {
    var obj = new Object();
    obj.guid = $('#editEmployeeModal').attr('data-guid');
    obj.nik = $("#enik").val();
    obj.firstName = $("#efirstName").val();
    obj.lastName = $("#elastName").val();
    obj.birthDate = new Date($("#ebirth-date").val()).toISOString();
    obj.gender = parseInt($("input[name='egenderRadioButton']:checked").val());
    obj.hiringDate = new Date($("#ehiring-date").val()).toISOString();
    obj.email = $("#eemail").val();
    obj.phoneNumber = $("#ephone-number").val();

    console.log(obj);
    $.ajax({
        url: "https://localhost:7155/api/Employee",
        type: "PUT",
        data: JSON.stringify(obj), // Menggunakan 'data' untuk mengirimkan objek JSON
        contentType: "application/json", // Mengatur header 'Content-Type'
        dataType: "json", // Tipe data yang diharapkan dari respons server
        success: function (result) {
          
            $('#editEmployeeModal').modal('hide');

            Swal.fire({
                icon: 'success',
                title: 'Success',
                text: result.message,
            })

            employeesTable.ajax.reload();
        },
        error: function (error) {
            let errorMessage;
            if (Array.isArray(error.responseJSON.error)) {
                errorMessage = error.responseJSON.error[0];
            } else {
                errorMessage = error.responseJSON.error;

            }
            Swal.fire({
                title: "Failed!!",
                text: errorMessage,
                icon: "error",
                button: "Close",
            });
        }
    });
}


// Menangkap klik tombol "Hapus" pada setiap baris tabel
$('#table-employee').on('click', '.delete-button', function () {
    var guid = $(this).data('guid'); // Mendapatkan GUID dari atribut data-guid

    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            Delete(guid)
            Swal.fire(
                'Deleted!',
                'Your file has been deleted.',
                'success'
            )
        }
    })
/*    swal({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover this data!",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                Delete(guid)
                
            } else {
                swal("Your data is safe!");
            }
        });*/
    $('#deleteEmployeeModal').attr('data-guid', guid); // Menyimpan GUID ke dalam atribut data-guid modal
});

$('#table-employee').on('click', '.edit-button', function () {
    let emp = $(this).data('emp');
    const birhtDate = (new Date(emp.birthDate).toISOString().slice(0, 10));
    const hiringDate = (new Date(emp.hiringDate).toISOString().slice(0, 10));
    $('#editEmployeeModal').attr('data-guid', emp.guid);
    $('#enik').val(emp.nik);
    $('#efirstName').val(emp.firstName);
    $('#elastName').val(emp.lastName);
    $('#eemail').val(emp.email);
    $('#ebirth-date').val(birhtDate);
    $('#ehiring-date').val(hiringDate);
    $("input[name=egenderRadioButton][value=" + emp.gender + "]").attr('checked', true);
    $('#ephone-number').val(emp.phoneNumber);

});



/*function errorMessage(error) {
    if (Array.isArray(error.responseJSON.error)) {
        return errorMessage = error.responseJSON.error[0];
    }
    return errorMessage = error.responseJSON.error;
}*/
function formatDateTime(dateTimeString) {
    const date = new Date(dateTimeString);
    const day = date.getDate();
    const monthIndex = date.getMonth();
    const year = date.getFullYear();

    // Months are zero-based, so add 1 to the month index
    const month = monthIndex + 1;

    // Format the date, month, and year as a string
    const formattedDate = `${day}-${month}-${year}`;

    return formattedDate;
}




//ini kodingan dari tombol 'insert' yang ada atribut onclick="Insert()"
/*function Insert() {
    var obj = new Object(); //sesuaikan sendiri nama objectnya dan beserta isinya
    //ini ngambil value dari tiap inputan di form nya
    obj.firstName = $("#firstName").val();
    obj.lastName = $("#lastName").val();
    obj.birthDate = $("#birth-date").val();
    obj.gender = $("input[name='genderRadioButton']:checked").val(); 
    obj.hiringDate= $("#hiring-date").val();
    obj.email = $("#email").val();
    obj.phoneNumber = $("#phone-number").val();


    console.log(obj);
    //isi dari object kalian buat sesuai dengan bentuk object yang akan di post
    $.ajax({
        url: "https://localhost:7155/api/Employee",
        type: "POST",
        dataType: JSON.stringify(obj),
        headers: {
            Accept: 'application/json;charset=utf-8',
            contentType: 'application/json;charset=utf-8'
        }
            //jika terkena 415 unsupported media type (tambahkan headertype Json & JSON.Stringify();)
}).done((result) => {
    alert(result.message);
}).fail((error) => {
    alert(error.message);
    //alert pemberitahuan jika gagal
})  
}
*/

/*

function detail(stringUrl) {
    $.ajax({
        url: stringUrl
    }).done((res) => {
       *//* let spanClass = "badge rounded-pill";
        switch (res.type) {
            case "Normal"
        };*//*
        $(".modal-title").html(res.name);
        $("#foto-pokemon").attr("src", res.sprites.other["official-artwork"].front_default);
        console.log(res);
        
    })
}*/