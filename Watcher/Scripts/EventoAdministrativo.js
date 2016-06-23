var InsEdiDel = 0, combo = null, pidcategoria = 0, reloj = null, evento = null, timerAlarmasEvento = null, viewAlarma = 0;
var registrosPagina = 25, paginasBloque = 10, indiceActualBloque = 0, indiceActualPagina, matriz = [];
var salir = 1;
function inicioEventoAdministrativo() {
    window.onload = function () {
        ConfigurarExit();
        if (getCookie("PU") == "2" || parseInt(getCookie("PU")) > 4) {
            document.getElementById("tAlarmasPerfil").innerHTML = "ALARMA";
            document.getElementById("tdAlarmasPerfil").innerHTML = "<table style='width:100%'>" +
                            "<tr id='trListadoAlarmas'><td id='tbListadoAlarmas' style='width:900px'></td></tr>" +
                            "</table>";
        }
        //PopupAlarma();
        //VerAlarma(rpta.Data);
        //reloj = setInterval("DisplayDate();", 1000);
        //MostrarFichaOAlarmas(true);
        //VerAlarma(rpta.Data);
        //reloj = setInterval("DisplayDate();", 1000);
        //timerAlarmasEvento = setInterval("HistorialEvento()", 30000);
        //Habilitar();
        //exitoDataExtra(rpta);
    }

    window.addEventListener("unload", function () {
        if (salir > 0) {

            enviarServidor("../Modulo/LogOut", Exit, false);
        }
    });
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

function enter(event) {
    var keyCode = ('which' in event) ? event.which : event.keyCode;
    if (keyCode == 13) {
        return true;
    }
    return false;
}

function PopupAlarma() {
    $.ajax({
        type: "post",
        url: "../Modulo/PopupMonitoreo",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: exitoPopup,
        error: function (data) { }
    });
}

function exitoPopup(rpta) {
    Deshabilitar();
    if (rpta.Perfil == "2" || parseInt(rpta.Perfil) > 4) {
        var contenido = document.getElementById("tbListadoAlarmas");
        if (rpta.Pend != 1) {
            if (rpta.Pend == 0) {
                contenido.innerHTML = "Hay una alarma pendiente de nombre " + rpta.Data.Descripcion + " del Abonado " + rpta.Data.AbndCode + " ➤ " + rpta.Data.Oficina + " del Distribuidor " + rpta.Data.DealerName + " generada a las " + rpta.Data.Fecha.substring(11);
                document.getElementById("trListadoAlarmas").innerHTML += "<td id='tdAtencion' style='text-align:center'><input id='hdAHID' type='hidden' /><button id='btnMonAsignar' style='width:200px;font-weight:bold'>ASIGNAR</button></td>";
                document.getElementById("hdAHID").innerHTML = rpta.Data.AlarmHistoryID;
                MostrarFichaOAlarmas(false);
                document.getElementById("btnMonAsignar").removeAttribute("disabled");
                var prueba = document.getElementById("iduserall").innerHTML;
                //alert('esat prueba'+prueba);
                var audio = new Audio("../Sounds/API.mp3");
                audio.loop = true;
                audio.play();
                $("#btnMonAsignar").click(function () {
                    $("#btnMonAsignar").unbind("click");
                    $("#tdAtencion").remove();
                    audio.pause();
                    audio = null;
                    $.ajax({
                        type: "post",
                        url: "../Modulo/AsignarAlarma",
                        contentType: "application/json; charset=utf-8",
                        data: "{AlarmHistoryID:" + rpta.Data.AlarmHistoryID + ", user:'" + prueba + "'}",
                        dataType: "json",
                        success: function (data) {
                            if (data != "") {
                                alert(data);
                                PopupAlarma();
                            } else {
                                MostrarFichaOAlarmas(true);
                                VerAlarma(rpta.Data);
                                reloj = setInterval("DisplayDate();", 1000);
                                Habilitar();
                                timerAlarmasEvento = setInterval("HistorialEvento()", 30000);
                            }
                        },
                        error: error
                    });
                });
            }
            else {
                $("#tdAtencion").remove();
                contenido.innerHTML = "No hay alarmas disponibles por el momento, espere mientras se encuentra una alarma";
                MostrarFichaOAlarmas(false);
                PopupAlarma();
            }
        } else {
            MostrarFichaOAlarmas(true);
            VerAlarma(rpta.Data);
            reloj = setInterval("DisplayDate();", 1000);
            timerAlarmasEvento = setInterval("HistorialEvento()", 30000);
            Habilitar();
            exitoDataExtra(rpta);
        }
    }
    else if (rpta.Perfil == "3" || rpta.Perfil == "1") {
        var contenido = $("#tbListadoAlarmas");
        if (rpta.Pend != 1) {
            if (viewAlarma == 0) MostrarFichaOAlarmas(false);
            contenido.empty();
            if (rpta.Data.length == 0) {
                contenido.append("<tr style='border-top: 1px solid #222;border-bottom: 1px solid #222'><td colspan='10' style='text-align:left'>No se encontraron registros</td></tr>");
                document.getElementById("PaginadoAlarma").style.display = "none";
            }
            else {
                document.getElementById("PaginadoAlarma").style.display = "";
                var tr = "";
                for (var i = 0; i < rpta.Data.length; i++) {
                    tr = "<tr style='border-top: 1px solid #222;border-bottom: 1px solid #222'>";
                    if (rpta.Data[i].Escala == 1) tr = "<tr class='parpadea' style='background-color:red;color:white;border-top: 1px solid #222;border-bottom: 1px solid #222'>";
                    contenido.append(tr + "<td style='width: 200px'>" + rpta.Data[i].DealerName + "</td>" +
                                         "<td style='width: 80px'>" + rpta.Data[i].AbndCode + "</td>" +
                                         "<td style='width: 240px'>" + rpta.Data[i].Oficina + "</td>" +
                                         "<td style='width: 80px'>" + rpta.Data[i].Alarmcode + "</td>" +
                                         "<td style='width: 240px'>" + rpta.Data[i].Descripcion + "</td>" +
                                         "<td style='width: 80px;text-align:center'>" + rpta.Data[i].Priority + "</td>" +
                                         "<td style='width: 110px'>" + rpta.Data[i].Fecha + "</td>" +
                                         "<td style='width: 80px" + (rpta.Data[i].Escala != 1 ? (rpta.Data[i].Estado == "SIN ASIGNAR" ? ";background-color:red;color:white" : (rpta.Data[i].Estado == "EN ATENCION" ? ";background-color:yellow" : "")) : "") + "'>" + rpta.Data[i].Estado + "</td>" +
                                         "<td>" + rpta.Data[i].Usuario + "</td>" +
                                         "<td style='width: 80px'><button id='btnView" + i + "' type='button' class='btn btn-primary' style='border-color:red;background-color:red'><img src='../Images/Mantenimiento/view.png' alt='' /></button>" +
                                                                 "<button id='btnAssign" + i + "' type='button' class='btn btn-primary' style='border-color:red;background-color:red'><img src='../Images/Mantenimiento/assign.png' alt='' /></button></td></tr>");
                    $("#btnView" + i).click({ obj: rpta.Data[i] }, function (event) {
                        viewAlarma = 1;
                        VerAlarma(event.data.obj);
                        reloj = setInterval("DisplayDate();", 1000);
                        MostrarFichaOAlarmas(true);
                        document.getElementById("btnMonListadoAlarmas").removeAttribute("disabled");
                        $("#btnMonListadoAlarmas").click(function () {
                            viewAlarma = 0;
                            Deshabilitar();
                            $("#btnMonListadoAlarmas").unbind("click");
                            MostrarFichaOAlarmas(false);
                        });
                    });

                    $("#btnAssign" + i).click({ obj: rpta.Data[i] }, function (event) {
                        $.ajax({
                            type: "post",
                            url: "../Modulo/AsignarAlarma",
                            contentType: "application/json; charset=utf-8",
                            data: "{AlarmHistoryID:" + event.data.obj.AlarmHistoryID + "}",
                            dataType: "json",
                            success: function (data) {
                                if (data != "") {
                                    alert(data);
                                    PopupAlarma();
                                } else {
                                    clearTimeout(timerAlarmasEvento)
                                    MostrarFichaOAlarmas(true);
                                    VerAlarma(event.data.obj);
                                    reloj = setInterval("DisplayDate();", 1000);
                                    $.ajax({
                                        type: "post",
                                        url: "../Modulo/DataPopupSenal",
                                        contentType: "application/json; charset=utf-8",
                                        data: "{AlarmHistoryID:" + $("#MonAlarmhistory").html().trim() + "}",
                                        dataType: "json",
                                        success: exitoDataExtra,
                                        error: error
                                    });
                                    Habilitar();
                                    document.getElementById("btnMonListadoAlarmas").setAttribute("disabled", "disabled");
                                    $("#txtPosicionAlarmas").val("");
                                    $("#btnMonListadoAlarmas, #FirstAlarmas, #BeforeAlarmas, #NextAlarmas, #LastAlarmas").unbind("click");
                                    contenido.empty();
                                    timerAlarmasEvento = setInterval("HistorialEvento()", 30000);
                                }
                            },
                            error: error
                        });
                    });
                    $("#txtPosicionAlarmas").val(rpta.current + 1 + " de " + (rpta.last + 1));
                    $("#FirstAlarmas, #BeforeAlarmas, #NextAlarmas, #LastAlarmas").unbind("click");
                    $("#FirstAlarmas, #BeforeAlarmas, #NextAlarmas, #LastAlarmas").click(function () {
                        $.ajax({
                            type: "post",
                            url: "../Modulo/BusquedaAlarmas",
                            contentType: "application/json; charset=utf-8",
                            data: "{Pagina: '" + this.value + "'}",
                            dataType: "json",
                            success: exitoBusq,
                            error: error
                        });
                    });
                }
            }
            clearTimeout(timerAlarmasEvento);
            timerAlarmasEvento = setTimeout("PopupAlarma()", 30000);
        }
        else {
            MostrarFichaOAlarmas(true);
            VerAlarma(rpta.Data[0]);
            reloj = setInterval("DisplayDate();", 1000);
            var palarmhistoryid = $("#MonAlarmhistory").html().trim();
            $.ajax({
                type: "post",
                url: "../Modulo/DataPopupSenal",
                contentType: "application/json; charset=utf-8",
                data: "{AlarmHistoryID:" + palarmhistoryid + "}",
                dataType: "json",
                success: exitoDataExtra,
                error: error
            });
            Habilitar();
            document.getElementById("btnMonListadoAlarmas").setAttribute("disabled", "disabled");
            $("#txtPosicionAlarmas").val("");
            $("#btnMonListadoAlarmas, #FirstAlarmas, #BeforeAlarmas, #NextAlarmas, #LastAlarmas").unbind("click");
            clearTimeout(timerAlarmasEvento);
            timerAlarmasEvento = setInterval("HistorialEvento()", 30000);
        }
    }
}

function exitoBusq(rpta) {
    var contenido = $("#tbListadoAlarmas");
    contenido.empty();
    if (rpta.Data.length == 0) {
        contenido.append("<tr style='border-top: 1px solid #222;border-bottom: 1px solid #222'><td colspan='10'>No se encontraron registros</td></tr>");
    }
    else {
        var tr = "";
        for (var i = 0; i < rpta.Data.length; i++) {
            tr = "<tr style='border-top: 1px solid #222;border-bottom: 1px solid #222'>";
            if (rpta.Data[i].Escala == 1) tr = "<tr class='parpadea' style='background-color:red;color:white;border-top: 1px solid #222;border-bottom: 1px solid #222'>";
            contenido.append(tr + "<td style='width: 200px'>" + rpta.Data[i].DealerName + "</td>" +
                                 "<td style='width: 80px'>" + rpta.Data[i].AbndCode + "</td>" +
                                 "<td style='width: 240px'>" + rpta.Data[i].Oficina + "</td>" +
                                 "<td style='width: 80px'>" + rpta.Data[i].Alarmcode + "</td>" +
                                 "<td style='width: 240px'>" + rpta.Data[i].Descripcion + "</td>" +
                                 "<td style='width: 80px;text-align:center'>" + rpta.Data[i].Priority + "</td>" +
                                 "<td style='width: 110px'>" + rpta.Data[i].Fecha + "</td>" +
                                 "<td style='width: 80px" + (rpta.Data[i].Escala != 1 ? (rpta.Data[i].Estado == "SIN ASIGNAR" ? ";background-color:red;color:white" : (rpta.Data[i].Estado == "EN ATENCION" ? ";background-color:yellow" : "")) : "") + "'>" + rpta.Data[i].Estado + "</td>" +
                                         "<td>" + rpta.Data[i].Usuario + "</td>" +
                                 "<td>" + rpta.Data[i].Usuario + "</td>" +
                                 "<td style='width: 80px'><button id='btnView" + i + "' type='button' class='btn btn-primary' style='border-color:red;background-color:red'><img src='../Images/Mantenimiento/view.png' alt='' /></button>" +
                                                         "<button id='btnAssign" + i + "' type='button' class='btn btn-primary' style='border-color:red;background-color:red'><img src='../Images/Mantenimiento/assign.png' alt='' /></button></td></tr>");
            $("#btnView" + i).click({ obj: rpta.Data[i] }, function (event) {
                viewAlarma = 1;
                VerAlarma(event.data.obj);
                reloj = setInterval("DisplayDate();", 1000);
                MostrarFichaOAlarmas(true);
                document.getElementById("btnMonListadoAlarmas").removeAttribute("disabled");
                $("#btnMonListadoAlarmas").click(function () {
                    viewAlarma = 0;
                    Deshabilitar();
                    $("#btnMonListadoAlarmas").unbind("click");
                    MostrarFichaOAlarmas(false);
                });
            });

            $("#btnAssign" + i).click({ obj: rpta.Data[i] }, function (event) {
                $.ajax({
                    type: "post",
                    url: "../Modulo/AsignarAlarma",
                    contentType: "application/json; charset=utf-8",
                    data: "{AlarmHistoryID:" + event.data.obj.AlarmHistoryID + "}",
                    dataType: "json",
                    success: function (data) {
                        if (data != "") {
                            alert(data);
                            PopupAlarma();
                        } else {
                            clearTimeout(timerAlarmasEvento)
                            MostrarFichaOAlarmas(true);
                            VerAlarma(event.data.obj);
                            reloj = setInterval("DisplayDate();", 1000);
                            $.ajax({
                                type: "post",
                                url: "../Modulo/DataPopupSenal",
                                contentType: "application/json; charset=utf-8",
                                data: "{AlarmHistoryID:" + $("#MonAlarmhistory").html().trim() + "}",
                                dataType: "json",
                                success: exitoDataExtra,
                                error: error
                            });
                            Habilitar();
                            document.getElementById("btnMonListadoAlarmas").setAttribute("disabled", "disabled");
                            $("#txtPosicionAlarmas").val("");
                            $("#btnMonListadoAlarmas, #FirstAlarmas, #BeforeAlarmas, #NextAlarmas, #LastAlarmas").unbind("click");
                            contenido.empty();
                            timerAlarmasEvento = setInterval("HistorialEvento()", 30000);
                        }
                    },
                    error: error
                });
            });
            $("#txtPosicionAlarmas").val(rpta.current + 1 + " de " + (rpta.last + 1));
        }
    }
}

function DisplayDate() {
    var today = new Date();
    var h = today.getHours();
    var m = today.getMinutes();
    var s = today.getSeconds();
    document.getElementById("relojNivel").innerHTML = "<label style='vertical-align:top'>" + h + "<label style='font-size:20px'>:</label>" + (m <= 9 ? "0" + m : m) + "<label style='font-size:20px'>:</label>" + (s <= 9 ? "0" + s : s) + "</label>";
    evento = evento + 1000;
    MostrarEvento(evento);
}

function HistorialEvento() {
    var palarmhistoryid = $("#MonAlarmhistory").html().trim();
    $.ajax({
        type: "post",
        url: "../Modulo/SenalHistoricoEvento",
        contentType: "application/json; charset=utf-8",
        data: "{AlarmHistoryID: " + palarmhistoryid + "}",
        dataType: "json",
        success: exitoHistorialEvento,
        error: error
    });
}

function Habilitar() {
    var buttons = document.getElementsByTagName("button");
    for (var i = 0; i < buttons.length; i++) {
        if (buttons[i].id.indexOf("Mon") > -1) {
            buttons[i].removeAttribute("disabled");
            if (buttons[i].id.indexOf("MonEm") > -1)
                buttons[i].setAttribute("data-target", "#myModalEmergencia");
            else if (buttons[i].id == "btnMonAlarmFake")
                buttons[i].setAttribute("data-target", "#myResolution");
            else if (buttons[i].id == "btnMonContactoLoc")
                buttons[i].setAttribute("data-target", "#myModal");
            else if (buttons[i].id == "btnMonResSeg")
                buttons[i].setAttribute("data-target", "#myModalSeg");
            else if (buttons[i].id == "btnMonUserLogIn")
                buttons[i].setAttribute("data-target", "#myUserLogIn");
        }
    }

    $("#btnMonEmPolicial").click(function () {
        ConsultaPolicial();
    });

    $("#btnMonEmFuego").click(function () {
        ConsultaFuego();
    });

    $("#btnMonEmMedica").click(function () {
        ConsultaMedica();
    });

    $("#btnMonEmServicios").click(function () {
        ConsultaServicios();
    });

    $("#btnMonEmRondaMovil").click(function () {
        ConsultaRondaMovil();
    });

    $("#btnMonEmStopATM").click(function () {
        ConsultaFraudeATM();
    });

    $("#btnMonNuevo").click(function () {
        if (pidcategoria != "6") {
            if (InsEdiDel == 0) {
                InsEdiDel = 1;
                var palarmhistoryid = $("#MonAlarmhistory").html().trim();
                $.ajax({
                    type: "post",
                    url: "../Modulo/ComboEntidad",
                    contentType: "application/json; charset=utf-8",
                    data: "{palarmhistoryid: " + palarmhistoryid + ", pidcategoria: " + pidcategoria + ", pidentidad: " + -1 + ", Pagina: null}",
                    dataType: "json",
                    success: exitoNuevo,
                    error: error
                });
            }
        } else {
            if (InsEdiDel == 0) {
                InsEdiDel = 1;
                $.ajax({
                    type: "post",
                    url: "../Modulo/ComboMotivo",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: exitoNuevo,
                    error: error
                });
            }
        }
    });

    $("#btnMonAlarmReal").click(function () {
        if (confirm("Desea cerrar la alarma con la resolución ALARMA REAL?")) {
            var ahid = $("#MonAlarmhistory").html().trim();
            $.ajax({
                type: "post",
                url: "../Modulo/GestionAlarma",
                contentType: "application/json; charset=utf-8",
                data: "{AlarmHistoryID: " + ahid + ", Minutes: 0, Resolution: 'AV', opc: 1}",
                dataType: "json",
                success: function (data) {
                    if (data == true) {
                        alert("Se ha cerrado la alarma correctamente");
                    } else {
                        alert("La alarma ha sido escalada, se buscará una alarma sin asignar");
                    }
                    //PopupAlarma();
                    window.location = '../Modulo/Monitoreo';
                },
                error: error
            });
        }
    });

    $("#btnMonAlarmWait").click(function () {
        do {
            var minutes = prompt("Ingrese número de minutos de espera", "");
        } while (minutes != null && (isNaN(parseInt(minutes)) || minutes >= 60 || minutes < 1));
        if (minutes != null) {
            var ahid = $("#MonAlarmhistory").html().trim();
            $.ajax({
                type: "post",
                url: "../Modulo/GestionAlarma",
                contentType: "application/json; charset=utf-8",
                data: "{AlarmHistoryID: " + ahid + ", Minutes: " + minutes + ", Resolution: '', opc: 0}",
                dataType: "json",
                success: function (data) {
                    if (data == true) {
                        alert("Se ha puesto la alarma en espera");
                    } else {
                        alert("La alarma ha sido escalada, se buscará una alarma sin asignar");
                    }
                    //PopupAlarma();
                    window.location = '../Modulo/Monitoreo';
                },
                error: error
            });
        }
    });

    $("#btnMonCloseAlarm").click(function () {
        var ResolutionSelect = document.getElementById("ddlResolution");
        if (confirm("Desea cerrar la alarma con la resolución " + ResolutionSelect.options[ResolutionSelect.selectedIndex].text.toUpperCase() + "?")) {
            var ahid = $("#MonAlarmhistory").html().trim();
            $.ajax({
                type: "post",
                url: "../Modulo/GestionAlarma",
                contentType: "application/json; charset=utf-8",
                data: "{AlarmHistoryID: " + ahid + ", Minutes: 0, Resolution: '" + ResolutionSelect.options[ResolutionSelect.selectedIndex].value + "', opc: 1}",
                dataType: "json",
                success: function (data) {
                    if (data == true) {
                        $("#myResolution").modal("hide");
                        alert("Se ha cerrado la alarma correctamente");
                    } else {
                        alert("La alarma ha sido escalada, se buscará una alarma sin asignar");
                    }
                    //PopupAlarma();
                    window.location = '../Modulo/Monitoreo';
                },
                error: error
            });
        }
    });

    $("#btnMonScalar").click(function () {
        if (confirm("Desea escalar la alarma a un usuario supervisor?")) {
            var ahid = $("#MonAlarmhistory").html().trim();
            $.ajax({
                type: "post",
                url: "../Modulo/GestionAlarma",
                contentType: "application/json; charset=utf-8",
                data: "{AlarmHistoryID: " + ahid + ", Minutes: 0, Resolution: '', opc: 2}",
                dataType: "json",
                success: function (data) {
                    if (data == true) {
                        alert("Se ha escalado la alarma correctamente");
                    } else {
                        alert("La alarma ya ha sido escalada, se buscará una alarma sin asignar");
                    }
                    PopupAlarma();
                },
                error: error
            });
        }
    });



    $("#btnMonUserLogIn").click(function () {
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
                                error: error
                            });
                        }
                    });
                }
            },
            error: error
        });
    });

    $("#txtMonComentario").keypress(function (event) {
        var coment = this;
        if (enter(event)) {
            var ahid = $("#MonAlarmhistory").html().trim();
            var today = new Date();
            d = today.getDate();
            M = today.getMonth() + 1;
            y = today.getFullYear();
            h = today.getHours();
            m = today.getMinutes();
            s = today.getSeconds();
            $.ajax({
                type: "post",
                url: "../Modulo/AgregarComentario",
                contentType: "application/json; charset=utf-8",
                data: "{AlarmHistoryID: " + ahid + ", HoraRequerimiento: '" + (d <= 9 ? "0" + d : d) + "/" + (M <= 9 ? "0" + M : M) + "/" + (y <= 9 ? "0" + y : y) + " " + (h <= 9 ? "0" + h : h) + ":" + (m <= 9 ? "0" + m : m) + ":" + (s <= 9 ? "0" + s : s) + "', Comentario: '" + coment.value + "'}",
                dataType: "json",
                success: function (data) {
                    if (data == "") {
                        document.getElementById("txtMonReportEvent").innerHTML = "Fecha: " + (d <= 9 ? "0" + d : d) + "/" + (M <= 9 ? "0" + M : M) + "/" + (y <= 9 ? "0" + y : y) + " " + (h <= 9 ? "0" + h : h) + ":" + (m <= 9 ? "0" + m : m) + ":" + (s <= 9 ? "0" + s : s) + "\n" + coment.value.toUpperCase() + "\n" + document.getElementById("txtMonReportEvent").innerHTML;
                        coment.value = "";
                    }
                    else {
                        alert(data);
                        PopupAlarma();
                    }
                },
                error: error
            });
        }
    });

    $("#myModalEmergencia").on("hide.bs.modal", function () { InsEdiDel = 0; $("#btnMonNuevo").removeAttr("disabled"); pi = 0; combo = null; });
    $("#myUserLogIn").on("hide.bs.modal", function () { $("#tbUserLogIn").empty(); });
}

