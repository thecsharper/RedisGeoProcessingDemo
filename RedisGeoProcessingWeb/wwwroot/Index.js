﻿let map;

async function seedData() {

    $.getJSON("https://localhost:32419/load", function () {
        console.log("success");
    })
        .done(function () {
            initMap();
            console.log("second success");
        }).fail(function (jqXHR, textStatus, errorThrown) {
            console.log("error " + textStatus);
            console.log("incoming Text " + jqXHR.responseText + " " + errorThrown);
        })
        .always(function () {
            console.log("complete");
        });
}

async function initMap() {

    const position = { lat: 51.454514, lng: -2.58 };

    //@ts-ignore
    const { Map } = await google.maps.importLibrary("maps");

    map = new Map(document.getElementById("map"), {
        zoom: 10,
        center: position,
        mapId: "DEMO_MAP_ID",
    });

    new google.maps.Marker({
        position: position,
        map,
        title: "Initial point",
    });

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

//seedData();
initMap();