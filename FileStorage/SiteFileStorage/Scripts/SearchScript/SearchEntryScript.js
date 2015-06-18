
    function getList(node,type)
    {
        var id = $('#list .active')[0].id;
        if (node.className != "active")
        {
            $('.active').removeClass("active");
            node.className = "active";
            $('#'+type + 'List').show();
            $('#'+id + 'List').hide();
        }
            
    }