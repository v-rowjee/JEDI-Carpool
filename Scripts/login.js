$(function () {

    $('#loginForm').submit((e) => {
        e.preventDefault();
        return false;
    })

    $('#login').click(() => {
        var email = $('#email').val()
        var password = $('#password').val()

        var error = ''

        if (email === '') {
            error += "Email Address required<br><br>"
        }
        if (password === '') {
            error += "Password required<br><br>"
        }
        if (error != '') {
            Snackbar.show({ text: error });
            return false
        }

        var formData = { Email: email, Password: password }

        $.ajax({
            type: "POST",
            url: "/Login/Authenticate",
            data: formData,
            dataType: "json",
            success: (response) => {
                if (response.result) {
                    Snackbar.show({ text: "Authentication successful!" });
                    window.location.replace(response.url);
                }
                else {
                    Snackbar.show({ text: "Unable to authenticate" });
                }
            }
        })
    })

})