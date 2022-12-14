$(document).ready(function () {
    var yNforms = document.getElementsByClassName("yesNoForm");
    for (var frm of yNforms) {
        frm.onclick = toggleYesNos;
        let yNchecks = frm.getElementsByTagName("input");
        for (let chk of yNchecks) {
            chk.onchange = handleCheckYesNo;
        }
    }


});

function handleCheckYesNo() {
    let assocToggleID = this.getAttribute("data-toggle");
    let assocToggle = document.getElementById(assocToggleID);
    let yesToggle = assocToggle.getElementsByClassName("yes")[0];
    let noToggle = assocToggle.getElementsByClassName("no")[0];
    yesToggle.classList.remove("selected");
    noToggle.classList.remove("selected");
    if (this.checked) {
        yesToggle.classList.add("selected");
    } else {
        noToggle.classList.add("selected");
    }
}

function toggleYesNos(evt) {
    evt = evt || window.event;
    var clickedItem = evt.target || evt.srcElement;
    var par = clickedItem.parentNode;
    if (clickedItem.tagName === "SPAN" && hasClass(par, "yesNoToggle")) {
        var grnPar = par.parentNode;
        var chk = grnPar.getElementsByTagName("input")[0];
        //alert(chk.outerHTML);
        for (var chld of par.children) {
            chld.classList.remove("selected");
        }
        clickedItem.classList.add("selected");
        if (hasClass(clickedItem, "yes")) {
            // check the checkbox
            chk.checked = true;
            chk.value = true;
            // if Admin, check all boxes on the page.
            if (chk.id === "IsAdmin") {
                checkAll(this, true);
            }
        } else if (hasClass(clickedItem, "no")) {
            // uncheck the checkbox
            chk.checked = false;
            chk.value = false;
            // if Admin, check all boxes on the page.
            if (chk.id === "IsAdmin") {
                checkAll(this, false);
            } else {
                let isAdminCheck = document.getElementById("IsAdmin");
                isAdminCheck.checked = false;
                isAdminCheck.value = false;
                isAdminCheck.onchange();
            }
        } else {
            // should not hit this.
        }
        chk.onchange();
    }
}

function checkAll(parentEle, isChecked) {
    let inpts = parentEle.getElementsByTagName("input");
    for (let inpt of inpts) {
        if (inpt.type === "checkbox") {
            inpt.checked = isChecked;
            inpt.value = isChecked;
            inpt.onchange();
        }
    }
}

function hasClass(ele, clName) {
    var found = false;
    for (var cN of ele.classList) {
        if (!found && cN === clName) {
            found = true;
        }
    }
    return found;
}

