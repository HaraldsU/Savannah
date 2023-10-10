// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function upgradeGrid(grid) {
    console.log("Started upgradeGrid method");
    console.log("gridNotParsed = ", grid);
    try {
        // console.log("gridArray =", gridArray);
        const colons = $('[id*="col"]');
        let count = 0;
        // console.log(colons);

        colons.each(function () {
            const spanChild = $(this).children('span');
            const currentAnimal = grid[count].animal;
            // console.log("count =", count);
            // console.log("currentAnimal =", currentAnimal);
            let newText = '\u00AD'; // Use let to reassign newText

            if (currentAnimal && currentAnimal.prey) {
                newText = currentAnimal.prey.firstLetter; // Use lowercase for object properties
                // console.log("Added a Prey!");
            }
            else if (currentAnimal && currentAnimal.predator) {
                newText = currentAnimal.predator.firstLetter; // Use lowercase for object properties
                // console.log("Added a Predator!");
            }
            else {
                // console.log("Added nothing!");
            }
            spanChild.text(newText);
            count++;
        });
    } catch (error) {
        console.error("Error parsing JSON:", error);
    }
}

function addAnimal(animalName) {
    // const token = $('[name="__RequestVerificationToken"]').val();
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
            console.log(data);
            upgradeGrid(data);
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
            // console.log(data);
            console.log("moveAnimals Successfull !!!");
            upgradeGrid(data);
        },
        error: function (error) {
            console.log("moveAnimals Error !!!");
            console.error(error);
        },
        complete: function () {
            console.log("moveAnimals Request Complete.");
            isRequestInProgress = false;
            setTimeout(moveAnimals, 500);
        }
    });
}
moveAnimals();

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