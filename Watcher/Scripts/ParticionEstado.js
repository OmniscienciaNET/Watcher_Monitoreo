var registrosPagina = 25, paginasBloque = 10, indiceActualBloque = 0, indiceActualPagina, matriz = [], timer = null, timerSenal = null;
var salir = 1;
window.addEventListener("load", function () {
   // RestriccionesCombo();
    BuscarAutomatico();
    //BuscarAbonadoDistribuidor();
    BuscarLocalDesc();
   // ColocarFiltros();
    ConfigurarExit();
    BuscarParticionEstado();
    LimpiarBusqueda();
    timerSenal = setInterval(SenalPendiente, 10000);
    //ComboLocal_Subscriber();
    ComboTipoParticion();
    ComboTipo_Abonado();
});

window.addEventListener("unload", function () {
    if (salir > 0) enviarServidor("../Modulo/LogOut", Exit, false);
});

function getCookie(cname) {
    var name = "V" + cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}

function setCookie(nombre, valor) {
    var now = new Date();
    now.setMonth(now.getMonth() + 1);
    document.cookie = "V" + nombre + "=" + valor + ";expires=" + now.toUTCString() + ";path=/";
}

function enviarServidor(url, metodo, estado) {
    estado = estado == undefined ? true : estado;
    var xhr = new XMLHttpRequest();
    xhr.open("GET", url, estado);
    xhr.setRequestHeader("xhr", "OSC");
    xhr.onreadystatechange = function () {
        if (xhr.status == 200 && xhr.readyState == 4) {
            metodo(xhr.responseText);
        }
    }
    xhr.send();
}

function RestriccionesCombo() {
    document.getElementById("ddlInOutSide").onchange = function () { if (document.getElementById("ddlDpto").value == "00" && this.value == "≠") this.value = "="; setCookie("InOutSide", this.value); }
    document.getElementById("ddlDpto").onchange = function () { if (document.getElementById("ddlInOutSide").value == "≠" && this.value == "00") document.getElementById("ddlInOutSide").value = "="; setCookie("Dpto", this.value); }
    document.getElementById("ddlTipoAbonado").onchange = function () { setCookie("TipoAbonado", this.value); }
}

function BuscarAutomatico() {
    var btnPlay = document.getElementById("btnPlay");
    var btnStop = document.getElementById("btnStop");
    btnPlay.onclick = function () {
        var imgPlay = document.getElementById("imgbtnPlay")
        if (imgPlay.getAttribute("src").indexOf("_d.png") == -1) {
            var imgStop = document.getElementById("imgbtnStop");
            imgPlay.setAttribute("src", imgPlay.getAttribute("src").replace('.png', '_d.png'));
            imgStop.setAttribute("src", imgStop.getAttribute("src").replace('_d', ''));
            btnPlay.setAttribute("disabled", "disabled");
            btnStop.removeAttribute("disabled");
        }
        timer = setInterval(function () { document.getElementById("btnSearch").click(); }, 10000);
    }
    btnStop.onclick = function () {
        var imgStop = document.getElementById("imgbtnStop");
        if (imgStop.getAttribute("src").indexOf("_d.png") == -1) {
            var imgPlay = document.getElementById("imgbtnPlay")
            imgStop.setAttribute("src", imgStop.getAttribute("src").replace('.png', '_d.png'));
            imgPlay.setAttribute("src", imgPlay.getAttribute("src").replace('_d', ''));
            btnStop.setAttribute("disabled", "disabled");
            btnPlay.removeAttribute("disabled");
        }
        clearInterval(timer);
    }
}

