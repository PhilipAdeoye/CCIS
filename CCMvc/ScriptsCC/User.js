/// <reference path="Site.js" />
/// <reference path="../Scripts/jquery-2.1.4.js" />

var userTablePanel = $("#userTablePanel");

$(document).ready(function () {


    drawUserTable();

});

function drawUserTable() {
    userTablePanel.parent().submit();
}

function getUsersSucceeded() {

    var userTable = userTablePanel.find("#userTable");
    userTable.dataTable();

    
}