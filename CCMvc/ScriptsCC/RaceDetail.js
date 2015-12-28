/// <reference path="Site.js" />
/// <reference path="../Scripts/jquery-2.1.4.js" />

function editRaceSucceeded() {
    var errors = $("#raceEditErrorAlert");

    if (!errors.is(":visible")) {
        toastrRegularSuccess("Your changes have been saved", "Success!");
    }
}

function editRaceFailed() {
    toastrStickyError("Sorry, but we're unable to save your changes, please try again...", "Error!");
}