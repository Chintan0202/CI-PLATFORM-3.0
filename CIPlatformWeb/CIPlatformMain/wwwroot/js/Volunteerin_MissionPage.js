


//To Add Rating
var ratingval
$('.rating-star').on('click', function () {
    console.log($(this).attr('id'));
    ratingval = $(this).attr('id');
    rating();
})


function rating() {
    $.ajax({
        type: 'POST',
        url: '/Home/Giverating',
        data: {
            user_rating: ratingval
        },
        success:

            function (res) {

                location.reload();
                var newm = $($.parseHTML(res)).find(".rating-div").html();

                $(".rating-div").html(newm);
                console.log(newm);
                var newl = $($.parseHTML(res)).find(".user_rating").html();
                console.log(newl);
                $(".user_rating").html(newl);

            },
        failure:
            function () {
                console.log('error');

            }
    });
}

//Add to Favrouite
function addtofav(favvalue, favmissionid) {
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
                console.log(res);
                var newm = $($.parseHTML(res)).find(".fab-button").html();
                $(".fab-button").html(newm);



                Swal.fire({
                    position: 'top-end',
                    icon: 'success',
                    title: 'Added To Favrouite',
                    showConfirmButton: false,
                    timer: 1500
                })


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
        success:

            function (res) {
                console.log(res);
                var newm = $($.parseHTML(res)).find(".fab-button").html();

                $(".fab-button").html(newm);
            },
        failure:
            function () {
                console.log('error');
            }
    });
}



//To Add Comment
function postcomment() {
    console.log($('.comment-text-box').val());
    var commenttext1 = $('.comment-text-box').val();
    if (commenttext1 == "") {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Please Write Something in Comment!',
            
        })
    }
    else {
        $.ajax({
            type: 'POST',
            url: '/Home/AddComment',
            data: {
                commenttext: commenttext1,
            },
            success:
                function (res) {
                    console.log(res);
                    var newm = $($.parseHTML(res)).find(".comment-box").html();
                    $(".comment-box").html(newm);


                    Swal.fire({
                        position: 'top-end',
                        icon: 'success',
                        title: 'Comment Added',
                        showConfirmButton: false,
                        timer: 1500
                    })

                },
            failure:
                function () {
                    console.log('error');
                }
        });
    }

    $('.comment-text-box').val('');
}


//To Recommend to Co-Worker
$('.recommend').on('click', function () {
    console.log($(this).val());
    console.log($('#mission_id').val());
    $.ajax({
        type: 'POST',
        url: '/Home/Recommend',
        data: {
            to_userid: $(this).val(),
            missionid: $('#mission_id').val()
        },
        success:
            function (res) {
                console.log(res);

                Swal.fire({
                    position: 'top-end',
                    icon: 'success',
                    title: 'Mail Sent',
                    showConfirmButton: false,
                    timer: 1500
                })

                
            },
        failure:
            function () {
                console.log('error');
            }
    });
})

//To Apply To Mission
$('.apply-btn').on('click', function () {
    var mission_id = $(this).val();
    console.log(mission_id);
    $.ajax({
        type: 'POST',
        url: '/Home/MissionApplication',
        data: {
            missionid: mission_id

        },
        success:
            function (res) {
                location.reload();
            },
        failure:
            function () {
               
            }
    });
})


let slideIndex = 1;
showSlides(slideIndex);

// Next/previous controls
function plusSlides(n) {
    showSlides(slideIndex += n);
}

// Thumbnail image controls
function currentSlide(n) {
    showSlides(slideIndex = n);
}

function showSlides(n) {
    let i;
    let slides = document.getElementsByClassName("mySlides");
    let dots = document.getElementsByClassName("demo");
    let captionText = document.getElementById("caption");
    if (n > slides.length) { slideIndex = 1 }
    if (n < 1) { slideIndex = slides.length }
    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }
    for (i = 0; i < dots.length; i++) {
        dots[i].className = dots[i].className.replace(" active", "");
    }
    slides[slideIndex - 1].style.display = "block";
    dots[slideIndex - 1].className += " active";
    captionText.innerHTML = dots[slideIndex - 1].alt;
}

