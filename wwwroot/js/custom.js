// to get current year
function getYear() {
    var currentDate = new Date();
    var currentYear = currentDate.getFullYear();
    document.querySelector("#displayYear").innerHTML = currentYear;
}

getYear();

/** google_map js **/
function myMap() {
    var mapOptions = {
        center: { lat: 40.40204201502193, lng: 49.95351858465297 },
        zoom: 17,
    };

    var map = new google.maps.Map(document.getElementById('googleMap'), mapOptions);
}

//function initMap() {
    
//}
//$(".client_owl-carousel").owlCarousel({
//    loop: true,
//    margin: 0,
//    dots: false,
//    nav: true,
//    navText: [],
//    autoplay: true,
//    autoplayHoverPause: true,
//    navText: [
//        '<i class="fa fa-angle-left" aria-hidden="true"></i>',
//        '<i class="fa fa-angle-right" aria-hidden="true"></i>'
//    ],
//    responsive: {
//        0: {
//            items: 1
//        },
//        768: {
//            items: 2
//        },
//        1000: {
//            items: 2
//        }
//    }
//});