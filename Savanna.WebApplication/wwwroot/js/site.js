﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function updateGrid(grid) {
    const colons = $('[id*="col"]');
    let count = 0;
    colons.each(function () {
        const spanChild = $(this).children('span');
        let newText = grid[count];
        spanChild.text(newText);
        count++;
    });
}
function makeEmptyGrid(dimensions) {
    var gridContainer = document.getElementById("gridContainer");
    var gridHtml = "";

    for (var i = 0; i < dimensions; i++) {
        var rowHtml = '<div class="row">';

        for (var j = 0; j < dimensions; j++) {
            var colId = i * dimensions + j;
            var gridText = '\u00AD';

            var colHtml = '<div id="col' + colId + '" class="col box bg-secondary border d-flex justify-content-center">';
            colHtml += '<span id="gridCells" style="margin-top: 50%;">' + gridText + '</span>';
            colHtml += '</div>';

            rowHtml += colHtml;
        }

        rowHtml += '</div>';
        gridHtml += rowHtml;
    }

    gridContainer.innerHTML = gridHtml;
}
function updateGameInfo(gameInfo) {
    const gameInfoSpans = $('[id*="info"]');
    let count = 0;
    gameInfoSpans.each(function () {
        let newText = gameInfo[count] + ' |';
        if (count == gameInfo.length - 1) {
            newText = gameInfo[count];
        }
        $(this).text(newText);
        count++;
    });
}

// Ajax calls
function addAnimal(animalName) {
    console.log("animalName = ", animalName);
    $.ajax({
        url: "Index?handler=AddAnimal",
        method: "post",
        headers: {
            RequestVerificationToken:
                document.getElementById("RequestVerificationToken").value
        },
        data: {
            animalName: animalName
        },
        dataType: "json",
        success: function (data) {
            updateGrid(data.grid);
            updateGameInfo(data.gameInfo);
        },
        error: function (error) {
            console.error(error);
        }
    });
}
function startGame() {
    var dimensions = $("#gameDimensions").val();
    $.ajax({
        url: "Index?handler=StartGame",
        method: "post",
        headers: {
            RequestVerificationToken:
                document.getElementById("RequestVerificationToken").value
        },
        data: {
            dimensions: dimensions
        },
        success: function (data) {
            console.log(data);
            makeEmptyGrid(data);
        },
        error: function (error) {
            console.error(error);
        }
    });
}
function saveGame() {
    $.ajax({
        url: "Index?handler=SaveGame",
        method: "post",
        headers: {
            RequestVerificationToken:
                document.getElementById("RequestVerificationToken").value
        },
        success: function (data) {
            console.log(data);
        },
        error: function (error) {
            console.error(error);
        }
    });
}
function loadGame() {
    var gameId = $("#loadId").val();
    $.ajax({
        url: "Index?handler=LoadGame",
        method: "post",
        headers: {
            RequestVerificationToken:
                document.getElementById("RequestVerificationToken").value
        },
        data: {
            gameId: gameId
        },
        success: function (data) {
            console.log(data);
        },
        error: function (error) {
            console.error(error);
        }
    });
}
let isRequestInProgress = false;
function moveAnimals() {
    if (isRequestInProgress) {
        return;
    }
    isRequestInProgress = true;

    $.ajax({
        url: "Index?handler=MoveAnimals",
        method: "post",
        headers: {
            RequestVerificationToken:
                document.getElementById("RequestVerificationToken").value
        },
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            updateGrid(data.grid);
            updateGameInfo(data.gameInfo);;
        },
        error: function (error) {
            console.error(error);
        },
        complete: function () {
            isRequestInProgress = false;
            setTimeout(moveAnimals, 500);
        }
    });
}
moveAnimals();

// Helper
function getTime() {
    const currentDate = new Date();
    const hours = currentDate.getHours();
    const minutes = currentDate.getMinutes();
    const seconds = currentDate.getSeconds();

    const formattedHours = hours.toString().padStart(2, '0');
    const formattedMinutes = minutes.toString().padStart(2, '0');
    const formattedSeconds = seconds.toString().padStart(2, '0');

    const currentTime = `${formattedHours}:${formattedMinutes}:${formattedSeconds}`;
    console.log(currentTime);
}