function BuscarAbonadoDistribuidor() {
    var codAbnd = document.getElementById("txtAbndCod");
    var codDstb = document.getElementById("txtDstbCod");
    codAbnd.onblur = function () {
        var Desc = document.getElementById("txtAbndDesc");
        if (this.value.trim() == "") Desc.value = "";
        else {
            Desc.value = "";
            var exito = false;
            var nRegistros = matrizBusq.length;
            for (var i = 0; i < nRegistros; i++) {
                if ((codDstb.value.trim() == "" && matrizBusq[i][3].toLowerCase() == this.value.toLowerCase()) || (codDstb.value.trim().toLowerCase() == matrizBusq[i][0].toLowerCase() && matrizBusq[i][3].toLowerCase() == this.value.toLowerCase())) {
                    Desc.value = matrizBusq[i][4];
                    exito = true;
                    break;
                }
            }
            if (!exito) {
                if (codDstb.value.trim() == "") alert("No existe un abonado con este código");
                else alert("No existe un abonado con este código asociado al distribuidor " + codDstb.value.toUpperCase());
                this.value = "";
            }
        }
        setCookie("AbndCod", this.value);
        setCookie("AbndDesc", Desc.value);
    }

    codDstb.onblur = function () {
        var Desc = document.getElementById("txtDstbDesc");
        if (this.value.trim() == "") Desc.value = "";
        else {
            Desc.value = "";
            var exito = false;
            var nRegistros = matrizBusq.length;
            for (var i = 0; i < nRegistros; i++) {
                if ((matrizBusq[i][0].toLowerCase() == this.value.toLowerCase() && codAbnd.value.trim() == "") || (this.value.trim().toLowerCase() == matrizBusq[i][0].toLowerCase() && matrizBusq[i][3].toLowerCase() == codAbnd.value.toLowerCase())) {
                    Desc.value = matrizBusq[i][1];
                    exito = true;
                    break;
                }
            }
            if (!exito) {
                if (codAbnd.value.trim() == "") alert("No existe un distribuidor con este código");
                else alert("No existe un distribuidor con este código asociado al abonado " + codAbnd.value.toUpperCase());
                this.value = "";
            }
        }
        setCookie("DstbCod", this.value);
        setCookie("DstbDesc", Desc.value);
    }
}

function ColocarFiltros() {
    document.getElementById("txtDstbCod").value = getCookie("DstbCod");
    document.getElementById("txtDstbDesc").value = getCookie("DstbDesc");
    document.getElementById("txtAbndCod").value = getCookie("AbndCod");
    document.getElementById("txtAbndDesc").value = getCookie("AbndDesc");
    if (getCookie("InOutSide") != "") document.getElementById("ddlInOutSide").value = getCookie("InOutSide");
    if (getCookie("Dpto") != "") document.getElementById("ddlDpto").value = getCookie("Dpto");
    if (getCookie("TipoAbonado") != "") document.getElementById("ddlTipoAbonado").value = getCookie("TipoAbonado");
}

