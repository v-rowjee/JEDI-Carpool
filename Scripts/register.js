$(function () {

    // fetching data from ipregistry api
    fetch('https://api.ipregistry.co/?key=tryout')
        .then(function (response) {
            return response.json();
        })
        .then(function (payload) {
            $('#region').val(payload.location.region.name)
            $('#city').val(payload.location.city)
            $('#country').val(payload.location.country.name)
        });

    $('#registerForm').submit((e) => {
        e.preventDefault();
        return false;
    })

    $('#register').click(() => {
        var email = $('#email').val()
        var password = $('#password').val()
        var fname = $('#fname').val()
        var lname = $('#lname').val()
        var phone = $('#phone').val()
        var region = $('#region').val()
        var city = $('#city').val()
        var country = $('#country').val()

        var error = ''

        if (!email) error += "Email Address required<br><br>"
        if (!password) error += "Password required<br><br>"
        if (!fname) error += "First Name required<br><br>"
        if (!lname) error += "Last Name required<br><br>"
        if (!phone) error += "Phone number required<br><br>"
        if (!region) error += "Region required<br><br>"
        if (!city) error += "City required<br><br>"
        if (!country) error += "Country required<br><br>"


        $("#email-feedback").html("Please provide a valid email.");

        if (error != '') {
            error = error.slice(0, -8) // to remove the last <br><br>
            Snackbar.show({
                text: error,
                actionTextColor: "#CFE2FF"
            });
        }
        else {

            if (!region || !city || !country) {
                var RegisterViewModelObj = {
                    Email: email,
                    Password: password,
                    FirstName: fname,
                    LastName: lname,
                    Phone: phone
                }
            }
            else {
                var RegisterViewModelObj = {
                    Email: email,
                    Password: password,
                    FirstName: fname,
                    LastName: lname,
                    Phone: phone,
                    Address: {
                        Region: region,
                        City: city,
                        Country: country
                    }
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
                            actionTextColor: "#CFE2FF"
                        });
                        window.location.replace(response.url);
                    }
                    else if (response.result == "DuplicatedEmail") {

                        $("#email").addClass("is-invalid")
                        $("#email-feedback").html("Email already exists.");

                        Snackbar.show({
                            text: "An account with this email already exist.",
                            actionText: "LOGIN",
                            actionTextColor: "#CFE2FF",
                            onActionClick: () => { window.location.replace(response.url) }
                        });
                    }
                    else {
                        Snackbar.show({
                            text: "Unable to register",
                            actionTextColor: "#CFE2FF"
                        });
                    }
                }
            })
        }

        


    })

})