$(document).ready(function () {

        $('#close').click(function () {
            $('#shadow').hide();
            $('#window').hide();
        });

        $('#edit').click(function () {
            $('#shadow').show();
            $('#window').slideToggle(500);
        });
        $('#savepost').click(function()
        {
            var name = $('#Name').val();
            var description = $('#Description').val();
            var id = $('#Id').val();
            var reg = "/[a-z,0-9]*/";
            if (name.toString().length < 3 | reg.match(name)) {
                $('#error').append("Value must be bigger than 3 character or invalid argument");
            }
            else {
                var post = { Name: name, Description: description, Id: id };
                $.post("/Files/SaveChanges", post, null);
                $('#shadow').hide();
                $('#window').hide();
                $('#newName').html("<h2>"+name+"</h2>");
                $('#newDesc').html("<em>"+description+"</em>");
            }
        })

        $(".rating-star-block .star").mouseleave(function () {
            $("#" + $(this).parent().attr('id') + " .star").each(function () {
                $(this).addClass("outline");
                $(this).removeClass("filled");
            });
        });
        $(".rating-star-block .star").mouseenter(function () {
            var hoverVal = $(this).attr('rating');
            $(this).prevUntil().addClass("filled");
            $(this).addClass("filled");
            $("#RAT").html(hoverVal);
        });
        $(".rating-star-block .star").click(function () {
                
            var v = $(this).attr('rating');
            var newScore = 0;
            var updateP = "#" + $(this).parent().attr('id') + ' .CurrentScore';
            var postID = $("#" + $(this).parent().attr('id') + ' .postID').val();
                
            $("#" + $(this).parent().attr('id') + " .star").hide();
            $("#" + $(this).parent().attr('id') + " .yourScore").html("Your Score is : &nbsp;<b style='color:#ff9900; font-size:15px'>" + v + "</b>");
            $.ajax({
                type: "POST",
                url: "/Files/SaveRating",
                data: "{postID: '" + postID + "',rate: '" + v + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    setNewScore(updateP, data.d);
                },
                error: function (data) {
                    alert("You already rated this post");
                }
            });
        });
        function setNewScore(container, data) {
            $(container).html(data);
            $("#myElem").show('1000').delay(2000).queue(function (n) {
                $(this).hide(); n();
            });
        };
        $('#addComment').click(function () {
            var comment = $('#comment').val();
            var postId = $('#Id').val();
            $.ajax({
                type: "POST",
                url: "/Files/SaveComment",
                data: "{postId: '" + postId + "',comment: '" + comment + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $('#list').html(comment + " posted by " +"<a href=\"/Profile/?email="+data.UserEmail+"\">"+ data.UserName+"</a>");
                    $('#comment').val("");
                    $('#errorCom').html("");
                },
                error: function (data) {
                    $('#errorCom').html("Enter comment!");
                    $('#errorCom').css('color','red');
                }
            })
        })

    })