function ConfigurarExit() {
    var msj;
    document.getElementById("btnExit").onclick = function () {

        if (confirm("Se relevarán las alarmas pendientes") == true) {

            $("#myUserLogIn").on("hide.bs.modal", function () { $("#tbUserLogIn").empty(); });
            document.getElementById("btnExit").setAttribute("data-target", "#myUserLogIn");

            $.ajax({
                type: "post",
                url: "../Modulo/ListarUsuarioEnLinea",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var contenido = $("#tbUserLogIn");
                    if (data.length == 0) {
                        contenido.append("<tr style='border-top: 1px solid #222;border-bottom: 1px solid #222'><td colspan='10'>No se encontraron registros</td></tr>");
                    }
                    else {
                        var tr = "";
                        for (var i = 0; i < data.length; i++) {

                            tr = "<tr id='TR_User_" + data[i].User + "' style='border-top: 1px solid #222;border-bottom: 1px solid #222;cursor:pointer' onmouseover=\"this.style.backgroundColor='red';this.style.color='white';\" onmouseout=\"this.style.backgroundColor='white';this.style.color='black';\">";
                            contenido.append(tr + "<td id='TD1_User_" + data[i].User + "'>" + data[i].Nombres + "</td><td id='TD2_User_" + data[i].User + "' style='width: 200px'>" + data[i].Cargo + "</td></tr>");
                        }
                        $("[id*=TR_User_]").click(function () {
                            var id = this.id;
                            var usuario = id.substring(8, id.length);
                            var nombreUsuario = document.getElementById("TD1_User_" + usuario);
                            if (confirm("Desea asignar todas sus alarmas pendientes, incluyendo esta, al usuario " + nombreUsuario.innerHTML)) {
                                $.ajax
                                    ({
                                        type: "post",
                                        url: "../Modulo/RelevarAlarmas",
                                        contentType: "application/json; charset=utf-8",
                                        data: "{UsuarioAsignado: '" + usuario + "'}",
                                        dataType: "json",
                                        success: function (data) {
                                            if (data == true) {
                                                $("#myUserLogIn").modal("hide");
                                                alert("Se han relevado las alarmas pendientes correctamente");
                                                PopupAlarma();
                                                salir = 0;
                                                enviarServidor("../Modulo/LogOut", Exit);
                                            }
                                            else {
                                                alert("No hay alarmas pendientes para relevar");
                                                salir = 0;
                                                enviarServidor("../Modulo/LogOut", Exit);
                                            }
                                        },
                                        //error: error
                                    });
                            }
                        });
                    }
                },
                //error: error
            });
        }
        else {
            salir = 0;
            enviarServidor("../Modulo/LogOut", Exit);
        }
    }
}
/*
function ConfigurarExit() {
    var msj;
    document.getElementById("btnExit").onclick = function () {

        if (confirm("Desea hacer revelo de alarmas?") == true) {

            $("#myUserLogIn").on("hide.bs.modal", function () { $("#tbUserLogIn").empty(); });
            document.getElementById("btnExit").setAttribute("data-target", "#myUserLogIn");

            $.ajax({
                type: "post",
                url: "../Modulo/ListarUsuarioEnLinea",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var contenido = $("#tbUserLogIn");
                    if (data.length == 0) {
                        contenido.append("<tr style='border-top: 1px solid #222;border-bottom: 1px solid #222'><td colspan='10'>No se encontraron registros</td></tr>");
                    }
                    else {
                        var tr = "";
                        for (var i = 0; i < data.length; i++) {

                            tr = "<tr id='TR_User_" + data[i].User + "' style='border-top: 1px solid #222;border-bottom: 1px solid #222;cursor:pointer' onmouseover=\"this.style.backgroundColor='red';this.style.color='white';\" onmouseout=\"this.style.backgroundColor='white';this.style.color='black';\">";
                            contenido.append(tr + "<td id='TD1_User_" + data[i].User + "'>" + data[i].Nombres + "</td><td id='TD2_User_" + data[i].User + "' style='width: 200px'>" + data[i].Cargo + "</td></tr>");
                        }
                        $("[id*=TR_User_]").click(function () {
                            var id = this.id;
                            var usuario = id.substring(8, id.length);
                            var nombreUsuario = document.getElementById("TD1_User_" + usuario);
                            if (confirm("Desea asignar todas sus alarmas pendientes, incluyendo esta, al usuario " + nombreUsuario.innerHTML)) {
                                $.ajax({
                                    type: "post",
                                    url: "../Modulo/RelevarAlarmas",
                                    contentType: "application/json; charset=utf-8",
                                    data: "{UsuarioAsignado: '" + nombreUsuario + "'}",
                                    dataType: "json",
                                    success: function (data) {
                                        if (data == true) {
                                            $("#myUserLogIn").modal("hide");
                                            alert("Se han relevado las alarmas pendientes correctamente");
                                            PopupAlarma();
                                        } else alert("No se ha podido relevar las alarmas pendientes, intente de nuevo en unos segundos");
                                    },
                                    //error: error
                                });
                            }
                        });
                    }
                },
                //error: error
            });
        }
        else {
            salir = 0;
            enviarServidor("../Modulo/LogOut", Exit);
        }
    }
}
*/

//function BuscarLocal_subscriber() {
//    var msj;
//    document.getElementById("LookForLocal").onclick = function () {

//        $("#myLocal_subscriber").on("hide.bs.modal", function () { $("#tbLocal_subscriber").empty(); });
//        document.getElementById("LookForLocal").setAttribute("data-target", "#myLocal_subscriber");

//            $.ajax({
//                type: "post",
//                url: "../Modulo/ListarLocal_Subscriber",
//                contentType: "application/json; charset=utf-8",
//                dataType: "json",
//                success: function (data) {
//                    var contenido = $("#tbLocal_subscriber");
//                    if (data.length == 0) {
//                        contenido.append("<tr style='border-top: 1px solid #222;border-bottom: 1px solid #222'><td colspan='10'>No se encontraron registros</td></tr>");
//                    }
//                    else {
//                        var tr = "";
//                        for (var i = 0; i < data.length; i++) {

