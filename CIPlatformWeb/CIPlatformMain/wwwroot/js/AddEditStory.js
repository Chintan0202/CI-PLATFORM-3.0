
tinymce.init({
    selector: '#editor',
    toolbar: ' bold italic strikethrough | superscript subscript removeformat'
});


const inputDiv = document.querySelector(".input-div")
const input = document.querySelector("#StoryImages")
const output = document.querySelector("#output")


const output1 = document.querySelector("#output1")
let imagesArray = []

let ajaximagesArray = []




input.addEventListener("change", () => {
    const files = input.files
    for (let i = 0; i < files.length; i++) {
        imagesArray.push(files[i])
    }
    displayImages()
})

inputDiv.addEventListener("drop", () => {
    e.preventDefault()
    const files = e.dataTransfer.files
    for (let i = 0; i < files.length; i++) {
        if (!files[i].type.match("image")) continue;

        if (imagesArray.every(image => image.name !== files[i].name))
            imagesArray.push(files[i])
    }
    displayImages()
})
function displayImages() {
    let images = ""
    imagesArray.forEach((image, index) => {
        images += `<div class="image">
                <img src="${URL.createObjectURL(image)}" alt="image">
                <span onclick="deleteImage(${index})">&times;</span>
              </div>`
    })
    output.innerHTML = images
}

function deleteImage(index) {
    imagesArray.splice(index, 1)
    displayImages()
}

var formData = new FormData();
$('#MissionId').on('change', function () {

    $.ajax({
        url: '/Story/GetStory',
        data: {
            MissionId: $('#MissionId').val()
        },
        success: function (res) {

            if (res == "NoStoryFound") {
                console.log("hey");
                $('#StoryVideoUrl').val("");
                $('#StoryTitle').val("");
                tinymce.activeEditor.setContent("");
                $('#StoryDate').val("");
               
                output1.innerHTML = "";
                $('#StoryId').val("");
                formData.append('StoryId', 0);
               
            }
            else {
                res = JSON.parse(res);
                console.log(res);
                $('#StoryTitle').val(res.Title);
                tinymce.activeEditor.setContent(res.Description);

                $('#StoryId').val(res.StoryId);
             
                var date = res.CreatedAt.split('T')[0];
                console.log(date);
                $('#StoryDate').val(date);

               


                for (var i = 0; i < res.StoryMedia.$values.length; i++) {
                    console.log(i);

                    if (res.StoryMedia.$values[i].Type == "URL") {
                        $('#StoryVideoUrl').val(res.StoryMedia.$values[i].Path);
                    }
                    else {
                        ajaximagesArray.push(res.StoryMedia.$values[i].Path);
                    }
                }
                console.log(ajaximagesArray)
                displayajaxImages();
            }
        }

    })
})
var deleteimg = [];

function displayajaxImages() {
    let images = ""
    ajaximagesArray.forEach((image, index) => {
        images += `<div class="image">
                <img src="${image}" alt="image">
                <span onclick="deleteajaxImage(${index})">&times;</span>
              </div>`
    })
    output1.innerHTML = images
}

function deleteajaxImage(index) {
    deleteimg.push(ajaximagesArray[index]);
    ajaximagesArray.splice(index, 1)

    displayajaxImages()
}


function AddEditStory(status) {



    var editor = tinymce.get('editor');
    var Description = editor.getContent();
    var files = imagesArray;

    ajaximagesArray.forEach((image, index) => {
        formData.append('Preloaded', image);
    })

    
    MissionId = $('#MissionId').val()
    StoryTitle = $('#StoryTitle').val()
    StoryDescription = Description

    StoryDate = $('#StoryDate').val()
    StoryUrl = $('#StoryVideoUrl').val()
    StoryId = $('#StoryId').val()
    console.log($('#StoryId').val())
    formData.append('StoryId', StoryId)
    formData.append('Status', status);
    formData.append('MissionId', MissionId);
    formData.append('StoryDescription', StoryDescription);
    formData.append('StoryDate', StoryDate);
    formData.append('StoryTitle', StoryTitle);
    formData.append('StoryVideoURL', StoryUrl);



    var files = imagesArray;
    for (var i = 0; i < files.length; i++) {
        formData.append('StoryImages', files[i]);
    }
    if (MissionId == 0 || StoryTitle == "" || StoryDate == "") {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Please Enter Valid Input!',

        })
    }
    else {


        $.ajax({
            type: 'POST',
            url: '/Story/AddEditStoryPost',
            data: formData,
            contentType: false,
            processData: false,
            success: function (res) {
                console.log(res)
                if (res = "success") {

                    Swal.fire({
                        position: 'top-end',
                        icon: 'success',
                        title: 'Story Saved SuccessFully',
                        showConfirmButton: false,
                        timer: 1500
                    })
                   
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Story Is Not Saved!',

                    })
                }
            }
        });
    }
}