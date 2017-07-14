var txtSearchedWord = '#txtSearchedWord';
var Cell_Mark = ".Cell_Mark";
var cellId = "#Cell_";
var classMarkCell = "Cell_Mark";
var MarkCells=[];
var charsWord = [];

$(document).ready(function () {

    $(txtSearchedWord).on('input', function(e) {
        var word = $(txtSearchedWord).val();
        CleanMarks();
        charsWord = [];
        MarkCells = [];
        var arraytemp = $(txtSearchedWord).val().toUpperCase().split('');
        for (var i = 0; i < arraytemp.length; i++) {
            charsWord.push(arraytemp[i]);
            Search();
        }
        //charsWord = $(txtSearchedWord).val().toUpperCase().split('');
        //Search();
        MarkAllCells();
    });
});

function Search() {
    
    if (charsWord.length==1) {
        for (var y = 0; y < window.symbolString.length; y++) {
            for (var x = 0; x < window.symbolString[y].length; x++) {
                if (window.symbolString[y][x] == charsWord[charsWord.length - 1]) {
                    MarkCells.push([
                    {
                        letter: window.symbolString[y][x],
                        cellNumber:(y * window.symbolString[y].length)+ x+1,
                        coordinateY: y,
                        coordinateX: x
                    }]);
                }
            }
        }
    } else if (charsWord.length >= 1) {

        var originalSize = window.MarkCells.length;
        for (var t = 0; t < originalSize; t++)
        {
            var addedCell = CheckNearCells(MarkCells[t][MarkCells[t].length - 1].coordinateY, MarkCells[t][MarkCells[t].length - 1].coordinateX, t);
            if (!addedCell) {
                t--;
                originalSize--;
            }
        }
    }
    
}

function CheckNearCells(coordinateY, coordinateX, markCellId) {
    var cellFound = false;
    for (var ry = coordinateY - 1; ry <= coordinateY + 1; ry++) {
        for (var rx = coordinateX - 1; rx <= coordinateX + 1; rx++) {
            if (ry >= 0 && ry < window.symbolString.length && rx >= 0 && rx < window.symbolString[ry].length) {

                var newMarkCell = {
                    letter: window.symbolString[ry][rx],
                    cellNumber: (ry * window.symbolString[ry].length) + rx + 1,
                    coordinateY: ry,
                    coordinateX: rx
                }
                var cellExist = CheckIfCellWasAdded(markCellId, newMarkCell);
                var charIsEqual = window.symbolString[ry][rx] == charsWord[charsWord.length - 1];
                if (charIsEqual && (!cellFound) && (!cellExist)) {
                    MarkCells[markCellId].push(newMarkCell);
                    cellFound = true;
                } else if (charIsEqual && (!cellExist)) {
                    var temp = MarkCells[markCellId].slice();
                    temp[temp.length - 1] = newMarkCell;
                    MarkCells.push(temp);
                }
            }
        }
    }
    if (!cellFound) {
        MarkCells.splice(markCellId, 1);
    }
    return cellFound;
}

function CheckIfCellWasAdded(markCellId,newMarkCell) {
    
    for (var x = 0; x < MarkCells[markCellId].length; x++) {
        var equalY = MarkCells[markCellId][x].coordinateY == newMarkCell.coordinateY;
        var equalX = MarkCells[markCellId][x].coordinateX == newMarkCell.coordinateX;
        if (equalY && equalX) {
            return true;
        }
    }
    return false;

}

function CleanMarks() {
    $(Cell_Mark).each(function (index) {
            $(this).attr("class", "");
        }
        );
}

function MarkAllCells() {
    for (var y = 0; y < window.MarkCells.length; y++) {
        for (var x = 0; x < window.MarkCells[y].length; x++) {
            $(cellId + (MarkCells[y][x].cellNumber)).attr("class", classMarkCell);
        }
    }
}