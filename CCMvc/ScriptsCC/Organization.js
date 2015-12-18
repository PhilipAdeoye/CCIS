/// <reference path="Site.js" />
/// <reference path="../Scripts/jquery-2.1.4.intellisense.js" />

var organizationTablePanel = $("#organizationTablePanel");
$(document).ready(function () {

    var createModal = $("#organizationCreateModal");

    $("#addNewOrganizationBtn").on("click", function () {
        var url = $(this).data("url");

        $.get(url, {
        }).done(function (data) {
            createModal.find(".modal-body #createOrganizationForm").html(data);
            createModal.modal("show");
        }).fail(function () {
            toastrRegularError("Sorry, School creation encountered an error", "Error");
        });

    });

    createModal.on("click", ".modal-footer > .btn-primary", submitForm);

    drawOrganizationTable();

    function submitForm() {
        var formId = $(this).data("formId");
        var form = $("#" + formId);
        if (form.length > 0) {
            form.submit();
        }
    }
});

function drawOrganizationTable() {
    organizationTablePanel.parent().submit();
}

function getOrganizationsSucceeded() {
    var organizationTable = $("#organizationTable");
    organizationTable.dataTable();
}

function createOrganizationSucceeded() {
    var errors = $("#organizationErrorAlert");

    if (!errors.is(":visible")) {
        drawOrganizationTable();

        toastrRegularSuccess("The school was successfully added", "Success");

        $("#organizationCreateModal").modal("hide");
    }
}

function createOrganizationFailed() {
    toastrStickyError("The school could not be created, try again...", "Error!");
}