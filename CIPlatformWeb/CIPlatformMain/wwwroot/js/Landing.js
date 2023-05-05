
//Add to Favrouite
function addtofav(favvalue, favmissionid) {
    console.log($(this).attr('id'));
    console.log(favvalue);
    console.log(favmissionid);
    $.ajax({
        type: 'POST',
        url: '/Home/Favrouite',
        data: {
            favvalue: favvalue,
            MissionId: favmissionid

        },
        success:

            function (res) {
               
              

                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top-end',
                    showConfirmButton: false,
                    timer: 2000,
                    timerProgressBar: true,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })

                Toast.fire({
                    icon: 'success',
                    title: 'Mission added to favrouite'
                })
                var newm = $($.parseHTML(res)).find("#card-cont").html();
                $("#card-cont").html(newm);

                var newl = $($.parseHTML(res)).find(".mission-list-cont").html();
                $(".mission-list-cont").html(newl);

                var newp = $($.parseHTML(res)).find(".page-links").html();
                $(".page-links").html(newp);
            },
        failure:
            function () {
                console.log('error');
            }
    });
}


//Remove From Favrouite
function removefav(favvalue, favmissionid) {
   
    console.log(favvalue);
    console.log(favmissionid);
    $.ajax({
        type: 'POST',
        url: '/Home/Favrouite',
        data: {
            favvalue: favvalue,
            MissionId: favmissionid

        },
        success:function (res) {
            
                
            var newm = $($.parseHTML(res)).find("#card-cont").html();
            $("#card-cont").html(newm);

            var newl = $($.parseHTML(res)).find(".mission-list-cont").html();
            $(".mission-list-cont").html(newl);

            var newp = $($.parseHTML(res)).find(".page-links").html();
            $(".page-links").html(newp);

            
            },
        failure:
            function () {
                console.log('error');
            }
    });
}
/ Cookie Script /
if (getCookie('accepted') === 'yes') {
    document.getElementById("cookiediv").style.display = "none";
}

// user clicks the confirmation -> set the 'yes' value to cookie and set 'accepted' as name
function accpetCookie() {
    setCookie('accepted', 'yes', 100);
    document.getElementById("cookiediv").style.display = "none";
}

// code from :http://stackoverflow.com/a/4825695/191220
// set cookie method
function setCookie(c_name, value, exdays) {
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + exdays);
    var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
    document.cookie = c_name + "=" + c_value;
}

// get cookie method
function getCookie(c_name) {
    var i, x, y, ARRcookies = document.cookie.split(";");
    for (i = 0; i < ARRcookies.length; i++) {
        x = ARRcookies[i].substr(0, ARRcookies[i].indexOf("="));
        y = ARRcookies[i].substr(ARRcookies[i].indexOf("=") + 1);
        x = x.replace(/^\s+|\s+$/g, "");
        if (x == c_name) {
            return unescape(y);
        }
    }
}
function closecookiediv() {
    $('#cookiediv').addClass('d-none');
}

