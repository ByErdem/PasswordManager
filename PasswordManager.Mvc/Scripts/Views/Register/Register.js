
$(function () {
    $(document).on("click", ".btn", function (e) {
        e.preventDefault();

        var user = {
            Name: $(".txtName").val(),
            Surname: $(".txtSurname").val(),
            Username: $(".txtUsername").val(),
            Password: $(".txtPassword").val(),
        }

        if (user.Name == "" || user.Surname == "" || user.Username == "" || user.Password == "") {
            swal("Uyarı", "Lütfen tüm bilgileri doldurunuz.", "error");
        }


        var settings = {
            link: "/Login/Register",
            data: user,
            object: null,
            tokenNeeded: false,
            event: function (rsp) {

                if (rsp.ResultStatus == 0) {

                    var user = {
                        Username: $(".txtUsername").val(),
                        Password: $(".txtPassword").val()
                    }

                    Login(user);
                }
            }
        }

        CallRequest(settings);
    });
});