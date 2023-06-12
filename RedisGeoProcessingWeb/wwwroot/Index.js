let map;
let markersArray = [];

async function initMap() {

    const position = { lat: 51.454514, lng: -2.58 };

    //@ts-ignore
    const { Map } = await google.maps.importLibrary("maps");

    map = new Map(document.getElementById("map"), {
        zoom: 7,
        center: position,
        mapId: "Demo_Map",
    });

    function placeMarkerAndPanTo(latLng, map) {
       let marker = new google.maps.Marker({
            position: latLng,
            map: map,
        });
        map.panTo(latLng);
        markersArray.push(marker);
    }

    map.addListener("click", (e) => {
        placeMarkerAndPanTo(e.latLng, map);
        getMarkers(e.latLng.lat, e.latLng.lng);
    });

    new google.maps.Marker({
        position: position,
        map,
        title: "Initial point",
    });

    function deleteOverlays() {
        if (markersArray) {
            for (var i in markersArray) {
                markersArray[i].setMap(null);
            }
            markersArray.length = 0;
        }
    }

    function getMarkers(lat, lng) {

        deleteOverlays();

        $.getJSON("https://localhost:32419/",
            {
                lat: lat,
                lng: lng
            },
            function (data) {
                var items = [];
                $.each(data, function (key, val) {
                    items.push(val);
                   var marker = new google.maps.Marker({
                        position: new google.maps.LatLng(val.position.longitude, val.position.latitude),
                        title: String("Distance to selected: " + val.distance)
                    });
                    markersArray.push(marker);
                    marker.setMap(map);
                });              
            });
    }

    // Runs on load to get initial marker point
    $.getJSON("https://localhost:32419/",
        {
            lat: "54.9783",
            lng: "1.6178"
        },
        function (data) {
        var items = [];
        $.each(data, function (key, val) {
            items.push(val);
        });
        $(
            items.forEach(function (number) {
                new google.maps.Marker({
                    position: new google.maps.LatLng(number.position.longitude, number.position.latitude),
                    title: String(number.distance) 
                }).setMap(map);
            })
         )
        });

    $(document).ready(function () {
        $("#search-box").keyup(function () {
            $.ajax({
                type: "GET",
                url: "https://localhost:32419/places",
                data: 'location=' + $(this).val(),
                beforeSend: function () {
                    $("#search-box").css("background", "#FFF url(LoaderIcon.gif) no-repeat 165px");
                },
                success: function (data) {
                    $("#suggestion-box").show();
                    $("#suggestion-box").html(data);
                    $("#search-box").css("background", "#FFF");
                }
            });
        });
    });  
}

initMap();