function Deshabilitar() {
    LimpiarDatos();
    $("#btnMonEmPolicial, #btnMonEmFuego, #btnMonEmMedica, #btnMonEmServicios, #btnMonEmRondaMovil, #btnMonEmStopATM, #btnMonNuevo, #btnMonCloseAlarm, #btnMonAlarmReal, #btnMonAlarmWait, #btnMonScalar, #btnMonUserLogIn").unbind("click");
    $("#txtMonComentario").unbind("keypress");
    $("#myModalEmergencia").unbind("on");
    $("#myUserLogIn").unbind("on");
    var buttons = document.getElementsByTagName("button");
    for (var i = 0; i < buttons.length; i++) {
        if (buttons[i].id.indexOf("Mon") > -1)
            buttons[i].setAttribute("disabled", "disabled");
        if (buttons[i].id.indexOf("MonEm") > -1)
            buttons[i].removeAttribute("data-target");
    }
    document.getElementById("btnMonAlarmFake").removeAttribute("data-target");
    document.getElementById("btnMonContactoLoc").removeAttribute("data-target");
    document.getElementById("btnMonResSeg").removeAttribute("data-target");
    if (document.getElementById("btnMonUserLogIn") != null) document.getElementById("btnMonUserLogIn").removeAttribute("data-target");
    clearInterval(reloj);
    clearInterval(timerAlarmasEvento);
}

