/// <reference path="../Scripts/jquery-2.1.4.js" />
/// <reference path="Site.js" />

$(document).ready(function () {
    $("#selectAllRunnersBtn").on("click", function () {
        $("input[type='checkbox'].runnerIsEnrolledCB").each(function () {
            $(this).prop("checked", true);
        });
    });

    $("#unselectAllRunnersBtn").on("click", function () {
        $("input[type='checkbox'].runnerIsEnrolledCB").each(function () {
            $(this).prop("checked", false);
        });
    });
});

function manageRunnersSucceeded() {
    var errors = $("#manageRunnersErrorAlert");

    if (!errors.is(":visible")) {
        toastrRegularSuccess("Your changes have been saved", "Success!");

        var editRaceForm = $("#editRaceForm");
        $.get(editRaceForm.data("editUrl"), {
            raceId: editRaceForm.data("raceId"),
            organizationId: editRaceForm.data("organizationId")
        })
        .done(function (data) {
            editRaceForm.html(data);
        });      
    }
}

function manageRunnersFailed() {
    toastrRegularError("Sorry, but your changes were not saved", "Error!");
}