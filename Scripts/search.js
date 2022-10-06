$(function () {

    $('#searchForm').submit((e) => {
        e.preventDefault();
        return false;
    })

    $('#search').click(() => {
        var oAddress = $("#oAddress").val()
        var oCity = $("#oCity").val()
        var oCountry = $("#oCountry").val()
        var dAddress = $("#dAddress").val()
        var dCity = $("#dCity").val()
        var dCountry = $("#dCountry").val()
        var date = $("#date").val()

        var SearchRideViewModelObj = {
            Origin: {
                Address: oAddress,
                City: oCity,
                Country: oCountry
            },
            Destination: {
                Address: dAddress,
                City: dCity,
                Country: dCountry
            },
            Date: date
        }

        $.ajax({
            type: "POST",
            url: "/Ride/Search",
            data: SearchRideViewModelObj,
            dataType: "json",
            success: (response) => {
                if (response.result) {
                    $("#rides").html('');
                    $.each(response.data, (i, ride) => {
                        $("#rides").append(`
                            <div class="col-md-6">
                                <div class="card shadow-sm mb-5">
                                    <div class="card-body p-0">
                                        <div class="card-header">
                                            <div class="row pt-2">
                                                <div class="col">
                                                    <h5 class="fw-bold">
                                                        <i class="fa-solid fa-user me-2"></i>
                                                        ${ride.Driver.FirstName} ${ride.Driver.LastName}
                                                    </h5>
                                                </div>
                                                <div class="col text-end">
                                                    <h5 class="fw-bold">
                                                        <i class="fa-solid fa-circle-dollar-to-slot me-2"></i>
                                                        ${ride.Fare}
                                                    </h5>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-body">
                                            <p>
                                                <i class="fa-solid fa-car-side me-2"></i>
                                                ${ride.Car.Model} (${ride.Car.Year})
                                            </p>
                                            <p>
                                                <i class="fa-solid fa-location-dot me-2"></i>
                                                ${ride.Origin.Address}, ${ride.Origin.City}
                                                <i class="fa-solid fa-arrow-right mx-3"></i>
                                                ${ride.Destination.Address}, ${ride.Destination.City}
                                            </p>
                                            <p>
                                                <i class="fa-solid fa-calendar-days me-2"></i>
                                                ${new Date(ride.DateTime)}
                                                <i class="fa-solid fa-clock ms-4 me-2"></i>
                                                ${Date.parse(ride.DateTime)}
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>`)
                    })
                }
                else {
                    Snackbar.show({ text: "No Rides found" });
                }
            }
        })


    })

})