function LimpiarDatos() {
    document.getElementById("MonAlarmaCode").innerHTML = "";
    document.getElementById("MonDescripcion").innerHTML = "";
    document.getElementById("MonPhysicalZone").innerHTML = "";
    document.getElementById("MonNomZona").innerHTML = "";
    document.getElementById("MonTipoEvento").innerHTML = "";
    document.getElementById("MonAlarmhistory").innerHTML = "";
    document.getElementById("MonCSID").innerHTML = "";
    document.getElementById("MonOficina").innerHTML = "";
    document.getElementById("MonDireccion").innerHTML = "";
    document.getElementById("FecOcurrencia").innerHTML = "";
    document.getElementById("txtMonComentario").value = "";
    document.getElementById("txtMonReportEvent").innerHTML = "";
    document.getElementById("relojEvento").innerHTML = "00:00:00";
    document.getElementById("relojNivel").innerHTML = "<label style='vertical-align:top'>00<label style='font-size:20px'>:</label>00<label style='font-size:20px'>:</label>00</label>";
    document.getElementById("MonTipoCSID").innerHTML = "";
    var cabecera = $("#tbCabecera"), tabla = $("#tbRegConsultaEmergencia"), locacion = $("#tbRegcontactoloc"), seguridad = $("#tbRegcontactoseg");
    cabecera.empty();
    tabla.empty();
    locacion.empty();
    seguridad.empty();
}

