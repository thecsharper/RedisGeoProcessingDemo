﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

let map;
let markersArray = [];

const position = { lat: 51.454514, lng: -2.58 };

const contentString =
    '<div id="content">' +
    '<div id="siteNotice">' +
    "</div>" +
    '<h1 id="firstHeading" class="firstHeading">Uluru</h1>' +
    '<div id="bodyContent">' +
    "<p><b>Uluru</b>, also referred to as <b>Ayers Rock</b>, is a large " +
    "sandstone rock formation in the southern part of the " +
    "Northern Territory, central Australia. It lies 335&#160;km (208&#160;mi) " +
    "south west of the nearest large town, Alice Springs; 450&#160;km " +
    "(280&#160;mi) by road. Kata Tjuta and Uluru are the two major " +
    "features of the Uluru - Kata Tjuta National Park. Uluru is " +
    "sacred to the Pitjantjatjara and Yankunytjatjara, the " +
    "Aboriginal people of the area. It has many springs, waterholes, " +
    "rock caves and ancient paintings. Uluru is listed as a World " +
    "Heritage Site.</p>" +
    '<p>Attribution: Uluru, <a href="https://en.wikipedia.org/w/index.php?title=Uluru&oldid=297882194">' +
    "https://en.wikipedia.org/w/index.php?title=Uluru</a> " +
    "(last visited June 22, 2009).</p>" +
    "</div>" +
    "</div>";

async function initMap() {

    //@ts-ignore
    const { Map } = await google.maps.importLibrary("maps");

    map = new Map(document.getElementById("map"), {
        zoom: 7,
        center: position,
        mapId: "Demo_Map",
    });

    map.addListener("click", (e) => {
        placeMarkerAndPanTo(e.latLng);
        getMarkers(e.latLng.lat, e.latLng.lng);
    });

    new google.maps.Marker({
        position: position,
        map,
        title: "Initial point",
    });
};

initMap();

function selectCountry(val) {
    $("#search-box").val(val);
    $("#suggestion-box").hide();
}

function selectMap(lat, lng) {
    let latlng = new google.maps.LatLng(lng, lat);

    let marker = new google.maps.Marker({
        position: latlng,
        map: map,
    });

    map.panTo(latlng);
    markersArray.push(marker);
    placeMarkerAndPanTo(latlng);
    getMarkers(lng, lat);
    $("#search-box").val("");
}

function placeMarkerAndPanTo(latLng) {
    let marker = new google.maps.Marker({
        position: latLng,
        map: map,
    });

    map.panTo(latLng);
    markersArray.push(marker);
}

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
                    position: new google.maps.LatLng(val.lng, val.lat),
                    title: String("Distance to selected: " + val.distance),
                    label: String("Distance to selected: " + val.distance)
                });

                const infowindow = new google.maps.InfoWindow({
                    content: val.city,
                    ariaLabel: "Location",
                });

                marker.addListener("click", () => {
                    infowindow.open({
                        anchor: marker,
                        map,
                    });
                });

                markersArray.push(marker);
                marker.setMap(map);
            });
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
