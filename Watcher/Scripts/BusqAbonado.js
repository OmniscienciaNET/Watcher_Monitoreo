var registrosPaginaBusq = 25, paginasBloqueBusq = 10, indiceActualBloqueBusq = 0, indiceActualPaginaBusq, listaBusq, matrizBusq = [], matrizFiltro = [];

window.onload = function () {
    BusquedaAbonado();
    Busqueda();
}

function Seleccionar(CodA, DescA, CodB, DescB) {
    document.getElementById('txtAbndCod').value = CodA;
    document.getElementById('txtAbndDesc').value = DescA;
    setCookie("AbndCod", CodA);
    setCookie("AbndDesc", DescA);
    document.getElementById('txtDstbCod').value = CodB;
    document.getElementById('txtDstbDesc').value = DescB;
    setCookie("DstbCod", CodB);
    setCookie("DstbDesc", DescB);
}

function BusquedaAbonado() {
    var divPaginacion = document.getElementById("divPaginacionBusq");
    divPaginacion.style.display = "inline";
    listaBusq = sessionStorage.getItem("DstbAbnd").split("_");
    crearMatriz();
    matrizFiltro = matrizBusq;
    mostrarPagina(0);
    crearPaginasBloque();
    seleccionarPaginaActual();
}

function Busqueda() {
    document.getElementById("btnSearchAbnd").onclick = function () {
        indiceActualBloqueBusq = 0;
        var textos = document.getElementsByClassName("filtrosBusq");
        var nRegistros = matrizBusq.length;
        var con = 0;
        matrizFiltro = [];
        for (var i = 0; i < nRegistros; i++) {
            if (((textos[0].value == "") || (matrizBusq[i][4].toLowerCase().indexOf(textos[0].value.toLowerCase()) > -1)) &&
                    ((textos[1].value == "") || (matrizBusq[i][1].toLowerCase().indexOf(textos[1].value.toLowerCase()) > -1)) &&
                    ((textos[2].value == "") || (matrizBusq[i][2].toLowerCase().indexOf(textos[2].value.toLowerCase()) > -1))) {
                matrizFiltro[con] = matrizBusq[i];
                con++;
            }
        }
        mostrarPagina(0);
        crearPaginasBloque();
        seleccionarPaginaActual();
    }
}

function crearMatriz() {
    var nRegistros = listaBusq.length;
    var nCampos;
    var campos;
    for (i = 0; i < nRegistros; i++) {
        matrizBusq[i] = [];
        campos = listaBusq[i].split("@");
        nCampos = campos.length;
        for (j = 0; j < nCampos; j++) {
            matrizBusq[i][j] = campos[j].trim();
        }
    }
}

function mostrarPagina(indicePagina) {
    indiceActualPaginaBusq = indicePagina;
    var nRegistros = matrizFiltro.length;
    var contenido = "";
    var inicio = indicePagina * registrosPaginaBusq;
    var fin = inicio + registrosPaginaBusq;
    if (nRegistros == 0) {
        contenido += "<tr style='border-top: 1px solid #222;border-bottom: 1px solid #222;'><td colspan='5' style='width:90px;'>No se encontraron registros</td></tr>";
    }
    else {
        for (var z = inicio; z < fin; z++) {
            if (z < nRegistros) {
                contenido += "<tr data-dismiss=\"modal\" onclick=\"Seleccionar('" + matrizFiltro[z][3] + "','" + matrizFiltro[z][4] + "','" + matrizFiltro[z][0] + "','" + matrizFiltro[z][1] + "');\"" +
                             "style='border-top: 1px solid #222;border-bottom: 1px solid #222;cursor:pointer'>";
                contenido += "<td style='width:200px;'>" + matrizFiltro[z][1] + "</td>";
                contenido += "<td style='width:90px;'>" + matrizFiltro[z][3] + "</td>";
                contenido += "<td style='width:300px;'>" + matrizFiltro[z][4] + "</td>";
                contenido += "<td>" + matrizFiltro[z][2] + "</td>";
                contenido += "</tr>";
            }
        }
    }
    var tbVisor = document.getElementById("tbRegistrosBusq");
    tbVisor.innerHTML = contenido;
}

