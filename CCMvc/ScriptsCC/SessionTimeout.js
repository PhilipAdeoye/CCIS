/// <reference path="Site.js" />
/// <reference path="../Scripts/toastr.js" />
/// <reference path="../Scripts/jquery-2.1.4.intellisense.js" />

// The session times out ever so often. The unchanged default is 20 mins. sessionTimeout is/becomes a
// timeout object that triggers a warning informing the user that they are about to be logged out. As soon
// as that happens, warningTimeout is initialized to a timeout object that triggers a window.location
// change to /Home/Logout

var sessionTimeout;
var warningTimeout;

$(function () {


    var setSessionTimeout = function () {

        if (sessionTimeout) {
            clearTimeout(sessionTimeout);
        }
        if (warningTimeout) {
            clearTimeout(warningTimeout);
        }

        sessionTimeout = setTimeout(function () {
            showTimeoutAlert();
            setWarningTimeout();
        }, 1140000); // 1140000 - 19 minutes


        function showTimeoutAlert() {

            toastr.options.timeOut = toastrTimeoutOptions.sticky;
            toastr.options.extendedTimeOut = toastrTimeoutOptions.sticky;
            var defaultPosition = toastr.options.positionClass;
            toastr.options.positionClass = "toast-top-full-width";

            toastr.warning(
                '<div>' +
                    '<button type="button" id="keepMeLoggedInBtn" class="btn btn-primary">Wait, wait! Keep me logged in</button>' +
                    '<a href="/Home/Logout" role="button" class="btn btn-default left-gutter" style="color:#666666">Oh! Okay. Log me out</a>' +
                '</div>',
                "You are about to be logged off");

            toastr.options.positionClass = defaultPosition;
            toastr.options.extendedTimeOut = 1000; // 1000 is the default
            document.title = "Your session is expiring";
        }

        function setWarningTimeout() {
            if (warningTimeout) {
                clearInterval(warningTimeout);
            }
            warningTimeout = setTimeout(function () {
                window.location = "/Home/Logout";
            }, 60000); // 60000 - 1 minute
        }
    };
    setSessionTimeout();
        
    $(document)
        .on("click", "#keepMeLoggedInBtn", function () {
            $.get("/Home/KeepMeLoggedIn", {})
                .done(function () { document.title = title; });
        })
        .ajaxStop(function () {            
            setSessionTimeout();
        });

});