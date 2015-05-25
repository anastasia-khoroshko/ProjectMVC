
    function getList(node,type)
    {
        var id = $('#list .active')[0].id;
        if (node.className != "active")
        {
            $('.active').removeClass("active");
            node.className = "active";
            document.getElementById(type + "List").style.display = 'block';
            document.getElementById(id + "List").style.display = 'none';
        }
            
    }