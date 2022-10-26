// bootstrap form validations
(() => {
    'use strict'

    // Fetch all the forms we want to apply custom Bootstrap validation styles to
    const forms = document.querySelectorAll('.needs-validation')

    // Loop over them and prevent submission
    Array.from(forms).forEach(form => {
        form.addEventListener('submit', event => {
            if (!form.checkValidity()) {
                event.preventDefault()
                event.stopPropagation()
            }

            form.classList.add('was-validated')
        }, false)
    })
})()

//page laod
$(window).on('load', function () {
    $('#loading').css("width", "100%")
    setTimeout(() => {
        $('#loading').fadeOut()
    }, 750);
})

// https://codepen.io/adethis/pen/BaWMLKO
$(document).ready(function () {
    // Action next
    $('.btn-next').on('click', function () {
        // Get value from data-to in button next
        const n = $(this).attr('data-to');
        // Action trigger click for tag a with id in value n
        $(n).trigger('click');
    });
    // Action back
    $('.btn-prev').on('click', function () {
        // Get value from data-to in button prev
        const n = $(this).attr('data-to');
        // Action trigger click for tag a with id in value n
        $(n).trigger('click');
    });
});

// Animation
/*document.addEventListener('DOMContentLoaded', function () { window.setTimeout(document.querySelector('svg').classList.add('animated'), 1000); })*/

// Tooltip
const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]')
const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl))

// add shadow to all cards when hover
//(() => {
//    $(".card").hover(
//        function () { $(this).addClass('shadow-sm') },
//        function () { $(this).removeClass('shadow-sm') }
//    )
//})()