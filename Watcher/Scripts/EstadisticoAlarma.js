var registrosPagina = 25, paginasBloque = 10, indiceActualBloque = 0, indiceActualPagina, matriz = [], timer = null;
var salir = 1;
window.addEventListener("load", function () {
    ListarAlarmas();
    ConfigurarFechas();
    ConfigurarAlarmas();
    RestriccionesCombo();
    BuscarAbonadoDistribuidor();
    ColocarFiltros();
    ConfigurarExit();
    BuscarEstadistico();
    Exportar();
    timerSenal = setInterval(SenalPendiente, 10000);
});

window.addEventListener("unload", function () {
    if (salir > 0) enviarServidor("../Modulo/LogOut", Exit, false);
});

function getCookie(cname) {
    var name = "EA" + cname + "=";
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
    document.cookie = "EA" + nombre + "=" + valor + ";expires=" + now.toUTCString() + ";path=/";
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

function ListarAlarmas() {
    var url = "../Modulo/ListarAlarmas";
    enviarServidor(url, LlenarCombo);
}

function LlenarCombo(rpta) {
    if (rpta != "") {
        var lista = rpta.split("_"), contenido = "";
        var n = lista.length, campos;
        for (var i = 0; i < n; i++) {
            campos = lista[i].split("@");
            contenido += "<option value='" + campos[0] + "'>" + campos[1] + "</option>";
        }
        document.getElementById("ddlAlarmas").innerHTML = contenido;
    }
}

function ConfigurarFechas() {
    document.getElementById("cbCtrlFecha").onchange = function () {
        var fechas = document.getElementsByClassName("Fecha");
        if (!this.checked) {
            for (var i = 0; i < fechas.length; i++) {
                fechas[i].setAttribute("disabled", "disabled");
            }
        }
        else {
            for (var i = 0; i < fechas.length; i++) {
                fechas[i].removeAttribute("disabled");
            }
        }
    }

    var FechaIni = $("#txtFechaIni"), FechaFin = $("#txtFechaFin");
    var HoraIni = $("#txtHoraIni"), HoraFin = $("#txtHoraFin");
    FechaIni.datepicker({
        dateFormat: "dd/mm/yy",
        onSelect: function () {
            var dd = parseFloat(this.value.split("/")[0]);
            var mm = parseFloat(this.value.split("/")[1]) - 1;
            var yy = parseFloat(this.value.split("/")[2]);
            FechaFin.datepicker("option", "minDate", new Date(yy, mm, dd));
            
            if (this.value == FechaFin.val()) {
                var Ini = new Date(1970, 1, 1, parseInt(HoraIni.val().substring(0, 2)), parseInt(HoraIni.val().substring(3, 5)));
                var Fin = new Date(1970, 1, 1, parseInt(HoraFin.val().substring(0, 2)), parseInt(HoraFin.val().substring(3, 5)));
                if (Ini > Fin) HoraFin.val(HoraIni.val());
                else HoraIni.val(HoraFin.val());
            }
        }
    });
    FechaFin.datepicker({
        dateFormat: "dd/mm/yy",
        onSelect: function () {
            if (FechaIni.val() == this.value) {
                var Ini = new Date(1970, 1, 1, parseInt(HoraIni.val().substring(0, 2)), parseInt(HoraIni.val().substring(3, 5)));
                var Fin = new Date(1970, 1, 1, parseInt(HoraFin.val().substring(0, 2)), parseInt(HoraFin.val().substring(3, 5)));
                if (Ini > Fin) HoraFin.val(HoraIni.val());
                else HoraIni.val(HoraFin.val());
            }
        }
    });
    FechaIni.datepicker("setDate", new Date());
    FechaIni.datepicker("option", "maxDate", new Date());
    FechaFin.datepicker({ dateFormat: "dd/mm/yy" });
    FechaFin.datepicker("setDate", new Date());
    FechaFin.datepicker("option", "minDate", new Date());
    FechaFin.datepicker("option", "maxDate", new Date());

    HoraIni.timepicker({
        onSelect: function () {
            if (FechaIni.val() == FechaFin.val()) {
                var Ini = new Date(1970, 1, 1, parseInt(HoraIni.val().substring(0, 2)), parseInt(HoraIni.val().substring(3, 5)));
                var Fin = new Date(1970, 1, 1, parseInt(HoraFin.val().substring(0, 2)), parseInt(HoraFin.val().substring(3, 5)));
                if (Ini > Fin) HoraFin.val(HoraIni.val());
            }
        }
    });

    HoraFin.timepicker({
        onSelect: function () {
            if (FechaIni.val() == FechaFin.val()) {
                var Ini = new Date(1970, 1, 1, parseInt(HoraIni.val().substring(0, 2)), parseInt(HoraIni.val().substring(3, 5)));
                var Fin = new Date(1970, 1, 1, parseInt(HoraFin.val().substring(0, 2)), parseInt(HoraFin.val().substring(3, 5)));
                if (Fin < Ini) HoraIni.val(HoraFin.val());
            }
        }
    });
    HoraIni.val("00:00");
    HoraFin.val(((new Date).getHours() < 10 ? "0" + (new Date).getHours().toString() : (new Date).getHours().toString()) + ":" + ((new Date).getMinutes() < 10 ? "0" + (new Date).getMinutes().toString() : (new Date).getMinutes().toString()));
}

function ConfigurarAlarmas() {
    var alarm = document.getElementById("ddlAlarmas");
    var txtAlarm = document.getElementById("txtCodAlarma")

    alarm.onchange = function () {
        txtAlarm.value=this.value;
    }

    txtAlarm.keyup = function () {
        alarm.value = $("#ddlAlarmas option[value='" + txtAlarm.value.toUpperCase() + "']").length > 0 ? txtAlarm.value.toUpperCase() : "";
    }
}

function RestriccionesCombo() {
    document.getElementById("ddlInOutSide").onchange = function () { if (document.getElementById("ddlDpto").value == "00" && this.value == "≠") this.value = "="; setCookie("InOutSide", this.value); }
    document.getElementById("ddlDpto").onchange = function () { if (document.getElementById("ddlInOutSide").value == "≠" && this.value == "00") document.getElementById("ddlInOutSide").value = "="; setCookie("Dpto", this.value); }
    document.getElementById("ddlTipoAbonado").onchange = function () { setCookie("TipoAbonado", this.value); }
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

function BuscarEstadistico() {
    document.getElementById("btnSearch").onclick = function () {
        var dstb = document.getElementById("txtDstbCod").value;
        var abnd = document.getElementById("txtAbndCod").value;
        var ctrlFecha = (document.getElementById("cbCtrlFecha").checked == true ? "1" : "0");
        var fechaIni = document.getElementById("txtFechaIni").value;
        var horaIni = document.getElementById("txtHoraIni").value;
        var fechaFin = document.getElementById("txtFechaFin").value;
        var horaFin = document.getElementById("txtHoraFin").value;
        var codAlarma = document.getElementById("txtCodAlarma").value;
        var inOutSide = document.getElementById("ddlInOutSide").value;
        var dpto = document.getElementById("ddlDpto").value;
        var tipoAbonado = document.getElementById("ddlTipoAbonado").value;
        var url = "../Modulo/GrillaEstadisticoAlarmas?DstbCod=" + dstb + "&AbndCod=" + abnd + "&CtrlFecha=" + ctrlFecha + "&FechaIni=" + fechaIni + "&HoraIni=" + horaIni + "&FechaFin=" + fechaFin + "&HoraFin=" + horaFin + "&CodAlarma=" + codAlarma + "&InOutSide=" + inOutSide + "&Ubigeo=" + dpto + "&TipoAbonado=" + tipoAbonado + "&t=" + Math.random();
        enviarServidor(url, GrillaEstadistico);
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
                window.navigator.msSaveBlob(blob, "EstadisticoAlarmas.xls");
            else {
                this.download = "EstadisticoAlarmas.xls";
                this.href = window.URL.createObjectURL(blob);
            }
        }
        else alert("No existen registros en la busqueda actual, intente de nuevo cambiando las opciones de búsqueda");
    }
}

function crearReporteExcel() {
    var excel = "<html><head><meta charset='utf-8'/></head><table style='font-size: x-small;border:none;border-collapse:collapse'>";
    excel += "<thead><tr style='background-color: #A9A9A9; font-size: 9pt;font-weight:bold;height:30px'>";
    excel += "<th style='width:150px'>Código Distribuidor</th>";
    excel += "<th style='width:70px'>Abonado</th>";
    excel += "<th style='width:350px;'>Oficina</th>";
    excel += "<th style='width:100px'>Actividad</th>";
    excel += "<th style='width:200px'>Descripción Actividad</th>";
    excel += "<th>Cantidad</th></tr></thead>";
    var contenido = "<tbody>";
    var nReg = matriz.length;
    for (var o = 0; o < nReg; o++) {
        contenido += "<tr><td style='width:150px;'>" + matriz[o][0] + "</td>";
        contenido += "<td style='width:70px;'>=\"" + matriz[o][4] + "\"</td>";
        contenido += "<td style='width:350px;'>" + matriz[o][5] + "</td>";
        contenido += "<td style='width:100px;'>" + matriz[o][1] + "</td>";
        contenido += "<td style='width:200px;'>" + matriz[o][2] + "</td>";
        contenido += "<td>" + matriz[o][3] + "</td>";
        contenido += "</tr>";
    }
    excel += contenido;
    excel += "</tbody></table></html>";
    return excel;
}

function GrillaCierre(rpta) {
    if (rpta != "") {
        indiceActualBloque = 0;
        var divPaginacion = document.getElementById("divPaginacionCierre");
        divPaginacion.style.display = "inline";
        var lista = rpta.split("_");
        matriz = [];
        crearMatrizCierre(lista);
        mostrarPaginaCierre(0);
        crearPaginasBloqueCierre();
        seleccionarPaginaActualCierre();
    }
    else {
        document.getElementById("tbRegistrosCierre").innerHTML = "<tr style='border-top: 1px solid #222;border-bottom: 1px solid #222'><td colspan='5' style='width:90px;'>No se encontraron registros</td></tr>";
        document.getElementById("divPaginacionCierre").style.display = "none";
        document.getElementById("liPaginaCierre").innerHTML = "";
    }
}

function GrillaEstadistico(rpta) {
    if (rpta != "") {
        indiceActualBloque = 0;
        var divPaginacion = document.getElementById("divPaginacionEstadistico");
        divPaginacion.style.display = "inline";
        var lista = rpta.split("_");
        matriz = [];
        crearMatrizEstadistico(lista);
        mostrarPaginaEstadistico(0);
        crearPaginasBloqueEstadistico();
        seleccionarPaginaActualEstadistico();
    }
    else {
        document.getElementById("tbRegistrosEstadistico").innerHTML = "<tr style='border-top: 1px solid #222;border-bottom: 1px solid #222'><td colspan='5' style='width:90px;'>No se encontraron registros</td></tr>";
        document.getElementById("divPaginacionEstadistico").style.display = "none";
        document.getElementById("liPaginaEstadistico").innerHTML = "";
    }
}

function crearMatrizEstadistico(lista) {
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

function mostrarPaginaEstadistico(indicePagina) {
    indiceActualPagina = indicePagina;
    var nRegistros = matriz.length;
    var contenido = "";
    var campos;
    var inicio = indicePagina * registrosPagina;
    var fin = inicio + registrosPagina;
    for (var z = inicio; z < fin; z++) {
        if (z < nRegistros) {
            contenido += "<tr style='border-top: 1px solid #222;border-bottom: 1px solid #222'>";
            contenido += "<td style='width:150px;'>" + matriz[z][0] + "</td>";
            contenido += "<td style='width:70px;'>" + matriz[z][4] + "</td>";
            contenido += "<td style='width:350px;'>" + matriz[z][5] + "</td>";
            contenido += "<td style='width:100px;'>" + matriz[z][1] + "</td>";
            contenido += "<td style='width:200px;'>" + matriz[z][2] + "</td>";
            contenido += "<td>" + matriz[z][3] + "</td>";
            contenido += "</tr>";
        }
    }
    var tbEstadistico = document.getElementById("tbRegistrosEstadistico");
    tbEstadistico.innerHTML = contenido;
}

function crearPaginasBloqueEstadistico() {
    var contenido = "";
    var existeBloque = (matriz.length > (registrosPagina * paginasBloque));
    if (existeBloque) {
        contenido += "<a href='#' onclick='paginarEstadistico(-1);' title='Ir al primer grupo de páginas'>&lt;&lt;</a>";
        contenido += "<a href='#' onclick='paginarEstadistico(-2);' title='Ir al anterior grupo de páginas'>&lt;</a>";
    }
    var inicio = indiceActualBloque * paginasBloque;
    var indiceUltimaPagina = Math.floor(matriz.length / registrosPagina);
    if (matriz.length % registrosPagina == 0) indiceUltimaPagina--;
    var n;
    for (i = 0; i < paginasBloque; i++) {
        n = inicio + i;
        if (n <= indiceUltimaPagina) {
            contenido += "<a href='#' onclick='paginarEstadistico(" + n + ");'  title='Ir a la pagina " + (n + 1).toString() + "' id='a" + n.toString() + "' class='pagina' >" + (n + 1).toString() + "</a>";
        }
    }
    if (existeBloque) {
        contenido += "<a href='#' onclick='paginarEstadistico(-3);' title='Ir al siguiente grupo de páginas'>&gt;</a>";
        contenido += "<a href='#' onclick='paginarEstadistico(-4);' title='Ir al último grupo de páginas'>&gt;&gt;</a>";
    }
    var tdPagina = document.getElementById("liPaginaEstadistico");
    tdPagina.innerHTML = contenido;
}

function paginarEstadistico(indicePagina) {
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
        mostrarPaginaEstadistico(indicePagina);
        crearPaginasBloqueEstadistico();
    }
    else {
        indiceActualBloque = Math.floor(indicePagina / paginasBloque);
        if (indicePagina > 0 && (indicePagina % paginasBloque == 0)) indiceActualBloque--;
        mostrarPaginaEstadistico(indicePagina);
    }
    var aPaginas = document.getElementsByClassName("pagina");
    var nPaginas = aPaginas.length;
    for (i = 0; i < nPaginas; i++) {
        aPaginas[i].style.backgroundColor = "#fff";
        aPaginas[i].style.color = "#950208";
    }
    seleccionarPaginaActualEstadistico();
}

function seleccionarPaginaActualEstadistico() {
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