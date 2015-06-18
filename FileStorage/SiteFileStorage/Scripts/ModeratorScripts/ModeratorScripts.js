
    function btnSaveCat(form) {
        var id = $('#Id').val();
        var file_name = $('#FileName').val();
        var file_type = $('#FileType').val();
        var name = $('#Name').val();
        var desсription = $('#Description').val();
        var permit = $('#Permit').val();
        var checkboxes = [];
        var j = 0;
        for (i = 0; i < form.length - 1; i++) {
            if (form.elements[i].checked) {
                checkboxes[j++] = form.elements[i].name;
            }
        }
        if (checkboxes.length == 0)
        {
            $('#list li').each(function(i)
            {
                checkboxes[i] = $(this).text();
            })
        }
        var obj = { Id: id, Name: name, Description: desсription, FileName: file_name, FileType: file_type, Permit: permit, Categories: checkboxes };
        $.ajax({
            type: "POST",
            url: "/Moderator/SaveChanges",
            data: JSON.stringify(obj),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var listCat = []
                $.each(data, function (i, elem) {
                    listCat[i] = "<li>" + elem + "</li>";
                })
                $('#list').html(listCat);
            },
           
        })
    }
    