/* Copyright (C) 2014 Dražen Janjiček, Johann Baziret, Robin Hermanussen

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program. If not, see http://www.gnu.org/licenses/. */

var contentUsageToolsComponentNumber = 1;
var contentUsageToolsCurrentId = '';

function checkComponentUpdate() {

    this.contentUsageToolsComponentNumber++;

    var scChromeToolbar = document.getElementsByClassName('scChromeToolbar');

    if (scChromeToolbar) {

        // skip the first, as it's not the kind of scChromeToolbar we're looking for
        for (var j = 1; j < scChromeToolbar.length; j++) {

            var toolBarId = this.determineToolbarId(scChromeToolbar[j]);

            if (toolBarId != null) {

                if (toolBarId !== this.contentUsageToolsCurrentId) {
                    this.contentUsageToolsCurrentId = toolBarId;
                    Sitecore.PageModes.PageEditor.postRequest('orange:showreferences(id=' + toolBarId + ')', null, false);
                }
            } else {
                this.removePanels();
            }
        }
    }
}

// use polling to determine if the 'show references' window should be displayed.
setInterval(this.checkComponentUpdate, 500);

// Render an overview of all pages that reference the given datasource item.
function showComponentReferences(datasourceItemId, data) {

    this.removePanels(); // Make sure we start clean

    var arr = data.split(',');

    this.contentUsageToolsComponentNumber++;

    var compId = 'cut_comp_' + this.contentUsageToolsComponentNumber;
    var count = (arr.length > 0 && arr[0] !== '') ? arr.length : 0;
    var toAppend = '<div id="' + compId + '" class="divReferences" onblur="removePanels();"><span class=\"align-normal\">Content usage (' + count + ')</span><span class=\"align-right\"><a class=\"closePanel\" href=\"#\" onclick=\"removePanels();\">x</a></span><hr /><ul class="ulReferences">';

    if (arr.length > 0 && arr[0] !== '') {

        for (var i = 0, l = arr.length; i < l; i++) {

            var arr2 = arr[i].split('|');
            toAppend = toAppend + '<li><a href="' + arr2[1] + '">' + arr2[0] + "</a></li>";
        }
    } else {

        toAppend = toAppend + '<li><i>- No other pages use this content -</i></li>';
    }

    toAppend = toAppend + '</ul><br /></div>';

    var scChromeToolbar = document.getElementsByClassName('scChromeToolbar');

    if (scChromeToolbar) {

        // skip the first, as it's not the kind of scChromeToolbar we're looking for
        for (var j = 1; j < scChromeToolbar.length; j++) {

            var toolBarId = this.determineToolbarId(scChromeToolbar[j]);

            if (toolBarId != null && toolBarId === datasourceItemId) {

                window.onclick = this.removePanels;
                scChromeToolbar[j].insertAdjacentHTML('beforeend', toAppend);

                var addedComp = document.getElementById(compId);

                addedComp.style.position = 'fixed';
                addedComp.style.bottom = '0px';
                addedComp.focus();

                break;
            }
        }
    }
}

// Selected Chrome has changed, clean up all panels.
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

// Receives the ID of the given element.
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