function MostrarFichaOAlarmas(estado) {
    if (estado == false) {
        document.getElementById("tbAlarmas").style.visibility = "visible";
        document.getElementById("tbMonitoreo").style.visibility = "hidden";
    }
    else {
        document.getElementById("tbAlarmas").style.visibility = "hidden";
        document.getElementById("tbMonitoreo").style.visibility = "visible";
    }
}

function exitoHistorialEvento(rpta) {
    var historial = "";
    for (var m = 0; m < rpta.length; m++) historial += "Fecha:" + rpta[m].Fecha + "\n" + "Alarma: " + rpta[m].AlarmCode + " - " + rpta[m].AlarmDesc + "\n";
    document.getElementById("txtLastHistoryEvent").innerHTML = historial;
}

function exitoDataExtra(rpta) {
    ContactoLocacion(rpta.Data2);
    ContactoSeguridad(rpta.Data3);
    ReporteEventos(rpta.Data4);
}

function VerAlarma(Data) {
    document.getElementById("MonAlarmaCode").innerHTML = Data.Alarmcode;
    document.getElementById("MonDescripcion").innerHTML = Data.Descripcion;
    document.getElementById("MonPhysicalZone").innerHTML = Data.PhysicalZone;
    document.getElementById("MonNomZona").innerHTML = Data.Nomzona;
    document.getElementById("MonTipoEvento").innerHTML = Data.Tipoevento;
    document.getElementById("MonAlarmhistory").innerHTML = Data.AlarmHistoryID;
    document.getElementById("MonCSID").innerHTML = Data.AbndCode;
    document.getElementById("MonOficina").innerHTML = Data.Oficina;
    document.getElementById("MonDireccion").innerHTML = Data.Direccion;
    document.getElementById("FecOcurrencia").innerHTML = Data.Fecha;
    document.getElementById("MonTipoCSID").innerHTML = Data.TipoParticiondes;
    var date = Data.FechaA.split(" ");
    var fecha = date[0].split("/");
    var hora = date[1].split(":");
    evento = Math.abs(new Date() - new Date(fecha[2], fecha[1] - 1, fecha[0], hora[0], hora[1], hora[2]));
    MostrarEvento(evento);
}

function MostrarEvento(evento) {
    document.getElementById("relojEvento").innerHTML = (evento / 3600000 < 10 ? "0" : "") + parseInt(evento / 3600000) + ":" + ((evento / 60000) % 60 < 10 ? "0" : "") + parseInt((evento / 60000) % 60) + ":" + ((evento / 1000) % 60 < 10 ? "0" : "") + parseInt((evento / 1000) % 60);
}

function ConsultaPolicial() {    
    pidcategoria = '1';
    Emergencia(pidcategoria);
}

function ConsultaFuego() {
    pidcategoria = '2';
    Emergencia(pidcategoria);
}

function ConsultaMedica() {
    pidcategoria = '3';
    Emergencia(pidcategoria);
}

function ConsultaServicios() {
    pidcategoria = '4';
    Emergencia(pidcategoria);
}

function ConsultaRondaMovil() {
    pidcategoria = '5';
    Emergencia(pidcategoria);
}

function ConsultaFraudeATM() {
    pidcategoria = '6';
    Emergencia(pidcategoria);
}

function ContactoLocacion(rpta) {
    var tabla = $("#tbRegcontactoloc");
    tabla.empty();
    for (i = 0; i < rpta.length; i++) {
        tabla.append("<tr data-dismiss=\"modal\" style='border-top: 1px solid #222;border-bottom: 1px solid #222;'><td style='width:230px;'>" + rpta[i].Name + "</td>" +
            "<td style='width:350px'>" + rpta[i].Title + "</td>" +
            "<td style='width:230px'>" + rpta[i].PhoneNumber + "</td>" +
            "</tr>");
    }
}

function ContactoSeguridad(rpta) {
    var tabla = $("#tbRegcontactoseg");
    tabla.empty();
    for (i = 0; i < rpta.length; i++) {
        tabla.append("<tr data-dismiss=\"modal\" style='border-top: 1px solid #222;border-bottom: 1px solid #222;'><td style='width:230px;'>" + rpta[i].NombreContact + " " + rpta[i].ApePatContact + " " + rpta[i].ApeMatContact + "</td>" +
            "<td style='width:350px'>" + rpta[i].CargoContact + "</td>" +
            "<td style='width:230px'>" + rpta[i].TelefonoContact + "</td>" +
            "</tr>");
    }
}


function ReporteEventos(rpta) {
    var reporte = "";
    for (var d = 0; d < rpta.length; d++) reporte += "Fecha:" + rpta[d].horarequerimiento + "\n" + rpta[d].Observacion + "\n";
    document.getElementById("txtMonReportEvent").innerHTML = reporte;
}

function Emergencia(pidcategoria) {
    switch (pidcategoria) {
        case "1":
            $("#myConsultaEmergencialLabel").html("EMERGENCIA POLICIAL");
            break;
        case "2":
            $("#myConsultaEmergencialLabel").html("EMERGENCIA INCENDIO");
            break;
        case "3":
            $("#myConsultaEmergencialLabel").html("EMERGENCIA MÉDICA");
            break;
        case "4":
            $("#myConsultaEmergencialLabel").html("EMERGENCIA DE MANTENIMIENTO Y SERVICIOS");
            break;
        case "5":
            $("#myConsultaEmergencialLabel").html("EMERGENCIA RONDA MÓVIL INTERBANK");
            break;
        case "6":
            $("#myConsultaEmergencialLabel").html("EMERGENCIA FRAUDE EN ATM - STOP / APAGADO");
            break;
    }
    $.ajax({
        type: "post",
        url: "../Modulo/ConsultarEmergencia",
        contentType: "application/json; charset=utf-8",
        data: "{pidcategoria: " + pidcategoria + ", Pagina: null}",
        dataType: "json",
        success: exitoConsultarEmergencia,
        error: error
    });
}

