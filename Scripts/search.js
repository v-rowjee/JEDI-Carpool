$(function () {

    $("#swap-location").click(() => {

        var oRegion = $("input[name=RegionFrom]").val();
        var oCity = $("input[name=CityFrom]").val();
        var dRegion = $("input[name=RegionTo]").val();
        var dCity = $("input[name=CityTo]").val();

        $("input[name=RegionFrom]").val(dRegion)
        $("input[name=CityFrom]").val(dCity)

        $("input[name=RegionTo]").val(oRegion)
        $("input[name=CityTo]").val(oCity)
    })

})