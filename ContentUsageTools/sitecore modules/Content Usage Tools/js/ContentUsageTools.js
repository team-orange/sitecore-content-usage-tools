
var contentUsageToolsComponentNumber = 1;
var contentUsageToolsCurrentId = '';

function checkComponentUpdate() {
    contentUsageToolsComponentNumber++;

    var scChromeToolbar = document.getElementsByClassName('scChromeToolbar');

    if (scChromeToolbar) {
        // skip the first, as it's not the kind of scChromeToolbar we're looking for
        for (var j = 1; j < scChromeToolbar.length; j++) {
            var toolBarId = determineToolbarId(scChromeToolbar[j]);
            if (toolBarId != null) {
                if (toolBarId !== contentUsageToolsCurrentId) {
                    contentUsageToolsCurrentId = toolBarId;
                    Sitecore.PageModes.PageEditor.postRequest('orange:showreferences(id=' + toolBarId + ')', null, false);
                }
            } else {
                removePanels();
            }
        }
    }
}

// use polling to determine if the 'show references' window should be displayed.
setInterval(checkComponentUpdate, 500);

function showComponentReferences(datasourceItemId, data) {
    removePanels(); // Make sure we start clean

    var arr = data.split(',');
    
    contentUsageToolsComponentNumber++;
    var compId = 'cut_comp_' + contentUsageToolsComponentNumber;
    var toAppend = '<div id="' + compId + '" class="scChromeControls divReferences" onblur="removePanels();"><ul class="ulReferences">';

    if (arr.length > 0 && arr[0] !== '') {
        for (var i = 0, l = arr.length; i < l; i++) {

            var arr2 = arr[i].split('|');

            toAppend = toAppend + '<li><a href="' + arr2[1] + '">' + arr2[0] + "</a></li>";
        }
    } else {
        toAppend = toAppend + '<li><i>- No other pages use this content -</i></li>';
    }
    toAppend = toAppend + '</ul><br /><a href=\"#\" onclick=\"removePanels();\">Close</a></div>';

    var scChromeToolbar = document.getElementsByClassName('scChromeToolbar');

    if (scChromeToolbar) {
        // skip the first, as it's not the kind of scChromeToolbar we're looking for
        for (var j = 1; j < scChromeToolbar.length; j++) {
            var toolBarId = determineToolbarId(scChromeToolbar[j]);
            if (toolBarId != null && toolBarId === datasourceItemId) {
                window.onclick = removePanels;
                scChromeToolbar[j].insertAdjacentHTML('beforeend', toAppend);
                var addedComp = document.getElementById(compId);
                addedComp.style.left = (document.body.clientWidth - addedComp.clientWidth) + 'px';
                addedComp.style.top = (document.body.clientHeight - addedComp.clientHeight) + 'px';
                addedComp.focus();
                break;
            }
        }
    }
}

function removePanels() {

    var references = document.getElementsByClassName('divReferences');
    var deletes = new Array();
    for (var i = 0; i < references.length; i++) {
        if (references[i] && references[i].parentNode) {
            deletes.push(references[i]);
        }
    }
    for (var j = 0; j < deletes.length; j++) {
        deletes[j].parentNode.removeChild(deletes[j]);
    }
}

function determineToolbarId(toolbarElem) {
    var chromeCommands = toolbarElem.querySelectorAll(".scChromeCommand");
    for (var i = 0; i < chromeCommands.length; i++) {
        var onclickAttr = chromeCommands[i].getAttribute('onclick');
        if (onclickAttr != null && onclickAttr.toString().indexOf('webedit:setdatasource') >= 0) {
            var toolbarId = onclickAttr.toString().substr(onclickAttr.toString().indexOf('id={') + 3);
            toolbarId = toolbarId.substr(0, toolbarId.indexOf('}') + 1);
            return toolbarId;
        }
    }
    return null;
}
