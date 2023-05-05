var clockElement = document.getElementById('clock');
var userlist;
function clock() {
    clockElement.textContent = new Date().toString().substring(0, 25);
}

setInterval(clock, 1000);


//----------------------------------------------------------User CRUD Methods--------------------------------------------------------


//To AddUser 
function AddUser() {

    $.ajax({
        url: '/Admin/AddUser',

        success: function (res) {


            userlist = $(document).find("#UserData").html();
            console.log(userlist);

            var newbody = $($.parseHTML(res)).find("#AddUser").html();

            $("#DynamicDiv").html(newbody);

        }
    });
}

//To UserData
function GetUserData() {
    $('#v-pills-CMS-tab').removeClass('active-tab');
    $('#v-pills-Mission-tab').removeClass('active-tab');
    $.ajax({
        url: '/Admin/UserData',

        success: function (res) {



            console.log(res);

            var newbody = $($.parseHTML(res)).html();

            $("#DynamicDiv").html(res);

            var table = new DataTable('#myTable', {

                "lengthChange": false,
                "ordering": false
            });
            $('#myInputTextField').keyup(function () {
                table.search($(this).val()).draw();
            });

        }
    });


}

//To Get CMSlIST
function GetCMSList() {
    $('#v-pills-Mission-tab').removeClass('active-tab');
    $('#UserList').removeClass('active-tab');
    $.ajax({
        url: '/Admin/CMSList',

        success: function (res) {


            $("#DynamicDiv").html(res);

            var CMSTable = new DataTable('#CMSTable', {

                "lengthChange": false,
                "ordering": false
            });

            $('#CMSSearch').keyup(function () {
                CMSTable.search($(this).val()).draw();
            });
        }
    });
}

//To call AddEditUser Get Method
function AddEditUser(userid) {
    $.ajax({
        url: '/Admin/AddEditUserPage',
        data: {
            UserId: userid
        },
        success: function (res) {


            $("#DynamicDiv").html(res);


        }
    });
}


//----------------------------------------------------------CMS CRUD Methods--------------------------------------------------------


//To call Add CMSPage Get Method
function AddCMS() {

    $.ajax({
        url: '/Admin/AddEditCMSPage',

        success: function (res) {


            $("#DynamicDiv").html(res);


        }
    });
}

//To call Edit CMSPage Get Method
function EditCMS(CMSPageId) {

    $.ajax({
        url: '/Admin/AddEditCMSPage',
        data: {
            CMSId: CMSPageId
        },
        success: function (res) {


            $("#DynamicDiv").html(res);


        }
    });
}

//To call Delete CMS Page Method
function DeleteCMS(CMSPageId) {
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
                url: '/Admin/DeleteCMSPage',
                data: {
                    CMSId: CMSPageId
                },
                success: function (res) {
                    GetCMSList();
                }
            });
        }
    })
}

//To Get Mission Application List
function GetMissionApplication() {
    $('#v-pills-CMS-tab').removeClass('active-tab');
    $('#UserList').removeClass('active-tab');
    $('#v-pills-Mission-tab').removeClass('active-tab');

    $.ajax({
        url: '/Admin/MissionApplicationList',

        success: function (res) {



            console.log(res);

            var newbody = $($.parseHTML(res)).html();

            $("#DynamicDiv").html(res);

            let MissionAplicationtable = new DataTable('#MissionApplicationTable', {

                "lengthChange": false,
                "ordering": false
            });

            $('#SearchMissionApplication').keyup(function () {
                MissionAplicationtable.search($(this).val()).draw();
            });


        }
    });


}

//To Get Theme List
function GetThemeList() {
    $('#v-pills-CMS-tab').removeClass('active-tab');
    $('#UserList').removeClass('active-tab');
    $('#v-pills-Mission-tab').removeClass('active-tab');
    $.ajax({
        url: '/Admin/MissionThemeList',

        success: function (res) {


            $("#DynamicDiv").html(res);

            var ThemeTable = new DataTable('#ThemeTable', {

                "lengthChange": false,
                "ordering": false
            });

            $('#ThemeTableSearch').keyup(function () {
                ThemeTable.search($(this).val()).draw();
            });
        }
    });
}


