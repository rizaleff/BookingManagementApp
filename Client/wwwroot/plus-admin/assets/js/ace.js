function setActive(element) {
    // Remove 'active' class from all list items
    var listItems = document.querySelectorAll('.sidebar ul li');
    listItems.forEach(function (li) {
        li.classList.remove('active');
    });

    // Add 'active' class to the clicked list item
    element.parentElement.classList.add('active');
}