//                            tr = "<tr id='TR_User_" + data[i].Localid + "' style='border-top: 1px solid #222;border-bottom: 1px solid #222;cursor:pointer' onmouseover=\"this.style.backgroundColor='red';this.style.color='white';\" onmouseout=\"this.style.backgroundColor='white';this.style.color='black';\">";

//                            contenido.append(tr + "<td id='TD1_User_" + data[i].Localid + "'>" + data[i].Localid + "</td><td id='TD2_User_" + data[i].Locades + "' style='width: 200px'>" + data[i].Locades + "</td></tr>");
//                        }                        
//                    }
//                },
//                //error: error
//            });

//    }
//}

function Exit(rpta) {
    if (rpta != "") {
        if (rpta == "1") {
            sessionStorage.removeItem("DstbAbnd");
            location.href = "../Login/Login";
        }
        else {
            alert("Hubo un problema al momento de desconectarse");
            return false;
        }
    }
}

function LogOut(rpta) {
    if (rpta != "") {
        if (rpta == "1") sessionStorage.removeItem("DstbAbnd");
        else {
            alert("Hubo un problema al momento de desconectarse");
            return false;
        }
    }
}

function BuscarParticionEstado() {
    document.getElementById("btnSearch").onclick = function () {
        var LocalId = document.getElementById("txtLocalid").value;        
        var csid = document.getElementById("txtAbndCod").value;
        //var inOutSide = document.getElementById("ddlInOutSide").value;
        //var dpto = document.getElementById("ddlDpto").value;
        var Tipo_Abonado_id = document.getElementById("ddlTipo_Abonado").value;
        //var evitar = (document.getElementById("chkEvitar").checked == true ? "1" : "0");
        //var Localid = document.getElementById("txtLocalid").value;
        var TipoParticionid = document.getElementById("ddlParticion").value;
        var TipoEvento = document.getElementById("ddlEstado").value;
        var inOutSide = document.getElementById("ddlInOutSide").value;
        var dpto = document.getElementById("ddlDpto").value;
        //var url = "../Modulo/ParticionEStado?DstbCod=" + dstb + "&AbndCod=" + abnd + "&Evitar=" + evitar + "&InOutSide=" + inOutSide + "&Ubigeo=" + dpto + "&TipoAbonado=" + tipoAbonado + "&Localid=" + Localid + "&TipoParticionid=" + TipoParticionid + "&t=" + Math.random();        
        var url = "../Modulo/GrillaParticionEstado?LocalId=" + LocalId + "&csid=" + csid + "&Tipo_Abonado_id=" + Tipo_Abonado_id + "&TipoParticionid=" + TipoParticionid + "&TipoEvento=" + TipoEvento + "&InOutSide=" + inOutSide + "&Ubigeo=" + dpto + "&t=" + Math.random();
//        var url = "../Modulo/ParticionEStado?t=" + Math.random();

        //alert(url);
        //alert(GrillaParticionEstado);        
        enviarServidor(url, GrillaParticionEstado);


    }
}

function LimpiarBusqueda() {
    document.getElementById("btnClean").onclick = function () {
        document.getElementById("txtAbndCod").value = "";
        document.getElementById("txtAbndDesc").value = "";
        document.getElementById("txtLocalid").value = "";
        document.getElementById("txtLocalDesc").value = "";
        setCookie("AbndCod", "");
        setCookie("AbndDesc", "");        
        setCookie("LocalId", "");
        setCookie("LocalDesc", "");        
        document.getElementById("ddlTipo_Abonado").value = "0";
        document.getElementById("ddlParticion").value = "0";
        document.getElementById("ddlEstado").value = "0";        
        setCookie("Tipo_Abonado", "0");
    }
}

function GrillaParticionEstado(rpta) {

    if (rpta != "") {
        indiceActualBloque = 0;
        var divPaginacion = document.getElementById("divPaginacionParticionEst");
        divPaginacion.style.display = "inline";
        var lista = rpta.split("_");
        matriz = [];
        crearMatrizParticionEst(lista);
        mostrarPaginaParticionEst(0);
        crearPaginasBloqueParticionEst();
        seleccionarPaginaActualParticionEst();
    }
    else {
        document.getElementById("tbRegistrosParticionEst").innerHTML = "<tr style='border-top: 1px solid #222;border-bottom: 1px solid #222'><td colspan='5' style='width:90px;'>No se encontraron registros</td></tr>";
        document.getElementById("divPaginacionParticionEst").style.display = "none";
        document.getElementById("liPaginaParticionEst").innerHTML = "";
    }
}




