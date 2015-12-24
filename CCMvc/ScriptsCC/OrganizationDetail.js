/// <reference path="Site.js" />
/// <reference path="../Scripts/jquery-2.1.4.js" />

var userTablePanel = $("#userTablePanel");
var editModal = $("#userEditModal");

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
    editModal.on("click", ".modal-footer > .btn-primary", submitForm);

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


    userTable.on("click", ".editUserBtn", function () {

        var url = $(this).data("url");
        var userId = $(this).data("id");
        var organizationId = $(this).data("organizationId");

        $.get(url, {
            userId: userId,
            organizationId: organizationId
        }).done(function (data) {
            editModal.find(".modal-body #editUserForm").html(data);
            editModal.modal("show");
        }).fail(function () {
            toastrRegularError("Sorry, User editing encountered an error", "Error");
        });

        return false;
    });

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

function editUserSucceeded() {
    var errors = $("#userEditErrorAlert");

    if (!errors.is(":visible")) {
        drawUserTable();
        toastrRegularSuccess("User successfully modified", "Success!");

        $("#userEditModal").modal("hide");
    }
}

function editUserFailed() {
    toastrStickyError("Sorry, but the user could not be edited, please try again...", "Error!");
}

function editOrganizationSucceeded() {
    var errors = $("#organizationEditErrorAlert");

    if (!errors.is(":visible")) {
        toastrRegularSuccess("Your changes have been saved", "Success!");
    }
}

function editOrganizationFailed() {
    toastrStickyError("Sorry, but we're unable to save your changes, please try again...", "Error!");
}