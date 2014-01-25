
function showComponentReferences(data) {
    if (data) {

        var arr = data.split(',');
        var toAppend = '<ul>';

        for (var i = 0, l = arr.length; i < l; i++) {

            var arr2 = arr[i].split('|');

            toAppend = toAppend + '<a href="' + arr2[1] + '">' + arr2[0] + "</a>";
        }

        var scChromeToolbar = document.getElementsByClassName('scChromeToolbar')[0];

        if (scChromeToolbar) {
            scChromeToolbar.insertAdjacentHTML('beforeend', toAppend + '</ul>');
        }
    }
}