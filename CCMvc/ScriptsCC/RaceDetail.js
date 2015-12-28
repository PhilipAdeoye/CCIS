/// <reference path="Site.js" />
/// <reference path="../Scripts/jquery-2.1.4.js" />

var markAsCompletedModal = $("#markAsCompletedModal");
var cancelModal = $("#cancelModal");

var completeConfirmBtn = markAsCompletedModal.find(".btn-warning");
var cancelConfirmBtn = cancelModal.find(".btn-danger");

$(document).ready(function () {

    $("#markAsCompletedBtn").on("click", function () {
        var el = $(this);
        var raceId = el.data("raceId");
        var organizationId = el.data("organizationId");
        if (raceId && organizationId && completeConfirmBtn.length > 0) {
            completeConfirmBtn.data("raceId", raceId);
            completeConfirmBtn.data("organizationId", organizationId);

            markAsCompletedModal.modal("show");
        }

    });

    completeConfirmBtn.on("click", function () {
        var button = $(this);
        var url = button.data("url");
        $.post(url, {
            raceId: button.data("raceId"),
            organizationId: button.data("organizationId")
        })
        .done(function (data) {
            markAsCompletedModal.modal("hide");
            if (data.Errors) {
                showErrors("", data.Errors);
            }
            else {
                location.reload();
            }
        })
        .fail(function () {
            markAsCompletedModal.modal("hide");
            toastrRegularError("Something went wrong fulfilling your request", "Error!");
        });

    });

    $("#cancelBtn").on("click", function () {
        var el = $(this);
        var raceId = el.data("raceId");
        var organizationId = el.data("organizationId");
        if (raceId && organizationId && cancelConfirmBtn.length > 0) {
            cancelConfirmBtn.data("raceId", raceId);
            cancelConfirmBtn.data("organizationId", organizationId);

            cancelModal.modal("show");
        }
    });

    cancelConfirmBtn.on("click", function () {
        var button = $(this);
        var url = button.data("url");
        $.post(url, {
            raceId: button.data("raceId"),
            organizationId: button.data("organizationId")
        })
        .done(function (data) {
            cancelModal.modal("hide");
            if (data.Errors) {
                showErrors("", data.Errors);
            }
            else {
                toastrRegularInfo("The race has been cancelled", "Info");
                window.setTimeout(function () {
                    location.href = $("#cancelBtn").data("redirectUrl");
                }, 2000);
            }
        })
        .fail(function () {
            cancelModal.modal("hide");
            toastrRegularError("Something went wrong fulfilling your request", "Error!");
        });

    });
});

function editRaceSucceeded() {
    var errors = $("#raceEditErrorAlert");

    if (!errors.is(":visible")) {
        toastrRegularSuccess("Your changes have been saved", "Success!");
    }
}

function editRaceFailed() {
    toastrStickyError("Sorry, but we're unable to save your changes, please try again...", "Error!");
}