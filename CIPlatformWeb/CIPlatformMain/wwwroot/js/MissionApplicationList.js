

function Approve(MissionId, UserId) {
    console.log(MissionId);
    console.log(UserId);
    $.ajax({
        url: '/Admin/ApproveMissionApplication',
        data: {
            UserId: UserId,
            MissionId: MissionId
        },
        success: function (res) {
            GetMissionApplication();
            Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'Your work has been saved',
                showConfirmButton: false,
                timer: 1500
            })
        }
    });
}
function Decline(MissionId, UserId) {
    $.ajax({
        url: '/Admin/DeclineMissionApplication',
        data: {
            UserId: UserId,
            MissionId: MissionId
        },
        success: function (res) {
            GetMissionApplication();
            Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'Your work has been saved',
                showConfirmButton: false,
                timer: 1500
            })
        }
    });
}