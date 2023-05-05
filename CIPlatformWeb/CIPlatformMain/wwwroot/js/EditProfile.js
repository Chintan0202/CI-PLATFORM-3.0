

var loadFile = function (event) {
    var image = document.getElementById("output");
    image.src = URL.createObjectURL(event.target.files[0]);
    console.log(image)
};

$("#changepass").on('click', function () {
    var uoldpassword = $("#OldPassword").val();
    var unewpassword = $('#NewPassword').val();
    var uconformpassword = $("#Conformpassword").val();

    if (unewpassword != uconformpassword) {

        Swal.fire({
            icon: 'warning',
            title: 'Oops...',
            text: 'Password And Conform Password must be same'
        })
    }
    else if (uoldpassword == "") {
        Swal.fire({
            icon: 'warning',
            title: 'Oops...',
            text: 'Enter Something in OldPassword'
        })
    }
    else if (unewpassword == "") {
        Swal.fire({
            icon: 'warning',
            title: 'Oops...',
            text: 'Enter Something in newpassword'
        })
    }
    else if (uconformpassword == "") {
        Swal.fire({
            icon: 'warning',
            title: 'Oops...',
            text: 'Enter Something in conformpassword'
        })
    }

    else {
        $.ajax({
            type: 'POST',
            url: '/User/chnagepassword',
            data: {
                oldpassword: uoldpassword,
                ConformPassword: uconformpassword,
                NewPassword: unewpassword
            },
            success:
                function (res) {
                    Swal.fire({
                        position: 'top-end',
                        icon: 'success',
                        title: 'Password Is Succcessfully Changed',
                        showConfirmButton: false,
                        timer: 1500
                    })
                    $("#OldPassword").val("");
                    $('#NewPassword').val("");
                    $("#Conformpassword").val("");
                    $("#staticBackdrop").modal('hide');
                },
            failure:
                function () {
                    console.log('error');
                }
        });
    }
})
$('#country').on('change', function () {

    $.ajax({
        type: 'GET',

        url: '/User/GetCities',
        data: {

            Country_id: $('#country').val()
        },
        success:
            function (res) {
                console.log(res);
                $(".city-drop").empty();
                for (var i = 0; i < res.length; i++) {
                    console.log(res[i]);
                    $('.city-drop').append(`<option value="${res[i].cityId}">${res[i].name}</option>`);
                }
                $('.city-drop').removeAttr("disabled");
                $('.city-alert').hide();
            },
        failure:
            function () {

            }
    });
});

$(function () {

    $('body').on('click', '.list-group .list-group-item', function () {
        $(this).toggleClass('active');
    });
    $('.list-arrows a').click(function () {
        var $button = $(this), actives = '';
        if ($button.hasClass('move-left')) {
            actives = $('.list-right ul li.active');
            actives.clone().appendTo('.list-left ul');
            actives.remove();
        } else if ($button.hasClass('move-right')) {
            actives = $('.list-left ul li.active');
            actives.clone().appendTo('.list-right ul');
            actives.remove();
        }
    });
    $('.dual-list .selector').click(function () {
        var $checkBox = $(this);
        if (!$checkBox.hasClass('selected')) {
            $checkBox.addClass('selected').closest('.well').find('ul li:not(.active)').addClass('active');
            $checkBox.children('i').removeClass('glyphicon-unchecked').addClass('glyphicon-check');
        } else {
            $checkBox.removeClass('selected').closest('.well').find('ul li.active').removeClass('active');
            $checkBox.children('i').removeClass('glyphicon-check').addClass('glyphicon-unchecked');
        }
    });
    $('[name="SearchDualList"]').keyup(function (e) {
        var code = e.keyCode || e.which;
        if (code == '9') return;
        if (code == '27') $(this).val(null);
        var $rows = $(this).closest('.dual-list').find('.list-group li');
        var val = $.trim($(this).val()).replace(/ +/g, ' ').toLowerCase();
        $rows.show().filter(function () {
            var text = $(this).text().replace(/\s+/g, ' ').toLowerCase();
            return !~text.indexOf(val);
        }).hide();
    });

});

$('#Addskills').on('click', function () {
    var selected = [];
    console.log($('.selected-list list'));
    $(".selected-list .list-group-item").map(function () {
     
        if (selected.includes($(this).val()) === false) selected.push($(this).val());
        console.log(selected);
    });
    $.ajax({
        method: 'POST',
        url: '/User/Updateskills',
        data: {
            Userskills: selected
        },
        success: function (res) {
            location.reload();
        }
    });
    console.log(selected);
})