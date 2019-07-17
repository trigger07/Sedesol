function encode(data)
{
  var str = "";
  for (var i = 0; i < data.length; i++)
    str += String.fromCharCode(data[i]);

  return btoa(str).split(/(.{75})/).join("\n").replace(/\n+/g, "\n").trim();
  }


function ShowImageModal(type, idSource) {
        /*
            type:
                1 = Activities
                2 = Details
        */
    var params = '{"type":"' +type + '", "idSource":"' +idSource + '"}';

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: params,
        url: "/Image/GetImage",
        dataType: "json",
        success: function (result) {
            if (result != "")
            {
                if (result.Url != null && result.Url.trim() != "")
                {
                    $(".modal-body #imagePop").prop("src", result.Url);
                }
                else if (result.Content != null)
                {
                    //var base64 = encode(result.Content)
                    var format;
                    if (result.Format != null)
                    {
                        format = result.Format.toLowerCase();
                    }
                    if (format != 'jpg' && format != 'jpeg' && format != 'gif' &&
                        format != 'bmp' && format != 'png' && format != 'tiff')
                    {
                        format = 'jpeg';
                    }
                    var url = "data:image/" + format + ";base64," + result.ContentB64 + ";"
                    $(".modal-body #imagePop").prop("src", url);
                }
                else
                {
                    $(".modal-body #imagePop").prop("src", "");
                }
                $(".modal-body #imagePop").prop("alt", result.AlternativeText);
                $(".modal-body #additionalInfo").html(result.AdditionalText);
                $(".modal-header #modalTitle").html(result.Title);
            }
            else
            {
                $(".modal-body #imagePop").prop("src", "");
                $(".modal-body #imagePop").prop("alt", "Vacío");
                $(".modal-body #additionalInfo").html("");
                $(".modal-header #modalTitle").html("Vacío");
            }

            $('#imageModal').modal('show');
        },
        error: function (jqXHR, exception) {
            if (jqXHR.status == 0) {
                alert('Unable to connect to server. Please Verify Network.');
            } else if (jqXHR.status == 404) {
                alert('Requested page not found. [404]');
            } else if (jqXHR.status == 500) {
                alert('Internal Server Error [500].');
            } else if (exception == 'timeout') {
                alert('Time out error.');
            } else if (exception == 'abort') {
                alert('Ajax request aborted.');
            } else {
                alert('Uncaught Error.\n' + jqXHR.responseText);
            }
        }
    });
}