function exitoConsultarEmergencia(rpta) {
    if (pidcategoria != "6") {
        var cabecera = $("#tbCabecera");
        cabecera.empty();
        cabecera.append("<tr style='background-color: darkgray; font-size: 9pt;font-weight:bold;height:30px'>" +
                        "<td style='width: 120px; text-align: center'>TIPO UNIDAD </td>" +
                        "<td style='width: 110px; text-align: center'>TELEFONO 01</td>" +
                        "<td style='width: 110px; text-align: center'>TELEFONO 02</td>" +
                        "<td style='width: 110px; text-align: center'>HORA REQUERIMIENTO</td>" +
                        "<td style='width: 110px; text-align: center'>HORA ARRIBO</td>" +
                        "<td style='width: 160px; text-align: center'>NRO. IDENTIFICACION UNIDAD/PERSONA</td>" +
                        "<td style='text-align: center'>NOMBRE RESPONSABLE</td>" +
                        "<td style='width: 80px; text-align: left'></td></tr>")
        var tabla = $("#tbRegConsultaEmergencia");
        tabla.empty();
        for (i = 0; i < rpta.length; i++) {
            tabla.append("<tr id='Tr" + i + "' style='border-top: 1px solid #222;border-bottom: 1px solid #222'>" +
                "<td id='Td" + i + "_1' style='width:120px;'>" + rpta[i].nomentidad + "</td>" +
                "<td id='Td" + i + "_2' style='width:110px'>" + rpta[i].telefono01 + "</td>" +
                "<td id='Td" + i + "_3' style='width:110px'>" + rpta[i].telefono02 + "</td>" +
                "<td id='Td" + i + "_4' style='width:110px'>" + rpta[i].horarequerimiento + "</td>" +
                "<td id='Td" + i + "_5' style='width:110px'>" + rpta[i].horaarribo + "</td>" +
                "<td id='Td" + i + "_6' style='width:160px'>" + rpta[i].nroidenpersontrans + "</td>" +
                "<td id='Td" + i + "_7'>" + rpta[i].nomresponsable + "</td>" +
                "<td id='TdMtto" + i + "' style='width:80px'><button id='btnEdit" + i + "' type='button' class='btn btn-primary'><img src='../Images/Mantenimiento/edit.png' alt='' /></button>" +
                "<button id='btnDel" + i + "' type='button' class='btn btn-primary'><img src='../Images/Mantenimiento/delete.png' alt='' /></button></td>" +
                "</tr>");
        }
        $("[id*=btnEdit]").click(function () {
            if (InsEdiDel == 0) {
                InsEdiDel = 2;
                var id = $(this).attr("id");
                var indice = id.substring(id.length - 1, id.length);
                EditarEmergencia(rpta[indice].identidad, indice);
            }
        });

        $("[id*=btnDel]").click(function () {
            if (InsEdiDel == 0) {
                var id = $(this).attr("id");
                var indice = id.substring(id.length - 1, id.length);
                if (confirm("¿Desea eliminar el registro de Tipo Unidad " + rpta[indice].nomentidad + "?")) {
                    InsEdiDel = 0;
                    EliminarEmergencia(rpta[indice].identidad);
                }
            }
        });
    }
    else {
        var cabecera = $("#tbCabecera");
        cabecera.empty();
        cabecera.append("<tr style='background-color: darkgray; font-size: 9pt;font-weight:bold;height:30px'>" +
                        "<td style='width: 110px; text-align: center'>HORA REQUERIMIENTO</td>" +
                        "<td style='width: 500px; text-align: center'>OBSERVACION</td>" +
                        "<td style='width: 160px; text-align: center'>MOTIVO</td>" +
                        "<td style='text-align: center'>NOMBRE RESPONSABLE</td>" +
                        "<td style='width: 80px; text-align: left'></td></tr>")
        var tabla = $("#tbRegConsultaEmergencia");
        tabla.empty();
        for (i = 0; i < rpta.length; i++) {
            tabla.append("<tr id='Tr" + i + "' style='border-top: 1px solid #222;border-bottom: 1px solid #222'>" +
                "<td id='Td" + i + "_4' style='width:110px'>" + rpta[i].horarequerimiento + "</td>" +
                "<td id='Td" + i + "_5' style='width:500px'>" + rpta[i].Observacion + "</td>" +
                "<td id='Td" + i + "_6' style='width:160px'>" + rpta[i].nomMotivo + "</td>" +
                "<td id='Td" + i + "_7'>" + rpta[i].nomresponsable + "</td>" +
                "<td id='TdMtto" + i + "' style='width:80px'><button id='btnEdit" + i + "' type='button' class='btn btn-primary'><img src='../Images/Mantenimiento/edit.png' alt='' /></button>" +
                "<button id='btnDel" + i + "' type='button' class='btn btn-primary'><img src='../Images/Mantenimiento/delete.png' alt='' /></button></td>" +
                "</tr>");
        }
        $("[id*=btnEdit]").click(function () {
            if (InsEdiDel == 0) {
                InsEdiDel = 2;
                var id = $(this).attr("id");
                var indice = id.substring(id.length - 1, id.length);
                EditarEmergencia(rpta[indice].idMotivo, indice);
            }
        });

        $("[id*=btnDel]").click(function () {
            if (InsEdiDel == 0) {
                var id = $(this).attr("id");
                var indice = id.substring(id.length - 1, id.length);
                if (confirm("¿Desea eliminar el registro con el motivo " + rpta[indice].nomMotivo + "?")) {
                    InsEdiDel = 0;
                    EliminarEmergencia(rpta[indice].idMotivo);
                }
            }
        });
    }
}

function ActualizarDataEmergencia(pidcategoria) {
    var palarmhistoryid = $("#MonAlarmhistory").html().trim();
    $.ajax({
        type: "post",
        url: "../Modulo/ActualizarDataEmergencia",
        contentType: "application/json; charset=utf-8",
        data: "{palarmhistoryid: " + palarmhistoryid + ", pidcategoria: " + pidcategoria + ", Pagina: null}",
        dataType: "json",
        success: exitoConsultarEmergencia,
        error: error
    });
}

