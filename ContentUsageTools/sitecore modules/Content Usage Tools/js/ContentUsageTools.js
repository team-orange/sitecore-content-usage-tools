
function showComponentReferences(data) {
    if (data) {

        removePanel(); // Make sure we start clean

        var arr = data.split(',');
        var toAppend = '<div id="divReferences" class="scChromeControls"><ul id="ulReferences">';

        for (var i = 0, l = arr.length; i < l; i++) {

            var arr2 = arr[i].split('|');

            toAppend = toAppend + '<li><a href="' + arr2[1] + '">' + arr2[0] + "</a></li>";
        }

        var scChromeToolbar = document.getElementsByClassName('scChromeToolbar')[1];

        if (scChromeToolbar) {

            window.onclick = removePanel;
            scChromeToolbar.insertAdjacentHTML('beforeend', toAppend + '</ul></div>');
        }
    }
}

function removePanel() {

    var references = document.getElementById('divReferences');

    if (references && references.parentNode) {
        references.parentNode.removeChild(references);
    }
}