function crearMatrizParticionEst(lista) {
    var nRegistros = lista.length;
    var nCampos;
    var campos;
    for (i = 0; i < nRegistros; i++) {
        matriz[i] = [];
        campos = lista[i].split("@");
        nCampos = campos.length;
        for (j = 0; j < nCampos; j++) {
            matriz[i][j] = campos[j].trim();
        }
    }
}

function mostrarPaginaParticionEst(indicePagina) {
    indiceActualPagina = indicePagina;
    var nRegistros = matriz.length;
    var contenido = "";
    var campos;
    var inicio = indicePagina * registrosPagina;
    var fin = inicio + registrosPagina;
    for (var z = inicio; z < fin; z++) {
        if (z < nRegistros) {
            contenido += "<tr id='VS_" + matriz[z][4] + "' data-toggle='modal' data-target='#myModalComent' style='border-top: 1px solid #222;border-bottom: 1px solid #222;cursor:pointer'>";            
            contenido += "<td style='width:60px;'>" + matriz[z][1] + "</td>";
            contenido += "<td style='width:80px;'>" + matriz[z][2] + "</td>";
            contenido += "<td style='width:200px;'>" + matriz[z][3] + "</td>";
            contenido += "<td style='width:70px;'>" + matriz[z][4] + "</td>";
            contenido += "<td style='width:50px;'>" + matriz[z][5] + "</td>";
            contenido += "<td style='width:80px;'>" + matriz[z][6] + "</td>";
            contenido += "<td style='width:80px;'>" + matriz[z][7] + "</td>";
            contenido += "<td style='width:50px; text-align:center'>" + matriz[z][8] + "</td>";
            contenido += "<td style='width:80px;'>" + matriz[z][9] + "</td>";
            contenido += "<td style='width:80px; text-align:center'>" + matriz[z][10] + "</td>";
            contenido += "<td style='width:80px;'>" + matriz[z][11] + "</td>";            
            contenido += "</tr>";
        }
    }
    
    var tbParticionEstado = document.getElementById("tbRegistrosParticionEst");
    tbParticionEstado.innerHTML = contenido;
    //var trs = document.getElementsByTagName("tr");
    //for (var k = 0; k < trs.length; k++) {
    //    if (trs[k].id.indexOf("VS_") > -1) {
    //        trs[k].onclick = function () {
    //            document.getElementById("refComent").click();
    //            enviarServidor("../Modulo/ComentariosLlamadas?ahid=" + this.id.substring(3, this.id.length), mostrarComentarios);
    //        }
    //    }
    //}
}

function mostrarComentarios(rpta) {
    if (rpta != "◄") {
        var lsta = rpta.split("◄");
        var coment = "";
        var call = "";
        if (lsta[0] != "") {
            var coments = lsta[0].split("_");
            var com;
            var nComent = coments.length;
            for (var m = 0; m < nComent; m++) {
                com = coments[m].split("@");
                coment += "<tr style='border-top: 1px solid #222;border-bottom: 1px solid #222'>";
                coment += "<td style='width:110px;'>";
                coment += com[0];
                coment += "</td>";
                coment += "<td>";
                coment += com[1];
                coment += "</td>";
                coment += "<td style='width:150px;'>";
                coment += com[2];
                coment += "</td></tr>";
            }
        }
        else
            coment += "<tr style='border-top: 1px solid #222;border-bottom: 1px solid #222'><td colspan='3' style='width:90px;'>No se encontraron registros</td></tr>";
        document.getElementById("tableComent").innerHTML = coment;
        if (lsta[1] != "") {
            var calls = lsta[1].split("_");
            var cal;
            var nCall = calls.length;
            for (var n = 0; n < nCall; n++) {
                cal = calls[n].split("@");
                call += "<tr style='border-top: 1px solid #222;border-bottom: 1px solid #222'>";
                call += "<td style='width:110px;'>";
                call += cal[1];
                call += "</td>";
                call += "<td style='width:150px;'>";
                call += cal[0];
                call += "</td>";
                call += "<td>";
                call += cal[2];
                call += "</td>";
                call += "<td style='width:150px;'>";
                call += cal[3];
                call += "</td></tr>";
            }
        }
        else
            call += "<tr style='border-top: 1px solid #222;border-bottom: 1px solid #222'><td colspan='4' style='width:90px;'>No se encontraron registros</td></tr>";
        document.getElementById("tableCalls").innerHTML = call;
    }
    else {
        document.getElementById("tableComent").innerHTML = "<tr style='border-top: 1px solid #222;border-bottom: 1px solid #222'><td colspan='3' style='width:90px;'>No se encontraron registros</td></tr>";
        document.getElementById("tableCalls").innerHTML = "<tr style='border-top: 1px solid #222;border-bottom: 1px solid #222'><td colspan='4' style='width:90px;'>No se encontraron registros</td></tr>";
    }
}

