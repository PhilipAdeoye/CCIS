/// <reference path="../Scripts/toastr.js" />
/// <reference path="../Scripts/jquery-2.1.4.intellisense.js" />

// DataTables default settings
$.extend($.fn.dataTable.defaults, {
    "columnDefs": [{
        "targets": "no-sort",
        "orderable": false
    }, {
        "targets": "no-search",
        "searchable": false
    }]
});

// Default options for the toastr plugin
toastr.options = {
    "closeButton": true
};
var toastrTimeoutOptions = {
    sticky: 0,
    regular: 5000
};

$(function () {

    // By default, when the close button on bootstrap alerts is clicked,
    // the alert is removed from the DOM and cannot be shown again.
    // This hides it instead so that it can be shown again

    $(document).on("click", "[data-hide]", function () {

        // Hide the closest parent with the class in $(this)'s 
        // data-hide attribute
        $(this).closest("." + $(this).attr("data-hide")).hide();
    });

    $.ajaxSetup({
        statusCode: {
            401: function () {
                window.setTimeout(function () {
                    window.location.href = '/';
                }, 2000)
            },
            500: function () {
                window.setTimeout(function () {
                    window.location.href = '/Error/Error';
                }, 2000)
            }
        }
    });

    var uiActivityIndicator = $("#uiActivityIndicator");
    var requestVerificationToken = $("[name=__RequestVerificationToken]").val();

    $(document)
        .ajaxStart(function () {
            uiActivityIndicator.show();
        })
        .ajaxStop(function () {
            uiActivityIndicator.fadeOut(200);
        })
        .ajaxSend(function (event, request, settings) {
            if (settings.type === "POST"
                && typeof requestVerificationToken != "undefined"
                && typeof settings.data != "object") { // exempt ajax calls using the FormData api

                // If the post request doesn't already have it
                if (settings.data.indexOf("__RequestVerificationToken") === -1) {
                    if (settings.data.length > 0) {
                        settings.data += "&__RequestVerificationToken=" + encodeURIComponent(requestVerificationToken);
                    }
                    else {
                        settings.data = "__RequestVerificationToken=" + encodeURIComponent(requestVerificationToken);
                    }
                }
            }
        });
});



// Function to encapsulate what seems to be the most common success use-case 
function toastrRegularSuccess(message, title) {
    toastr.options.timeOut = toastrTimeoutOptions.regular;
    toastr.clear();
    toastr.success(message, title);
}

// Function to encapsulate what seems to be the most common error use-case
function toastrStickyError(message, title) {
    toastr.options.timeOut = toastrTimeoutOptions.sticky;
    toastr.error(message, title);
}

function toastrRegularError(message, title) {
    toastr.options.timeOut = toastrTimeoutOptions.regular;
    toastr.error(message, title);
}

function toastrRegularInfo(message, title) {
    toastr.options.timeOut = toastrTimeoutOptions.regular;
    toastr.info(message, title);
}

function toastrRegularWarning(message, title) {
    toastr.options.timeOut = toastrTimeoutOptions.regular;
    toastr.warning(message, title);
}

function convertISODateTimeToUSFormatForDataTables(data, type, row, meta) {

    // Convert the date only for displaying
    if (data && type === "display") {
        var d = new Date(data);
        return (d.getMonth() + 1) + "/" + (d.getDate()) + "/" + (d.getFullYear());
    }

    return data;
}

function getFileExtension(filePath) {
    return filePath.substring(filePath.lastIndexOf(".")).toLowerCase();
}

function showErrors(elementId, errors, title) {

    var errorContainer = $("#" + elementId);
    var errorUL = errorContainer.length > 0 && errorContainer.children("ul");

    var errorString = "";
    errors.forEach(function (error) {
        errorString = errorString + "<li>" + error + "</li>";
    });

    if (errorUL.length > 0) {
        errorUL.empty();
        errorUL.append(errorString);

        if (title) {
            var titleEl = errorContainer.children("strong");
            if (titleEl.length > 0) {
                titleEl.html(title);
            }
        }

        errorContainer.show();
    }
    else {

        //resort to using toastr

        errorString = "<ul>" + errorString + "</ul>";

        toastr.options.timeOut = toastrTimeoutOptions.sticky;
        var defaultPosition = toastr.options.positionClass;
        toastr.options.positionClass = "toast-top-full-width";

        toastr.error(errorString, title ? title : "Please correct the following errors");

        toastr.options.positionClass = defaultPosition;
    }
}