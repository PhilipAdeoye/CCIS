/// <reference path="Site.js" />
/// <reference path="../Scripts/toastr.js" />
/// <reference path="../Scripts/jquery-2.1.4.intellisense.js" />

// The session times out ever so often. The unchanged default is 20 mins. sessionTimeout is/becomes a
// timeout object refreshes the user's session when it's about to expire

var sessionTimeout;

$(function () {
    var setSessionTimeout = function () {

        if (sessionTimeout) {
            clearTimeout(sessionTimeout);
        }

        sessionTimeout = setTimeout(refreshSession, 1140000); // 1140000 - 19 minutes

        function refreshSession() {
            $.get("/Home/KeepMeLoggedIn", {});
        }
    };
    setSessionTimeout();

    $(document)
        .ajaxStop(function () {
            setSessionTimeout();
        });

});