function exitoNuevo(rpta) {
    if (pidcategoria != "6") {
        if (rpta.length > 0) {
            $("[id*=btnEdit]").attr("disabled", "disabled");
            $("[id*=btnDel]").attr("disabled", "disabled");
            $("#btnMonNuevo").attr("disabled", "disabled");
            var liscombo = '';
            combo = new Array();
            for (i = 0; i < rpta.length; i++) {
                liscombo += "<option value=" + rpta[i].identidad + " >" + rpta[i].nomentidad + "</option> ";
                combo["" + rpta[i].identidad + ""] = rpta[i].telefono01 + "|" + rpta[i].telefono02;
            }
            var today = new Date();
            d = today.getDate();
            M = today.getMonth() + 1;
            y = today.getFullYear();
            h = today.getHours();
            m = today.getMinutes();
            s = today.getSeconds();
            var tabla = $("#tbRegConsultaEmergencia");
            tabla.append("<tr id='TrNew' style='border-top: 1px solid #222;border-bottom: 1px solid #222;'>" +
                        "<td id='TdNew_1' style='width:120px;'><select id='cboEntidad' style='width:110px;height:20px;font-size:11px'>" + liscombo + "</select></td>" +
                        "<td id='TdNew_2' style='width:110px;'>" + rpta[0].telefono01 + "</td>" +
                        "<td id='TdNew_3' style='width:110px;'>" + rpta[0].telefono02 + "</td>" +
                        "<td id='TdNew_4' style='width:110px'>" + (d <= 9 ? "0" + d : d) + "/" + (M <= 9 ? "0" + M : M) + "/" + (y <= 9 ? "0" + y : y) + " " + (h <= 9 ? "0" + h : h) + ":" + (m <= 9 ? "0" + m : m) + ":" + (s <= 9 ? "0" + s : s) + "</td>" +
                        "<td id='TdNew_5' style='width:110px'>SIN ARRIBO</td>" +
                        "<td id='TdNew_6' style='width:160px'><input type='text' maxlength='20' id='txtNroIdPer' style='width:90%;text-transform:uppercase;font-size:11px'/></td>" +
                        "<td id='TdNew_7'><input type='text' maxlength='60' id='txtNomResp' style='width:90%;text-transform:uppercase;font-size:11px'/></td>" +
                        "<td id='TdMttoNew' style='width:80px'><button id='btnGrabar' type='button' class='btn btn-primary'><img src='../Images/Mantenimiento/save.png' /></button>" +
                            "<button id='btnCancelar' type='button' class='btn btn-primary'><img src='../Images/Mantenimiento/cancel.png' /></button></td>" +
                        "</tr>");

            $("#btnGrabar").click(function () {
                if (document.getElementById("txtNroIdPer").value != "") {
                    if (document.getElementById("txtNomResp").value != "") {
                        Grabar(-1, 0);
                        combo = null;
                    }
                    else document.getElementById("txtNomResp").focus();
                }
                else document.getElementById("txtNroIdPer").focus();
            });

            $("#btnCancelar").click(function () {
                NormalizarRegistro(-1, "", "", "", "", "", "");
                combo = null;
            });

            $("#cboEntidad").change(function () {
                $("#TdNew_2").html((combo[$(this).val()]).split("|")[0]);
                $("#TdNew_3").html((combo[$(this).val()]).split("|")[1]);
            });
            document.getElementById("txtNroIdPer").focus();
        }
        else {
            alert("No hay más tipos de unidades que agregar a la emergencia");
            InsEdiDel = 0;
        }
    } else {
        if (rpta.length > 0) {
            $("[id*=btnEdit]").attr("disabled", "disabled");
            $("[id*=btnDel]").attr("disabled", "disabled");
            $("#btnMonNuevo").attr("disabled", "disabled");
            var liscombo = '';
            for (i = 0; i < rpta.length; i++) {
                liscombo += "<option value=" + rpta[i].idMotivo + " >" + rpta[i].nomMotivo + "</option> ";
            }
            var today = new Date();
            d = today.getDate();
            M = today.getMonth() + 1;
            y = today.getFullYear();
            h = today.getHours();
            m = today.getMinutes();
            s = today.getSeconds();
            var tabla = $("#tbRegConsultaEmergencia");
            tabla.append("<tr id='TrNew' style='border-top: 1px solid #222;border-bottom: 1px solid #222;'>" +
                        "<td id='TdNew_4' style='width:110px'>" + (d <= 9 ? "0" + d : d) + "/" + (M <= 9 ? "0" + M : M) + "/" + (y <= 9 ? "0" + y : y) + " " + (h <= 9 ? "0" + h : h) + ":" + (m <= 9 ? "0" + m : m) + ":" + (s <= 9 ? "0" + s : s) + "</td>" +
                        "<td id='TdNew_5' style='width:500px'><textarea id='txtObservacion' maxlength='500' style='width:90%;text-transform:uppercase;font-size:11px;resize:none;height:30px' value=''></textarea></td>" +
                        "<td id='TdNew_6' style='width:160px'><select id='cboMotivo' style='width:150px;height:20px;font-size:11px'>" + liscombo + "</select></td>" +
                        "<td id='TdNew_7'><input type='text' maxlength='60' id='txtNomResp' style='width:90%;text-transform:uppercase;font-size:11px'/></td>" +
                        "<td id='TdMttoNew' style='width:80px'><button id='btnGrabar' type='button' class='btn btn-primary'><img src='../Images/Mantenimiento/save.png' /></button>" +
                            "<button id='btnCancelar' type='button' class='btn btn-primary'><img src='../Images/Mantenimiento/cancel.png' /></button></td>" +
                        "</tr>");

            $("#btnGrabar").click(function () {
                if (document.getElementById("txtObservacion").value != "") {
                    if (document.getElementById("txtNomResp").value != "") Grabar(-1, 0);
                    else document.getElementById("txtNomResp").focus();
                }
                else document.getElementById("txtObservacion").focus();
            });

            $("#btnCancelar").click(function () {
                NormalizarRegistro(-1, "", "", "", "", "", "");
            });
            document.getElementById("txtObservacion").focus();
        }
        else {
            alert("No hay más Motivos que agregar a la emergencia");
            InsEdiDel = 0;
        }
    }
}

function EditarEmergencia(idEntMot, indice) {
    $("[id*=btnEdit]").attr("disabled", "disabled");
    $("[id*=btnDel]").attr("disabled", "disabled");
    $("#btnMonNuevo").attr("disabled", "disabled");
    if (pidcategoria != "6") {
        var campo1 = $("#Td" + indice + "_1").html();
        var campo5 = $("#Td" + indice + "_5").html();
        var campo6 = $("#Td" + indice + "_6").html();
        var campo7 = $("#Td" + indice + "_7").html();

        var today = new Date();
        d = today.getDate();
        M = today.getMonth() + 1;
        y = today.getFullYear();
        h = today.getHours();
        m = today.getMinutes();
        s = today.getSeconds();
        $("#Td" + indice + "_5").html((d <= 9 ? "0" + d : d) + "/" + (M <= 9 ? "0" + M : M) + "/" + (y <= 9 ? "0" + y : y) + " " + (h <= 9 ? "0" + h : h) + ":" + (m <= 9 ? "0" + m : m) + ":" + (s <= 9 ? "0" + s : s));
        $("#Td" + indice + "_6").html("<input id='txtNroIdPer' type='text' maxlength='20' style='width:90%;text-transform:uppercase;font-size:11px' value='" + campo6 + "'></input>");
        $("#Td" + indice + "_7").html("<input id='txtNomResp' type='text' maxlength='60' style='width:90%;text-transform:uppercase;font-size:11px' value='" + campo7 + "'></input>");
        $("#TdMtto" + indice).html("<button id='btnGrabar' type='button' class='btn btn-primary'><img src='../Images/Mantenimiento/save.png' /></button>" +
                                    "<button id='btnCancelar' type='button' class='btn btn-primary'><img src='../Images/Mantenimiento/cancel.png' /></button>");

        $("#btnCancelar").click(function () {
            NormalizarRegistro(indice, campo1, idEntMot, $("#Td" + indice + "_4").html(), campo5, campo6, campo7);
            combo = null;
        });

        document.getElementById("txtNroIdPer").focus();

        var palarmhistoryid = $("#MonAlarmhistory").html().trim();
        var pidEntMot = idEntMot
        var idEntMotAct = 0;
        $.ajax({
            type: "post",
            url: "../Modulo/ComboEntidad",
            contentType: "application/json; charset=utf-8",
            data: "{palarmhistoryid: " + palarmhistoryid + ", pidcategoria: " + pidcategoria + ", pidentidad: " + pidEntMot + ", Pagina: null}",
            dataType: "json",
            success: function (data) {
                var liscombo = '', opCombo = campo1.toUpperCase();
                combo = new Array();
                for (i = 0; i < data.length; i++) {
                    combo["" + data[i].identidad + ""] = data[i].telefono01 + "|" + data[i].telefono02;
                    if (data[i].nomentidad == opCombo) {
                        idEntMotAct = data[i].identidad;
                        liscombo += "<option value=" + data[i].identidad + " selected='selected' >" + data[i].nomentidad + "</option> ";
                    }
                    else liscombo += "<option value=" + data[i].identidad + " >" + data[i].nomentidad + "</option> ";
                }
                $("#Td" + indice + "_1").html("<select id='cboEntidad' style='width:110px;height:20px;font-size:11px'>" + liscombo + "</select>");

                $("#btnGrabar").click(function () {
                    if (document.getElementById("txtNroIdPer").value != "") {
                        if (document.getElementById("txtNomResp").value != "") {
                            if (idEntMotAct != 0) Grabar(indice, idEntMotAct);
                            combo = null;
                        }
                        else document.getElementById("txtNomResp").focus();
                    }
                    else document.getElementById("txtNroIdPer").focus();
                });

                $("#cboEntidad").change(function () {
                    $("#Td" + indice + "_2").html((combo[$(this).val()]).split("|")[0]);
                    $("#Td" + indice + "_3").html((combo[$(this).val()]).split("|")[1]);
                });
            },
            error: error
        });
    } else {
        var campo5 = $("#Td" + indice + "_5").html();
        var campo6 = $("#Td" + indice + "_6").html();
        var campo7 = $("#Td" + indice + "_7").html();

        $("#Td" + indice + "_5").html("<textarea id='txtObservacion' maxlength='500' style='width:90%;text-transform:uppercase;font-size:11px;resize:none;height:30px' value='" + campo5 + "'></textarea>");
        $("#Td" + indice + "_7").html("<input id='txtNomResp' type='text' maxlength='60' style='width:90%;text-transform:uppercase;font-size:11px' value='" + campo7 + "'></input>");
        $("#TdMtto" + indice).html("<button id='btnGrabar' type='button' class='btn btn-primary'><img src='../Images/Mantenimiento/save.png' /></button>" +
                                    "<button id='btnCancelar' type='button' class='btn btn-primary'><img src='../Images/Mantenimiento/cancel.png' /></button>");

        $("#btnCancelar").click(function () {
            NormalizarRegistro(indice, campo6, idEntMot, $("#Td" + indice + "_4").html(), "", campo5, campo7);
        });

        document.getElementById("txtObservacion").focus();

        var idEntMotAct = 0;
        $.ajax({
            type: "post",
            url: "../Modulo/ComboMotivo",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var liscombo = '', opCombo = campo6.toUpperCase();
                for (i = 0; i < data.length; i++) {
                    if (data[i].nomMotivo == opCombo) {
                        idEntMotAct = data[i].idMotivo;
                        liscombo += "<option value=" + data[i].idMotivo + " selected='selected' >" + data[i].nomMotivo + "</option> ";
                    }
                    else liscombo += "<option value=" + data[i].idMotivo + " >" + data[i].nomMotivo + "</option> ";
                }
                $("#Td" + indice + "_6").html("<select id='cboMotivo' style='width:150px;height:20px;font-size:11px'>" + liscombo + "</select>");

                $("#btnGrabar").click(function () {
                    if (document.getElementById("txtObservacion").value != "") {
                        if (document.getElementById("txtNomResp").value != "") {
                            if (idEntMotAct != 0) Grabar(indice, idEntMotAct);
                        }
                        else document.getElementById("txtNomResp").focus();
                    }
                    else document.getElementById("txtObservacion").focus();
                });
            },
            error: error
        });
    }
}

