$(function () {

    // CREATE CAR

    $('#createCarForm').submit((e) => {
        e.preventDefault();
        return false;
    })

    $('#create').click(() => {
        var model = $('#model').val()
        var plateNumber = $('#plateNumber').val()
        var year = $('#year').val()
        var color = $('#color').val()
        var seat = $('#seat').val()

        var error = ''

        if (!model) error += "Model of car required<br><br>"
        if (!plateNumber) error += "Plate Number required<br><br>"
        if (!year) error += "Year of car required<br><br>"
        if (!color) error += "Color of car required<br><br>"
        if (!seat) error += "Capacity of car required<br><br>"

        if (year < 1000 || year > 9999) {
            $("#year").addClass("is-invalid")
            error += "Enter a valid year<br><br>"
        }

        if (error != '') {
            error = error.slice(0, -8) // to remove the last <br><br>
            Snackbar.show({
                text: error,
                actionTextColor: "#CFE2FF"
            });
        }
        else {

            var CarModelObj = {
                Model: model,
                PlateNumber: plateNumber,
                Year: year,
                Color: color,
                Seat: seat
            }

            $.ajax({
                type: "POST",
                url: "/Car/Create",
                data: CarModelObj,
                dataType: "json",
                success: (response) => {
                    if (response.result) {
                        Snackbar.show({
                            text: "Car details added successfully!",
                            actionTextColor: "#CFE2FF"
                        });
                        window.location.replace(response.url);
                    }
                    else {
                        Snackbar.show({
                            text: "Unable to add car details",
                            actionTextColor: "#CFE2FF"
                        });
                    }
                }
            })
        }

    })


    // EDIT CAR
    $('#editCarForm').submit((e) => {
        e.preventDefault();
        return false;
    })

    $('#edit').click(() => {
        var model = $('#model').val()
        var plateNumber = $('#plateNumber').val()
        var year = $('#year').val()
        var color = $('#color').val()
        var seat = $('#seat').val()

        var error = ''

        if (!model) error += "Model of car required<br><br>"
        if (!plateNumber) error += "Plate Number required<br><br>"
        if (!year) error += "Year of car required<br><br>"
        if (!color) error += "Color of car required<br><br>"
        if (!seat) error += "Capacity of car required<br><br>"

        if (year<1000 || year>9999) error += "Enter a valid year<br><br>"

        if (error != '') {
            error = error.slice(0, -8) // to remove the last <br><br>
            Snackbar.show({
                text: error,
                actionTextColor: "#CFE2FF"
            });
        }
        else {

            var CarModelObj = {
                Model: model,
                PlateNumber: plateNumber,
                Year: year,
                Color: color,
                Seat: seat
            }

            $.ajax({
                type: "POST",
                url: "/Car/Edit",
                data: CarModelObj,
                dataType: "json",
                success: (response) => {
                    if (response.result) {
                        Snackbar.show({
                            text: "Car details updated successfully!",
                            actionTextColor: "#CFE2FF"
                        });
                        window.location.replace(response.url);
                    }
                    else {
                        Snackbar.show({
                            text: "Unable to add car details",
                            actionTextColor: "#CFE2FF"
                        });
                    }
                }
            })
        }

    })


    // DELETE CAR
    $('#deleteCarForm').submit((e) => {
        e.preventDefault();
        return false;
    })
    $('#delete').click(() => {
        $.ajax({
            type: "POST",
            url: "/Car/Delete",
            dataType: "json",
            success: (response) => {
                if (response.result == "Success") {
                    Snackbar.show({
                        text: "Car details removed!",
                        actionTextColor: "#CFE2FF"
                    });
                    window.location.replace(response.url);
                }
                else if (response.result == "HasRide") {
                    Snackbar.show({
                        text: "You have an active ride. Cannot delete car.",
                        actionTextColor: "#CFE2FF",
                        actionText: "VIEW",
                        onActionClick: ()=>{
                            window.location.replace(response.url)
                        }
                    });
                }
                else {
                    Snackbar.show({
                        text: "Unable to remove car details",
                        actionTextColor: "#CFE2FF"
                    });
                }
            }
        })
    })

})

