/// <reference path="Site.js" />
/// <reference path="../Scripts/jquery-2.1.4.js" />

var upcomingRacesTablePanel = $("#upcomingRacesTablePanel");
var completedRacesTablePanel = $("#completedRacesTablePanel");

$(document).ready(function () {

    drawUpcomingRacesTable();
    drawCompletedRacesTable();
});

function drawUpcomingRacesTable() {
    upcomingRacesTablePanel.parent().submit();
}

function drawCompletedRacesTable() {
    completedRacesTablePanel.parent().submit();
}

function getUpcomingRacesSucceeded() {
    var upcomingRacesTable = upcomingRacesTablePanel.find("#upcomingRacesTable");
    upcomingRacesTable.find("td.utc-to-local").each(function () {
        $(this).html((new Date($(this).text())).toLocaleString());
    });
}

function getCompletedRacesSucceeded() {
    var completedRacesTable = completedRacesTablePanel.find("#completedRacesTable");
    
    completedRacesTable.find("td.utc-to-local").each(function () {
        $(this).html((new Date($(this).text())).toLocaleString());
    });
    
    completedRacesTable.dataTable({
        "bLengthChange": false,
        "pageLength": 25
    });
}