//----------------------------------------------------------Skill CRUD Methods--------------------------------------------------------


//To Get SkillList
function GetSkillsList() {
    $('#v-pills-CMS-tab').removeClass('active-tab');
    $('#UserList').removeClass('active-tab');
    $('#v-pills-Mission-tab').removeClass('active-tab');
    $.ajax({
        url: '/Admin/MissionSkillList',

        success: function (res) {


            $("#DynamicDiv").html(res);

            var SkillTable = new DataTable('#SkillTable', {

                "lengthChange": false,
                "ordering": false
            });

            $('#SkillTableSearch').keyup(function () {
                SkillTable.search($(this).val()).draw();
            });
        }
    });
}

//To Draft skill For Edit
function FillModal(Skillid) {

    $('#SkillEdit').modal('show');
    $.ajax({
        url: '/Admin/DraftSkill',
        data: {
            SkillId: Skillid
        },
        success: function (res) {
            if (res != "Add") {
                $('#SkillName').val(res.skillName);
                $('#SkillId').val(res.skillId);
            }
            
        }
    });

}



//To Call Add/Edit Skill Method
function AddEditSkill() {

    $.ajax({
        url: '/Admin/AddEditSkill',
        data: {
            SkillId: $('#SkillId').val(),
            SkillName: $('#SkillName').val(),
            SkillStatus: $('#SkillStatus').val()
        },
        success: function (res) {
            $('#SkillEdit').modal('hide');
            GetSkillsList();

        }
    });
}

//To call Delete Method Skill
function DeleteSkill(Skillid) {
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
                url: '/Admin/DeleteSkill',
                data: {
                    SkillId: Skillid
                },
                success: function (res) {
                    $('#SkillEdit').modal('hide');
                    GetSkillsList();
                }
            });
        }
    })
}



//----------------------------------------------------------Story CRUD Methods--------------------------------------------------------



//To Get Story List
function GetStoryList() {
    $('#v-pills-CMS-tab').removeClass('active-tab');
    $('#UserList').removeClass('active-tab');
    $('#v-pills-Mission-tab').removeClass('active-tab');
    $.ajax({
        url: '/Admin/StoryList',

        success: function (res) {


            $("#DynamicDiv").html(res);

            var StoryTable = new DataTable('#StoryTable', {

                "lengthChange": false,
                "ordering": false
            });

            $('#SearchStory').keyup(function () {
                StoryTable.search($(this).val()).draw();
            });
        }
    });
}

//To Get StoryDetails
function GetStoryDetails(storyid) {


    $.ajax({
        url: '/Admin/StoryDetails',
        data: {
            StoryId: storyid
        },
        success: function (res) {
            $("#DynamicDiv").html(res);
        }
    });
}

//To Delete Story
function DeleteStory(StoryId) {
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
                url: '/Admin/StoryDelete',
                data: {
                    StoryId: StoryId
                },
                success: function (res) {

                    Swal.fire({
                        position: 'top-end',
                        icon: 'success',
                        title: 'Your work has been saved',
                        showConfirmButton: false,
                        timer: 3000
                    })
                    GetStoryList();
                }
            });

        }
    })




}

//To Approve Story Request
function ApproveStory(StoryId) {
    $.ajax({
        url: '/Admin/StoryApprove',
        data: {
            StoryId: StoryId
        },
        success: function (res) {

            Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'Your work has been saved',
                showConfirmButton: false,
                timer: 3000
            })
            GetStoryList();
        }
    });
}

//To Decline Story
function DeclineStory(StoryId) {
    $.ajax({
        url: '/Admin/StoryDecline',
        data: {
            StoryId: StoryId
        },
        success: function (res) {

            Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'Your work has been saved',
                showConfirmButton: false,
                timer: 3000
            })
            GetStoryList();
        }
    });
}


