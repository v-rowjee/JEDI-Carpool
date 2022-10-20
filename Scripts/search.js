$(function () {

    $('#search').click(() => {
        var oRegion = $("#oRegion").val()
        var oCity = $("#oCity").val()
        var dRegion = $("#dRegion").val()
        var dCity = $("#dCity").val()

        var SearchRideViewModelObj = {
            RegionFrom: oRegion,
            CityFrom: oCity,
            RegionTo: dRegion,
            CityTo: dCity
        }

        //var urlQuery = new URLSearchParams(SearchRideViewModelObj).toString();
        //var url = "https://localhost:44306/Ride/Search/?"
        //window.location.replace(url + urlQuery)

    })

})