function crearPaginasBloqueParticionEst() {
    var contenido = "";
    var existeBloque = (matriz.length > (registrosPagina * paginasBloque));
    if (existeBloque) {
        contenido += "<a href='#' onclick='paginarParticionEst(-1);' title='Ir al primer grupo de páginas'>&lt;&lt;</a>";
        contenido += "<a href='#' onclick='paginarParticionEst(-2);' title='Ir al anterior grupo de páginas'>&lt;</a>";
    }
    var inicio = indiceActualBloque * paginasBloque;
    var indiceUltimaPagina = Math.floor(matriz.length / registrosPagina);
    if (matriz.length % registrosPagina == 0) indiceUltimaPagina--;
    var n;
    for (i = 0; i < paginasBloque; i++) {
        n = inicio + i;
        if (n <= indiceUltimaPagina) {
            contenido += "<a href='#' onclick='paginarParticionEst(" + n + ");'  title='Ir a la pagina " + (n + 1).toString() + "' id='a" + n.toString() + "' class='pagina' >" + (n + 1).toString() + "</a>";
        }
    }
    if (existeBloque) {
        contenido += "<a href='#' onclick='paginarParticionEst(-3);' title='Ir al siguiente grupo de páginas'>&gt;</a>";
        contenido += "<a href='#' onclick='paginarParticionEst(-4);' title='Ir al último grupo de páginas'>&gt;&gt;</a>";
    }
    var tdPagina = document.getElementById("liPaginaParticionEst");
    tdPagina.innerHTML = contenido;
}

function paginarParticionEst(indicePagina) {
    if (indicePagina < 0) {
        var indiceUltimoBloque;
        var indiceUltimaPagina = Math.floor(matriz.length / registrosPagina);
        if (matriz.length % registrosPagina == 0) indiceUltimaPagina--;
        switch (indicePagina) {
            case -1:
                indiceActualBloque = 0;
                indicePagina = 0;
                break;
            case -2:
                if (indiceActualBloque > 0) {
                    indiceActualBloque--;
                }
                indicePagina = indiceActualBloque * paginasBloque;
                break;
            case -3:
                indiceUltimoBloque = Math.floor(indiceUltimaPagina / paginasBloque);
                if (indiceUltimaPagina % paginasBloque == 0) indiceUltimoBloque--;
                if (indiceActualBloque < indiceUltimoBloque) {
                    indiceActualBloque++;
                }
                indicePagina = indiceActualBloque * paginasBloque;
                break;
            case -4:
                indiceUltimoBloque = Math.floor(indiceUltimaPagina / paginasBloque);
                if (indiceUltimaPagina % paginasBloque == 0) indiceUltimoBloque--;
                indiceActualBloque = indiceUltimoBloque;
                indicePagina = indiceUltimoBloque * paginasBloque;
                break;
        }
        mostrarPaginaParticionEst(indicePagina);
        crearPaginasBloqueParticionEst();
    }
    else {
        indiceActualBloque = Math.floor(indicePagina / paginasBloque);
        if (indicePagina > 0 && (indicePagina % paginasBloque == 0)) indiceActualBloque--;
        mostrarPaginaParticionEst(indicePagina);
    }
    var aPaginas = document.getElementsByClassName("pagina");
    var nPaginas = aPaginas.length;
    for (i = 0; i < nPaginas; i++) {
        aPaginas[i].style.backgroundColor = "#fff";
        aPaginas[i].style.color = "#950208";
    }
    seleccionarPaginaActualParticionEst();
}

