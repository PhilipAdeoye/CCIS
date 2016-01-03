/// <reference path="../Scripts/jquery-2.1.4.js" />
/// <reference path="Site.js" />


var splitManageModal = $("#splitManageModal");

$(document).ready(function () {
    var manageRunnersForm = $("#manageRunnersForm");

    manageRunnersForm.on("click", ".runnerSplitBtn", function () {
        var btn = $(this);
        var runnerRaceRecordId = btn.data("runnerRaceRecordId");
        var url = btn.data("url");

        if (runnerRaceRecordId && url) {
            $.get(url, {
                runnerRaceRecordId: runnerRaceRecordId
            })
            .done(function (data) {
                splitManageModal.find(".modal-body").html(data);
                $(".elapsedTime").inputmask();
                splitManageModal.modal("show");
            })
            .fail(function () {
                toastrRegularError("Sorry, but something went wrong", "Error!");
            });
        }
    });

    splitManageModal.on("change", ".elapsedTime", recomputeSplits);

    splitManageModal.on("click", ".delete-icon", function () {
        var icon = $(this);
        icon.closest(".split-view").remove();
        recomputeSplits();
    });

    splitManageModal.on("click", "#addSplitBtn", function () {
        $("#extraSplitContainer .split-view").clone().appendTo("#splitsContainer");
        $(".elapsedTime").inputmask();
    });

    splitManageModal.on("click", ".modal-footer .btn-primary", function () {
        var btn = $(this);
        btn.prop("disabled", true);

        var input = $("#hiddenRunnerRaceRecordId");
        var url = input.data("url");
        var runnerRaceRecordId = input.val();
        if (url && runnerRaceRecordId) {

            var splits = [];
            $(".elapsedTime").not("#extraSplitContainer .elapsedTime").each(function () {
                var value = elapsedInputToSeconds($(this).val());
                if (value) {
                    splits.push(value);
                }
            });

            var frmManageSplits = document.getElementById("frmManageSplits");
            var formData = new FormData(frmManageSplits);
            for (var i = 0; i < splits.length; i++) {
                formData.append("Splits[" + i + "].ElapsedTimeInSeconds", splits[i]);
            }

            $.ajax({
                url: url,
                type: 'POST',
                dataType: 'json',
                data: formData,
                success: function (data) {
                    btn.prop("disabled", false);
                    if (data.Errors) {
                        showErrors("splitManageErrorAlert", data.Errors);
                    }
                    else {
                        toastrRegularSuccess("Your changes have been saved", "Success!");
                        splitManageModal.modal("hide");
                    }
                },
                error: function () {
                    btn.prop("disabled", false);
                    toastrRegularError("Your changes were not saved, and something went very wrong", "Error!");
                },
                contentType: false,
                processData: false
            });
        }
    });

    function recomputeSplits() {
        var elapsedInputs = $(".elapsedTime").not("#extraSplitContainer .elapsedTime");
        var intervalDisplays = $(".intervalTime").not("#extraSplitContainer .intervalTime");
        var interval;

        for (var i = 0; i < elapsedInputs.length; i++) {
            var input = $(elapsedInputs[i]);
            var display = $(intervalDisplays[i]);

            if (i === 0) {
                display.html(secondsToDisplayFormat(elapsedInputToSeconds(input.val())));
            }
            else {
                interval = elapsedInputToSeconds(input.val()) - elapsedInputToSeconds($(elapsedInputs[i - 1]).val());
                display.html(secondsToDisplayFormat(interval));
                if (interval < 0) {
                    display.addClass("bg-danger");
                }
                else {
                    display.removeClass("bg-danger");
                }
            }
        }
        function secondsToDisplayFormat(seconds) {
            var minutes = "" + Math.floor(seconds / 60);
            var seconds = "" + seconds % 60;
            if (minutes.length === 1) {
                minutes = "0" + minutes;
            }
            if (seconds.length === 1) {
                seconds = "0" + seconds;
            }
            return minutes + ":" + seconds;
        }
    }

    function elapsedInputToSeconds(elapsedTime) {
        if (!elapsedTime) {
            return 0;
        }

        var minutes = parseInt(elapsedTime.substring(0, 2), 10);
        var seconds = parseInt(elapsedTime.substring(3, 5), 10);

        return (minutes * 60) + seconds;
    }
});