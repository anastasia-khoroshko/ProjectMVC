$(document).ready(function () {
    $("#fileupload").fileupload({
        dataType: 'json',
        url: '/Files/SaveFiles',
        autoUpload: true,
        done: function (e, data) {
            $('#file_name').val(data.result.name);
            $('#file_type').val(data.result.type);
            $('#file_size').val(data.result.size);
            show();
        }
    }).on('fileuploadprogressall', function (e, data) {
        var progress = parseInt(data.loaded / data.total * 100, 10);
        $('.progress .progress-bar').css('width', progress + '%');
    });
});

function show() {
    $('#formUpload').css('display', 'block');
}
function getNameFile(name) {
    var res = name.value;
    if (res == "")
        $('#btncreate').prop('disabled', true);
    else
        $('#btncreate').prop('disabled', false);
}

function btnClick(form) {
    $('#file_name').prop('disabled', false);
    $('#file_type').prop('disabled', false);
    $('#file_size').prop('disabled', false);
    var file_name = $('#file_name').val();
    var file_type = $('#file_type').val();
    var file_size = $('#file_size').val();
    var name = $('#name').val();
    var desсription = $('#desсription').val();
    var checkboxes = [];
    var j = 0;
    for (i = 0; i < form.length - 1; i++) {
        if (form.elements[i].checked) {
            checkboxes[j++] = form.elements[i].name;
        }
    }
    var obj = { FileName: file_name, FileType: file_type, FileSize: file_size, Name: name, Description: desсription, Categories: checkboxes };
    $.ajax({
        type: "POST",
        url: "/Files/CreatePost",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#formUpload').css('display', 'none');
            alert("Your file upload successfully!");
        },
        error: function (data) {
            alert("Your file doesn't upload!")
        }
    })
}

$("#contentPager a").click(function () {
    $.ajax({
        url: $(this).attr("href"),
        type: 'GET',
        cache: false,
        success: function (result) {
            $('#content').html(result);
        }
    });
    return false;
});