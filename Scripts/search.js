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

        var FormData = {
            OAddress: oAddress,
            OCity: oCity,
            OCountry: oCountry,
            DAddress: dAddress,
            DCity: dCity,
            DCountry: dCountry,
            Date: date
        }

        $.ajax({
            type: "POST",
            url: "/Ride/Search",
            data: FormData,
            dataType: "json",
            success: (response) => {
                if (response.result) {
                    $("#rides").html('');
                    $.each(response.data, (i,ride) => {
                        $("#rides").append(`
                            <div class="col-md-6">
                                <div class="card shadow-sm mb-5">
                                    <div class="card-body">
                                        <h5>${ride.FirstName}</h5>
                                        <p>from</p>
                                        <p>to</p>
                                    </div>
                                </div>
                            </div>
                        `)
                    })
                }
                else {
                    Snackbar.show({ text: "No Rides found" });
                }
            }
        })


    })

})