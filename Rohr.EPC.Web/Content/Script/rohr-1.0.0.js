$(document).ready(function () {
    $(function submitPesquisa() {
        $('#contentBody_txtPesquisar').keypress(function (e) {
            var key = e.which;
            if (key == 13) {
                if ($(this).val().length >= 1) {
                    $('#contentBody_linkButtonPesquisar').focus();
                    __doPostBack('ctl00$contentBody$linkButtonPesquisar', '');
                }
            }
        });
    });

    $("#fixedTopHeader").scrollToFixed({ marginTop: 42 });

    $("#tableDocumentos tbody tr").click(function (event) {
        if (event.target.type !== "checkbox") {
            $(":checkbox", this).trigger("click");
        }
    });

    $("#checkBoxAll").click(function () {
        $(this).closest("table").find("td input:checkbox").prop("checked", this.checked);
    });

    $("#tablePesquisaDocumento tr").click(function (event) {
        if (event.target.type !== "checkbox") {
            $(":checkbox", this).trigger("click");
        }
    });
    $(".tabelaObjeto tr").click(function (event) {
        if (event.target.type !== "checkbox") {
            $(":checkbox", this).trigger("click");
        }
    });
    $(".tabelaObjeto tr td input[type=checkbox]").click(function () {
        $(this).closest("tr").toggleClass("highlightObjeto");
    });
    $("#contentBody_tableColunas input[type=checkbox]").click(function () {
        var coluna = $(this).attr("id");
        if (coluna.substring(0, 12) === "contentBody_") {
            coluna = (coluna.substring(coluna.length - 2, coluna.length));
            if ($.isNumeric(coluna)) {
                $("#contentBody_panelObjeto th:nth-child(" + coluna + ')').fadeToggle();
                $("#contentBody_panelObjeto td:nth-child(" + coluna + ')').fadeToggle();
            }
        }
    });

    $("ol.progtrckr").each(function () {
        $(this).attr("data-progtrckr-steps", $(this).children("li").length);
    });

    var totalWidth = 0;
    $(".progtrckr-done").each(function () {
        totalWidth += $(this).outerWidth();
    });

    $(".scrolls").animate({
        scrollLeft: (totalWidth - 270)
    }, 1500);

    contarItensSelecionados();
    verificarDocumentosSelecionados();
    habilitarClickLinha();
    pesquisaTela();

    CheckColunaObjeto();

    $(".checkBoxAllObjeto").click(function () {
        if ($(this).prop("checked") === false) {
            $(this).closest("table").find("tbody tr").addClass("highlightObjeto");
        } else {
            $(this).closest("table").find("tbody tr").removeClass("highlightObjeto");
        }
        $(this).closest("table").find("tr input:checkbox").prop("checked", this.checked);
    });

    $(".tabelaObjeto").each(function () {

        if ($(this).closest("table").find("tr td input[type=checkbox]:checked").length > 0)
            $(this).closest("table").find("tr th input[type=checkbox]").prop("checked", true);
        else
            $(this).closest("table").find("tr th input[type=checkbox]").prop("checked", false);
    });

    $(".tabelaObjeto").closest("table").find("tr td input[type=checkbox]:not(:checked)").each(function () {
        $(this).closest("tr").toggleClass("highlightObjeto");
    });
});

function habilitarClickLinha() {
    $(".tableDocumentos #table tr").click(function (event) {
        if (event.target.type !== "checkbox") {
            $(":checkbox", this).trigger("click");
        }
    });

    $(".objeto input[type=checkbox]").click(function () {
        $(this).closest("tr").toggleClass("highlight");
    });
}

function ClientClickDisable(botao) {
    botao.disabled = true;
    document.getElementById("imgLoad").style.display = "inline";
}

function contarItensSelecionados() {
    var contarChecked = function () {


        var n = $("#tableDocumentos tbody tr input:checked").length;
        if (n === 0)
            $("#totalDocSelecionados").text("Nenhum documento selecionado");
        else {
            $("#totalDocSelecionados").text(n + (n === 1 ? " documento selecionado" : " documentos selecionados"));
            var totalRegistros = $("#tableDocumentos tbody tr input:checkbox").length;

            if (totalRegistros === n)
                $("#checkBoxAll").prop("checked", this.checked);
        }
    };

    $("input[type=checkbox]").on("click", contarChecked);

    contarChecked();
};

function verificarDocumentosSelecionados() {
    if ($("#tableDocumentos tbody>tr").length === 0) {
        $("#totalDocumentos").text("Nenhum documento disponível para exibição");
    }
    else if ($("#tableDocumentos tbody>tr").length === 1)
        $("#totalDocumentos").text("Exibindo 1 documento");
    else
        $("#totalDocumentos").text("Exibindo " + $("#tableDocumentos tbody>tr").length + " documentos");
};

function pesquisaTela() {
    if ($("#tableDocumentos tbody>tr").length === 0)
        $("#totalDocumentos").text("Nenhum documento disponível para exibição");
    else if ($("#table tbody>tr").length === 1)
        $("#totalDocumentos").text("Exibindo 1 documento");
    else
        $("#totalDocumentos").text("Exibindo " + $("#tableDocumentos tbody>tr").length + " documentos");
}
function reloadPage() {
    location.reload();
}

function finaliza() {
    var xmlHttp;
    try {
        xmlHttp = new XMLHttpRequest();
    } catch (e1) {
        try {
            xmlHttp = new ActiveXObject("Msxml12.XMLHTTP");
        } catch (e2) {
            xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
        }
    }

    xmlHttp.open("GET", "Redirecionar.aspx?p=o1pYxTAzZds=", true);
    xmlHttp.send(null);
}

function CheckColunaObjeto() {
    $("#contentBody_tableColunas input[type=checkbox]:not(:checked)").each(function () {
        var coluna = $(this).attr("id");
        if (coluna.substring(0, 12) === "contentBody_") {
            coluna = (coluna.substring(coluna.length - 2, coluna.length));
            if ($.isNumeric(coluna)) {
                $("#contentBody_panelObjeto th:nth-child(" + coluna + ')').fadeToggle();
                $("#contentBody_panelObjeto td:nth-child(" + coluna + ')').fadeToggle();
            }
        }
    });
}