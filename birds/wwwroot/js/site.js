// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function swapNavBar(layout) {
    var b = document.getElementById("navbarBirds");
    var s = document.getElementById("navbarStudies");
    if (b.style.display === "none") {
        b.style.display = "block";
        s.style.display = "none";
    }
    else if (layout == "Birds") {
        b.style.display = "none";
        s.style.display = "block";
    }
    else if (layout == "Studies") {
        b.style.display = "block";
        s.style.display = "none";
    }
    else {
        b.style.display = "none";
        s.style.display = "block";
    }
}