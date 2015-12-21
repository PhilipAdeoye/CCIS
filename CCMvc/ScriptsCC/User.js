/// <reference path="Site.js" />
/// <reference path="../Scripts/jquery-2.1.4.js" />

var userTablePanel = $("#userTablePanel");

$(document).ready(function () {

    var createModal = $("#userCreateModal");

    $("#addNewUserBtn").on("click", function () {
        var url = $(this).data("url");
        var organizationId = $(this).data("organizationId");
        $.get(url, {
            organizationId: organizationId
        }).done(function (data) {
            createModal.find(".modal-body #createUserForm").html(data);
            createModal.modal("show");
        }).fail(function () {
            toastrRegularError("Sorry, User creation encountered an error", "Error");
        });
    });

    createModal.on("click", ".modal-footer > .btn-primary", submitForm);

    drawUserTable();

    function submitForm() {
        var formId = $(this).data("formId");
        var form = $("#" + formId);
        if (form.length > 0) {
            form.submit();
        }
    }

});

function drawUserTable() {
    userTablePanel.parent().submit();
}

function getUsersSucceeded() {

    var userTable = userTablePanel.find("#userTable");
    userTable.dataTable();


}

function createUserSucceeded() {
    var errors = $("#userCreateErrorAlert");

    if (!errors.is(":visible")) {
        drawUserTable();

        toastrRegularSuccess("User successfully added", "Success!");
        $("#userCreateModal").modal("hide");
    }
}

function createUserFailed() {
    toastrStickyError("Sorry, but the user could not be added, please try again...", "Error!");
}