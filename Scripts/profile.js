$(function () {

    $('#accountForm').submit((e) => {
        e.preventDefault();
        return false;
    })

    $('#edit').click(() => {
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

            var AccountModelObj = {
                Email: email,
                FirstName: fname,
                LastName: lname,
                Phone: phone,
                Address: {
                    Region: region,
                    City: city,
                    Country: country
                }
            }

            $.ajax({
                type: "POST",
                url: "/Profile/Edit",
                data: AccountModelObj,
                dataType: "json",
                success: (response) => {
                    if (response.result == "Success") {
                        Snackbar.show({
                            text: "Account edited successfully!",
                            actionTextColor: "#CFE2FF"
                        });
                        window.location.replace(response.url);
                    }
                    else if (response.result == "DuplicatedEmail") {

                        $("#email").addClass("is-invalid")
                        $("#email-feedback").html("Email already exists.");

                        Snackbar.show({
                            text: "An account with this email already exist.",
                            actionTextColor: "#CFE2FF",
                        });
                    }
                    else if (response.result == "NoUpdate") {
                        Snackbar.show({
                            text: "No changes made.",
                            actionTextColor: "#CFE2FF"
                        });
                    }
                }
            })
        }




    })

})