//----------------------------------------------------------Mission CRUD Methods--------------------------------------------------------



//To Get Mission List Table
function GetMissionList() {
    $('#v-pills-CMS-tab').removeClass('active-tab');
    $('#UserList').removeClass('active-tab');


    $.ajax({
        url: '/Admin/MissionList',

        success: function (res) {


            $("#DynamicDiv").html(res);

            var MissionTable = new DataTable('#MissionTable', {

                "lengthChange": false,
                "ordering": false
            });

            $('#MissionSearch').keyup(function () {
                MissionTable.search($(this).val()).draw();
            });
        }
    });
}

//To Delete Mission 
function DeleteMission(MissionId) {
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
                url: '/Admin/DeleteMission',
                data: {
                    MissionId: MissionId
                },
                success: function (res) {

                    Swal.fire({
                        position: 'top-end',
                        icon: 'success',
                        title: 'Your work has been saved',
                        showConfirmButton: false,
                        timer: 3000
                    })
                    GetMissionList();
                }
            });

        }
    })

}

//To AddEdit MissionTable
function AddEditMission(missionid) {
    $.ajax({
        url: '/Admin/AddEditMission',
        data: {
            MissionId: missionid
        },
        success: function (res) {


            $("#DynamicDiv").html(res);


        }
    });
}


//----------------------------------------------------------Theme CRUD Methods--------------------------------------------------------



//To Drft Theme For Edit
function DraftTheme(themeid) {
    $('#ThemeEdit').modal('show');
    $.ajax({
        url: '/Admin/DraftTheme',
        data: {
            MissionThemeId: themeid
        },
        success: function (res) {
            console.log(res);

            if (res != "add") {
                $('#ThemeName').val(res.title);
                $('#ThemeId').val(res.missionThemeId);
                $('#Status').val(res.status);
            }
            else {
                $('#ThemeName').val("");
                $('#ThemeId').val("");
                $('#Status').val("");
            }
            }
    });
}

//To AddEdit Theme
function AddEditTheme() {
    $.ajax({
        url: '/Admin/AddEditTheme',
        data: {
            ThemeId: $('#ThemeId').val(),
            ThemeName: $('#ThemeName').val(),
            Status: $('#Status').val()
        },
        success: function (res) {
            $('#ThemeEdit').modal('hide');
            GetThemeList();

        }
    });
}

//To Delete Theme
function DeleteTheme(ThemeId) {
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
                url: '/Admin/DeleteTheme',
                data: {
                    MissionThemeId: ThemeId
                },
                success: function (res) {
                    GetThemeList();
                }
            });
        }
    })
}

//----------------------------------------------------------Banner CRUD Methods--------------------------------------------------------


//To Get Banner List Table
function GetBannerList() {

    $('#UserList').removeClass('active-tab');
    $('#v-pills-Mission-tab').removeClass('active-tab');

    $.ajax({
        url: '/Admin/GetBannerList',

        success: function (res) {


            $("#DynamicDiv").html(res);

            var BannerTable = new DataTable('#BannerTable', {

                "lengthChange": false,
                "ordering": false
            });

            $('#BannerTableSearch').keyup(function () {
                BannerTable.search($(this).val()).draw();
            });
        }
    });
}

//Drfat Banner For Edit
function DraftBanner(Bannerid) {
    $('#ThemeEdit').modal('show');
    $.ajax({
        url: '/Admin/DraftBanner',
        data: {
            BannerId: Bannerid
        },
        success: function (res) {
            console.log(res);
            $('#BannerSort').val(res.sortOrder);
            $('#BannerText').val(res.text);

            $('#BannerId').val(res.bannerId);
            $('#output').attr('src', res.image);

        }
    });
}


function DeleteBanner(Bannerid) {
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
        url: '/Admin/DeleteBanner',
        data: {
            BannerId: Bannerid
        },
        success: function (res) {

            GetBannerList()

        }
    });


        }
    })
}