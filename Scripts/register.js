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

        if (email === '') error += "Email Address required<br><br>"
        if (password === '') error += "Password required<br><br>"
        if (fname === '') error += "First Name required<br><br>"
        if (lname === '') error += "Last Name required<br><br>"
        //if (address === '') {
        //    error += "Address required<br><br>"
        //}
        //if (city === '') {
        //    error += "City required<br><br>"
        //}
        //if (country === '') {
        //    error += "Country required<br><br>"
        //}

        $("#email-feedback").html("Please provide a valid email.");

        if (error != '') {
            error = error.slice(0, -8) // to remove the last <br><br>
            Snackbar.show({
                text: error,
                actionTextColor: "#2B80FD"
            });
        }
        else {

            var RegisterViewModelObj = {
                Email: email,
                Password: password,
                FirstName: fname,
                LastName: lname,
                Address: {
                    Address: address,
                    City: city,
                    Country: country
                }
            }

            $.ajax({
                type: "POST",
                url: "/Register/Register",
                data: RegisterViewModelObj,
                dataType: "json",
                success: (response) => {
                    if (response.result == "Success") {
                        Snackbar.show({
                            text: "Registration successful!",
                            actionTextColor: "#2B80FD"
                        });
                        window.location.replace(response.url);
                    }
                    else if (response.result == "DuplicatedEmail") {

                        $("#email").addClass("is-invalid")
                        $("#email-feedback").html("Email already exists.");

                        Snackbar.show({
                            text: "An account with this email already exist.",
                            actionText: "LOGIN",
                            actionTextColor: "#2B80FD",
                            onActionClick: () => { window.location.replace(response.url) }
                        });
                    }
                    else {
                        Snackbar.show({
                            text: "Unable to register",
                            actionTextColor: "#2B80FD"
                        });
                    }
                }
            })
        }

        


    })

})