function crearPaginasBloque() {
    var contenido = "";
    var existeBloque = (matrizFiltro.length > (registrosPaginaBusq * paginasBloqueBusq));
    if (existeBloque) {
        contenido += "<a href='#' onclick='paginar(-1);' title='Ir al primer grupo de páginas'>&lt;&lt;</a>";
        contenido += "<a href='#' onclick='paginar(-2);' title='Ir al anterior grupo de páginas'>&lt;</a>";
    }
    var inicio = indiceActualBloqueBusq * paginasBloqueBusq;
    var indiceUltimaPagina = Math.floor(matrizFiltro.length / registrosPaginaBusq);
    if (matrizFiltro.length % registrosPaginaBusq == 0) indiceUltimaPagina--;
    var n;
    for (i = 0; i < paginasBloqueBusq; i++) {
        n = inicio + i;
        if (n <= indiceUltimaPagina) {
            contenido += "<a href='#' onclick='paginar(" + n + ");'  title='Ir a la pagina " + (n + 1).toString() + "' id='a" + n.toString() + "Busq' class='paginaBusq' >" + (n + 1).toString() + "</a>";
        }
    }
    if (existeBloque) {
        contenido += "<a href='#' onclick='paginar(-3);' title='Ir al siguiente grupo de páginas'>&gt;</a>";
        contenido += "<a href='#' onclick='paginar(-4);' title='Ir al último grupo de páginas'>&gt;&gt;</a>";
    }
    var tdPagina = document.getElementById("liPaginaBusq");
    tdPagina.innerHTML = contenido;
}

function paginar(indicePagina) {
    if (indicePagina < 0) {
        var indiceUltimoBloque;
        var indiceUltimaPagina = Math.floor(matrizFiltro.length / registrosPaginaBusq);
        if (matrizFiltro.length % registrosPaginaBusq == 0) indiceUltimaPagina--;
        switch (indicePagina) {
            case -1:
                indiceActualBloqueBusq = 0;
                indicePagina = 0;
                break;
            case -2:
                if (indiceActualBloqueBusq > 0) {
                    indiceActualBloqueBusq--;
                }
                indicePagina = indiceActualBloqueBusq * paginasBloqueBusq;
                break;
            case -3:
                indiceUltimoBloque = Math.floor(indiceUltimaPagina / paginasBloqueBusq);
                if (indiceUltimaPagina % paginasBloqueBusq == 0) indiceUltimoBloque--;
                if (indiceActualBloqueBusq < indiceUltimoBloque) {
                    indiceActualBloqueBusq++;
                }
                indicePagina = indiceActualBloqueBusq * paginasBloqueBusq;
                break;
            case -4:
                indiceUltimoBloque = Math.floor(indiceUltimaPagina / paginasBloqueBusq);
                if (indiceUltimaPagina % paginasBloqueBusq == 0) indiceUltimoBloque--;
                indiceActualBloqueBusq = indiceUltimoBloque;
                indicePagina = indiceUltimoBloque * paginasBloqueBusq;
                break;
        }
        mostrarPagina(indicePagina);
        crearPaginasBloque();
    }
    else {
        indiceActualBloqueBusq = Math.floor(indicePagina / paginasBloqueBusq);
        if (indicePagina > 0 && (indicePagina % paginasBloqueBusq == 0)) indiceActualBloqueBusq--;
        mostrarPagina(indicePagina);
    }
    var aPaginas = document.getElementsByClassName("paginaBusq");
    var nPaginas = aPaginas.length;
    for (i = 0; i < nPaginas; i++) {
        aPaginas[i].style.backgroundColor = "#fff";
        aPaginas[i].style.color = "#950208";
    }
    seleccionarPaginaActual();
}

function seleccionarPaginaActual() {
    var aPagina = document.getElementById("a" + indiceActualPaginaBusq + "Busq");
    if (aPagina != null) {
        aPagina.style.backgroundColor = "#950208";
        aPagina.style.color = "#fff";
    }
}