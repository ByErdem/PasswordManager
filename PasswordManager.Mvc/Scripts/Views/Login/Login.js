
$(function () {

    console.log("deneme");

    $(document).on("click", ".btn", function (e) {
        e.preventDefault();

        var user = {
            Username: $(".txtUsername").val(),
            Password: $(".txtPassword").val()
        }

        CallRequest("/Login/SignIn", user, null, false, function (rsp) {

            if (rsp.ResultStatus == 0) {
                localStorage.setItem("guid", rsp.Data.GuidKey)
                window.location.replace("/Dashboard");
            }
            else {
                swal("Uyarı", rsp.ErrorMessage, "error");
            }
        });

    });
});