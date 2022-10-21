$(function () {

    $('#search').click(() => {
        var oRegion = $("#oRegion").val()
        var oCity = $("#oCity").val()
        var dRegion = $("#dRegion").val()
        var dCity = $("#dCity").val()
        var date = $("#date").val()

        var SearchRideViewModelObj = {
            RegionFrom: oRegion,
            CityFrom: oCity,
            RegionTo: dRegion,
            CityTo: dCity,
            Date: date
        }

        $.get("/Ride/Search", (data, status) => {
            alert(data)
        })

    })

})