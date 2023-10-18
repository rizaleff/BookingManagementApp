$.ajax({
    url: "/university/GetAllUniversity/"
}).done((res) => {
    console.log(res);
}).fail((err) => {
    console.log(err);
})