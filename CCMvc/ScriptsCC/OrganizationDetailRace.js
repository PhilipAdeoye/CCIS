/// <reference path="Site.js" />
/// <reference path="../Scripts/jquery-2.1.4.js" />

var upcomingRacesTablePanel = $("#upcomingRacesTablePanel");
var completedRacesTablePanel = $("#completedRacesTablePanel");

$(document).ready(function () {

    var createModal = $("#raceCreateModal");

    $("#addNewRaceBtn").on("click", function () {
        var url = $(this).data("url");
        var organizationId = $(this).data("organizationId");
        $.get(url, {
            organizationId: organizationId
        }).done(function (data) {
            createModal.find(".modal-body #createRaceForm").html(data);
            createModal.modal("show");
        }).fail(function () {
            toastrRegularError("Sorry, Race creation encountered an error", "Error");
        });
    });

    createModal.on("click", ".modal-footer > .btn-primary", submitForm);

    drawUpcomingRacesTable();
    drawCompletedRacesTable();

    function submitForm() {
        var formId = $(this).data("formId");
        var form = $("#" + formId);
        if (form.length > 0) {
            form.submit();
        }
    }
});

function createRaceSucceeded() {
    var errors = $("#raceCreateErrorAlert");

    if (!errors.is(":visible")) {
        drawUpcomingRacesTable();

        toastrRegularSuccess("Race successfully added", "Success!");
        $("#raceCreateModal").modal("hide");
    }
}

function createRaceFailed() {
    toastrStickyError("Sorry, but the race could not be added, please try again...", "Error!");
}

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