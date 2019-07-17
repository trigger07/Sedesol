
$("#txtEstudiante").autocomplete({
    source: function (request, response) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "BecaEstudiante/ObtenerEstudiantesActivos",
            data: "{'prefixText':' " + document.getElementById('txtEstudiante').value + " '}",
            dataType: "json",
            success: function (data) {
                if (data.isRedirect) {
                    window.location.href = data.redirectUrl;
                }
                else
                    response(data);
            },
            error: function (jqXHR, exception) {
            }
        });
    },
    minLength: 3,
    select: function (event, ui) {
        log(ui.item ?
          "Selected: " + ui.item.label :
          "Nothing selected, input was " + this.value);
    },
    open: function () {
        $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
    },
    close: function () {
        $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
    }
});