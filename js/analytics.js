function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function recordVisit(page) {
    $.ajax({
        url: '/pages/ajax/analytics.aspx?page=' + page,
        dataType: 'html',
        success: function(html) {
            console.log('Analytics Connected.' + html);
        },
        error: function(xhr) {
            console.log('Analytics Error: ' + xhr.status);
        },
        complete: function() {
            console.log('Analytics Done!');
        },
        beforeSend: function() {
            console.log('Analytics Connecting...');
        }
    });
}

var page = window.location.href;
var time = new Date().getTime();
if (getCookie(page)) {
    var def = time - Number(getCookie(page));
    if (def > 60000) {
        setCookie(page, '', -1);
        setCookie(page, time, 1);
        recordVisit(page);
    }
} else {
    setCookie(page, time, 1);
    recordVisit(page);
}