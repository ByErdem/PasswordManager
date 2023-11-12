
$(function () {
    $(document).on("click", ".btn", function (e) {
        e.preventDefault();

        var user = {
            Name: $(".txtName").val(),
            Surname: $(".txtSurname").val(),
            Username: $(".txtUsername").val(),
            Password: $(".txtPassword").val(),
        }


        var settings = {
            link: "/Login/Register",
            data: user,
            object: null,
            tokenNeeded: false,
            event: function (rsp) {
                if (rsp.IsSuccess) {
                    window.location.replace("/Dashboard");
                }
            }
        }

        CallRequest(settings);
    });
});