function Grabar(indice, idEntMotOld) {
    var alarmhistoryid = $("#MonAlarmhistory").html().trim();
    if (pidcategoria != "6") {
        var identidadnew = $("#cboEntidad").val();
        var identidadold = 0;
        var horarequerimiento = $("#TdNew_4").html();
        var horaarribo = "";
        if (InsEdiDel == 2) {
            identidadold = idEntMotOld;
            horaarribo = $("#Td" + indice + "_5").html();
        }
        var nroidenpersontrans = $("#txtNroIdPer").val();
        var nomresponsable = $("#txtNomResp").val();
        $.ajax({
            type: "post",
            url: "../Modulo/InsertarEditarEmergencia",
            contentType: "application/json; charset=utf-8",
            data: "{Alarmhistoryid: " + alarmhistoryid + ", IdentidadOld: " + identidadold + ", IdentidadNew: " + identidadnew + ", Horarequerimiento: '" + horarequerimiento + "', Horaarribo: '" + horaarribo + "', Nroidenpersontrans: '" + nroidenpersontrans + "', Nomresponsable: '" + nomresponsable + "'}",
            dataType: "json",
            success: function (data) {
                if (InsEdiDel == 1) {
                    if (data == true) {
                        alert('Se agregó correctamente el registro');
                        ActualizarDataEmergencia(pidcategoria);
                        InsEdiDel = 0;
                        $("#btnMonNuevo").removeAttr("disabled");
                    }
                    else {
                        alert('No se pudo agregar el registro');
                    }
                }
                else if (InsEdiDel == 2) {
                    if (data == true) {
                        alert('El registro fue actualizado correctamente.');
                        ActualizarDataEmergencia(pidcategoria);
                        InsEdiDel = 0;
                        $("#btnMonNuevo").removeAttr("disabled");
                    }
                    else {
                        alert('El registro no fue actualizado');
                    }
                }
            },
            error: error
        });
    } else {
        var idMotivoNew = $("#cboMotivo").val();
        var idMotivoOld = 0;
        var horarequerimiento = $("#TdNew_4").html();
        if (InsEdiDel == 2) idMotivoOld = idEntMotOld;
        var observacion = $("#txtObservacion").val();
        var nomresponsable = $("#txtNomResp").val();
        $.ajax({
            type: "post",
            url: "../Modulo/InsertarEditarEmergenciaStopFraude",
            contentType: "application/json; charset=utf-8",
            data: "{Alarmhistoryid: " + alarmhistoryid + ", IdMotivoOld: " + idMotivoOld + ", IdMotivoNew: " + idMotivoNew + ", Horarequerimiento: '" + horarequerimiento + "', Observacion: '" + observacion + "', Nomresponsable: '" + nomresponsable + "'}",
            dataType: "json",
            success: function (data) {
                if (InsEdiDel == 1) {
                    if (data == true) {
                        alert('Se agregó correctamente el registro');
                        ActualizarDataEmergencia(pidcategoria);
                        InsEdiDel = 0;
                        $("#btnMonNuevo").removeAttr("disabled");
                    }
                    else {
                        alert('No se pudo agregar el registro');
                    }
                }
                else if (InsEdiDel == 2) {
                    if (data == true) {
                        alert('El registro fue actualizado correctamente.');
                        ActualizarDataEmergencia(pidcategoria);
                        InsEdiDel = 0;
                        $("#btnMonNuevo").removeAttr("disabled");
                    }
                    else {
                        alert('El registro no fue actualizado');
                    }
                }
            },
            error: error
        });
    }
}

function NormalizarRegistro(indice, nomEntMot, idEntMot, hrareq, hraarribo, nroIdPerObser, nomresp) {
    $("[id*=btnEdit]").removeAttr("disabled");
    $("[id*=btnDel]").removeAttr("disabled");
    $("#btnMonNuevo").removeAttr("disabled");
    if (pidcategoria != "6") {
        if (indice != -1) {
            $("#Td" + indice + "_1").html(nomEntMot);
            $("#Td" + indice + "_4").html(hrareq);
            $("#Td" + indice + "_5").html(hraarribo);
            $("#Td" + indice + "_6").html(nroIdPerObser);
            $("#Td" + indice + "_7").html(nomresp);
            $("#TdMtto" + indice).html("<button id='btnEdit" + indice + "' type='button' class='btn btn-primary'><img src='../Images/Mantenimiento/edit.png' alt='' /></button>" +
                                    "<button id='btnDel" + indice + "' type='button' class='btn btn-primary'><img src='../Images/Mantenimiento/delete.png' alt='' /></button>");
            $("#btnEdit" + indice).click(function () {
                if (InsEdiDel == 0) {
                    InsEdiDel = 2;
                    EditarEmergencia(idEntMot, indice);
                }
            });

            $("#btnDel" + indice).click(function () {
                if (InsEdiDel == 0) {
                    if (confirm("¿Desea eliminar el registro de Tipo Unidad " + nomEntMot + "?")) {
                        InsEdiDel = 0;
                        EliminarEmergencia(idEntMot);
                    }
                }
            });
        }
        else $("#TrNew").remove();
        InsEdiDel = 0;
    } else {
        if (indice != -1) {
            $("#Td" + indice + "_4").html(hrareq);
            $("#Td" + indice + "_5").html(nroIdPerObser);
            $("#Td" + indice + "_6").html(nomEntMot);
            $("#Td" + indice + "_7").html(nomresp);
            $("#TdMtto" + indice).html("<button id='btnEdit" + indice + "' type='button' class='btn btn-primary'><img src='../Images/Mantenimiento/edit.png' alt='' /></button>" +
                                    "<button id='btnDel" + indice + "' type='button' class='btn btn-primary'><img src='../Images/Mantenimiento/delete.png' alt='' /></button>");
            $("#btnEdit" + indice).click(function () {
                if (InsEdiDel == 0) {
                    InsEdiDel = 2;
                    EditarEmergencia(idEntMot, indice);
                }
            });

            $("#btnDel" + indice).click(function () {
                if (InsEdiDel == 0) {
                    if (confirm("¿Desea eliminar el registro del motivo " + nomEntMot + "?")) {
                        InsEdiDel = 0;
                        EliminarEmergencia(idEntMot);
                    }
                }
            });
        }
        else $("#TrNew").remove();
        InsEdiDel = 0;
    }
}

