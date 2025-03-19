/*!
* Start Bootstrap - Grayscale v7.0.6 (https://startbootstrap.com/theme/grayscale)
* Copyright 2013-2023 Start Bootstrap
* Licensed under MIT (https://github.com/StartBootstrap/startbootstrap-grayscale/blob/master/LICENSE)
*/
//
// Scripts
// 

window.addEventListener('DOMContentLoaded', event => {

    // Navbar shrink function
    var navbarShrink = function () {
        const navbarCollapsible = document.body.querySelector('#mainNav');
        if (!navbarCollapsible) {
            return;
        }
        if (window.scrollY === 0) {
            navbarCollapsible.classList.remove('navbar-shrink')
        } else {
            navbarCollapsible.classList.add('navbar-shrink')
        }

    };

    // Shrink the navbar 
    navbarShrink();

    // Shrink the navbar when page is scrolled
    document.addEventListener('scroll', navbarShrink);

    // Activate Bootstrap scrollspy on the main nav element
    const mainNav = document.body.querySelector('#mainNav');
    if (mainNav) {
        new bootstrap.ScrollSpy(document.body, {
            target: '#mainNav',
            rootMargin: '0px 0px -40%',
        });
    };

    // Collapse responsive navbar when toggler is visible
    const navbarToggler = document.body.querySelector('.navbar-toggler');
    const responsiveNavItems = [].slice.call(
        document.querySelectorAll('#navbarResponsive .nav-link')
    );
    responsiveNavItems.map(function (responsiveNavItem) {
        responsiveNavItem.addEventListener('click', () => {
            if (window.getComputedStyle(navbarToggler).display !== 'none') {
                navbarToggler.click();
            }
        });
    });

});

function openDeleteModalForStudents(Id) {
    var deleteUrl = '/Student/Delete/' + Id;
    document.getElementById('confirmDeleteBtn').setAttribute('href', deleteUrl);
    $('#deleteModal').modal('show'); // Show the modal
}
function openDeleteModalForDepartments(Id) {
    var deleteUrl = '/Department/Delete/' + Id;
    document.getElementById('confirmDeleteBtn').setAttribute('href', deleteUrl);
    $('#deleteModal').modal('show'); // Show the modal
}
function CloseModal() {
    $('#deleteModal').modal('hide');
}

//$(document).ready(function () {
//    $("#departmentDropdown").change(function () {
//        var deptId = $(this).val();
//        $.ajax({
//            url: '@Url.Action("GetStudentsByDepartment", "Student")',
//            type: 'GET',
//            data: { departmentId: deptId },
//            success: function (data) {
//                $("#studentList").html(data);
//            }
//        });
//    });
//});