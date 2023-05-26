let map;

async function initMap() {

    const position = { lat: 51.454514, lng: -2.58 };

    //@ts-ignore
    const { Map } = await google.maps.importLibrary("maps");

    map = new Map(document.getElementById("map"), {
        zoom: 10,
        center: position,
        mapId: "DEMO_MAP_ID",
    });

    function placeMarkerAndPanTo(latLng, map) {
        new google.maps.Marker({
            position: latLng,
            map: map,
        });
        map.panTo(latLng);
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

    function getMarkers(lat, lng) {

        $.getJSON("https://localhost:32419/",
            {
                lat: lat,
                lng: lng
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
    }

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
}

initMap();