function seleccionarPaginaActualParticionEst() {
    var aPagina = document.getElementById("a" + indiceActualPagina);
    if (aPagina != null) {
        aPagina.style.backgroundColor = "#950208";
        aPagina.style.color = "#fff";
    }
}

function SenalPendiente() {
    var url = "../Modulo/SenalPendienteNoMonitoreo";
    enviarServidor(url, function (rpta) {
        if (rpta == 1) location.href = "../Modulo/Monitoreo";
    });
}

function ComboLocal_subscriber() {
    $.ajax({
        type: "post",
        url: "../Modulo/ComboLocal_Subscriber",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: exitoNuevo,
        error: error
    });
}

function error() {
    alert('Ocurrio un error en Local_Subscriber');
}

function enviarServidor(url, metodo, estado) {
    estado = estado == undefined ? true : estado;
    var xhr = new XMLHttpRequest();
    xhr.open("GET", url, estado);
    xhr.setRequestHeader("xhr", "OSC");
    xhr.onreadystatechange = function () {
        if (xhr.status == 200 && xhr.readyState == 4) {
            metodo(xhr.responseText);
        }
    }
    xhr.send();
}

//function ComboLocal_Subscriber() {
//    var url = "../Modulo/ComboLocal_Subscriber";
//    enviarServidor(url, LlenarComboLocal_Subscriber);
//}

//function LlenarComboLocal_Subscriber(rpta) {
//    if (rpta != "") {
//        var lista = rpta.split("_"), contenido = "";
//        var n = lista.length, campos;
//        for (var i = 0; i < n; i++) {
//            campos = lista[i].split("@");
//            contenido += "<option value='" + campos[0] + "'>" + campos[1] + "</option>";
//        }
//        document.getElementById("ddlLocal").innerHTML = contenido;
//    }
//}

function ComboTipoParticion() {
    var url = "../Modulo/ComboTipoParticion";
    enviarServidor(url, LlenarComboTipoParticion);
}

function ComboTipo_Abonado() {
    var url = "../Modulo/ComboTipo_Abonado";
    enviarServidor(url, LlenarComboTipo_Abonado);
}


function LlenarComboTipoParticion(rpta) {
    if (rpta != "") {
        var lista = rpta.split("_"), contenido = "";
        var n = lista.length, campos;
        for (var i = 0; i < n; i++) {
            campos = lista[i].split("@");
            contenido += "<option value='" + campos[0] + "'>" + campos[1] + "</option>";
        }
        document.getElementById("ddlParticion").innerHTML = contenido;
    }
}

function LlenarComboTipo_Abonado(rpta) {
    if (rpta != "") {
        var lista = rpta.split("_"), contenido = "";
        var n = lista.length, campos;
        for (var i = 0; i < n; i++) {
            campos = lista[i].split("@");
            contenido += "<option value='" + campos[0] + "'>" + campos[1] + "</option>";
        }
        document.getElementById("ddlTipo_Abonado").innerHTML = contenido;
    }
}


function BuscarLocalDesc() {
    var LocalId = document.getElementById("txtLocalid");
    var LocalDesc = document.getElementById("txtLocalDesc");
    LocalId.onblur = function () {
        var Desc = document.getElementById("txtLocalDesc");
        if (this.value.trim() == "") Desc.value = "";
        else {
            Desc.value = "";
            var exito = false;
            var nRegistrosLoc = matrizBusqLoc.length;

            for (var i = 0; i < nRegistrosLoc; i++) {
                if ((LocalId.value.trim() == matrizBusqLoc[i][0].toLowerCase())) {
                    Desc.value = matrizBusqLoc[i][1];
                    exito = true;
                    break;
                }
            }
            if (!exito) {
                if (LocalId.value.trim() == "") alert("No existe un Local con este código");
                else alert("No existe un Local " + LocalId.value.toUpperCase());
                this.value = "";
            }
        }
        setCookie("txtlocalid", this.value);
        setCookie("txtLocalDesc", Desc.value);
    }

}

