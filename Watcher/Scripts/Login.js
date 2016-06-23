function enviarServidor(url, metodo) {
    var xhr = new XMLHttpRequest();
    xhr.open("get", url);
    //alert(url);
    xhr.setRequestHeader("xhr", "OSC");
    xhr.onreadystatechange = function () {
        if (xhr.status == 200 && xhr.readyState == 4) {
            //alert(xhr.responseText);
            metodo(xhr.responseText);
        }
    }
    xhr.send();

}

function enter(event) {
    var keyCode = ('which' in event) ? event.which : event.keyCode;
    if (keyCode == 13) return true;
    return false;
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}

function fadeIn(elem, ms) {
    if (!elem) return;
    elem.style.opacity = 0;
    elem.style.filter = "alpha(opacity=0)";
    elem.style.display = "inline-block";
    elem.style.visibility = "visible";
    if (ms) {
        var opacity = 0;
        var timer = setInterval(function () {
            opacity += 50 / ms;
            if (opacity >= 1) {
                clearInterval(timer);
                opacity = 1;
            }
            elem.style.opacity = opacity;
            elem.style.filter = "alpha(opacity=" + opacity * 100 + ")";
        }, 50);
    }
    else {
        elem.style.opacity = 1;
        elem.style.filter = "alpha(opacity=1)";
    }
}

function fadeOut(elem, ms) {
    if (!elem) return;
    if (ms) {
        var opacity = 1;
        var timer = setInterval(function () {
            opacity -= 50 / ms;
            if (opacity <= 0) {
                clearInterval(timer);
                opacity = 0;
                elem.style.display = "none";
                elem.style.visibility = "hidden";
            }
            elem.style.opacity = opacity;
            elem.style.filter = "alpha(opacity=" + opacity * 100 + ")";
        }, 50);
    }
    else {
        elem.style.opacity = 0;
        elem.style.filter = "alpha(opacity=0)";
        elem.style.display = "none";
        elem.style.visibility = "hidden";
    }
}

function AccesoRapido() {
    document.getElementById("txtUsername").onkeyup = function (event) {
        if (enter(event)) document.getElementById("btnLogin").click();
    }
    document.getElementById("txtPassword").onkeyup = function (event) {
        if (enter(event)) document.getElementById("btnLogin").click();
    }
    document.getElementById("txtDstb").onkeyup = function (event) {
        if (enter(event)) document.getElementById("btnLogin").click();
    }
}

function Iniciar() {
    var user = document.getElementById("txtUsername");
    var pass = document.getElementById("txtPassword");
    var dstb = document.getElementById("txtDstb");
    if (user.value == "") user.focus();
    else if (pass.value == "") pass.focus();
    else dstb.focus();
}

window.onload = function () {
    if (getCookie("Chk")!="") {
        document.getElementById("txtUsername").value = getCookie("Usuario");
        document.getElementById("chkRemember").checked = true;
    }
    var ca = document.cookie.split(';');
    var now = new Date();
    now.setMonth(now.getMonth() - 1);
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i].split("=");
        document.cookie = c[0] + "=; expires=" + now.toUTCString()+";path=/";
    }
    AccesoRapido();
    Iniciar();
    document.getElementById("btnLogin").addEventListener("click", EventoLogin);
}

function EventoLogin() {
    var valid = true;
    var errormsg = "Campo es obligatorio!";
    var errorcn = "error";
    var camposError = document.getElementsByClassName(errorcn);
    for (var i = 0; i < camposError.length; i++) {
        fadeOut(camposError[i], 100);
        camposError[i].parentNode.removeChild(camposError[i]);
        i--;
    }
    var campos = document.getElementsByClassName("required");
    var padre = null;
    for (var j = 0; j < campos.length; j++) {
        if (campos[j].value == "") {
            padre = campos[j].parentNode;
            padre.innerHTML += "<span class='" + errorcn + "'>" + errormsg + "</span>";
            fadeIn(padre.lastChild, 100);
            valid = false;
            padre = null;
        }
    }
    if (valid) {
        enviarServidor("../Login/ValidarAcceso?usuario=" + campos[0].value + "&pass=" + campos[1].value + "&dstb=" + campos[2].value + "&rmbr=" + (document.getElementById("chkRemember").checked ? "1" : "0") + "&t=" + Math.random(), Revision);
    }
    else {
        AccesoRapido();
        Iniciar();
        return valid;
    }
}

function Revision(rpta) {
    if (rpta != "" && rpta != "0") {

        var rptapartido = rpta.split('⌂');//ascii 127

        //alert('uno '+ rptapartido[0]);
        //alert('dos '+rptapartido[1]);

        sessionStorage.DstbAbnd = rptapartido[0];
        //sessionStorage.DstbAbnd = rpta;
        sessionStorage.Local_subscriber = rptapartido[1];
        
        document.getElementById("btnLogin").removeEventListener("click",EventoLogin);
        document.getElementById("divError").innerHTML = "Ingresando...";
        location.href = "../Modulo/Visor";
    }
    else {
        if (rpta == "0") document.getElementById("divError").innerHTML = "Usuario/Password incorrecto";
        document.getElementById("txtPassword").focus();
    }
}
