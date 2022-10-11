$(function () {

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

        if (error != '') {
            error = error.slice(0, -8) // to remove the last <br><br>
            Snackbar.show({
                text: error,
                actionTextColor: "#CFE2FF"
            });
        }
        else {

            var CarModelObj = {
                DriverId: 1,
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

})