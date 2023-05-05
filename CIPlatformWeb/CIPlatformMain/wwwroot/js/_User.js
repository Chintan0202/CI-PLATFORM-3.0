


function DeleteUser(UserId) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {

            $.ajax({
                url: '/Admin/DeleteUser',
                type: 'POST',

                data: {
                    UserId: UserId
                },
                success: function (result) {
                    
                    Swal.fire(
                        'Deleted!',
                        'User has been deleted.',
                        'success'
                    )
                   

                }
            });

        }
    })
}
function EditUser(userId) {

   
    console.log(userId);
    $.ajax({
        url: '/Admin/GetUserData',
        type: 'POST',

        data: {
            UserId: userId
        },
        success: function (res) {
            

        }
    });

}

