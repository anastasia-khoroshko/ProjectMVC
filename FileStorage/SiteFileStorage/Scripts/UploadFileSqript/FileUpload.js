     $(document).ready(function () {
          $("#fileupload").fileupload({
              dataType: 'json',
              url: '/Files/SaveFiles',
              autoUpload: true,
              done: function (e, data) {
                  $('#file_name').val(data.result.name);
                  $('#file_type').val(data.result.type);
                  $('#file_size').val(data.result.size);
                  show("block");
              }
          }).on('fileuploadprogressall', function (e, data) {
              var progress = parseInt(data.loaded / data.total * 100, 10);
              $('.progress .progress-bar').css('width', progress + '%');
          });
      });

function show(state) {
    document.getElementById('file_name').style.display = state;
    document.getElementById('file_type').style.display = state;
    document.getElementById('file_size').style.display = state;
    document.getElementById('name').style.display = state;
    document.getElementById('desсription').style.display = state;
    document.getElementById('bytes').style.display = state;
    document.getElementById('lblFN').style.display = state;
    document.getElementById('lblFT').style.display = state;
    document.getElementById('lblFS').style.display = state;
    document.getElementById('lblname').style.display = state;
    document.getElementById('lbldescription').style.display = state;
    document.getElementById('btncreate').style.display = state;
}
function getNameFile(name)
{
    var res = name.value;
    if (res == "")
        document.getElementById('btncreate').disabled = true;
    else
        document.getElementById('btncreate').disabled = false;
}
function btnClick()
{
    document.getElementById('file_name').disabled = false;
    document.getElementById('file_type').disabled = false;
    document.getElementById('file_size').disabled= false;
}