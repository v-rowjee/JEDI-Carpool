$(function () {

    $('#registerForm').submit((e) => {
        e.preventDefault();
        return false;
    })

    $('#register').click(() => {
        var email = $('#email').val()
        var password = $('#password').val()
        var fname = $('#fname').val()
        var lname = $('#lname').val()
        var address = $('#address').val()
        var city = $('#city').val()
        var country = $('#country').val()

        var error = ''

        if (email === '') {
            error += "Email Address required<br><br>"
        }
        if (password === '') {
            error += "Password required<br><br>"
        }
        if (fname === '') {
            error += "First Name required<br><br>"
        }
        if (lname === '') {
            error += "Last Name required<br><br>"
        }
        //if (address === '') {
        //    error += "Address required<br><br>"
        //}
        //if (city === '') {
        //    error += "City required<br><br>"
        //}
        //if (country === '') {
        //    error += "Country required<br><br>"
        //}
        if (error != '') {
            Snackbar.show({ text: error });
            return false
        }

        var formData = {
            Email: email,
            Password: password,
            FirstName: fname,
            LastName: lname,
            Address: address,
            City: city,
            Country: country
        }

        $.ajax({
            type: "POST",
            url: "/Register/Register",
            data: formData,
            dataType: "json",
            success: (response) => {
                if (response.result) {
                    Snackbar.show({ text: "Registration successful!" });
                    window.location.replace(response.url);
                }
                else {
                    Snackbar.show({ text: "Unable to register" });
                }
            }
        })
    })

})