function EliminarEmergencia(idEntMot) {
    var alarmhistoryid = $("#MonAlarmhistory").html().trim();
    if (pidcategoria != "6") {
        $.ajax({
            type: "post",
            url: "../Modulo/EliminarEmergencia",
            contentType: "application/json; charset=utf-8",
            data: "{alarmhistoryid: " + alarmhistoryid + ", identidad: " + idEntMot + "}",
            dataType: "json",
            success: exitoEliminar,
            error: error
        });
    } else {
        $.ajax({
            type: "post",
            url: "../Modulo/EliminarEmergenciaStopFraude",
            contentType: "application/json; charset=utf-8",
            data: "{alarmhistoryid: " + alarmhistoryid + ", idMotivo: " + idEntMot + "}",
            dataType: "json",
            success: exitoEliminar,
            error: error
        });
    }
}

function exitoEliminar(rpta) {
    if (rpta == true) {
        alert('Se eliminó el registro correctamente');
        ActualizarDataEmergencia(pidcategoria);
    }
    else {
        alert('No se pudo eliminar el registro');
    }
}

function error(rpta) {
    alert(rpta);
}

////CODIGO EVENTO ADMINISTRATIVO


$("#btnRegistrar").click(function () {
    Registrar();    
    Habilitar();
    reloj = setInterval("DisplayDate();", 1000);
    //PopupAlarma();
    //VerAlarma(rpta.Data);    
    //MostrarFichaOAlarmas(true);
    //VerAlarma(rpta.Data);    
    timerAlarmasEvento = setInterval("HistorialEvento()", 30000);
    //Habilitar();
    //exitoDataExtra(rpta);
    
});

$("#btnSelectAlarmAdm").click(function () {

    CargarAlarmAdm();
});

$("#btnBusSubscriber").click(function () {

    ListarSubscriber();    
    
});



function Registrar() {

    var alarmcode = document.getElementById("MonAlarmaCode").innerHTML;
    var csid = document.getElementById("MonCSID").innerHTML;
    var Physicalzone = document.getElementById("MonPhysicalZone").innerHTML;
    // var SignalIdentifier = document.getElementById("txtSignalIdentifier");
    //var area = document.getElementById("txtArea");
    //var Dealercode = document.getElementById("txtDealercode");

    // alert(alarmcode + ", csid: " + csid + ", Physicalzone: " + Physicalzone);

    if (confirm("¿Desea registrar la alarma?")) {
        //var ahid = $("#MonAlarmhistory").html().trim();        

        $.ajax({
            type: "post",
            url: "../Modulo/EventoAdministrativoup",
            contentType: "application/json; charset=utf-8",
            data: "{alarmcode: '" + alarmcode + "', csid: '" + csid + "', Physicalzone: '" + Physicalzone + "'}",
            //data: "{AlarmHistoryID:" + rpta.Data.AlarmHistoryID + ", user:'" + prueba + "'}",
            dataType: "json",
            success: function (data) {
                document.getElementById("MonAlarmhistory").innerHTML = data.codigo;
                document.getElementById("FecOcurrencia").innerHTML = data.fecha;
                alert(data.msm);

            },
            error: error
        });
    }

}


function CargarAlarmAdm() {

   $("#tbRegAlarmAdm").empty();

            $.ajax({
                type: "post",
                url: "../Modulo/ListarAlarmAdm",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {                    
                    var contenido = $("#tbRegAlarmAdm");
                    if (data.length == 0) {
                        contenido.append("<tr style='border-top: 1px solid #222;border-bottom: 1px solid #222'><td colspan='10'>No se encontraron registros</td></tr>");
                    }
                    else {
                        var tr = "";
                        for (var i = 0; i < data.length; i++) {
                            tr = "<tr id='TR_User_" + data[i].AlarmCode + "' style='border-top: 1px solid #222;border-bottom: 1px solid #222;cursor:pointer' onmouseover=\"this.style.backgroundColor='red';this.style.color='white';\" onmouseout=\"this.style.backgroundColor='white';this.style.color='black';\">";
                            contenido.append(tr + "<td id='TD1_User_" + data[i].AlarmCode + "'>"+ data[i].AlarmCode +"</td> <td id='TD2_User_" + data[i].AlarmCode + "' style='width: 200px'>" + data[i].Description + "</td></tr>");
                        }

                        $("[id*=TR_User_]").click(function () {
                                var id = this.id;
                                var palarmcode = id.substring(8, id.length);
                                var txtalarma=document.getElementById("TD2_User_" + palarmcode ).innerHTML;                                
                                document.getElementById('MonAlarmaCode').innerHTML = palarmcode;
                                document.getElementById('MonDescripcion').innerHTML = txtalarma;
                            //document.getElementById('txtLocalDesc').value = LocalDesc;                            
                                $("#myModalAlarmAdm").modal("hide");
                        });

                    }
                },
                //error: error
            });

}

function ListarSubscriber() {

    var csid = $("#txtParticionbus").val();
    var subscribername = $("#txtOficinabus").val();

    $("#tbRegSubscriber").empty();

    $.ajax({
        type: "post",
        url: "../Modulo/Listarsubscriber",
        contentType: "application/json; charset=utf-8",
        data: "{csid: '" + csid + "', subscribername: '" + subscribername + "'}",
        dataType: "json",
        success: function (data) {            
            var contenido = $("#tbRegSubscriber");
            if (data.length == 0) {
                contenido.append("<tr style='border-top: 1px solid #222;border-bottom: 1px solid #222'><td colspan='10'>No se encontraron registros</td></tr>");
            }
            else {
                var tr = "";
                for (var i = 0; i < data.length; i++) {

                    tr = "<tr id='TR_User_" + data[i].CSID + "' style='border-top: 1px solid #222;border-bottom: 1px solid #222;cursor:pointer' onmouseover=\"this.style.backgroundColor='red';this.style.color='white';\" onmouseout=\"this.style.backgroundColor='white';this.style.color='black';\">";
                    contenido.append(tr + "<td id='TD1_User_" + data[i].CSID + "'style='width: 50px'>" + data[i].CSID + "</td> <td id='TD2_User_" + data[i].CSID + "' style='width: 200px'>" + data[i].SubscriberName + "</td> <td id='TD3_User_" + data[i].CSID + "' style=' display:none'>" + data[i].AddressStreet + "</td> <td id='TD4_User_" + data[i].CSID + "' style=' display:none'>" + data[i].dpto + "</td><td id='TD5_User_" + data[i].CSID + "' style=' display:none'>" + data[i].prov + "</td><td id='TD6_User_" + data[i].CSID + "' style='display:none'>" + data[i].dist + "</td><td id='TD7_User_" + data[i].CSID + "' style='display:none'>" + data[i].TipoParticiondes + "</td></tr>");
                }

                $("[id*=TR_User_]").click(function () {
                    var id = this.id;
                    var palarmcode = id.substring(8, id.length);
                    var txtalarma = document.getElementById("TD2_User_" + palarmcode).innerHTML;
                    var txtdireccion = document.getElementById("TD3_User_" + palarmcode).innerHTML;
                    var txtdpto = document.getElementById("TD4_User_" + palarmcode).innerHTML;
                    var txtprov = document.getElementById("TD5_User_" + palarmcode).innerHTML;
                    var txtdist = document.getElementById("TD6_User_" + palarmcode).innerHTML;
                    var txttipoparticiondes = document.getElementById("TD7_User_" + palarmcode).innerHTML;

                    document.getElementById('MonCSID').innerHTML = palarmcode;
                    document.getElementById('MonOficina').innerHTML = txtalarma;
                    document.getElementById('MonDireccion').innerHTML = txtdireccion;
                    document.getElementById('MonDepartamento').innerHTML = txtdpto;
                    document.getElementById('MonProvincia').innerHTML = txtprov;
                    document.getElementById('MonDistrito').innerHTML = txtdist;
                    document.getElementById('MonTipoCSID').innerHTML = txttipoparticiondes;
                    //document.getElementById('txtLocalDesc').value = LocalDesc;
                    $("#myModalSubscriber").modal("hide");
                });
            }
        },        
        error: error
    });
}

