var registrosPagina = 25, paginasBloque = 10, indiceActualBloque = 0, indiceActualPagina, matriz = [], matrizAlarmas = [], timer = null;
var salir = 1;
window.addEventListener("load", function () {
    CondicionesAlarmaZona();
    RestriccionesCombo();
    RestriccionesRadio();
    BuscarAutomatico();
    BuscarAbonadoDistribuidor();
    Validacion();
    ColocarFiltros();
    ConfigurarExit();
    BuscarArea();
    Exportar();
    timerSenal = setInterval(SenalPendiente, 10000);
});

window.addEventListener("unload", function () {
    if (salir > 0) enviarServidor("../Modulo/LogOut", Exit, false);
});

function getCookie(cname) {
    var name = "CA1864" + cname + "=";
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
    document.cookie = "CA1864" + nombre + "=" + valor + ";expires=" + now.toUTCString() + ";path=/";
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

function CondicionesAlarmaZona() {
    var codPri = document.getElementById("txtCondicPrimCod");
    var codUlt = document.getElementById("txtCondicUltCod");
    var zona = document.getElementById("txtZona");
    codPri.onblur = function () {
        var Desc = document.getElementById("txtCondicPrimDes");
        if (this.value.trim() == "") Desc.value = "";
        else {
            Desc.value = "";
            var exito = false;
            var nRegistros = matrizAlarmas.length;
            for (var i = 0; i < nRegistros; i++) {
                if (matrizAlarmas[i][0].toLowerCase() == this.value.toLowerCase()) {
                    Desc.value = matrizAlarmas[i][1];
                    exito = true;
                    break;
                }
            }
            if (!exito) {
                alert("No existe una alarma con este código");
                codPri.value = "";
                codPri.focus();
            }
        }
        if ((codPri.value.toUpperCase() == "CAG" && codUlt.value.toUpperCase() == "OAG") || (codPri.value.toUpperCase() == "OAG" && codUlt.value.toUpperCase() == "CAG")) zona.value = "AGENCIA";
        else if ((codPri.value.toUpperCase() == "CP3" && codUlt.value.toUpperCase() == "OP3") || (codPri.value.toUpperCase() == "OP3" && codUlt.value.toUpperCase() == "CP3")) zona.value = "BOVEDA";
        else if ((codPri.value.toUpperCase() == "CCA" && codUlt.value.toUpperCase() == "OCA") || (codPri.value.toUpperCase() == "OCA" && codUlt.value.toUpperCase() == "CCA")) zona.value = "ATM";
        else if ((codPri.value.toUpperCase() == "CBV" && codUlt.value.toUpperCase() == "OBV") || (codPri.value.toUpperCase() == "OBV" && codUlt.value.toUpperCase() == "CBV")) zona.value = "VALORES";
        else zona.value = "";
        setCookie("PrimCod", this.value);
        setCookie("PrimDesc", Desc.value);
        setCookie("Zona", zona.value);
    }

    codUlt.onblur = function () {
        var Desc = document.getElementById("txtCondicUltDes");
        if (this.value.trim() == "") Desc.value = "";
        else {
            Desc.value = "";
            var exito = false;
            var nRegistros = matrizAlarmas.length;
            for (var i = 0; i < nRegistros; i++) {
                if (matrizAlarmas[i][0].toLowerCase() == this.value.toLowerCase()) {
                    Desc.value = matrizAlarmas[i][1];
                    exito = true;
                    break;
                }
            }
            if (!exito) {
                alert("No existe una alarma con este código");
                codUlt.value = "";
                codUlt.focus();
            }
        }
        if ((codPri.value.toUpperCase() == "CAG" && codUlt.value.toUpperCase() == "OAG") || (codPri.value.toUpperCase() == "OAG" && codUlt.value.toUpperCase() == "CAG")) zona.value = "AGENCIA";
        else if ((codPri.value.toUpperCase() == "CP3" && codUlt.value.toUpperCase() == "OP3") || (codPri.value.toUpperCase() == "OP3" && codUlt.value.toUpperCase() == "CP3")) zona.value = "BOVEDA";
        else if ((codPri.value.toUpperCase() == "CCA" && codUlt.value.toUpperCase() == "OCA") || (codPri.value.toUpperCase() == "OCA" && codUlt.value.toUpperCase() == "CCA")) zona.value = "ATM";
        else if ((codPri.value.toUpperCase() == "CBV" && codUlt.value.toUpperCase() == "OBV") || (codPri.value.toUpperCase() == "OBV" && codUlt.value.toUpperCase() == "CBV")) zona.value = "VALORES";
        else zona.value = "";
        setCookie("UltCod", this.value);
        setCookie("UltDesc", Desc.value);
        setCookie("Zona", zona.value);
    }

    zona.onblur = function () {
        setCookie("Zona", zona.value);
    }
    enviarServidor("../Modulo/ListarCondicionAlarmas", ListadoAlarmas);
}

function ListadoAlarmas(rpta) {
    if (rpta != "") {
        var lis = rpta.split("_");
        var nRegistros = lis.length;
        var nCampos;
        var campos;
        for (i = 0; i < nRegistros; i++) {
            matrizAlarmas[i] = [];
            campos = lis[i].split("@");
            nCampos = campos.length;
            for (j = 0; j < nCampos; j++) {
                matrizAlarmas[i][j] = campos[j].trim();
            }
        }
    }
}

function RestriccionesCombo() {
    document.getElementById("ddlInOutSide").onchange = function () { if (document.getElementById("ddlDpto").value == "00" && this.value == "≠") this.value = "="; setCookie("InOutSide", this.value); }
    document.getElementById("ddlDpto").onchange = function () { if (document.getElementById("ddlInOutSide").value == "≠" && this.value == "00") document.getElementById("ddlInOutSide").value = "="; setCookie("Dpto", this.value); }
    document.getElementById("ddlTipoAbonado").onchange = function () { setCookie("TipoAbonado", this.value); }
}

function RestriccionesRadio() {
    var All = document.getElementById("rdbAll");
    var Plus = document.getElementById("rdbPlus");
    var Time = document.getElementById("txtTime");
    All.onclick = function () {
        Plus.checked = false;
        Time.disabled = true;
    }

    Plus.onclick = function () {
        All.checked = false;
        Time.disabled = false;
    }
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
    document.getElementById("txtCondicPrimCod").value = getCookie("PrimCod");
    document.getElementById("txtCondicPrimDes").value = getCookie("PrimDesc");
    document.getElementById("txtCondicUltCod").value = getCookie("UltCod");
    document.getElementById("txtCondicUltDes").value = getCookie("UltDesc");
    document.getElementById("txtZona").value = getCookie("Zona");
    if (getCookie("InOutSide") != "") document.getElementById("ddlInOutSide").value = getCookie("InOutSide");
    if (getCookie("Dpto") != "") document.getElementById("ddlDpto").value = getCookie("Dpto");
    if (getCookie("TipoAbonado") != "") document.getElementById("ddlTipoAbonado").value = getCookie("TipoAbonado");
}

function Validacion() {
    document.getElementById("txtTime").onblur = function () {
        if (this.value == "") this.value = "0";
    }
}

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

function BuscarArea() {
    document.getElementById("btnSearch").onclick = function () {
        if (document.getElementById("txtCondicPrimCod").value.trim() == "" && document.getElementById("txtCondicUltCod").value.trim() != "") {
            alert("No se puede buscar sin una condición de alarma inicial");
            document.getElementById("txtCondicPrimCod").focus();
            return;
        }
        else {
            var dstb = document.getElementById("txtDstbCod").value;
            var abnd = document.getElementById("txtAbndCod").value;
            var condP = document.getElementById("txtCondicPrimCod").value;
            var condU = document.getElementById("txtCondicUltCod").value;
            var zona = document.getElementById("txtZona").value;
            var timer = document.getElementById("txtTime").value;
            var opc = document.getElementById("rdbAll").checked ? "0" : (timer.value != "" ? timer : "0");
            var pasado = (document.getElementById("chkDatosP").checked ? "1" : "0");
            var inOutSide = document.getElementById("ddlInOutSide").value;
            var dpto = document.getElementById("ddlDpto").value;
            var tipoAbonado = document.getElementById("ddlTipoAbonado").value;
            var url = "../Modulo/GrillaControlArea1864/?DstbCod=" + dstb + "&AbndCod=" + abnd + "&CondPrimCod=" + condP + "&CondUltCod=" + condU + "&Zona=" + zona + "&Time=" + opc + "&DatosP=" + pasado + "&InOutSide=" + inOutSide + "&Ubigeo=" + dpto + "&TipoAbonado=" + tipoAbonado + "&t=" + Math.random();
            enviarServidor(url, GrillaArea);
        }
    }
}

function Exportar() {
    document.getElementById("btnReport").onclick = function () {
        document.getElementById("exportarExcel").click();
    }
    document.getElementById("exportarExcel").onclick = function () {
        if (matriz.length != 0) {
            var excelExportar = crearReporteExcel();
            var blob = new Blob([excelExportar], { type: 'application/vnd.ms-excel' });
            if (navigator.appVersion.toString().indexOf('.NET') > 0)
                window.navigator.msSaveBlob(blob, "ControlArea1864.xls");
            else {
                this.download = "ControlArea1864.xls";
                this.href = window.URL.createObjectURL(blob);
            }
        }
        else alert("No existen registros en la busqueda actual, intente de nuevo cambiando las opciones de búsqueda");
    }
}

function crearReporteExcel() {
    var excel = "<html><head><meta charset='utf-8'/></head><table style='font-size: x-small;border:none;border-collapse:collapse'>";
    excel += "<thead><tr style='background-color: #A9A9A9; font-size: 9pt;font-weight:bold;height:30px'>";
    excel += "<th style='width:80px'>Fecha</th>";
    excel += "<th style='width:70px'>Hora</th>";
    excel += "<th style='width:70px'>Abonado</th>";
    excel += "<th style='width:280px;text-align:left'>Oficina</th>";
    excel += "<th style='width:350px;text-align:left'>Condición</th>";
    excel += "<th style='width:70px'>ID Señal</th>";
    excel += "<th>Usuario / Descripción</th></tr></thead>";
    var contenido = "<tbody>";
    var nReg = matriz.length;
    for (var o = 0; o < nReg; o++) {
        contenido += "<tr><td style='width:80px;'>" + matriz[o][5] + "</td>";
        contenido += "<td style='width:70px;'>" + matriz[o][6] + "</td>";
        contenido += "<td style='width:70px;'>=\"" + matriz[o][10] + "\"</td>";
        contenido += "<td style='width:290px;'>" + matriz[o][11] + "</td>";
        contenido += "<td style='width:350px;'>" + matriz[o][8] + "</td>";
        contenido += "<td style='width:70px;'>" + matriz[o][7] + "</td>";
        contenido += "<td>" + matriz[o][9] + "</td>";
        contenido += "</tr>";
    }
    excel += contenido;
    excel += "</tbody></table></html>";
    return excel;
}

function GrillaArea(rpta) {
    if (rpta != "") {
        indiceActualBloque = 0;
        var divPaginacion = document.getElementById("divPaginacionArea");
        divPaginacion.style.display = "inline";
        var lista = rpta.split("_");
        matriz = [];
        crearMatrizArea(lista);
        mostrarPaginaArea(0);
        crearPaginasBloqueArea();
        seleccionarPaginaActualArea();
    }
    else {
        document.getElementById("tbRegistrosArea").innerHTML = "<tr style='border-top: 1px solid #222;border-bottom: 1px solid #222'><td colspan='5' style='width:90px;'>No se encontraron registros</td></tr>";
        document.getElementById("divPaginacionArea").style.display = "none";
        document.getElementById("liPaginaArea").innerHTML = "";
    }
}

function crearMatrizArea(lista) {
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

function mostrarPaginaArea(indicePagina) {
    indiceActualPagina = indicePagina;
    var nRegistros = matriz.length;
    var contenido = "";
    var campos;
    var inicio = indicePagina * registrosPagina;
    var fin = inicio + registrosPagina;
    for (var z = inicio; z < fin; z++) {
        if (z < nRegistros) {
            contenido += "<tr style='border-top: 1px solid #222;border-bottom: 1px solid #222'>";
            contenido += "<td style='width:20px;'><img src='" + matriz[z][4] + "'/></td>";
            contenido += "<td style='width:70px;'>" + matriz[z][5] + "</td>";
            contenido += "<td style='width:70px;'>" + matriz[z][6] + "</td>";
            contenido += "<td style='width:70px;'>" + matriz[z][10] + "</td>";
            contenido += "<td style='width:280px;text-align:left'>" + matriz[z][11] + "</td>";
            contenido += "<td style='width:350px;text-align:left'>" + matriz[z][8] + "</td>";
            contenido += "<td style='width:70px;'>" + matriz[z][7] + "</td>";
            contenido += "<td>" + matriz[z][9] + "</td>";
            contenido += "</tr>";
        }
    }
    var tbArea = document.getElementById("tbRegistrosArea");
    tbArea.innerHTML = contenido;
}

function crearPaginasBloqueArea() {
    var contenido = "";
    var existeBloque = (matriz.length > (registrosPagina * paginasBloque));
    if (existeBloque) {
        contenido += "<a href='#' onclick='paginarArea(-1);' title='Ir al primer grupo de páginas'>&lt;&lt;</a>";
        contenido += "<a href='#' onclick='paginarArea(-2);' title='Ir al anterior grupo de páginas'>&lt;</a>";
    }
    var inicio = indiceActualBloque * paginasBloque;
    var indiceUltimaPagina = Math.floor(matriz.length / registrosPagina);
    if (matriz.length % registrosPagina == 0) indiceUltimaPagina--;
    var n;
    for (i = 0; i < paginasBloque; i++) {
        n = inicio + i;
        if (n <= indiceUltimaPagina) {
            contenido += "<a href='#' onclick='paginarArea(" + n + ");'  title='Ir a la pagina " + (n + 1).toString() + "' id='a" + n.toString() + "' class='pagina' >" + (n + 1).toString() + "</a>";
        }
    }
    if (existeBloque) {
        contenido += "<a href='#' onclick='paginarArea(-3);' title='Ir al siguiente grupo de páginas'>&gt;</a>";
        contenido += "<a href='#' onclick='paginarArea(-4);' title='Ir al último grupo de páginas'>&gt;&gt;</a>";
    }
    var tdPagina = document.getElementById("liPaginaArea");
    tdPagina.innerHTML = contenido;
}

function paginarArea(indicePagina) {
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
        mostrarPaginaArea(indicePagina);
        crearPaginasBloqueArea();
    }
    else {
        indiceActualBloque = Math.floor(indicePagina / paginasBloque);
        if (indicePagina > 0 && (indicePagina % paginasBloque == 0)) indiceActualBloque--;
        mostrarPaginaArea(indicePagina);
    }
    var aPaginas = document.getElementsByClassName("pagina");
    var nPaginas = aPaginas.length;
    for (i = 0; i < nPaginas; i++) {
        aPaginas[i].style.backgroundColor = "#fff";
        aPaginas[i].style.color = "#950208";
    }
    seleccionarPaginaActualArea();
}

function seleccionarPaginaActualArea() {
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