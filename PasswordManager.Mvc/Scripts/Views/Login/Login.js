
$(function () {

    $(document).on("click", ".btn", function (e) {
        e.preventDefault();

        var user = {
            Username: $(".txtUsername").val(),
            Password: $(".txtPassword").val()
        }

        var settings = {
            link: "/Login/SignIn",
            data: user,
            object: null,
            tokenNeeded: false,
            event: function (rsp) {
                if (rsp.ResultStatus == 0) {
                    localStorage.setItem("guid", rsp.Data.GuidKey)
                    window.location.replace("/Dashboard");
                }
                else {
                    swal("Uyarı", rsp.ErrorMessage, "error");
                }
            }
        }

        CallRequest(settings);

    });
});